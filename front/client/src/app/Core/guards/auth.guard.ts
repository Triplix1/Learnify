import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { map, of, switchMap } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AuthService);

  return accountService.tokenData$.pipe(
    switchMap(user => {
      console.log(new Date(user?.expires.getTime()))
      console.log(new Date(Date.now()))

      if (user !== null && user !== undefined) {
        if (user.expires.getTime() > Date.now())
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
