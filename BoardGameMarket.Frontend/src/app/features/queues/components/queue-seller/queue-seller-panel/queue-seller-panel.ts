import { Component, Input, ViewChild } from '@angular/core';
import { CdkDragDrop, CdkDropList, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { castTo } from '../../../../../core/utilities';
import { AsyncPipe, CommonModule, DatePipe, NgTemplateOutlet } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { QueueService } from '../../../queue.service';
import { QueueEntry, QueuePageResponse } from '../../../models/quque-entry.model';
import { Observable, startWith, Subject, switchMap } from 'rxjs';

@Component({
  selector: 'bgm-queue-seller-panel',
  imports: [DatePipe, DragDropModule, NgTemplateOutlet, MatCardModule, AsyncPipe, CommonModule],
  templateUrl: './queue-seller-panel.html',
  styleUrl: './queue-seller-panel.css',
})
export class QueueSellerPanel {
  @Input({ required: true }) listingsId!: string;

  private reload$ = new Subject<void>();

  readonly queues$ = this.reload$.pipe(
    startWith(void 0),
    switchMap(() => this.queueService.getQueues(this.listingsId)),
  );

  @ViewChild('priorityQueue') priorityContainer?: CdkDropList;
  @ViewChild('timeQueue') timeContainer?: CdkDropList;

  constructor(private queueService: QueueService) {}

  ngOnInit(): void {
    this.reload$.next();
  }

  toQueueEntry(item: any): QueueEntry {
    return castTo(item);
  }

  priorityChangeRequest<QueueEntry>($event: CdkDragDrop<QueueEntry>) {
    let requestedIndex = $event.currentIndex;
    const data = this.toQueueEntry($event.item.data);
    // Removal
    if ($event.container !== this.priorityContainer) {
      if ($event.previousContainer === this.priorityContainer) {
        requestedIndex = -1;
      }
    } else if (
      // Same entry
      $event.previousContainer === this.priorityContainer &&
      $event.currentIndex === $event.previousIndex
    ) {
      return;
    }

    this.queueService.updatePriority(this.listingsId, data.id, requestedIndex).subscribe({
      next: () => {
        this.reload$.next();
      },
      error: (value) => {
        console.log(value);
      },
    });
  }
}
