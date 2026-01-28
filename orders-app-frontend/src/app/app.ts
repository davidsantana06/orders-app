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
    console.log('App: Constructor chamado');
    // Carregamento inicial após a renderização
    afterNextRender(() => {
      console.log('App: afterNextRender - viewChild disponível?', !!this.orderList());
      const listComponent = this.orderList();
      if (listComponent) {
        console.log('App: Chamando loadOrders() inicial');
        listComponent.loadOrders();
      } else {
        console.error('App: OrderListComponent não encontrado no viewChild!');
      }
    });
  }

  ngOnInit(): void {
    console.log('App: ngOnInit chamado');
  }

  onFilterChanged(filter: OrderFilter): void {
    console.log('App: onFilterChanged chamado com filtro:', filter);
    const listComponent = this.orderList();
    if (listComponent) {
      listComponent.loadOrders(filter);
    } else {
      console.error('App: OrderListComponent não disponível para aplicar filtro!');
    }
  }

  onFilterCleared(): void {
    console.log('App: onFilterCleared chamado - carregando todos os pedidos');
    const listComponent = this.orderList();
    if (listComponent) {
      listComponent.loadOrders();
    } else {
      console.error('App: OrderListComponent não disponível para limpar filtro!');
    }
  }
}
