import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { QueueEntry, QueuePageResponse } from './models/quque-entry.model';

@Injectable({
  providedIn: 'root',
})
export class QueueService {
  constructor(private httpClient: HttpClient) {}

  getQueue(listingId: string): Observable<QueuePageResponse> {
    const date = new Date();
    const timeQueueEntries = [
      {
        id: crypto.randomUUID(),
        buyer: { displayName: 'buyer1', id: 'test1Id' },
        createdAt: date,
        status: 'QUEUE',
      },
      {
        id: crypto.randomUUID(),
        buyer: { displayName: 'buyer2', id: 'test1Id' },
        createdAt: this.addDays(date, 1),
        status: 'QUEUE',
      },
      {
        id: crypto.randomUUID(),
        buyer: { displayName: 'buyer3', id: 'test1Id' },
        createdAt: this.addDays(date, 2),
        status: 'QUEUE',
      },
      {
        id: crypto.randomUUID(),
        buyer: { displayName: 'buyer4', id: 'test1Id' },
        createdAt: this.addDays(date, 3),
        status: 'QUEUE',
      },
      {
        id: crypto.randomUUID(),
        buyer: { displayName: 'buyer5', id: 'test1Id' },
        createdAt: this.addDays(date, 4),
        status: 'QUEUE',
      },
      {
        id: crypto.randomUUID(),
        buyer: { displayName: 'buyer6', id: 'test1Id' },
        createdAt: this.addDays(date, 5),
        status: 'QUEUE',
      },
      {
        id: crypto.randomUUID(),
        buyer: { displayName: 'buyer7', id: 'test1Id' },
        createdAt: this.addDays(date, 6),
        status: 'QUEUE',
      },
    ];

    const priorityQueueEntries = [];
    priorityQueueEntries.push(timeQueueEntries[0], timeQueueEntries[2], timeQueueEntries[4]);
    return of({ timeQueue: timeQueueEntries, priorityQueue: priorityQueueEntries });
  }

  addDays(date: Date, days: number) {
    const newDate = new Date(date);
    newDate.setDate(newDate.getDate() + days);
    return newDate;
  }

  /**
   * Update priority of item.
   *
   * @param a Requested item Id.
   * @param b Requested new index.
   * @returns Priority result.
   */
  updatePriority(queueEntryId: string, requestedIndex: number): Observable<UpdatePriorityResult> {
    return of({
      isSuccessful: true,
    });
  }
}

export interface UpdatePriorityResult {
  isSuccessful: boolean;
  error?: string;
}
