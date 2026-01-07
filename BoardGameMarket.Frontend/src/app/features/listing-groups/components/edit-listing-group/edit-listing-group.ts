import { Component, Input, OnInit } from '@angular/core';
import { ListingGroupForm } from '../../../listing-forms/listing-group-form/listing-group-form';
import { ListingGroupService } from '../../services/listing-group.service';
import { ListingFormsFactory } from '../../../listing-forms/services/listing-forms-factory.service';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { map, Observable, take } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'bgm-edit-listing-group',
  imports: [ListingGroupForm, ReactiveFormsModule, AsyncPipe, MatButtonModule],
  templateUrl: './edit-listing-group.html',
  styleUrl: './edit-listing-group.css',
})
export class EditListingGroup implements OnInit {
  @Input({ required: true }) listingId!: string;
  formGroup$!: Observable<FormGroup>;

  constructor(
    private listingGroupService: ListingGroupService,
    private formFactory: ListingFormsFactory,
  ) {}

  ngOnInit(): void {
    this.formGroup$ = this.listingGroupService
      .getListingGroupDetails(this.listingId)
      .pipe(map((value) => this.formFactory.creatingListingGroupFormGroup(value)));
  }

  submitForm() {
    this.formGroup$.pipe(take(1)).subscribe((formGroup) => {
      if (formGroup.valid) {
        console.log('VALID');
      } else {
        console.log('INVALID');
      }
      console.log(formGroup.getRawValue());
    });
  }
}
