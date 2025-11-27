import { Component, Input } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddBggListingItemForm } from '../add-bgg-listing-item-form/add-bgg-listing-item-form';
import { AddCustomListingItemForm } from '../add-custom-listing-item-form/add-custom-listing-item-form';

@Component({
  selector: 'bgm-add-listing-item-form',
  imports: [ReactiveFormsModule, AddBggListingItemForm, AddCustomListingItemForm, FormsModule],
  templateUrl: './add-listing-item-form.html',
  styleUrl: './add-listing-item-form.css',
})
export class AddListingItemForm {
  @Input({ required: true }) itemForm!: FormGroup;
}
