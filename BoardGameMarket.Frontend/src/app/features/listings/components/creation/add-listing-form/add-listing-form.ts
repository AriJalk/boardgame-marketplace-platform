import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AddListingItemForm } from '../../../../listing-items/creation/add-listing-item-form/add-listing-item-form';

@Component({
  selector: 'bgm-add-listing-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, AddListingItemForm],
  templateUrl: './add-listing-form.html',
  styleUrl: './add-listing-form.css',
})
export class AddListingForm {
  @Input({ required: true }) listingForm!: FormGroup;
  @Output() listingItemAdded = new EventEmitter<FormGroup>();

  get listingItems(): FormArray<FormGroup> {
    return this.listingForm.get('listingItems') as FormArray;
  }
  addlistingItem() {
    this.listingItemAdded.emit(this.listingForm);
  };
}
