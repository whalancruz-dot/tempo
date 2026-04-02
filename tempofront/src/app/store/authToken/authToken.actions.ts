import { createAction, props } from '@ngrx/store';
import { IAuthToken } from '../../interface/authtoken.interfaces';

export const loadAuthToken = createAction('[AuthToken] Load AuthToken', props<{ authToken: IAuthToken }>());
export const clearAuthToken = createAction('[AuthToken] Clear AuthToken');
