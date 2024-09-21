import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-join-to-group',
  templateUrl: './join-to-group.component.html',
  styleUrls: ['./join-to-group.component.scss']
})
export class JoinToGroupComponent implements OnInit {
  joinForm: FormGroup = new FormGroup({});

  constructor(private readonly fb: FormBuilder, private readonly router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.joinForm = this.fb.group({
      groupName: ['', [Validators.required]],
    });
  }

  join() {
    if (this.joinForm.valid) {
      this.router.navigate([`/messages/${this.joinForm.controls['groupName'].value}`]);
    }
  }

}
