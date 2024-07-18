import { Component, HostListener, Input } from '@angular/core';
import { DropdownItem } from 'src/app/Models/DropdownItem';

@Component({
  selector: 'app-dropdown',
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.scss']
})
export class DropdownComponent {
  @Input({ required: true }) items: DropdownItem[] = [];
  isOpen = false;

  toggleDropdown() {
    this.isOpen = !this.isOpen;
  }

  closeDropdown() {
    this.isOpen = false;
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    const targetElement = event.target as Element;
    if (targetElement && !targetElement.closest('.relative')) {
      this.closeDropdown();
    }
  }
}
