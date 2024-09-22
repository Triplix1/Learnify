import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { map, of, switchMap } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AuthService);

  return accountService.tokenData$.pipe(
    switchMap(user => {
      if (user !== null && user !== undefined) {
        if (new Date(user.expires) > new Date(Date.now()))
          return of(user)

        console.log('refreshing token')
        return accountService.refreshToken().pipe(map(x => x.data))
      }

      return of(null);
    }),
    map(u => {
      if (u === null)
        return false;

      return true;
    })
  )
};
