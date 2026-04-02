export interface ICity {
  id: string;
  cidadeId?: number;
  name: string;
  state: string;
}

export interface IApiResponseCity {
  cidadeId: number;
  cidade: string;
  temperatura: string;
  clima: string
  umidade: string;
  vento: string;
}