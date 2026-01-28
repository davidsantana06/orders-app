import { Component, signal, viewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { TopBarComponent, SmartFilterComponent, OrderListComponent } from './components';
import { OrderFilter } from './models';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TopBarComponent, SmartFilterComponent, OrderListComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('orders-app-frontend');
  private readonly orderList = viewChild(OrderListComponent);

  onFilterChanged(filter: OrderFilter): void {
    this.orderList()?.loadOrders(filter);
  }
}
