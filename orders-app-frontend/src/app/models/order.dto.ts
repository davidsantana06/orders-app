import { CreateOrderItemDto } from './order-item.dto';

export interface CreateOrderDto {
  items: CreateOrderItemDto[];
}
