import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-top-bar',
  imports: [MatToolbarModule, MatIconModule],
  template: `
    <mat-toolbar color="primary">
      <mat-icon>local_mall</mat-icon>
      <span style="margin-left: 12px;">OrdersApp</span>
    </mat-toolbar>
  `,
  styles: ``,
})
export class TopBarComponent {}
