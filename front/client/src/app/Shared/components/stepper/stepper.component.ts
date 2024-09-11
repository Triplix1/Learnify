import { Component, ContentChildren, QueryList, AfterContentInit, Input } from '@angular/core';
import { StepComponent } from '../step/step.component';

@Component({
  selector: 'app-stepper',
  templateUrl: './stepper.component.html',
  styleUrls: ['./stepper.component.css']
})
export class StepperComponent implements AfterContentInit {
  @ContentChildren(StepComponent) steps: QueryList<StepComponent>;

  @Input() linear = true;  // Similar to MatStepper
  currentStepIndex = 0;

  ngAfterContentInit() {
    this.steps.toArray()[this.currentStepIndex].active = true;
  }

  next() {
    const stepsArray = this.steps.toArray();
    if (this.canGoNext()) {
      stepsArray[this.currentStepIndex].active = false;
      this.currentStepIndex++;
      stepsArray[this.currentStepIndex].active = true;
    }
  }

  previous() {
    const stepsArray = this.steps.toArray();
    if (this.currentStepIndex > 0) {
      stepsArray[this.currentStepIndex].active = false;
      this.currentStepIndex--;
      stepsArray[this.currentStepIndex].active = true;
    }
  }

  goToStep(index: number) {
    const stepsArray = this.steps.toArray();
    if (!this.linear || this.canGoToStep(index)) {
      stepsArray[this.currentStepIndex].active = false;
      this.currentStepIndex = index;
      stepsArray[this.currentStepIndex].active = true;
    }
  }

  canGoNext() {
    const stepsArray = this.steps.toArray();
    return (
      this.currentStepIndex < stepsArray.length - 1 &&
      (!this.linear || stepsArray[this.currentStepIndex].completed)
    );
  }

  canGoToStep(index: number) {
    const stepsArray = this.steps.toArray();
    if (!this.linear) return true;
    for (let i = 0; i < index; i++) {
      if (!stepsArray[i].completed) return false;
    }
    return true;
  }
}
