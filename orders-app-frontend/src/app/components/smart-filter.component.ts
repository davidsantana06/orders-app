import { Component, output, inject, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';

import { OrderItemService } from '../services';
import { OrderFilter } from '../models';

@Component({
  selector: 'app-smart-filter',
  imports: [MatCardModule, MatFormFieldModule, MatSelectModule, MatButtonModule, FormsModule],
  templateUrl: './smart-filter.component.html',
  styles: ``,
})
export class SmartFilterComponent implements OnInit {
  private readonly itemService = inject(OrderItemService);

  filterChanged = output<OrderFilter>();

  makeOptions: string[] = [];
  modelOptions: string[] = [];
  yearOptions: number[] = [];

  selectedMake?: string;
  selectedModel?: string;
  selectedYear?: number;

  ngOnInit(): void {
    this.loadMakes();
  }

  private loadMakes(): void {
    this.itemService.getMakes().subscribe({
      next: (makes) => (this.makeOptions = makes),
      error: (error) => console.error('Error while loading makes:', error.message),
    });
  }

  onMakeChange(): void {
    this.selectedModel = undefined;
    this.selectedYear = undefined;
    this.modelOptions = [];
    this.yearOptions = [];

    if (!this.selectedMake) return;

    this.itemService.getModels(this.selectedMake).subscribe({
      next: (models) => (this.modelOptions = models),
      error: (error) => console.error('Error while loading models:', error.message),
    });
  }

  onModelChange(): void {
    this.selectedYear = undefined;
    this.yearOptions = [];

    if (!this.selectedMake || !this.selectedModel) return;

    this.itemService.getYears(this.selectedMake, this.selectedModel).subscribe({
      next: (years) => (this.yearOptions = years),
      error: (error) => console.error('Error while loading years:', error.message),
    });
  }

  onFilter(): void {
    if (!this.isFilterValid) return;

    this.filterChanged.emit({
      make: this.selectedMake,
      model: this.selectedModel,
      year: this.selectedYear,
    });
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
