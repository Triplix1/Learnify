import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, Renderer2 } from '@angular/core';
import { initPublisher, initSession, OTError, Session, Subscriber } from '@opentok/client';
import { ActiveStream } from 'src/app/Models/Meeting/ActiveStreams';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-meeting',
  templateUrl: './meeting.component.html',
  styleUrls: ['./meeting.component.scss']
})
export class MeetingComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input({ required: true }) sessionId: string;
  @Input({ required: true }) token: string;
  @Output() onLogout: EventEmitter<void> = new EventEmitter<void>();

  allStreams: ActiveStream[] = [];
  activeStreamVideos: string[] = [];
  session: Session;
  subscriberDiv: HTMLElement;
  publisher: OT.Publisher;
  screenPublisher: OT.Publisher;
  screenSharing: boolean = false;
  selectedStream: string = null;

  constructor(private renderer: Renderer2, private el: ElementRef) { }

  ngOnInit(): void {
    this.initializeSession();
  }

  ngAfterViewInit(): void {
    this.subscriberDiv = this.el.nativeElement.querySelector('#subscriber');
  }

  ngOnDestroy(): void {
    if (this.session) {
      this.logout();
    }
  }

  initializeSession() {
    this.session = initSession(environment.vonageApplicationId, this.sessionId);

    // Create a publisher
    this.publisher = initPublisher('publisher', {
      insertMode: 'replace',
      width: '360px',
      height: '240px',
      audioFallback: {
        publisher: true,
        subscriber: true,
      },
      showControls: false
    }, this.handleError);

    // Connect to the session
    this.session.connect(this.token, (error) => {
      // If the connection is successful, publish to the session
      if (error) {
        this.handleError(error);
      } else {
        this.session.publish(this.publisher, this.handleError);
      }
    });

    // Subscribe to a newly created stream
    this.session.on('streamCreated', (event) => {
      this.handleNewStreamCration(event.stream);
    });

    this.session.on('streamDestroyed', (stream) => {
      this.hadnleStreamDisconnection(stream.stream);
    });
  }

  initializeScreenSharing(share: boolean) {
    this.screenSharing = share;

    if (!this.screenSharing) {
      this.stopScreenSharing();
    }
    else {
      this.startScreenSharing();
    }
  }

  stopScreenSharing() {
    if (this.screenPublisher) {
      this.screenPublisher.destroy();
      this.screenPublisher = null;
      this.screenSharing = false;
    }
  }

  startScreenSharing() {
    if (this.screenPublisher) {
      return;
    }

    const hiddenElement = this.renderer.createElement('screenShare');
    this.renderer.addClass(hiddenElement, "hide");
    this.screenPublisher = OT.initPublisher(hiddenElement,
      {
        videoSource: 'screen',
        width: '100%',
        height: '100%',
      },
      (error) => {
        if (error) {
        } else {
          this.session.publish(this.screenPublisher, error => {
            if (error) {
              console.log(error.message);
            }
          });
        }
      });

    this.screenPublisher.on('streamCreated', event => {
      this.handleNewStreamCration(event.stream);
    });

    this.screenPublisher.on('streamDestroyed', (stream) => {
      this.hadnleStreamDisconnection(stream.stream);
    });
  }

  addActiveStramIfNotEnought() {
    if (this.activeStreamVideos.length < 2 && this.allStreams.length > this.activeStreamVideos.length) {
      const streamToSubscribe = this.allStreams.filter(s => s.stream.hasVideo && !this.activeStreamVideos.includes(s.stream.streamId))[0];

      if (this.subscriberDiv) {
        // Create a new div container
        this.renderer.removeChild(this.subscriberDiv, streamToSubscribe.element);

        const newContainer = this.createNewSubscriberElement(streamToSubscribe.stream.streamId);

        const subscriber = this.session.subscribe(streamToSubscribe.stream, newContainer, {
          insertMode: 'replace',
          width: '100%',
          height: '100%',

        }, this.handleError);

        this.handleNewSubscriber(subscriber);

        streamToSubscribe.subscriber = subscriber;
        streamToSubscribe.element = newContainer;

        this.activeStreamVideos.push(streamToSubscribe.stream.streamId);
      }
    }
  }

  createNewSubscriberElement(streamId: string): HTMLElement {
    const newContainer = this.renderer.createElement('div');

    // Append the new div inside 'subscriber'
    this.renderer.appendChild(this.subscriberDiv, newContainer);

    this.renderer.listen(newContainer, 'click', () => {
      this.selectStream(streamId); // Call selectStream with the streamId
    });

    return newContainer;
  }

  handleError(error: OTError) {
    if (error) {
      alert(error.message);
    }
  }

  handleNewStreamCration(stream: OT.Stream) {
    const newContainer = this.renderer.createElement('div');
    this.renderer.addClass(newContainer, 'video-container');
    this.renderer.addClass(newContainer, 'hide');

    // Append the new div inside 'subscriber'
    this.renderer.appendChild(this.subscriberDiv, newContainer);

    const subscriber = this.session.subscribe(stream, newContainer, {
      insertMode: 'replace',
      width: '100%',
      height: '100%',
      subscribeToVideo: false
    }, this.handleError);

    const activeStream: ActiveStream = {
      stream: stream,
      subscriber: subscriber,
      element: newContainer
    };

    this.allStreams.push(activeStream);
    this.addActiveStramIfNotEnought();
    this.updateStreamLayout();  // Call function to update layout
  }

  updateStreamLayout() {
    const totalStreams = this.allStreams.filter(s => this.activeStreamVideos.includes(s.stream.streamId) && !s.element.classList.contains('hide')).length;

    this.renderer.removeClass(this.subscriberDiv, 'one-stream');
    this.renderer.removeClass(this.subscriberDiv, 'two-streams');
    this.renderer.removeClass(this.subscriberDiv, 'three-streams');
    this.renderer.removeClass(this.subscriberDiv, 'four-streams');
    this.renderer.removeClass(this.subscriberDiv, 'multi-streams');

    if (totalStreams === 1) {
      this.renderer.addClass(this.subscriberDiv, 'one-stream');
      this.renderer.setStyle(this.subscriberDiv, 'height', '100%');
    } else if (totalStreams === 2) {
      this.renderer.addClass(this.subscriberDiv, 'two-streams');
      this.renderer.setStyle(this.subscriberDiv, 'height', '100%');
    } else if (totalStreams === 3) {
      this.renderer.addClass(this.subscriberDiv, 'three-streams');
      this.renderer.setStyle(this.subscriberDiv, 'height', '100%');
    } else if (totalStreams === 4) {
      this.renderer.addClass(this.subscriberDiv, 'four-streams');
      this.renderer.setStyle(this.subscriberDiv, 'height', '100%');
    } else {
      this.renderer.addClass(this.subscriberDiv, 'multi-streams');
      this.renderer.setStyle(this.subscriberDiv, 'height', 'auto');
    }
  }

  hadnleStreamDisconnection(stream: OT.Stream) {
    if (stream.hasVideo) {
      this.handleVideoDisable(stream.streamId);
    }
    this.allStreams = this.allStreams.filter(s => s.stream.streamId !== stream.streamId);
  }

  handleVideoDisable(streamId: string) {
    const element = this.allStreams.find(s => s.stream.streamId === streamId)?.element;

    if (element) {
      this.allStreams = this.allStreams.filter(s => s.stream.streamId !== streamId);
      element.classList.add("hide");
      this.activeStreamVideos = this.activeStreamVideos.filter(s => s !== streamId);
      this.addActiveStramIfNotEnought();
      this.updateStreamLayout();
    }

    if (this.screenPublisher.stream.streamId === streamId) {
      this.stopScreenSharing();
    }
  }

  handleNewSubscriber(subscriber: OT.Subscriber) {
    subscriber.on("videoEnabled", (event) => {
      const index = this.allStreams.findIndex(x => x.stream.streamId === event.target.stream.streamId);
      if (index >= 0) {
        this.allStreams[index].stream = event.target.stream;
      }

      this.addActiveStramIfNotEnought();
    });

    subscriber.on("videoDisabled", (event) => {
      const index = this.allStreams.findIndex(x => x.stream.streamId === event.target.stream.streamId);
      if (index >= 0) {
        this.allStreams[index].stream = event.target.stream;
      }

      this.handleVideoDisable(event.target.stream.streamId);

      this.addActiveStramIfNotEnought();
    });
  }

  selectStream(streamId: string) {
    if (this.selectedStream !== null) {
      this.selectedStream = null;

      for (let stream of this.allStreams.filter(s => this.activeStreamVideos.includes(s.stream.streamId) && s.stream.streamId)) {
        stream.element.classList.remove('hide');
      }
    }
    else {
      this.selectedStream = streamId;

      for (let stream of this.allStreams.filter(s => s.stream.streamId != streamId && this.activeStreamVideos.includes(s.stream.streamId) && s.stream.streamId)) {
        stream.element.classList.add('hide');
      }
    }

    this.updateStreamLayout();
  }

  logout() {
    this.publisher.destroy()
    this.session.disconnect();
    this.onLogout.emit();
  }
}
