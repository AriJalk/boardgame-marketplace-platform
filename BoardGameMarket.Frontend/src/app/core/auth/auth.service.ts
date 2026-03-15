import { Inject, Injectable, OnDestroy, OnInit } from '@angular/core';
import {
  MSAL_GUARD_CONFIG,
  MsalGuardConfiguration,
  MsalService,
  MsalBroadcastService,
} from '@azure/msal-angular';
import {
  EventType,
  EventMessage,
  InteractionStatus,
  RedirectRequest,
  AccountInfo,
} from '@azure/msal-browser';
import { BehaviorSubject, Subject, filter, takeUntil } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isIframe = false;
  private readonly _destroying$ = new Subject<void>();

  private readonly _activeAccountSubject$ = new BehaviorSubject<AccountInfo | null>(null);
  activeAccount$ = this._activeAccountSubject$.asObservable();
  get activeAccountValue(): AccountInfo | null {
    return this._activeAccountSubject$.value;
  }

  private _ready = new BehaviorSubject<boolean>(false);
  readonly ready$ = this._ready.asObservable();

  get ready() {
    return this._ready.value;
  }

  constructor(
    @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private msalService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
  ) {}

  get isLoggedIn(): boolean {
    return this.msalService.instance.getActiveAccount() !== null;
  }

  refreshActiveAccount() {
    this._activeAccountSubject$.next(this.msalService.instance.getActiveAccount());
  }

  init(): void {
    this.msalService.handleRedirectObservable().subscribe();
    this.isIframe = window !== window.parent && !window.opener; // Remove this line to use Angular Universal

    this.msalService.instance.enableAccountStorageEvents(); // Optional - This will enable ACCOUNT_ADDED and ACCOUNT_REMOVED events emitted when a user logs in or out of another tab or window
    this.msalBroadcastService.msalSubject$
      .pipe(
        filter(
          (msg: EventMessage) =>
            msg.eventType === EventType.ACCOUNT_ADDED ||
            msg.eventType === EventType.ACCOUNT_REMOVED,
        ),
      )
      .subscribe((result: EventMessage) => {
        if (this.msalService.instance.getAllAccounts().length === 0) {
          window.location.pathname = '/';
        } else {
        }
      });

    this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
        takeUntil(this._destroying$),
      )
      .subscribe(() => {
        this.checkAndSetActiveAccount();
        this._ready.next(true);
      });
  }

  checkAndSetActiveAccount() {
    /**
     * If no active account set but there are accounts signed in, sets first account to active account
     * To use active account set here, subscribe to inProgress$ first in your component
     * Note: Basic usage demonstrated. Your app may require more complicated account selection logic
     */
    let activeAccount = this.msalService.instance.getActiveAccount();
    if (!activeAccount && this.msalService.instance.getAllAccounts().length > 0) {
      let accounts = this.msalService.instance.getAllAccounts();
      this.msalService.instance.setActiveAccount(accounts[0]);
    }
    this.refreshActiveAccount();
  }

  loginRedirect() {
    if (this.msalGuardConfig.authRequest) {
      this.msalService.loginRedirect({
        ...this.msalGuardConfig.authRequest,
      } as RedirectRequest);
    } else {
      this.msalService.loginRedirect();
    }
  }

  logout() {
    this.msalService.logoutRedirect();
  }

  destroy(): void {
    this._destroying$.next(undefined);
    this._destroying$.complete();
  }
}
