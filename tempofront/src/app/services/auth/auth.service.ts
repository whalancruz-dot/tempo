import { Injectable, inject } from '@angular/core';
import { Observable, tap, catchError, throwError, shareReplay } from 'rxjs';
import { Store } from '@ngrx/store';
import { toSignal } from '@angular/core/rxjs-interop';
import { IAuthToken } from '../../interface/authtoken.interfaces';
import { ITokenPayload } from '../../interface/tokenpayload.interfaces';
import { selectAuthToken } from '../../store/authToken/authToken.selectors';
import { HttpClientService } from '../request/http-client.service';
import { loadAuthToken } from '../../store/authToken/authToken.actions';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private readonly httpClientService = inject(HttpClientService);
  private store = inject(Store);
  private refreshToken$?: Observable<IAuthToken>;

  IAuthToken = toSignal(this.store.select(selectAuthToken), { initialValue: null });

  public checkIn(): Observable<IAuthToken> {
    if (this.refreshToken$) return this.refreshToken$;
    this.refreshToken$ = this.httpClientService.get<IAuthToken>(`auth/GetToken`)
      .pipe(
        tap(tokenData => {
          this.setToken(tokenData);
          this.refreshToken$ = undefined;
        }),
        catchError(error => {
          this.refreshToken$ = undefined;
          return throwError(() => error);
        }),
        shareReplay(1)
      );

    return this.refreshToken$;
  }

  private setToken(token: IAuthToken) {
    this.store.dispatch(
      loadAuthToken({ authToken: token })
    );
  }

  getToken(): IAuthToken | null {
    return this.IAuthToken();
  }

  hasToken(): boolean {
    return !!this.getToken();
  }

  getDecodedToken(): ITokenPayload | null {
    const tokenModel = this.getToken();
    if (tokenModel == null) return null;

    try {
      const base64Url = tokenModel.token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );

      return JSON.parse(jsonPayload);
    } catch (error) {
      console.error('Erro ao decodificar token:', error);
      return null;
    }
  }

  isTokenExpired(): boolean {
    const token = this.getDecodedToken();
    if (!token || !token.exp) return true;

    const expirationDate = new Date(token.exp * 1000);
    return expirationDate < new Date();
  }


}
