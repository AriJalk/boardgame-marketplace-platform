import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { API_CONFIG_SERVICE, DEV_API_CONFIG } from './core/api-config/api-config.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    // provideHttpClient(),
    { provide: API_CONFIG_SERVICE, useValue: DEV_API_CONFIG },
  ]
};
