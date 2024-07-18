import { Component, OnInit } from '@angular/core';
import { AuthService } from './Core/services/auth.service';
import { initFlowbite } from 'flowbite';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Learnify';
  darkMode: boolean;

  constructor(private readonly authService: AuthService) { }

  ngOnInit(): void {
    this.authService.updateTokenData();
    initFlowbite();
  }

  public switchedMode(isDarkMode: boolean): void {
    console.log(isDarkMode);
    this.darkMode = isDarkMode;
  }
}
