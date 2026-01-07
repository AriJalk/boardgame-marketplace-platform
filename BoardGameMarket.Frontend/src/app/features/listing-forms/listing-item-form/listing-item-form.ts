import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ListingItem } from '../../listing-items/models/listing-item.model';
import { CatalogItem } from '../../catalog/models/catalog-item.model';
import { ListingItemCard } from '../../listing-items/components/display/listing-item-card/listing-item-card';
import { AddBggListingItemForm } from "../../listing-items/components/creation/add-bgg-listing-item-form/add-bgg-listing-item-form";
import { AddCustomListingItemForm } from "../../listing-items/components/creation/add-custom-listing-item-form/add-custom-listing-item-form";

@Component({
  selector: 'bgm-listing-item-form',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    ListingItemCard,
    AddBggListingItemForm,
    AddCustomListingItemForm
],
  templateUrl: './listing-item-form.html',
  styleUrl: './listing-item-form.css',
})
export class ListingItemForm implements OnInit {
  @Input({ required: true }) formGroup!: FormGroup;

  get catalogItem() {
    return this.formGroup.get('catalogItem') as FormGroup;
  }
  listingItem?: ListingItem;

  ngOnInit(): void {
    if (this.formGroup.get('id')?.value !== null) {
      this.listingItem = this.formGroup.value as ListingItem;
    }
  }

  getValue(field: string): string {
    return this.formGroup.get(field)?.value as string;
  }

  getValueFromCatalogItem(field: string): string {
    return this.catalogItem.get(field)?.value as string;
  }
}
