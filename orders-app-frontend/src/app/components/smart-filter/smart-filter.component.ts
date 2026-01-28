import { Component, output, inject, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';

import { OrderItemService } from '../../services';
import { OrderFilter } from '../../models';

@Component({
  selector: 'app-smart-filter',
  imports: [
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    FormsModule,
  ],
  templateUrl: './smart-filter.component.html',
  styles: `
    .filter-header {
      display: flex;
      align-items: center;
      gap: 12px;
      color: rgba(0, 0, 0, 0.87);
    }

    .header-icon {
      font-size: 28px;
      width: 28px;
      height: 28px;
      color: #1976d2;
    }

    .filter-content {
      display: flex;
      gap: 20px;
      align-items: flex-start;
      flex-wrap: wrap;
      padding-top: 24px !important;
      padding-bottom: 8px !important;
    }

    mat-form-field {
      flex: 1;
      min-width: 200px;
    }

    .filter-button {
      height: 56px;
      min-width: 120px;
      font-weight: 500;
    }
  `,
})
export class SmartFilterComponent implements OnInit {
  private readonly itemService = inject(OrderItemService);

  filterChanged = output<OrderFilter>();
  filterCleared = output<void>();

  makeOptions: string[] = [];
  modelOptions: string[] = [];
  yearOptions: number[] = [];

  selectedMake?: string;
  selectedModel?: string;
  selectedYear?: number;

  ngOnInit(): void {
    this.loadInitialData();
  }

  public loadInitialData(): void {
    this.loadMakes();
  }

  private loadMakes(): void {
    this.itemService.getMakes().subscribe({
      next: (makes) => {
        this.makeOptions = makes;
      },
      error: (error) => console.error('Error while loading makes:', error),
    });
  }

  onMakeChange(): void {
    this.selectedModel = undefined;
    this.selectedYear = undefined;
    this.modelOptions = [];
    this.yearOptions = [];

    if (!this.selectedMake) return;

    this.itemService.getModels(this.selectedMake).subscribe({
      next: (models) => {
        this.modelOptions = models;
      },
      error: (error) => console.error('Error while loading models:', error),
    });
  }

  onModelChange(): void {
    this.selectedYear = undefined;
    this.yearOptions = [];

    if (!this.selectedMake || !this.selectedModel) return;

    this.itemService.getYears(this.selectedMake, this.selectedModel).subscribe({
      next: (years) => {
        this.yearOptions = years;
      },
      error: (error) => console.error('Error while loading years:', error),
    });
  }

  onFilter(): void {
    if (!this.isFilterValid) {
      return;
    }

    const filter: OrderFilter = {
      make: this.selectedMake,
      model: this.selectedModel,
      year: this.selectedYear,
    };

    this.filterChanged.emit(filter);
  }

  clearFilters(): void {
    this.selectedMake = undefined;
    this.selectedModel = undefined;
    this.selectedYear = undefined;

    this.modelOptions = [];
    this.yearOptions = [];

    this.filterCleared.emit();
  }

  get isFilterValid(): boolean {
    return !!(this.selectedMake && this.selectedModel && this.selectedYear);
  }

  get isModelDisabled(): boolean {
    return !this.selectedMake || this.modelOptions.length === 0;
  }

  get isYearDisabled(): boolean {
    return !this.selectedModel || this.yearOptions.length === 0;
  }
}
