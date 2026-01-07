import { Component, Input } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'bgm-add-custom-listing-item-form',
  imports: [MatFormFieldModule, MatInputModule, FormsModule, ReactiveFormsModule],
  templateUrl: './add-custom-listing-item-form.html',
  styleUrl: './add-custom-listing-item-form.css',
})
export class AddCustomListingItemForm {
  @Input({ required: true }) itemForm!: FormGroup;

  get catalogItem(): FormGroup {
    return this.itemForm.get('catalogItem') as FormGroup;
  }
}
