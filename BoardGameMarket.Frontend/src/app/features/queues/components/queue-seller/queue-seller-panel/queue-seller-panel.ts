import { Component, Input, ViewChild } from '@angular/core';
import { CdkDragDrop, CdkDropList, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { AsyncPipe, CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { QueueService } from '../../../queue.service';
import { startWith, Subject, switchMap } from 'rxjs';
import { QueueEntryCard } from '../queue-entry-card/queue-entry-card';
import { QueueEntry } from '../../../models/quque-entry.model';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'bgm-queue-seller-panel',
  imports: [
    DragDropModule,
    MatCardModule,
    AsyncPipe,
    CommonModule,
    QueueEntryCard,
    MatButtonModule,
  ],
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
  @ViewChild('queueByPriority') priorityContainer?: CdkDropList;

  constructor(private queueService: QueueService) {}

  ngOnInit(): void {
    this.reload$.next();
  }

  priorityChangeRequest($event: CdkDragDrop<QueueEntry[]>) {
    let requestedIndex = $event.currentIndex;
    const data = $event.item.data as QueueEntry;
    if ($event.currentIndex == $event.previousIndex) {
      return;
    }
    this.subscribeAndReloadUpdate(data, $event.currentIndex);
  }

  onRemovePriority($event: QueueEntry) {
    this.subscribeAndReloadUpdate($event, null);
  }

  private subscribeAndReloadUpdate(queueEntry: QueueEntry, requestedIndex: number | null) {
    this.queueService.updatePriority(this.listingsId, queueEntry.id, requestedIndex).subscribe({
      next: () => {
        this.reload$.next();
      },
      error: (value) => {
        console.log(value);
      },
    });
  }
}
