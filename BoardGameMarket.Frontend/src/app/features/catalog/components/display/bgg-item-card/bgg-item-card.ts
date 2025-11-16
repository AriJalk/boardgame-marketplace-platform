import { Component, Input, input } from '@angular/core';
import { BggItem } from '../../../models/catalog-item.model';

@Component({
  selector: 'bgm-bgg-item-card',
  imports: [],
  templateUrl: './bgg-item-card.html',
  styleUrl: './bgg-item-card.css',
})
export class BggItemCard {
  @Input() item!: BggItem
}
