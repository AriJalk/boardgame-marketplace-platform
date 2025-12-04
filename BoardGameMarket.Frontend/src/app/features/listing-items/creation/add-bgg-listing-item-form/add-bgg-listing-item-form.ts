import { Component, Input } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { SearchCatalog } from '../../../catalog/components/search-catalog/search-catalog';
import { BggItem } from '../../../catalog/models/catalog-item.model';

@Component({
  selector: 'bgm-add-bgg-listing-item-form',
  imports: [MatFormFieldModule, MatInputModule, FormsModule, ReactiveFormsModule, SearchCatalog],
  templateUrl: './add-bgg-listing-item-form.html',
  styleUrl: './add-bgg-listing-item-form.css',
})
export class AddBggListingItemForm {
  @Input({ required: true }) itemForm!: FormGroup;

  selectItem(item: BggItem) {
    this.itemForm.patchValue({ type: 'bgg', bggLink: item.bggLink, id: item.id, name: item.name });
  }
}
