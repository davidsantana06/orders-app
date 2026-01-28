import { OrderStatus } from './order.model';

export interface CreateOrderDto {
  items: CreateOrderItemDto[];
}

export interface CreateOrderItemDto {
  make: string;
  model: string;
  year: number;
  quantity: number;
  unitPrice: number;
}

export interface UpdateOrderDto {
  status: OrderStatus;
}

export interface UpdateOrderItemDto {
  make: string;
  model: string;
  year: number;
  quantity: number;
  unitPrice: number;
}
