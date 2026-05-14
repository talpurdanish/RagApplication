import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';

import { httpInterceptorProviders } from './BussinessLogic/Services/Generics/HttpInterceptor';
import { HttpApiClient } from './BussinessLogic/Services/Generics/HttpApiClient';
import { Progress } from './Layouts/MainLayout/progress/progress';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(),
    httpInterceptorProviders,
    HttpApiClient,
    Progress,
  ],
};
