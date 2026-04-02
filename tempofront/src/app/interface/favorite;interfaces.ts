export interface IApiResponseFavorite {
  id?: string;
  cidadeId: number;
  nome: string;
  state: string;
  usuarioId?: string;
}

export interface IFavorite {
  cidadeId?: number;
  nome: string;
  state: string;
  usuarioId?: string;
}