import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
      __uuid: [{ value: value?.id ?? crypto.randomUUID(), disabled: true }],
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
      id: [{ value: value?.id ?? null, disabled: true }],
      title: [{ value: value?.title ?? 'New listing', disabled: false }, [Validators.required]],
      price: [{ value: value?.price ?? '', disabled: false }, [Validators.required]],
      details: [{ value: value?.details ?? '', disabled: false }],
      listingItems: this.fb.array(listingItemsArray, { validators: [minArrayLength(1)] }),
      status: [{ value: value?.status ?? 'New', disabled: true }],
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
      pickupOptions: [value?.pickupOptions ?? '', [Validators.required]],
      location: [value?.location ?? '', [Validators.required]],
      details: [value?.details ?? ''],
      listings: this.fb.array(listingsArray, { validators: [minArrayLength(1)] }),
    });
  }
}
