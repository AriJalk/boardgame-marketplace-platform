import { Component } from '@angular/core';
import { QueueEntryCard } from '../queues/components/queue-seller/queue-entry-card/queue-entry-card';
import { QueueEntry } from '../queues/models/queue-entry.model';

@Component({
  selector: 'bgm-sandbox',
  imports: [QueueEntryCard],
  templateUrl: './sandbox.html',
  styleUrl: './sandbox.css',
})
export class Sandbox {
  entry: QueueEntry = {
    buyer: { displayName: 'testUser', userId: 'guidiD' },
    createdAt: new Date(),
    id: 'someId',
    status: 'queued',
    priority: 10,
  };
}
