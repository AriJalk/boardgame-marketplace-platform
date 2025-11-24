import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AddListingGroupForm } from "../add-listing-group-form/add-listing-group-form";
import { MatAnchor, MatButtonModule } from "@angular/material/button";

@Component({
  selector: 'bgm-add-listing-group-page',
  imports: [ReactiveFormsModule, AddListingGroupForm,MatAnchor, MatButtonModule],
  templateUrl: './add-listing-group-page.html',
  styleUrl: './add-listing-group-page.css',
})
export class AddListingGroupPage implements OnInit {
  formGroup!: FormGroup

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      pickupOptions: [''],
      location: [''],
      details: [''],
      listings: this.fb.array([this.createListingFormGroup()]),
    });
  }

  get listings(): FormArray<FormGroup> {
    return this.formGroup.get('listings') as FormArray;
  }

  getListingItems(listingFormGroup: FormGroup): FormArray<FormGroup> {
    return listingFormGroup.get('listingItems') as FormArray;
  }


  submitForm() {
    console.log(this.formGroup.value);
  }

  createListingFormGroup(): FormGroup {
    return this.fb.group({
      title: [''],
      price: [''],
      details: [''],
      listingItems: this.fb.array([this.createListingItemFormGroup()]),
    });
  }

  createListingItemFormGroup(): FormGroup {
    return this.fb.group({
      catalogItemId: [''],
      details: [''],
    });
  }

  addListing() {
    this.listings.push(this.createListingFormGroup());
  }

  addListingItem(listingFormGroup: FormGroup) {
    this.getListingItems(listingFormGroup).push(this.createListingItemFormGroup());
  }

}
