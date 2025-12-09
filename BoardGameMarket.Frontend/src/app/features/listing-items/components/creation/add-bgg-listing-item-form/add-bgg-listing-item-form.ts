import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RetrieveBggItemId } from '../../../../catalog/components/search/retrieve-bgg-item-id/retrieve-bgg-item-id';
import { BggItem } from '../../../../catalog/models/catalog-item.model';
@Component({
  selector: 'bgm-add-bgg-listing-item-form',
  imports: [MatFormFieldModule, MatInputModule, FormsModule, ReactiveFormsModule, RetrieveBggItemId],
  templateUrl: './add-bgg-listing-item-form.html',
  styleUrl: './add-bgg-listing-item-form.css',
})
export class AddBggListingItemForm {
  @Input({ required: true }) itemForm!: FormGroup;
  @Output() itemSelected = new EventEmitter<BggItem>();

  selectItem(item: BggItem) {
    this.itemForm.patchValue({ id: item.id, name: item.name });
  }

  attachId(itemId: string | null) {
    this.itemForm.patchValue({ id: itemId });
  }
}
