import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { OrderItem, CreateOrderItemDto, UpdateOrderItemDto } from '../models';
import { handleHttpError } from '../utils/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class OrderItemService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/orderitems`;

  getMany(): Observable<OrderItem[]> {
    return this.http.get<OrderItem[]>(this.apiUrl)
      .pipe(catchError(handleHttpError));
  }

  getById(id: number): Observable<OrderItem> {
    return this.http.get<OrderItem>(`${this.apiUrl}/${id}`)
      .pipe(catchError(handleHttpError));
  }

  create(dto: CreateOrderItemDto, orderId: number): Observable<OrderItem> {
    const params = new HttpParams().set('orderId', orderId.toString());
    
    return this.http.post<OrderItem>(this.apiUrl, dto, { params })
      .pipe(catchError(handleHttpError));
  }

  update(id: number, dto: UpdateOrderItemDto): Observable<OrderItem> {
    return this.http.put<OrderItem>(`${this.apiUrl}/${id}`, dto)
      .pipe(catchError(handleHttpError));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(catchError(handleHttpError));
  }

  getMakes(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/makes`)
      .pipe(catchError(handleHttpError));
  }

  getModels(make: string): Observable<string[]> {
    const encodedMake = encodeURIComponent(make);
    
    return this.http.get<string[]>(`${this.apiUrl}/${encodedMake}/models`)
      .pipe(catchError(handleHttpError));
  }

  getYears(make: string, model: string): Observable<number[]> {
    const encodedMake = encodeURIComponent(make);
    const encodedModel = encodeURIComponent(model);
    
    return this.http.get<number[]>(`${this.apiUrl}/${encodedMake}/${encodedModel}/years`)
      .pipe(catchError(handleHttpError));
  }

  calculateSubTotal(item: OrderItem): number {
    return item.quantity * item.unitPrice;
  }
}
