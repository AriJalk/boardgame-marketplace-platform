import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AddListingForm } from "../../../../listings/components/creation/add-listing-form/add-listing-form";
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'bgm-add-listing-group-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatButtonModule, MatInputModule, AddListingForm, JsonPipe],
  templateUrl: './add-listing-group-form.html',
  styleUrl: './add-listing-group-form.css',
})
export class AddListingGroupForm implements OnInit {

  listingGroupForm!: FormGroup

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.listingGroupForm = this.fb.group({
      pickupLocation: [''],
      listings: this.fb.array([])
    });
  }

  get listings(): FormArray<FormGroup> {
    return this.listingGroupForm.get('listings') as FormArray;
  }

  addlisting() {
    const group = this.fb.group({
      listingTitle: [''],
      details: [''],
      price: [''],
      listingItems: this.fb.array([]),
    });
    this.listings.push(group);
  }
}
