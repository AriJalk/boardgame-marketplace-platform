import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
@Component({
  selector: 'bgm-add-listing-item-form',
  imports: [MatFormFieldModule, MatInputModule, ReactiveFormsModule],
  templateUrl: './add-listing-item-form.html',
  styleUrl: './add-listing-item-form.css',
})
export class AddListingItemForm {
  @Input({ required: true }) itemForm!: FormGroup;
}
