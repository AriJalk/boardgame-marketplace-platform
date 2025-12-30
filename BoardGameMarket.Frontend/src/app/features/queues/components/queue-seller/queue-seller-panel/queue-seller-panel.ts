import { Component, Input, ViewChild } from '@angular/core';
import { CdkDragDrop, CdkDropList, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { castTo } from '../../../../../core/utilities';
import { DatePipe, NgTemplateOutlet } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { QueueService } from '../../../queue.service';
import { QueueEntry } from '../../../models/quque-entry.model';

const PRIORITY_GAP = 1000;

@Component({
  selector: 'bgm-queue-seller-panel',
  imports: [DatePipe, DragDropModule, NgTemplateOutlet, MatCardModule],
  templateUrl: './queue-seller-panel.html',
  styleUrl: './queue-seller-panel.css',
})
export class QueueSellerPanel {
  @Input({ required: true }) listingsId!: string;

  timeQueueEntries!: QueueEntry[];
  priorityQueueEntries: QueueEntry[] = [];

  @ViewChild('priorityQueue') priorityContainer?: CdkDropList;

  constructor(private queueService: QueueService) {}

  ngOnInit(): void {
    this.loadQueues();
  }

  loadQueues(): void {
    this.queueService.getQueue(this.listingsId).subscribe({
      next: (value) => {
        this.timeQueueEntries = value.timeQueue;
        this.priorityQueueEntries = value.priorityQueue;
      },
    });
  }

  toQueueEntry(item: any): QueueEntry {
    return castTo(item);
  }

  priorityChangeRequest<QueueEntry>($event: CdkDragDrop<QueueEntry>) {
    const requestedIndex = $event.currentIndex;
    const data = this.toQueueEntry($event.item.data);
    if (
      $event.previousContainer === this.priorityContainer &&
      $event.currentIndex === $event.previousIndex
    ) {
      return;
    }

    this.queueService.updatePriority(data.id, requestedIndex).subscribe({
      next: (value) => {
        if (value.isSuccessful) {
          // this.loadQueues();
          if (this.priorityQueueEntries.find((v) => v.id == data.id)) {
            moveItemInArray(this.priorityQueueEntries, $event.previousIndex, $event.currentIndex);
          } else {
            this.priorityQueueEntries.splice($event.currentIndex, 0, data);
          }
        }
      },
    });
  }
}
