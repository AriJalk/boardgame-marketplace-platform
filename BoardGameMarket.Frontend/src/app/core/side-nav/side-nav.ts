import { Component, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDrawer } from '@angular/material/sidenav';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'bgm-side-nav',
  imports: [MatButtonModule, RouterLink],
  templateUrl: './side-nav.html',
  styleUrl: './side-nav.css',
})
export class SideNav {
  @Input({ required: true }) drawer!: MatDrawer
}
