import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { TopBarComponent, SmartFilterComponent } from './components';
import { OrderFilter } from './models';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TopBarComponent, SmartFilterComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('orders-app-frontend');

  onFilterChanged(filter: OrderFilter): void {
    console.log('Filter applied:', filter);
  }
}
