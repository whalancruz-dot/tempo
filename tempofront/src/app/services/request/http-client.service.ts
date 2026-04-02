import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../../app.config';

@Injectable({ providedIn: 'root' })
export class HttpClientService {

  private http = inject(HttpClient);

  private config = inject(API_CONFIG);
  private apiUrl = this.config.baseUrl; // Agora nunca será undefined

  get<T>(endpoint: string, params?: Record<string, unknown>): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    let httpParams = new HttpParams();

    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined && params[key] !== '') {
          if (params[key] instanceof Date) {
            httpParams = httpParams.set(key, params[key].toISOString());
          } else {
            httpParams = httpParams.set(key, params[key].toString());
          }
        }
      });
    }

    return this.http.get<T>(url, { params: httpParams });
  }

  post<T>(endpoint: string, body: unknown, expectText = false): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;

    if (expectText) {
      return this.http.post(url, body, { responseType: 'text' }) as Observable<T>;
    }

    return this.http.post<T>(url, body);
  }

  put<T>(endpoint: string, body: unknown): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    return this.http.put<T>(url, body);
  }

  patch<T>(endpoint: string, body: unknown = {}): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    return this.http.patch<T>(url, body);
  }

  delete<T>(endpoint: string): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    return this.http.delete<T>(url);
  }

}
