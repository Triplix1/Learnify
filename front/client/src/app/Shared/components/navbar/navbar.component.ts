import { Component, EventEmitter, HostListener, OnInit, Output } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, UrlSerializer, UrlTree } from '@angular/router';
import { Subscription, filter } from 'rxjs';
import { MatIconRegistry } from '@angular/material/icon';
import { AuthService } from 'src/app/Core/services/auth.service';
import { DropdownItem } from 'src/app/Models/DropdownItem';
import { UserType } from 'src/app/Models/enums/UserType';
import { UserFromToken } from 'src/app/Models/UserFromToken';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent extends BaseComponent implements OnInit {
  dropdownItems: DropdownItem[];
  Theme: string = "theme";
  isDarkMode: boolean = localStorage.getItem(this.Theme) === 'true';
  @Output() switchedTheme: EventEmitter<boolean> = new EventEmitter();
  userData: UserFromToken;
  isAdmin: boolean = false;
  searchForm: FormGroup;
  adminRole = UserType.Admin;
  superAdminRole = UserType.SuperAdmin;
  teacherRole = UserType.Teacher;

  constructor(private accountService: AuthService, private router: Router, private fb: FormBuilder) {
    super();
  }

  ngOnInit(): void {
    this.switchedTheme.emit(this.isDarkMode);
    this.initializeDropdown();

    this.searchForm = this.fb.group(
      { search: "" }
    );

    this.accountService.userData$.subscribe(data => this.handleUserData(data));
  }

  handleUserData(userData: UserFromToken) {
    this.initializeDropdown();
    this.userData = userData;

    if (userData.role === UserType.Moderator) {
      this.dropdownItems.unshift({ itemText: "Оплати", link: "/payments" });
    }
    else if (userData.role === UserType.Teacher) {
      this.dropdownItems.unshift({ itemText: "Створити", link: "/course/managing" });
    }
    else if (userData.role === UserType.Admin || userData.role === UserType.SuperAdmin) {
      this.dropdownItems.unshift({ itemText: "Модератори", link: "/moderators/managing" });
    }

    if (userData.role === UserType.SuperAdmin) {
      this.dropdownItems.unshift({ itemText: "Адміни", link: "/admins/managing" });
    }
  }

  initializeDropdown() {
    this.dropdownItems = [
      { itemText: "Профіль", link: "/profile" },
      { itemText: "Вийти", action: () => this.logout() },
    ]
  }

  login() {
    this.router.navigate(['/login']);
  }

  logout() {
    this.accountService.logout();
  }

  switchTheme() {
    this.isDarkMode = !this.isDarkMode;
    localStorage.setItem(this.Theme, this.isDarkMode.toString());
    this.switchedTheme.emit(this.isDarkMode);
  }

  search() {
    if (this.searchForm.controls['search'].value) {
      const value = this.searchForm.controls['search'].value;
      this.searchForm.controls['search'].setValue('');

      this.router.navigate(["/home"], {
        queryParams: {
          search: value,
        },
        replaceUrl: true
      })
    }
  }

}
