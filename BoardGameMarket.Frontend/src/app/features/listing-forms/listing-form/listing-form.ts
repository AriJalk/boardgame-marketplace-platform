import { Component, EventEmitter, Input, Output } from '@angular/core';
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
import { AddListingItemForm } from '../../listing-items/components/creation/add-listing-item-form/add-listing-item-form';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { requireControl } from '../../../common/validators/require-control.validator';
import { ListingFormsFactoryService } from '../services/listing-forms-factory.service';

@Component({
  selector: 'bgm-listing-form',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    AddListingItemForm,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './listing-form.html',
  styleUrl: './listing-form.css',
})
export class ListingForm {
  @Input({ required: true }) listingForm!: FormGroup;
  @Output() listingRemoved = new EventEmitter<number>();

  constructor(private listingFormFactory: ListingFormsFactoryService) {}

  get listingItemFormGroups(): FormArray<FormGroup> {
    return this.listingForm.get('listingItems') as FormArray;
  }

  addBggListingItem() {
    this.listingItemFormGroups.push(this.listingFormFactory.createListingItemFormGroup());
  }

  addCustomListingItem() {
    this.listingItemFormGroups.push(this.listingFormFactory.createListingItemFormGroup());
  }

  removeListingItem(index: number) {
    this.listingItemFormGroups.removeAt(index);
  }
}
