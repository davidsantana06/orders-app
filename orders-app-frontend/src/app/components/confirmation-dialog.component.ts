import { Component, inject } from '@angular/core';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface ConfirmationDialogData {
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  confirmColor?: 'primary' | 'accent' | 'warn';
}

@Component({
  selector: 'app-confirmation-dialog',
  imports: [MatDialogModule, MatButtonModule, MatIconModule],
  template: `
    <h2 mat-dialog-title>
      <mat-icon style="vertical-align: middle; margin-right: 8px;">{{ getIcon() }}</mat-icon>
      {{ data.title }}
    </h2>

    <mat-dialog-content>
      <p style="margin: 0; font-size: 1rem; line-height: 1.5;">{{ data.message }}</p>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      @if (data.cancelText) {
        <button mat-button (click)="onCancel()">
          <mat-icon>close</mat-icon>
          {{ data.cancelText }}
        </button>
      }
      <button mat-raised-button [color]="data.confirmColor || 'warn'" (click)="onConfirm()">
        <mat-icon>check</mat-icon>
        {{ data.confirmText || 'Confirmar' }}
      </button>
    </mat-dialog-actions>
  `,
  styles: `
    mat-dialog-content {
      padding: 20px 24px;
      min-width: 300px;
    }

    mat-dialog-actions {
      padding: 16px 24px;
      gap: 8px;
    }

    h2 {
      display: flex;
      align-items: center;
      margin: 0;
    }
  `,
})
export class ConfirmationDialogComponent {
  protected readonly dialogRef = inject(MatDialogRef<ConfirmationDialogComponent>);
  protected readonly data = inject<ConfirmationDialogData>(MAT_DIALOG_DATA);

  getIcon(): string {
    return {
      primary: 'help_outline',
      accent: 'info',
      warn: 'warning',
    }[this.data.confirmColor!];
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
}
