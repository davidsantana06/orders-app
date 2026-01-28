import { OrderItem } from './order-item.model';

export interface Order {
  id: number;
  totalValue: number;
  status: OrderStatus;
  createdAt: string;
  updatedAt: string;
  orderItems: OrderItem[];
}

export type OrderStatus = 'Solicitado' | 'Em andamento' | 'Concluído' | 'Cancelado';
