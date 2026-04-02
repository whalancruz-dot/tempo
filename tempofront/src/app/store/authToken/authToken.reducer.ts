import { createReducer, on } from '@ngrx/store';
import { clearAuthToken, loadAuthToken } from './authToken.actions';
import { IAuthToken } from '../../interface/authtoken.interfaces';

export const initialState: IAuthToken | null = null;

export const authTokenReducer = createReducer<IAuthToken | null>(
  initialState,
  on(loadAuthToken, (state, { authToken }) => authToken),
  on(clearAuthToken, () => null)
);
