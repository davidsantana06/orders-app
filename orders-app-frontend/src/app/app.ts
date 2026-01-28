import { Component, signal, viewChild, OnInit, afterNextRender } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { TopBarComponent, SmartFilterComponent, OrderListComponent } from './components';
import { OrderFilter } from './models';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TopBarComponent, SmartFilterComponent, OrderListComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App implements OnInit {
  protected readonly title = signal('orders-app-frontend');
  private readonly orderList = viewChild(OrderListComponent);

  constructor() {
    afterNextRender(() => {
      const listComponent = this.orderList();
      if (listComponent) {
        listComponent.loadOrders();
      }
    });
  }

  ngOnInit(): void {}

  onFilterChanged(filter: OrderFilter): void {
    const listComponent = this.orderList();
    if (listComponent) {
      listComponent.loadOrders(filter);
    }
  }

  onFilterCleared(): void {
    const listComponent = this.orderList();
    if (listComponent) {
      listComponent.loadOrders();
    }
  }
}
