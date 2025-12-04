import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatFormField, MatInputModule } from '@angular/material/input';
import { BggItem } from '../../models/catalog-item.model';
import { CatalogItemCard } from '../display/catalog-item-card/catalog-item-card';
import { CatalogItemGetterService } from '../../services/catalog-item-getter.service';
import { Observable } from 'rxjs';
import { AsyncPipe, CommonModule } from '@angular/common';

@Component({
  selector: 'bgm-search-catalog',
  imports: [MatInputModule, CatalogItemCard, AsyncPipe, MatFormField, CommonModule],
  templateUrl: './search-catalog.html',
  styleUrl: './search-catalog.css',
})
export class SearchCatalog {
  @Output() itemSelected = new EventEmitter<BggItem>();
  selectedItem?: BggItem;
  searchItems$?: Observable<BggItem[]>;

  constructor(private catalogGetterService: CatalogItemGetterService) { }

  onValueChanged(event: Event) {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    this.searchItems$ = this.catalogGetterService.searchItems(value);
  }

  selectItem(item: BggItem) {
    this.selectedItem = item;
    this.itemSelected.emit(item);
  }
}
