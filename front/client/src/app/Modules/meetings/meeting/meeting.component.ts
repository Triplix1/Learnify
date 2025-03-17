import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, Renderer2 } from '@angular/core';
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

  allStreams: ActiveStream[] = [];
  activeStreamVideos: string[] = [];
  session: Session;
  subscriberDiv: HTMLElement;
  publisher: OT.Publisher;

  constructor(private renderer: Renderer2, private el: ElementRef) { }

  ngOnInit(): void {
    this.initializeSession();
  }

  ngAfterViewInit(): void {
    this.subscriberDiv = this.el.nativeElement.querySelector('#subscriber');
  }

  ngOnDestroy(): void {
    if (this.session) {
      this.session.disconnect();
    }
  }

  handleError(error: OTError) {
    if (error) {
      alert(error.message);
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
    const totalStreams = this.activeStreamVideos.length;

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

  addActiveStramIfNotEnought() {
    if (this.activeStreamVideos.length < 2 && this.allStreams.length > this.activeStreamVideos.length) {
      const streamToSubscribe = this.allStreams.filter(s => s.stream.hasVideo && !this.activeStreamVideos.includes(s.stream.streamId))[0];

      if (this.subscriberDiv) {
        // Create a new div container
        this.renderer.removeChild(this.subscriberDiv, streamToSubscribe.element);

        const newContainer = this.renderer.createElement('div');
        this.renderer.addClass(newContainer, 'w-45');
        this.renderer.addClass(newContainer, 'h-fit');

        // Append the new div inside 'subscriber'
        this.renderer.appendChild(this.subscriberDiv, newContainer);

        const subscriber = this.session.subscribe(streamToSubscribe.stream, newContainer, {
          insertMode: 'replace',
          width: '100%',
          height: '100%',
        }, this.handleError);

        this.handleNewSubscriber(subscriber);

        streamToSubscribe.subscriber = subscriber;

        this.activeStreamVideos.push(streamToSubscribe.stream.streamId);
      }
    }
  }

  hadnleStreamDisconnection(stream: OT.Stream) {
    if (stream.hasVideo) {
      this.handleVideoDisable(stream.streamId);
      this.allStreams = this.allStreams.filter(s => s.stream.streamId !== stream.streamId);
    }

    this.addActiveStramIfNotEnought();
    this.updateStreamLayout();
  }

  handleVideoDisable(streamId: string) {
    const element = this.allStreams.find(s => s.stream.streamId === streamId)?.element;

    if (element) {
      element.classList.add("hide");
      this.activeStreamVideos.filter(s => s !== streamId);
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
}
