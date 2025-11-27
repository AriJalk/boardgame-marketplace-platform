import { Component, Input } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'bgm-add-bgg-listing-item-form',
  imports: [MatFormFieldModule, MatInputModule, FormsModule, ReactiveFormsModule],
  templateUrl: './add-bgg-listing-item-form.html',
  styleUrl: './add-bgg-listing-item-form.css',
})
export class AddBggListingItemForm {
  @Input({ required: true }) itemForm!: FormGroup;
}
