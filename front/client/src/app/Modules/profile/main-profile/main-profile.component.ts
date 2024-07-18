import { Component, OnInit } from '@angular/core';
import { ProfileService } from 'src/app/Core/services/profile.service';

@Component({
  selector: 'app-main-profile',
  templateUrl: './main-profile.component.html',
  styleUrls: ['./main-profile.component.scss']
})
export class MainProfileComponent implements OnInit {

  constructor(private readonly profileService: ProfileService) { }

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

}
