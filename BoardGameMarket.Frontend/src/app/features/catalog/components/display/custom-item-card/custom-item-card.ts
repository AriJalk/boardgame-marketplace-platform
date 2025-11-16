import { Component, Input } from '@angular/core';
import { CustomItem } from '../../../models/catalog-item.model';

@Component({
  selector: 'bgm-custom-item-card',
  imports: [],
  templateUrl: './custom-item-card.html',
  styleUrl: './custom-item-card.css',
})
export class CustomItemCard {
  @Input() item!: CustomItem
}
