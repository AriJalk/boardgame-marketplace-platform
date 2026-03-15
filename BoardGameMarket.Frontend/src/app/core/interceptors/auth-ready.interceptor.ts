import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { switchMap, filter, take } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';

@Injectable()
export class AuthReadyInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Wait until auth is ready
    return this.authService.ready$.pipe(
      filter((isReady) => isReady), // only continue when ready
      take(1), // only the first time it's ready
      switchMap(() => next.handle(req)), // forward the request
    );
  }
}
