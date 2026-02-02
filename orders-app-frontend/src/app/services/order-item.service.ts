import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { OrderItem } from '../models';
import { handleHttpError } from '../utils/http-error-handler';

@Injectable({
  providedIn: 'root',
})
export class OrderItemService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/orderitems`;

  getMakes(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/makes`).pipe(catchError(handleHttpError));
  }

  getModels(make: string): Observable<string[]> {
    const encodedMake = encodeURIComponent(make);

    return this.http
      .get<string[]>(`${this.apiUrl}/${encodedMake}/models`)
      .pipe(catchError(handleHttpError));
  }

  getYears(make: string, model: string): Observable<number[]> {
    const encodedMake = encodeURIComponent(make);
    const encodedModel = encodeURIComponent(model);

    return this.http
      .get<number[]>(`${this.apiUrl}/${encodedMake}/${encodedModel}/years`)
      .pipe(catchError(handleHttpError));
  }
}
