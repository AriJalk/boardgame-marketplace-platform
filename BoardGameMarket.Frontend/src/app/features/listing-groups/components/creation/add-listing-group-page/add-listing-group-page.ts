import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AddListingGroupForm } from "../add-listing-group-form/add-listing-group-form";
import { MatAnchor, MatButtonModule } from "@angular/material/button";

@Component({
  selector: 'bgm-add-listing-group-page',
  imports: [ReactiveFormsModule, AddListingGroupForm, MatAnchor, MatButtonModule],
  templateUrl: './add-listing-group-page.html',
  styleUrl: './add-listing-group-page.css',
})
export class AddListingGroupPage implements OnInit {
  formGroup!: FormGroup

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      pickupOptions: [''],
      location: [''],
      details: [''],
    });
  }


  submitForm() {
    if (this.formGroup.valid) {
      console.log("VALID");
    }
    else {
      console.log("INVALID");
    }
    console.log(this.formGroup.getRawValue());
  }
}
