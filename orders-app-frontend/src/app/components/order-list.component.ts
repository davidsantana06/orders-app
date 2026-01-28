import { Component, OnInit, inject, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { DatePipe, CurrencyPipe } from '@angular/common';

import { OrderService } from '../services';
import { Order, OrderStatus, OrderFilter, OrderItem } from '../models';
import { OrderFormDialogComponent } from './order-form-dialog.component';
import { ConfirmationDialogComponent, ConfirmationDialogData } from './confirmation-dialog.component';

@Component({
  selector: 'app-order-list',
  imports: [
    MatExpansionModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatDividerModule,
    MatSnackBarModule,
    DatePipe,
    CurrencyPipe,
  ],
  templateUrl: './order-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styles: `
    .actions-container {
      display: flex;
      gap: 8px;
      padding: 16px;
      justify-content: flex-end;
    }

    .order-header {
      display: flex;
      align-items: center;
      gap: 16px;
      width: 100%;
    }

    .order-info {
      display: flex;
      gap: 24px;
      flex: 1;
      align-items: center;
    }

    .info-item {
      display: flex;
      flex-direction: column;
    }

    .info-label {
      font-size: 0.75rem;
      color: rgba(0, 0, 0, 0.6);
      font-weight: 500;
      text-transform: uppercase;
    }

    .info-value {
      font-size: 0.95rem;
      font-weight: 500;
      margin-top: 2px;
    }

    .items-table {
      width: 100%;
      border-collapse: collapse;
      margin: 16px 0;
    }

    .items-table th {
      text-align: left;
      padding: 12px;
      background-color: #f5f5f5;
      font-weight: 600;
      font-size: 0.875rem;
      color: rgba(0, 0, 0, 0.87);
      border-bottom: 2px solid #e0e0e0;
    }

    .items-table td {
      padding: 12px;
      border-bottom: 1px solid #e0e0e0;
      font-size: 0.875rem;
    }

    .items-table tr:hover {
      background-color: #fafafa;
    }

    .panel-actions {
      display: flex;
      justify-content: flex-end;
      padding: 16px;
      background-color: #fafafa;
      border-top: 1px solid #e0e0e0;
    }

    .empty-state {
      text-align: center;
      padding: 48px;
      color: rgba(0, 0, 0, 0.6);
    }

    mat-expansion-panel {
      margin-bottom: 16px;
    }

    mat-expansion-panel-header {
      min-height: 80px !important;
      height: auto !important;
      padding: 16px 24px !important;
    }

    mat-expansion-panel-header.mat-expanded {
      min-height: 80px !important;
    }
  `,
})
export class OrderListComponent implements OnInit {
  private readonly orderService = inject(OrderService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);
  private readonly cdr = inject(ChangeDetectorRef);

  orders: Order[] = [];
  currentFilter?: OrderFilter;

  ngOnInit(): void {
    console.log('OrderListComponent: ngOnInit chamado');
    this.loadOrders();
  }

  loadOrders(filter?: OrderFilter): void {
    console.log('OrderListComponent: loadOrders chamado com filtro:', filter);
    this.currentFilter = filter;
    this.orderService.getMany(filter).subscribe({
      next: (orders) => {
        console.log('OrderListComponent: Pedidos recebidos:', orders.length);
        this.orders = orders;
        // CRÍTICO: Com OnPush, precisamos marcar para verificação manual
        this.cdr.markForCheck();
      },
      error: (error) => {
        console.error('OrderListComponent: Erro ao carregar pedidos:', error);
        this.snackBar.open(`Erro ao carregar pedidos: ${error.message}`, 'Fechar', {
          duration: 5000,
        });
      },
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

  getItemSubtotal(item: OrderItem): number {
    return item.quantity * item.unitPrice;
  }

  deleteOrder(id: number): void {
    console.log('OrderListComponent: Solicitando confirmação para excluir pedido ID:', id);
    
    const dialogData: ConfirmationDialogData = {
      title: 'Confirmar Exclusão',
      message: 'Tem certeza que deseja excluir este pedido? Esta ação não pode ser desfeita.',
      confirmText: 'Excluir',
      cancelText: 'Cancelar',
      confirmColor: 'warn',
    };

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: dialogData,
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        console.log('OrderListComponent: Exclusão cancelada pelo usuário');
        return;
      }

      console.log('OrderListComponent: Excluindo pedido ID:', id);
      this.orderService.delete(id).subscribe({
        next: () => {
          console.log('OrderListComponent: Pedido excluído, atualizando lista');
          this.snackBar.open('Pedido excluído com sucesso!', 'Fechar', {
            duration: 3000,
          });
          this.loadOrders(this.currentFilter);
        },
        error: (error) => {
          console.error('OrderListComponent: Erro ao excluir pedido:', error);
          this.snackBar.open(`Erro ao excluir pedido: ${error.message}`, 'Fechar', {
            duration: 5000,
          });
        },
      });
    });
  }

  openCreateDialog(): void {
    console.log('OrderListComponent: Abrindo dialog de criação');
    const dialogRef = this.dialog.open(OrderFormDialogComponent, {
      width: '800px',
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('OrderListComponent: Dialog fechado com resultado:', result);
      if (result) {
        console.log('OrderListComponent: Pedido criado, atualizando lista');
        this.loadOrders(this.currentFilter);
      }
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
