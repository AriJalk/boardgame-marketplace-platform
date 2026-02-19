import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { CdkDragDrop, CdkDropList, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { AsyncPipe, CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { QueueService } from '../../../queue.service';
import { BehaviorSubject, startWith, Subject, switchMap } from 'rxjs';
import { QueueEntryCard } from '../queue-entry-card/queue-entry-card';
import {
  QueueEntry,
  QueuePageResponse,
  UpdatePriorityRequest,
} from '../../../models/quque-entry.model';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CdkAriaLive } from '@angular/cdk/a11y';
import { TimeQueueEntryTable } from '../time-queue-entry-table/time-queue-entry-table';
import { PriorityQueueEntryTable } from '../priority-queue-entry-table/priority-queue-entry-table';

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
  ],
  templateUrl: './queue-seller-panel.html',
  styleUrl: './queue-seller-panel.css',
})
export class QueueSellerPanel implements OnInit {
  @Input({ required: true }) listingsId!: string;

  private queuesBehaviorSubject = new BehaviorSubject<QueuePageResponse | null>(null);

  queues$ = this.queuesBehaviorSubject.asObservable();

  constructor(private queueService: QueueService) {}

  ngOnInit(): void {
    this.loadQueues();
  }

  private loadQueues() {
    this.queueService.getQueues(this.listingsId).subscribe({
      next: (data) => {
        this.queuesBehaviorSubject.next(data);
        // console.log("QUEUEDATA", data);
      },
    });
  }

  onPriorityUpdateRequested($event: UpdatePriorityRequest) {
    this.subscribeAndReloadUpdate($event.queueEntry, $event.requestedIndex);
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
    this.subscribeAndReloadUpdate($event, null);
  }

  private subscribeAndReloadUpdate(queueEntry: QueueEntry, requestedIndex: number | null) {
    this.queueService.updatePriority(this.listingsId, queueEntry.id, requestedIndex).subscribe({
      next: () => {
        this.loadQueues();
      },
      error: (value) => {
        console.log(value);
      },
    });
  }
}
