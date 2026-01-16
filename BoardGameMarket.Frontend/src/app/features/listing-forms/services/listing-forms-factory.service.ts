import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ListingDetail } from '../../listings/models/listing.model';
import { ListingItem } from '../../listing-items/models/listing-item.model';
import { minArrayLength } from '../../../common/validators/min-array-length.validator';
import { CatalogItem } from '../../catalog/models/catalog-item.model';
import { ListingGroupDetails } from '../../listing-groups/models/listing-group.model';

@Injectable({
  providedIn: 'root',
})
export class ListingFormsFactory {
  constructor(private fb: FormBuilder) {}

  createCatalogItemFormGroup(value?: CatalogItem, type?: 'bgg' | 'custom'): FormGroup {
    const formGroup = this.fb.group({
      id: this.fb.control(value?.id ?? null),
      name: this.fb.control(value?.name ?? ''),
      type: [value?.type ?? type ?? ''],
      bggUrl: [value?.bggUrl ?? ''],
    });
    if (type === 'bgg') {
      formGroup.get('id')?.addValidators(Validators.required);
    } else if (type === 'custom') {
      formGroup.get('name')?.addValidators(Validators.required);
    }
    return formGroup;
  }

  createListingItemFormGroup(value?: ListingItem, type?: 'bgg' | 'custom'): FormGroup {
    return this.fb.group({
      id: [value?.id ?? null],
      catalogItem: this.createCatalogItemFormGroup(value?.catalogItem, type),
      details: [value?.details ?? ''],
    });
  }

  createListingFormGroup(value?: ListingDetail) {
    const listingItemsArray: FormGroup[] = [];
    if (value) {
      for (const listingItem of value.items) {
        listingItemsArray.push(this.createListingItemFormGroup(listingItem));
      }
    }

    return this.fb.group({
      id: [value?.id ?? null],
      title: [value?.title ?? ''],
      price: [value?.price ?? ''],
      details: [value?.details ?? ''],
      listingItems: this.fb.array(listingItemsArray, { validators: [minArrayLength(1)] }),
    });
  }

  creatingListingGroupFormGroup(value?: ListingGroupDetails): FormGroup {
    const listingsArray: FormGroup[] = [];
    if (value) {
      for (const listing of value.listings) {
        listingsArray.push(this.createListingFormGroup(listing));
      }
    } else {
      listingsArray.push(this.createListingFormGroup());
    }
    return this.fb.group({
      id: [value?.id ?? null],
      pickupOptions: [value?.pickupOptions ?? ''],
      location: [value?.location ?? ''],
      details: [value?.details ?? ''],
      listings: this.fb.array(listingsArray, { validators: [minArrayLength(1)] }),
    });
  }
}
