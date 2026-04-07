import { Component, OnInit, signal, computed, effect, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AutoCompleteCompleteEvent, AutoCompleteModule } from 'primeng/autocomplete';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { IftaLabelModule } from 'primeng/iftalabel';
import { ICity } from '../../interface/city.interfaces';
import { CidadesService } from '../../services/cidades.service';
import { FavoriteService } from '../../services/favorite.service';
import { AuthService } from '../../services/auth/auth.service';
import { IFavorite } from '../../interface/favorite;interfaces';
import { IForecast } from '../../interface/forecast.interfaces';
import { LoginModalComponent } from '../../components/login/login.components';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    AutoCompleteModule,
    ButtonModule,
    ToastModule,
    ConfirmDialogModule,
    IftaLabelModule,
    LoginModalComponent
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']
})
export class Home implements OnInit {

  searchCity = signal<string>('');
  selectedCity = signal<ICity | null>(null);
  filteredCities = signal<ICity[]>([]);
  favorites = signal<ICity[]>([]);
  forecast = signal<IForecast[]>([]);
  isLoadingForecast = signal<boolean>(false);
  isCityFavorited = signal<boolean>(false);
  allCities: ICity[] = [];

  private cidadesService = inject(CidadesService);
  private favoriteService = inject(FavoriteService);
  public  authService = inject(AuthService);

  hasForecast = computed(() => this.forecast().length > 0);
  hasFavorites = computed(() => this.favorites().length > 0 && this.authService.getToken());
  selectedCityDisplay = computed(() => {
    const city = this.selectedCity();
    return city ? `${city.name}, ${city.state}` : '';
  });

  constructor() {

    effect(() => {
      const selected = this.selectedCity();
      const favs = this.favorites();
      if (selected) {
        const isFav = favs.some(c => c.id === selected.id);
        this.isCityFavorited.set(isFav);
      } else {
        this.isCityFavorited.set(false);
      }
    });

    effect(() => {
      const search = this.searchCity();
      if (search === '') {
        this.filteredCities.set([]);
      }
    });
  }

  ngOnInit() {
    this.loadCities();
    this.loadFavoritesFromApi();
  }

  private loadCities() {
    this.cidadesService.getCidadesIBGE().subscribe({
      next: (cities) => {
        this.allCities = cities;
      },
      error: (err) => {
        console.error('Erro ao carregar cidades do serviço:', err);
      }
    });
  }

  private loadFavoritesFromApi() {
    if(!this.authService.getToken()) return;

    this.favoriteService.getFavorite().subscribe({
      next: (response) => {

        const mappedCities: ICity[] = response.map(item => ({
          id: String(item.id),
          cidadeId: item.cidadeId,
          name: item.nome,
          state: item.state
        }));

        this.favorites.set(mappedCities);
      },
      error: (err) => console.error('Erro ao buscar favoritos da API', err)
    });
  }

  filterCities(event: AutoCompleteCompleteEvent) {
    const query = event.query.toLowerCase();
    const filtered = this.allCities
      .filter(city => city.name.toLowerCase().includes(query))
      .slice(0, 10);

    this.filteredCities.set(filtered);
  }

  selectCity(city: ICity) {
    this.selectedCity.set(city);
    this.searchCity.set(`${city.name}, ${city.state}`);
    this.filteredCities.set([]);

    this.cidadesService.getCity(city.name).subscribe({
      next: (response) => {
        this.loadForecast(response.cidadeId);
      }
    });

  }

  loadForecast(cidadeId: number) {
    this.isLoadingForecast.set(true);

    this.cidadesService.getForecast(cidadeId).subscribe({
      next: (response) => {
        this.forecast.set(response as IForecast[]);
        this.isLoadingForecast.set(false);
      }
    });

  }

  toggleFavorite() {
    const city = this.selectedCity();
    if (!city) return;

    const isFavorited = this.isCityFavorited();

    if (isFavorited) {
      this.removeFavorite(city);
    } else {
      this.addFavorite(city);
    }
  }

  addFavorite(city: ICity) {
    const currentFavs = this.favorites();
    const exists = currentFavs.some(c => c.id === city.id);

    if (exists) {
      return;
    };

    this.cidadesService.getCity(city.name).subscribe({
      next: (response) => {

        const paramtros: IFavorite = {
          cidadeId: Number(response.cidadeId),
          nome: city.name,
          state: city.state
        };

        this.favoriteService.addFavorite(paramtros).subscribe({
          next: () => {
            this.favorites.update(favs => [...favs, city]);
            this.isCityFavorited.set(true);
          }
        });
      }
    });


  }

  removeFavorite(city: ICity) {
    this.favoriteService.removeFavorite(city.id).subscribe({
      next: () => {
        this.favorites.update(favs => favs.filter(c => c.id !== city.id));
        this.isCityFavorited.set(false);
      }
    });
  }

  selectFavorite(city: ICity) {
    this.selectedCity.set(city);
    this.searchCity.set(`${city.name}, ${city.state}`);
    this.loadForecast(Number(city.cidadeId));
  }

}