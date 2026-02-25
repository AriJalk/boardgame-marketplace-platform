import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ListingFormsFactory } from '../services/listing-forms-factory.service';
import { ListingItemForm } from '../listing-item-form/listing-item-form';
import { QueueSellerPanel } from '../../queues/components/queue-seller/queue-seller-panel/queue-seller-panel';
import { ListingsService } from '../../listings/services/listings.service';
import { ListingDetail } from '../../listings/models/listing.model';

@Component({
  selector: 'bgm-listing-form',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    ListingItemForm,
    MatFormFieldModule,
    MatInputModule,
    QueueSellerPanel,
  ],
  templateUrl: './listing-form.html',
  styleUrl: './listing-form.css',
})
export class ListingForm implements OnInit {
  @Input({ required: true }) formGroup!: FormGroup;
  @Output() listingRemoved = new EventEmitter<number>();

  isLocked: boolean = false;

  constructor(
    private listingFormFactory: ListingFormsFactory,
    private listingsService: ListingsService,
  ) {}

  ngOnInit(): void {
    this.updateLockState(this.formGroup.get('status')?.value != 'active');
  }

  get listingItemFormGroups(): FormArray<FormGroup> {
    return this.formGroup.get('listingItems') as FormArray;
  }

  get listingId(): string {
    return this.formGroup.get('id')?.value as string;
  }

  addBggListingItem() {
    this.listingItemFormGroups.push(
      this.listingFormFactory.createListingItemFormGroup(undefined, 'bgg'),
    );
  }

  addCustomListingItem() {
    this.listingItemFormGroups.push(
      this.listingFormFactory.createListingItemFormGroup(undefined, 'custom'),
    );
  }

  removeListingItem(index: number) {
    this.listingItemFormGroups.removeAt(index);
  }

  onListingStatusChanged() {
    this.listingsService.getListing(this.listingId).subscribe({
      next: (data) => {
        this.updateFormFromModel(data);
        this.updateLockState(data.status != 'active');
      },
    });
  }

  private updateFormFromModel(data: ListingDetail) {
    this.formGroup.patchValue({
      status: data.status,
    });
  }

  private updateLockState(isLocked: boolean) {
    this.isLocked = isLocked;
    const controlsToToggle = ['title', 'details', 'price'];
    for (const name of controlsToToggle) {
      const control = this.formGroup.get(name);
      if (!control) continue;
      if (!isLocked) {
        control.enable({ emitEvent: false });
      } else {
        control.disable({ emitEvent: false });
      }
    }
  }
}
