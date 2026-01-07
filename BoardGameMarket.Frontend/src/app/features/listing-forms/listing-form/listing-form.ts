import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { requireControl } from '../../../common/validators/require-control.validator';
import { ListingFormsFactory } from '../services/listing-forms-factory.service';
import { ListingItemForm } from '../listing-item-form/listing-item-form';

@Component({
  selector: 'bgm-listing-form',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    ListingItemForm,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './listing-form.html',
  styleUrl: './listing-form.css',
})
export class ListingForm {
  @Input({ required: true }) formGroup!: FormGroup;
  @Output() listingRemoved = new EventEmitter<number>();

  constructor(private listingFormFactory: ListingFormsFactory) {}

  get listingItemFormGroups(): FormArray<FormGroup> {
    return this.formGroup.get('listingItems') as FormArray;
  }

  addBggListingItem() {
    this.listingItemFormGroups.push(
      this.listingFormFactory.createListingItemFormGroup(undefined, 'bgg'),
    );
  }

  addCustomListingItem() {
    this.listingItemFormGroups.push(
      this.listingFormFactory.createListingItemFormGroup(undefined, 'custom'),
    );
  }

  removeListingItem(index: number) {
    this.listingItemFormGroups.removeAt(index);
  }
}
