import { Component, Input, OnInit } from '@angular/core';
import { ListingGroupForm } from '../../../listing-forms/listing-group-form/listing-group-form';
import { ListingGroupService } from '../../services/listing-group.service';
import { ListingFormsFactory } from '../../../listing-forms/services/listing-forms-factory.service';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { BehaviorSubject, map, Observable, take } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'bgm-edit-listing-group',
  imports: [ListingGroupForm, ReactiveFormsModule, AsyncPipe, MatButtonModule],
  templateUrl: './edit-listing-group-page.html',
  styleUrl: './edit-listing-group-page.css',
})
export class EditListingGroupPage implements OnInit {
  @Input({ required: true }) listingId!: string;

  private formGroupSubject = new BehaviorSubject<FormGroup | null>(null);
  formGroup$ = this.formGroupSubject.asObservable();

  constructor(
    private listingGroupService: ListingGroupService,
    private formFactory: ListingFormsFactory,
  ) {}

  ngOnInit(): void {
    this.listingGroupService
      .getListingGroupDetails(this.listingId)
      .pipe(take(1))
      .subscribe((details) => {
        const formGroup = this.formFactory.creatingListingGroupFormGroup(details);
        this.formGroupSubject.next(formGroup);
      });
  }

  submitForm() {
    const formGroup = this.formGroupSubject.value;
    if (!formGroup) {
      return;
    }
    console.log(formGroup.valid ? 'Valid' : 'Invalid');
    console.log(formGroup.getRawValue());
  }
}
