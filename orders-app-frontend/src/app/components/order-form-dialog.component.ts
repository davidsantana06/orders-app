import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormArray, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar } from '@angular/material/snack-bar';

import { OrderService } from '../services';
import { CreateOrderDto, CreateOrderItemDto } from '../models';

@Component({
  selector: 'app-order-form-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
  ],
  templateUrl: './order-form-dialog.component.html',
  styles: `
    .items-container {
      display: flex;
      flex-direction: column;
      gap: 16px;
      margin-top: 16px;
    }

    .item-card {
      display: flex;
      gap: 12px;
      align-items: flex-start;
      flex-wrap: wrap;
    }

    .item-card mat-form-field {
      flex: 1;
      min-width: 150px;
    }
  `,
})
export class OrderFormDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly dialogRef = inject(MatDialogRef<OrderFormDialogComponent>);
  private readonly orderService = inject(OrderService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly data = inject(MAT_DIALOG_DATA, { optional: true });

  orderForm!: FormGroup;

  ngOnInit(): void {
    this.orderForm = this.fb.group({
      items: this.fb.array([this.createItemFormGroup()]),
    });
  }

  get items(): FormArray {
    return this.orderForm.get('items') as FormArray;
  }

  createItemFormGroup(): FormGroup {
    return this.fb.group({
      make: ['', Validators.required],
      model: ['', Validators.required],
      year: ['', [Validators.required, Validators.min(1900), Validators.max(2100)]],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: ['', [Validators.required, Validators.min(0)]],
    });
  }

  addItem(): void {
    this.items.push(this.createItemFormGroup());
  }

  removeItem(index: number): void {
    if (!this.items.length)
      this.snackBar.open('A ordem deve ter pelo menos um item!', 'Fechar', {
        duration: 3000,
      });
    this.items.removeAt(index);
  }

  cancel(): void {
    this.dialogRef.close();
  }

  save(): void {
    if (this.orderForm.invalid) {
      this.orderForm.markAllAsTouched();
      this.snackBar.open('Preencha todos os campos obrigatórios!', 'Fechar', {
        duration: 3000,
      });
      return;
    }

    const items: CreateOrderItemDto[] = this.items.value.map((item: any) => ({
      make: item.make,
      model: item.model,
      year: Number(item.year),
      quantity: Number(item.quantity),
      unitPrice: Number(item.unitPrice),
    }));

    const createOrderDto: CreateOrderDto = {
      items,
    };

    this.orderService.create(createOrderDto).subscribe({
      next: (order) => {
        this.snackBar.open('Ordem criada com sucesso!', 'Fechar', {
          duration: 3000,
        });
        this.dialogRef.close(order);
      },
      error: (error) =>
        this.snackBar.open(`Erro ao criar ordem: ${error.message}`, 'Fechar', {
          duration: 5000,
        }),
    });
  }

  getErrorMessage(field: string, itemIndex?: number): string {
    const control =
      itemIndex !== undefined ? this.items.at(itemIndex).get(field) : this.orderForm.get(field);

    if (!control) return '';

    if (control.hasError('required')) return 'Campo obrigatório';
    if (control.hasError('min')) return `Valor mínimo: ${control.errors?.['min'].min}`;
    if (control.hasError('max')) return `Valor máximo: ${control.errors?.['max'].max}`;

    return '';
  }
}
