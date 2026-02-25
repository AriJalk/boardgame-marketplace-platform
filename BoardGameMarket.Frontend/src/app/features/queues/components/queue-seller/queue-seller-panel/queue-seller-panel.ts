import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AsyncPipe, CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { QueueService } from '../../../queue.service';
import { BehaviorSubject, startWith, Subject, switchMap } from 'rxjs';
import {
  QueueEntry,
  QueuePageResponse,
  UpdatePriorityRequest,
} from '../../../models/quque-entry.model';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TimeQueueEntryTable } from '../time-queue-entry-table/time-queue-entry-table';
import { PriorityQueueEntryTable } from '../priority-queue-entry-table/priority-queue-entry-table';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'bgm-queue-seller-panel',
  imports: [
    DragDropModule,
    MatCardModule,
    AsyncPipe,
    CommonModule,
    MatButtonModule,
    MatIconModule,
    TimeQueueEntryTable,
    PriorityQueueEntryTable,
    RouterLink,
  ],
  templateUrl: './queue-seller-panel.html',
  styleUrl: './queue-seller-panel.css',
})
export class QueueSellerPanel implements OnInit {
  @Input({ required: true }) listingsId!: string;

  private queuesBehaviorSubject = new BehaviorSubject<QueuePageResponse | null>(null);
  queues$ = this.queuesBehaviorSubject.asObservable();

  topPriorityEntry?: QueueEntry;

  constructor(private queueService: QueueService) {}

  ngOnInit(): void {
    this.loadQueues();
  }

  private loadQueues() {
    this.queueService.getQueues(this.listingsId).subscribe({
      next: (data) => {
        this.queuesBehaviorSubject.next(data);
        // console.log("QUEUEDATA", data);
        if (data.queueByPriority.length > 0) {
          this.topPriorityEntry = data.queueByPriority[0];
        } else if (data.queueByTime.length > 0) {
          this.topPriorityEntry = data.queueByTime[0];
        }
      },
    });
  }

  onDeleteRequested($event: QueueEntry) {
    // console.log($event);
    this.queueService.unsubscribeFromListing(this.listingsId, $event.id).subscribe({
      next: (data) => {
        this.topPriorityEntry = undefined;
        this.loadQueues();
      },
    });
  }

  onPriorityUpdateRequested($event: UpdatePriorityRequest) {
    this.queueService
      .updatePriority(this.listingsId, $event.queueEntry.id, $event.requestedIndex)
      .subscribe({
        next: (data) => {
          this.loadQueues();
        },
      });
  }

  onPriorityAddRequested(queueEntry: QueueEntry) {
    this.queueService.addPriority(this.listingsId, queueEntry.id).subscribe({
      next: (data) => {
        // console.log(data);
        this.loadQueues();
      },
    });
  }

  onPriorityRemoveRequested($event: QueueEntry) {
    this.queueService.removePriority(this.listingsId, $event.id).subscribe({
      next: (data) => {
        this.loadQueues();
      },
    });
  }
}
