import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { AddListingForm } from '../../listings/components/creation/add-listing-form/add-listing-form';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ListingFormsFactoryService } from '../services/listing-forms-factory.service';
import { ListingForm } from '../listing-form/listing-form';

@Component({
  selector: 'bgm-listing-group-form',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule,
    ListingForm,
  ],

  templateUrl: './listing-group-form.html',
  styleUrl: './listing-group-form.css',
})
export class ListingGroupForm {
  @Input({ required: true }) formGroup!: FormGroup;

  constructor(private listingFormFactory: ListingFormsFactoryService) {}

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
}
