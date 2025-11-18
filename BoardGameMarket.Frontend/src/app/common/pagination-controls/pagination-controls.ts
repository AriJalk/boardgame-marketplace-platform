import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule, MatMiniFabButton } from "@angular/material/button";

@Component({
  selector: 'bgm-pagination-controls',
  imports: [MatIconModule, MatMiniFabButton],
  templateUrl: './pagination-controls.html',
  styleUrl: './pagination-controls.css',
})
export class PaginationControls {
  @Input() hasPrevious = false;
  @Input() hasNext = false;

  @Output() previous = new EventEmitter<void>();
  @Output() next = new EventEmitter<void>();
}
