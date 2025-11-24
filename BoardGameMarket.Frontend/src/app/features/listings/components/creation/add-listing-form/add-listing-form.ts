import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AddListingItemForm } from '../../../../listing-items/creation/add-listing-item-form/add-listing-item-form';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'bgm-add-listing-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, AddListingItemForm, MatInputModule, MatButtonModule],
  templateUrl: './add-listing-form.html',
  styleUrl: './add-listing-form.css',
})
export class AddListingForm {
  @Input({ required: true }) listingForm!: FormGroup;

  constructor(private fb: FormBuilder) { }

  get listingItems(): FormArray<FormGroup> {
    return this.listingForm.get('listingItems') as FormArray;
  }

  addlistingItem() {
    this.listingItems.push(this.fb.group({
      catalogItemId: [''],
      details: ['']
    }));
  }
}
