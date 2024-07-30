import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ImageService {

    constructor(private http: HttpClient) { }

    getImage(url: string): Observable<Blob> {
        return this.http.get(url, { responseType: 'blob' });
    }

    mimeToExtensionMap: { [key: string]: string } = {
        'image/jpeg': 'jpg',
        'image/png': 'png',
        'image/gif': 'gif',
        'application/pdf': 'pdf',
        // Add other mappings as needed
    };

    getExtensionFromMimeType(mimeType: string): string | undefined {
        return this.mimeToExtensionMap[mimeType];
    }
}