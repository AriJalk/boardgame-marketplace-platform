import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { switchMap, catchError, filter, take } from 'rxjs/operators';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { AuthService } from '../auth/auth.service';
import { devEnvironment } from '../../../environments/environment.development';
import { InteractionStatus } from '@azure/msal-browser';

@Injectable()
export class OptionalAuthInterceptor implements HttpInterceptor {
  constructor(
    private msalService: MsalService,
    private authService: AuthService,
    private msalBroadcastService: MsalBroadcastService,
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const requestPath = new URL(req.url, window.location.origin).pathname;
    const isOptionalEndpoint = devEnvironment.apiConfig.optionalAuthEndpoints.some((endpoint) =>
      requestPath.startsWith(endpoint),
    );

    console.log(requestPath);
    if (!isOptionalEndpoint) {
      return next.handle(req);
    }

    const account = this.authService.activeAccountValue;

    if (!account) {
      return next.handle(req);
    }

    return this.msalBroadcastService.inProgress$.pipe(
      filter((status) => status === InteractionStatus.None),
      take(1),
      switchMap(() => {
        const account = this.authService.activeAccountValue;

        if (!account) {
          return next.handle(req);
        }

        return from(
          this.msalService.acquireTokenSilent({
            account,
            scopes: [devEnvironment.apiConfig.accessAsUserScope],
          }),
        ).pipe(
          switchMap((result) => {
            const authReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${result.accessToken}`,
              },
            });

            return next.handle(authReq);
          }),
          catchError(() => next.handle(req)),
        );
      }),
    );
  }
}
