import { Component, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';
import { SideNav } from './core/side-nav/side-nav';
import { AddListingGroupForm } from "./features/listing-groups/components/creation/add-listing-group-form/add-listing-group-form";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SideNav, MatSidenavModule, MatToolbarModule, MatIconModule, MatButtonModule,
    AddListingGroupForm],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('frontend');
}
