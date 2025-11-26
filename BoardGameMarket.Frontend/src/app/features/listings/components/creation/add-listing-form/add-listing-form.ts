import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AddListingItemForm } from '../../../../listing-items/creation/add-listing-item-form/add-listing-item-form';
import { MatCardModule } from "@angular/material/card";

@Component({
  selector: 'bgm-add-listing-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, AddListingItemForm, MatCardModule],
  templateUrl: './add-listing-form.html',
  styleUrl: './add-listing-form.css',
})
export class AddListingForm implements OnInit {
  @Input({ required: true }) listingForm!: FormGroup;
  @Output() listingRemoved = new EventEmitter<number>();

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.listingForm.addControl('listingItems', this.fb.array([this.createListingItemFormGroup()]))
  }

  get listingItems(): FormArray<FormGroup> {
    return this.listingForm.get('listingItems') as FormArray;
  }

  createListingItemFormGroup(): FormGroup {
    return this.fb.group({
      catalogItemId: [''],
      details: [''],
    });
  }

  addListingItemFormGroup() {
    this.listingItems.push(this.createListingItemFormGroup());
  }

  removeListingItem(index: number) {
    this.listingItems.removeAt(index);
  }
}
