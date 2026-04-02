import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from './request/http-client.service';
import { IApiResponseFavorite as Favorite, IFavorite } from '../interface/favorite;interfaces';


@Injectable({ providedIn: 'root' })
export class FavoriteService {

  private readonly httpClientService = inject(HttpClientService);

  getFavorite(): Observable<Favorite[]> {
    return this.httpClientService.get<Favorite[]>(`Favorite`);
  }

  addFavorite(parametros: IFavorite): Observable<void> {
    return this.httpClientService.post(`Favorite`, parametros);
  }

  removeFavorite(id: string): Observable<void> {
    return this.httpClientService.delete(`Favorite/${id}`)
  }

}