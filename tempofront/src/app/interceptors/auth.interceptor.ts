import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  if (req.url.includes('api/auth/GetToken') || req.url.includes('ibge')) {
    return next(req);
  }

  const authReq = req.clone({
    setHeaders: {
      Authorization: `${token?.type || 'Bearer'} ${token?.token}`
    }
  });

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      return throwError(() => error);
    })
  );
};