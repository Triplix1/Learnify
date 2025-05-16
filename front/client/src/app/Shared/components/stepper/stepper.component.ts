import {
  Component,
  ContentChildren,
  QueryList,
  AfterContentInit,
  Input,
  ViewChild,
  ElementRef,
  Renderer2
} from '@angular/core';
import { StepComponent } from '../step/step.component';
import {
  trigger,
  style,
  animate,
  transition,
  query,
  group
} from '@angular/animations';

@Component({
  selector: 'app-stepper',
  templateUrl: './stepper.component.html',
  styleUrls: ['./stepper.component.scss'],
  animations: [
    trigger('stepAnimation', [
      transition(':increment', [
        style({ position: 'relative' }),
        query(':enter, :leave', [
          style({
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
          }),
        ]),
        group([
          query(':leave', [
            animate('300ms ease', style({ transform: 'translateX(-100%)' })),
          ]),
          query(':enter', [
            style({ transform: 'translateX(100%)' }),
            animate('300ms ease', style({ transform: 'translateX(0)' })),
          ]),
        ]),
      ]),
      transition(':decrement', [
        style({ position: 'relative' }),
        query(':enter, :leave', [
          style({
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
          }),
        ]),
        group([
          query(':leave', [
            animate('300ms ease', style({ transform: 'translateX(100%)' })),
          ]),
          query(':enter', [
            style({ transform: 'translateX(-100%)' }),
            animate('300ms ease', style({ transform: 'translateX(0)' })),
          ]),
        ]),
      ]),
    ]),
  ]
})
export class StepperComponent implements AfterContentInit {
  @ContentChildren(StepComponent) steps!: QueryList<StepComponent>;
  @Input() linear: boolean = true;
  @Input() disableButtons: boolean = false;
  currentStepIndex = 0;

  @ViewChild('contentWrapper') contentWrapper!: ElementRef;  // Reference to the content container

  constructor(private renderer: Renderer2) { }

  ngAfterContentInit() {
    this.steps.toArray()[this.currentStepIndex].active = true;
    this.adjustHeight(); // Set initial height based on content
  }

  // Adjust the height of the content container dynamically based on the current content
  adjustHeight() {
    const contentWrapperElement = this.contentWrapper.nativeElement;
    const currentContentHeight = contentWrapperElement.scrollHeight;  // Get the height of the content
    this.renderer.setStyle(contentWrapperElement, 'height', `${currentContentHeight}px`); // Set the height dynamically
  }

  // Reset the height to auto once the animation is complete
  resetHeightToAuto() {
    const contentWrapperElement = this.contentWrapper.nativeElement;
    this.renderer.setStyle(contentWrapperElement, 'height', 'auto'); // Reset height to auto after animation completes
  }

  next() {
    const stepsArray = this.steps.toArray();
    if (this.canGoNext()) {
      stepsArray[this.currentStepIndex].active = false;

      // Measure the height before changing the step
      this.adjustHeight();

      this.currentStepIndex++;
      stepsArray[this.currentStepIndex].active = true;

      // Adjust the height after the animation completes
      setTimeout(() => {
        this.adjustHeight();  // Adjust to new content height
        setTimeout(() => this.resetHeightToAuto(), 300);  // Reset height to auto after the animation completes
      }, 0);
    }
  }

  previous() {
    const stepsArray = this.steps.toArray();
    if (this.currentStepIndex > 0) {
      stepsArray[this.currentStepIndex].active = false;

      // Measure the height before changing the step
      this.adjustHeight();

      this.currentStepIndex--;
      stepsArray[this.currentStepIndex].active = true;

      // Adjust the height after the animation completes
      setTimeout(() => {
        this.adjustHeight();  // Adjust to new content height
        setTimeout(() => this.resetHeightToAuto(), 300);  // Reset height to auto after the animation completes
      }, 0);
    }
  }

  goToStep(index: number) {
    const stepsArray = this.steps.toArray();
    if (!this.linear || this.canGoToStep(index)) {
      stepsArray[this.currentStepIndex].active = false;

      // Measure the height before changing the step
      this.adjustHeight();

      this.currentStepIndex = index;
      stepsArray[this.currentStepIndex].active = true;

      // Adjust the height after the animation completes
      setTimeout(() => {
        this.adjustHeight();  // Adjust to new content height
        setTimeout(() => this.resetHeightToAuto(), 300);  // Reset height to auto after the animation completes
      }, 0);
    }
  }

  forceGoToStep(index: number) {
    const stepsArray = this.steps.toArray();
    stepsArray[this.currentStepIndex].active = false;

    // Measure the height before changing the step
    this.adjustHeight();

    this.currentStepIndex = index;
    stepsArray[this.currentStepIndex].active = true;

    // Adjust the height after the animation completes
    setTimeout(() => {
      this.adjustHeight();  // Adjust to new content height
      setTimeout(() => this.resetHeightToAuto(), 300);  // Reset height to auto after the animation completes
    }, 0);
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
