import { Component, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { JsonPipe } from '@angular/common';
import { AddListingForm } from '../../../../listings/components/creation/add-listing-form/add-listing-form';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'bgm-add-listing-group-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatButtonModule, MatInputModule, AddListingForm, MatCardModule],
  templateUrl: './add-listing-group-form.html',
  styleUrl: './add-listing-group-form.css',
})
export class AddListingGroupForm implements OnInit {

  @Input({ required: true }) listingGroupForm!: FormGroup

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.listingGroupForm.addControl('listings', this.fb.array([this.createListingFormGroup()]));
  }

  get listings(): FormArray<FormGroup> {
    return this.listingGroupForm.get('listings') as FormArray
  }

  createListingFormGroup(): FormGroup {
    return this.fb.group({
      title: [''],
      price: [''],
      details: [''],
    });
  }

  addListing() {
    this.listings.push(this.createListingFormGroup());
  }

  removeListing(index: number) {
    if (this.listings.length > 1) {
      this.listings.removeAt(index);
    }
  }
}
