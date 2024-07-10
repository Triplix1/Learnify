import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Learnify';
  darkMode: boolean;

  public switchedMode(isDarkMode: boolean): void {
    console.log(isDarkMode);
    this.darkMode = isDarkMode;
  }
}
