import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';


@Component({
  selector: 'bgm-add-item-card',
  imports: [MatButtonModule, MatDividerModule, MatIconModule],
  templateUrl: './add-item-card.html',
  styleUrl: './add-item-card.css',
})
export class AddItemCard {

}
