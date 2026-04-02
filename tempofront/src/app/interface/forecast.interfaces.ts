export interface IForecast {
  data: string;
  diaSemana: string;
  temperatura: string;
  tempMin: string;
  tempMax: string;
  clima: string;
  icone: string;
  vento: string;
  umidade: string;
}

export interface IApiResponseForecast {
  cidade: string;
  previsoes: IForecast[];
}