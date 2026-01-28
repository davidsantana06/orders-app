import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { Order, CreateOrderDto, UpdateOrderDto, OrderFilter } from '../models';
import { handleHttpError } from '../utils/http-error-handler';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/orders`;

  getMany(filter: OrderFilter = {}): Observable<Order[]> {
    let params = new HttpParams();
    const filterParams = this.buildFilterQuery(filter);

    Object.keys(filterParams).forEach((key) => {
      params = params.set(key, filterParams[key]);
    });

    return this.http.get<Order[]>(this.apiUrl, { params }).pipe(catchError(handleHttpError));
  }

  getById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${id}`).pipe(catchError(handleHttpError));
  }

  create(dto: CreateOrderDto): Observable<Order> {
    return this.http.post<Order>(this.apiUrl, dto).pipe(catchError(handleHttpError));
  }

  update(id: number, dto: UpdateOrderDto): Observable<Order> {
    return this.http.put<Order>(`${this.apiUrl}/${id}`, dto).pipe(catchError(handleHttpError));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(catchError(handleHttpError));
  }

  private buildFilterQuery(filter: OrderFilter): Record<string, string> {
    const params: Record<string, string> = {};

    if (filter.make) params['make'] = filter.make;
    if (filter.model) params['model'] = filter.model;
    if (filter.year !== undefined && filter.year !== null) {
      params['year'] = filter.year.toString();
    }

    return params;
  }
}
