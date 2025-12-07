import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { AddListingItemForm } from '../../../../listing-items/creation/add-listing-item-form/add-listing-item-form';
import { MatCardModule } from "@angular/material/card";
import { MatInputModule } from '@angular/material/input';
import { requireControl } from '../../../../../common/validators/require-control.validator';

@Component({
  selector: 'bgm-add-listing-form',
  imports: [ReactiveFormsModule, MatButtonModule, MatCardModule, AddListingItemForm, MatFormFieldModule, MatInputModule],
  templateUrl: './add-listing-form.html',
  styleUrl: './add-listing-form.css',
})
export class AddListingForm implements OnInit {
  @Input({ required: true }) listingForm!: FormGroup;
  @Output() listingRemoved = new EventEmitter<number>();

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.listingForm.addControl('listingItems', this.fb.array([]));
  }

  get listingItems(): FormArray<FormGroup> {
    return this.listingForm.get('listingItems') as FormArray;
  }

  createBggListingItemFormGroup(): FormGroup {
    return this.fb.group({
      id: new FormControl({ value: null, disabled: true }, [Validators.required]),
      type: new FormControl({ value: 'bgg', disabled: true }),
      details: [''],
    }, { validators: requireControl('id') });
  }

  createCustomListingItemFormGroup(): FormGroup {
    return this.fb.group({
      type: new FormControl({ value: 'custom', disabled: true }),
      name: new FormControl(null, [Validators.required]),
      details: [''],
    });
  }

  addBggListingItem() {
    this.listingItems.push(this.createBggListingItemFormGroup());
  }

  addCustomListingItem() {
    this.listingItems.push(this.createCustomListingItemFormGroup());
  }



  removeListingItem(index: number) {
    this.listingItems.removeAt(index);
  }
}
