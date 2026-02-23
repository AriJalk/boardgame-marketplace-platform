import { Component, Input, OnInit } from '@angular/core';
import { ListingGroupForm } from '../../../listing-forms/listing-group-form/listing-group-form';
import { ListingGroupService } from '../../services/listing-group.service';
import { ListingFormsFactory } from '../../../listing-forms/services/listing-forms-factory.service';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { BehaviorSubject, map, Observable, take } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { CreateListingGroupRequest } from '../../models/listing-group.model';
import { CreateListingRequestDto as CreateListingRequest } from '../../../listings/models/listing.model';
import { CreateListingItemRequest } from '../../../listing-items/models/listing-item.model';

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
    if (this.formGroup.invalid) {
      console.log('invalid');
    }
    const fg = this.formGroup;
    const request: CreateListingGroupRequest = {
      location: fg.get('location')?.value,
      details: fg.get('details')?.value,
      pickupOptions: fg.get('pickupOptions')?.value,
      listings: fg.get('listings')?.value.map(
        (l: any): CreateListingRequest => ({
          title: l.title,
          price: l.price,
          details: l.details,
          listingItems: l.listingItems.map(
            (li: any): CreateListingItemRequest => ({
              catalogId: li.catalogItem.id,
              details: li.details,
            }),
          ),
        }),
      ),
    };

    this.listingGroupService.postListingGroup(request).subscribe({
      next: (data) => {
        console.log(data);
      },
    });
  }
}
