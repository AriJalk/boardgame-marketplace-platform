import { Component, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'bgm-queue-buyer-panel',
  imports: [MatButtonModule],
  templateUrl: './queue-buyer-panel.html',
  styleUrl: './queue-buyer-panel.css',
})
export class QueueBuyerPanel {
  @Input({ required: true }) listingId!: string;
  @Input({ required: true }) isSubscribed!: boolean;

  onSubscribed() {
    console.log('SUB: ' + this.listingId);
    this.isSubscribed = true;
  }

  onUnsubscribed() {
    console.log('UNSUB: ' + this.listingId);
    this.isSubscribed = false;
  }
}
