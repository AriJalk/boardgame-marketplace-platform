import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { JsonPipe } from '@angular/common';
import { AddListingForm } from '../../../../listings/components/creation/add-listing-form/add-listing-form';

@Component({
  selector: 'bgm-add-listing-group-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatButtonModule, MatInputModule, AddListingForm],
  templateUrl: './add-listing-group-form.html',
  styleUrl: './add-listing-group-form.css',
})
export class AddListingGroupForm {
  @Input({ required: true }) listingGroupForm!: FormGroup
  @Output() listingAdded = new EventEmitter<void>();
  @Output() listingItemAdded = new EventEmitter<FormGroup>();


  get listings(): FormArray<FormGroup> {
    return this.listingGroupForm.get('listings') as FormArray
  }

  addListing() {
    this.listingAdded.emit();
  }

  addListingItem(listingFormGroup: FormGroup) {
    this.listingItemAdded.emit(listingFormGroup);
  }
}
