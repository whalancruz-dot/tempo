import { createFeatureSelector } from '@ngrx/store';
import { IAuthToken } from '../../interface/authtoken.interfaces';

export const selectAuthToken = createFeatureSelector<IAuthToken | null>('authToken');
