import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { toSignal } from '@angular/core/rxjs-interop';
import { IAuthToken } from '../../interface/authtoken.interfaces';
import { ITokenPayload } from '../../interface/tokenpayload.interfaces';
import { selectAuthToken } from '../../store/authToken/authToken.selectors';
import { HttpClientService } from '../request/http-client.service';
import { loadAuthToken } from '../../store/authToken/authToken.actions';
import { IResult } from '../../interface/result.interfaces';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private readonly httpClientService = inject(HttpClientService);
  private store = inject(Store);
  private refreshToken$?: Observable<IAuthToken>;

  IAuthToken = toSignal(this.store.select(selectAuthToken), { initialValue: null });

  checkIn(email: string, password: string): Observable<IResult<string>> {
    return this.httpClientService.get<IResult<string>>(`auth/GetToken`, { email, password });
  }

  setToken(token: IAuthToken) {
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


