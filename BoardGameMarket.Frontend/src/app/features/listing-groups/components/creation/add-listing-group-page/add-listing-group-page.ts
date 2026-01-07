import { Component, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatAnchor, MatButtonModule } from '@angular/material/button';
import { ListingGroupForm } from '../../../../listing-forms/listing-group-form/listing-group-form';
import { ListingFormsFactory } from '../../../../listing-forms/services/listing-forms-factory.service';
import { ListingGroupService } from '../../../services/listing-group.service';

@Component({
  selector: 'bgm-add-listing-group-page',
  imports: [ReactiveFormsModule, MatAnchor, MatButtonModule, ListingGroupForm],
  templateUrl: './add-listing-group-page.html',
  styleUrl: './add-listing-group-page.css',
})
export class AddListingGroupPage implements OnInit {
  formGroup!: FormGroup;

  constructor(private listingFormFactory: ListingFormsFactory, private listingGroupService : ListingGroupService) {}

  ngOnInit(): void {
    this.formGroup = this.listingFormFactory.creatingListingGroupFormGroup();
  }

  submitForm() {
    if (this.formGroup.valid) {
      console.log('VALID');
    } else {
      console.log('INVALID');
    }
    console.log(this.formGroup.getRawValue());
  }
}
