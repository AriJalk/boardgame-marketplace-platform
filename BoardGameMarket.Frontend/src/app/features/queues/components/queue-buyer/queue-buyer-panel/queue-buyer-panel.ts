import { Component, Input, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { QueueService } from '../../../queue.service';

@Component({
  selector: 'bgm-queue-buyer-panel',
  imports: [MatButtonModule],
  templateUrl: './queue-buyer-panel.html',
  styleUrl: './queue-buyer-panel.css',
})
export class QueueBuyerPanel implements OnInit {
  @Input({ required: true }) listingId!: string;

  isSubscribed!: boolean;

  constructor(private queueService: QueueService) {}

  ngOnInit(): void {
    this.queueService.isSubscribed(this.listingId).subscribe({
      next: (value) => {
        this.isSubscribed = value;
      },
    });
  }

  onSubscribed() {
    console.log('SUB: ' + this.listingId);
    this.isSubscribed = true;
  }

  onUnsubscribed() {
    console.log('UNSUB: ' + this.listingId);
    this.isSubscribed = false;
  }
}
