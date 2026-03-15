import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDrawerContent, MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';
import { SideNav } from './core/side-nav/side-nav';
import { AuthService } from './core/auth/auth.service';
import { AccountInfo } from '@azure/msal-browser';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    SideNav,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatDrawerContent,
    AsyncPipe,
  ],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit, OnDestroy {
  protected readonly title = signal('frontend');

  activeAccount?: Observable<AccountInfo | null>;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.init();
    this.activeAccount = this.authService.activeAccount$;
  }

  ngOnDestroy(): void {
    this.authService.destroy();
  }

  loginRedirect() {
    this.authService.loginRedirect();
  }

  logout() {
    this.authService.logout();
  }
}
