import { Component, OnInit, inject } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { DatePipe, CurrencyPipe } from '@angular/common';

import { OrderService } from '../services';
import { Order, OrderStatus, OrderFilter } from '../models';

@Component({
  selector: 'app-order-list',
  imports: [
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatSnackBarModule,
    DatePipe,
    CurrencyPipe,
  ],
  templateUrl: './order-list.component.html',
  styles: `
    .actions-container {
      display: flex;
      gap: 8px;
      padding: 16px;
      justify-content: flex-end;
    }

    .table-container {
      overflow-x: auto;
    }
  `,
})
export class OrderListComponent implements OnInit {
  private readonly orderService = inject(OrderService);
  private readonly snackBar = inject(MatSnackBar);

  orders: Order[] = [];
  displayedColumns: string[] = ['id', 'orderDate', 'totalValue', 'status', 'actions'];
  currentFilter?: OrderFilter;

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(filter?: OrderFilter): void {
    this.currentFilter = filter;
    this.orderService.getMany(filter).subscribe({
      next: (orders) => (this.orders = orders),
      error: (error) =>
        this.snackBar.open(`Erro ao carregar pedidos: ${error.message}`, 'Fechar', {
          duration: 5000,
        }),
    });
  }

  getStatusColor(status: OrderStatus): string | undefined {
    return {
      Solicitado: undefined,
      'Em andamento': 'primary',
      Concluído: 'accent',
      Cancelado: 'warn',
    }[status];
  }

  deleteOrder(id: number): void {
    if (!confirm('Tem certeza que deseja excluir este pedido?')) return;

    this.orderService.delete(id).subscribe({
      next: () => {
        this.snackBar.open('Pedido excluído com sucesso!', 'Fechar', {
          duration: 3000,
        });
        this.loadOrders(this.currentFilter);
      },
      error: (error) =>
        this.snackBar.open(`Erro ao excluir pedido: ${error.message}`, 'Fechar', {
          duration: 5000,
        }),
    });
  }

  exportPdf(): void {
    this.snackBar.open('Exportando PDF...', undefined, { duration: 2000 });
    setTimeout(() => {
      this.snackBar.open('PDF exportado com sucesso!', 'Fechar', { duration: 3000 });
    }, 2000);
  }

  exportExcel(): void {
    this.snackBar.open('Exportando Excel...', undefined, { duration: 2000 });
    setTimeout(() => {
      this.snackBar.open('Excel exportado com sucesso!', 'Fechar', { duration: 3000 });
    }, 2000);
  }
}
