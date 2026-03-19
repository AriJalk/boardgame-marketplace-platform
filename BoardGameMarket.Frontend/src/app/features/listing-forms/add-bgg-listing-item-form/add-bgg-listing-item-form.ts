import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RetrieveBggItemId } from '../../catalog/components/search/retrieve-bgg-item-id/retrieve-bgg-item-id';
import { BggItem, CatalogItem } from '../../catalog/models/catalog-item.model';
import { Subject } from 'rxjs';
import { AsyncPipe } from '@angular/common';
@Component({
  selector: 'bgm-add-bgg-listing-item-form',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    RetrieveBggItemId,
    AsyncPipe,
  ],
  templateUrl: './add-bgg-listing-item-form.html',
  styleUrl: './add-bgg-listing-item-form.css',
})
export class AddBggListingItemForm {
  @Input({ required: true }) formGroup!: FormGroup;
  @Output() itemSelected = new EventEmitter<BggItem>();

  private itemSubject$ = new Subject<CatalogItem>();
  item$ = this.itemSubject$.asObservable();

  get catalogItem() {
    return this.formGroup.get('catalogItem');
  }

  selectItem(item: BggItem) {
    this.formGroup.patchValue({ id: item.id, name: item.name });
  }

  attachItem(item: CatalogItem | null) {
    if (!item) {
      return;
    }
    this.catalogItem?.patchValue({ id: item?.id, name: item?.name });
    this.itemSubject$.next(item);
  }
}
