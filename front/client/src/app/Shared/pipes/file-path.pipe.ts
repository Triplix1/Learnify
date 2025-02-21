import { HttpClient } from '@angular/common/http';
import { Pipe, PipeTransform } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';

@Pipe({
  name: 'filePath'
})
export class FilePathPipe implements PipeTransform {
  constructor(private http: HttpClient) { }

  transform(fileUrl: string | null): Observable<File | null> {
    if (!fileUrl) return of(null);
    // Fetch subtitle as a Blob and create a local URL
    return this.http.get(fileUrl, { responseType: 'blob' }).pipe(
      map(blob => {
        return new File([blob], "name")
      }),
      catchError(error => {
        console.error('Error fetching subtitle:', error);
        return of(null);
      })
    );
  }
}