import { Component, Input, OnInit } from '@angular/core';
import { ListingGroupForm } from '../../../listing-forms/listing-group-form/listing-group-form';
import { ListingGroupService } from '../../services/listing-group.service';
import { ListingFormsFactory } from '../../../listing-forms/services/listing-forms-factory.service';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { BehaviorSubject, map, Observable, take } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'bgm-create-listing-group',
  imports: [ListingGroupForm, ReactiveFormsModule, MatButtonModule],
  templateUrl: './create-listing-group-page.html',
  styleUrl: './create-listing-group-page.css',
})
export class CreateListingGroupPage implements OnInit {
  @Input({ required: true }) listingId!: string;

  formGroup!: FormGroup;

  constructor(
    private listingGroupService: ListingGroupService,
    private formFactory: ListingFormsFactory,
  ) {}

  ngOnInit(): void {
    this.formGroup = this.formFactory.creatingListingGroupFormGroup();
  }

  submitForm() {
    console.log(this.formGroup.valid ? 'Valid' : 'Invalid');
    console.log(this.formGroup.getRawValue());
  }
}
