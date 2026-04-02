import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError, switchMap } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();
  const isExpired = authService.isTokenExpired();

  if (req.url.includes('api/auth/GetToken') || req.url.includes('ibge')) {
    return next(req);
  }

  if (!token || isExpired) {
    return authService.checkIn().pipe(
      switchMap((newToken) => {

        const newAuthReq = req.clone({
          setHeaders: {
            Authorization: `${newToken.type || 'Bearer'} ${newToken.token}`
          }
        });

        return next(newAuthReq);
      }),
      catchError((err) => {
        console.error('Falha crítica na renovação do token');
        return throwError(() => err);
      })
    );
  }


  const authReq = req.clone({
    setHeaders: {
      Authorization: `${token.type || 'Bearer'} ${token.token}`
    }
  });

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      return throwError(() => error);
    })
  );
};