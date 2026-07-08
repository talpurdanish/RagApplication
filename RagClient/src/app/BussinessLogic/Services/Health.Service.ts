// api-health.service.ts
import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  Observable,
  Subject,
  timer,
  switchMap,
  catchError,
  of,
  map,
  distinctUntilChanged,
  shareReplay,
  takeUntil,
} from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ApiHealthService implements OnDestroy {
  private readonly HEALTH_URL = 'https://localhost:4100/health';
  private readonly POLL_INTERVAL_MS = 10000; // check every 10 seconds

  private destroy$ = new Subject<void>();

  // Emits true/false on every heartbeat, but only re-emits to subscribers when the status actually changes
  public apiStatus$: Observable<boolean> = timer(0, this.POLL_INTERVAL_MS).pipe(
    switchMap(() =>
      this.http.get(this.HEALTH_URL, { responseType: 'text' }).pipe(
        map(() => {
          console.log('Back end api is healthy');
          return true;
        }),
        catchError(() => {
          console.error('Back end api is not healthy');
          return of(false);
        }),
      ),
    ),
    distinctUntilChanged(),
    shareReplay(1), // new subscribers immediately get the latest known status
    takeUntil(this.destroy$),
  );

  constructor(private http: HttpClient) {}

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
