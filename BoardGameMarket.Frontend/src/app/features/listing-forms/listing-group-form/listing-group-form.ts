import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ListingFormsFactory } from '../services/listing-forms-factory.service';
import { ListingForm } from '../listing-form/listing-form';
import { CdkAriaLive } from '../../../../../node_modules/@angular/cdk/types/_a11y-module-chunk';
import { MatAccordion, MatExpansionModule } from '@angular/material/expansion';

@Component({
  selector: 'bgm-listing-group-form',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule,
    ListingForm,
    MatAccordion,
    MatExpansionModule,
  ],

  templateUrl: './listing-group-form.html',
  styleUrl: './listing-group-form.css',
})
export class ListingGroupForm {
  @Input({ required: true }) formGroup!: FormGroup;

  constructor(private listingFormFactory: ListingFormsFactory) {}

  get listingFormGroups(): FormArray<FormGroup> {
    return this.formGroup.get('listings') as FormArray;
  }

  removeListing(index: number) {
    if (this.listingFormGroups.length > 1) {
      this.listingFormGroups.removeAt(index);
    }
  }

  addListing() {
    this.listingFormGroups.push(this.listingFormFactory.createListingFormGroup());
  }

  getListingTitle(listingFormGroup: FormGroup) {
    return listingFormGroup.get('title')?.getRawValue() ?? 'Unnamed listing';
  }

  getListingStatus(listingFormGroup: FormGroup){
    return listingFormGroup.get('status')?.getRawValue() ?? 'Status error';
  }
}
