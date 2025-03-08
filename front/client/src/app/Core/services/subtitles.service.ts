import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SubtitlesService {

  constructor() { }

  extractSubtitles(track: TextTrack): string {
    let subtitles = '';

    for (let cue of track.cues as any) {
      subtitles += cue.text + ' '; // Append text with spacing
    }

    return subtitles.trim(); // Remove extra spaces
  }
}
