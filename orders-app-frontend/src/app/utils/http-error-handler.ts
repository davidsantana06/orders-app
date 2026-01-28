import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

export function handleHttpError(error: HttpErrorResponse) {
  const message = error.error?.message ?? error.message ?? 'Unexpected error occurred';
  console.error(message);
  return throwError(() => new Error(message));
}
