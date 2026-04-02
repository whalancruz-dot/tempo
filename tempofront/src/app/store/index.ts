import { provideStore } from '@ngrx/store';
import { ActionReducer } from '@ngrx/store';
import { localStorageSync } from 'ngrx-store-localstorage';
import { authTokenReducer } from './authToken/authToken.reducer';


export const reducersAppStore = {
  authToken: authTokenReducer
};

export function provideAppStore() {
  return provideStore(reducersAppStore, {
    metaReducers: [localStorageSyncReducer],
  });
}

export function localStorageSyncReducer(reducer: ActionReducer<AppState>) {
  return localStorageSync({
    keys: Object.keys(reducersAppStore),
    rehydrate: true,
  })(reducer);
}

type AppState = { [K in keyof typeof reducersAppStore]: ReturnType<(typeof reducersAppStore)[K]> };
