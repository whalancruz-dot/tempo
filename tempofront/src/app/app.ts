import { Component, inject, signal } from '@angular/core';
import {
  Router,
  RouterOutlet,
} from '@angular/router';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('frotas-motorista');

  public router = inject(Router);



}
