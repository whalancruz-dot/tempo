import { inject, Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { IApiResponseCity, ICity } from '../interface/city.interfaces';
import { IBGECity } from '../interface/ibge.interfaces';
import { HttpClientService } from './request/http-client.service';
import { IApiResponseForecast, IForecast } from '../interface/forecast.interfaces';
import { HttpClient } from '@angular/common/http';


@Injectable({ providedIn: 'root' })
export class CidadesService {

  private readonly httpClientService = inject(HttpClientService);
  private readonly http = inject(HttpClient);
  private readonly API_IBGE = 'https://servicodados.ibge.gov.br/api/v1/localidades/municipios?orderBy=nome';

  getCidadesIBGE(): Observable<ICity[]> {
    return this.http.get<IBGECity[]>(this.API_IBGE).pipe(
      map(data => data.map(item => ({
        id: String(item.id),
        name: item.nome,
        state: item.microrregiao?.mesorregiao?.UF?.sigla ?? 'UF'
      } as ICity)))
    );
  }

  getCity(cidade: string): Observable<IApiResponseCity> {
    return this.httpClientService.get<IApiResponseCity>(`Weather/${cidade}`);
  }

  getForecast(cidadeId: number): Observable<IForecast[]> {
    return this.httpClientService.get<IApiResponseForecast>(`Weather/previsao/${cidadeId}`).pipe(
      map((response: IApiResponseForecast) => response.previsoes.map(item => ({
        data: item.data,
        diaSemana: item.diaSemana,
        temperatura: item.temperatura,
        tempMin: item.tempMin,
        tempMax: item.tempMax,
        clima: item.clima,
        icone: this.getWeatherEmoji(item.icone),
        vento: item.vento,
        umidade: item.umidade
      })))
    );
  }

  private getWeatherEmoji(iconCode: string): string {
    const map: Record<string, string> = {
      '01d': '☀️',
      '01n': '🌙',
      '02d': '⛅',
      '02n': '☁️',
      '03d': '☁️',
      '03n': '☁️',
      '04d': '☁️',
      '04n': '☁️',
      '09d': '🌧️',
      '09n': '🌧️',
      '10d': '🌦️',
      '10n': '🌧️',
      '11d': '⛈️',
      '11n': '⛈️',
      '13d': '❄️',
      '13n': '❄️',
      '50d': '🌫️',
      '50n': '🌫️',
    };
    return map[iconCode] ?? '🌡️';
  }

}