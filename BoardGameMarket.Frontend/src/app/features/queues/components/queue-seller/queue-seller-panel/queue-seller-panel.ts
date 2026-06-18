import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AsyncPipe, CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { QueueService } from '../../../queue.service';
import { BehaviorSubject, startWith, Subject, switchMap } from 'rxjs';
import {
  QueueEntry,
  QueuePageResponse,
  UpdatePriorityRequest,
} from '../../../models/queue-entry.model';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TimeQueueEntryTable } from '../time-queue-entry-table/time-queue-entry-table';
import { PriorityQueueEntryTable } from '../priority-queue-entry-table/priority-queue-entry-table';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../../core/auth/auth.service';

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
  @Input({ required: true }) listingId!: string;
  @Input() isLocked: boolean = false;

  @Output() listingStatusChanged = new EventEmitter();

  private queuesBehaviorSubject = new BehaviorSubject<QueuePageResponse | null>(null);
  queues$ = this.queuesBehaviorSubject.asObservable();

  topPriorityEntry?: QueueEntry;

  constructor(
    private queueService: QueueService,
    private router: Router,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.loadQueues();
  }

  private loadQueues() {
    this.authService
      .withAuthCall((header) => this.queueService.getQueues(this.listingId, header))
      .subscribe({
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
    this.authService
      .withAuthCall((header) =>
        this.queueService.unsubscribeFromListing(this.listingId, $event.id, header),
      )
      .subscribe({
        next: (data) => {
          this.topPriorityEntry = undefined;
          this.loadQueues();
        },
      });
  }

  onPriorityUpdateRequested($event: UpdatePriorityRequest) {
    this.authService
      .withAuthCall((header) =>
        this.queueService.updatePriority(
          this.listingId,
          $event.queueEntry.id,
          $event.requestedIndex,
          header,
        ),
      )
      .subscribe({
        next: (data) => {
          this.loadQueues();
        },
      });
  }

  onPriorityAddRequested(queueEntry: QueueEntry) {
    this.authService
      .withAuthCall((header) =>
        this.queueService.addPriority(this.listingId, queueEntry.id, header),
      )
      .subscribe({
        next: (data) => {
          // console.log(data);
          this.loadQueues();
        },
      });
  }

  onPriorityRemoveRequested($event: QueueEntry) {
    this.authService
      .withAuthCall((header) => this.queueService.removePriority(this.listingId, $event.id, header))
      .subscribe({
        next: (data) => {
          this.loadQueues();
        },
      });
  }

  onEntryReserveRequested() {
    const entryId = this.topPriorityEntry?.id;
    if (!entryId) {
      return;
    }
    this.authService
      .withAuthCall((header) =>
        this.queueService.reserveQueueEntry(this.listingId, entryId, header),
      )
      .subscribe({
        next: (data) => {
          this.listingStatusChanged.emit();
        },
        error: (err) => {
          console.log("Can't reserve", err);
        },
      });
  }
}
