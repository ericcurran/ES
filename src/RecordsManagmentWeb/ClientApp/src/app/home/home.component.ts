import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MsAdalAngular6Service } from 'microsoft-adal-angular6';
import { TokenInterceptor } from '../token.interceptor';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  
  /** Based on the screen size, switch from standard to one column per row */
  cards = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(({ matches }) => {
      if (matches) {
        return [ { title: 'Card 1', cols: 1, rows: 1 } ];
      }

      return [{ title: 'Card 1', cols: 2, rows: 1 }];
    })
  );

  constructor(private breakpointObserver: BreakpointObserver,
    private adal: MsAdalAngular6Service,
    private interceptor: TokenInterceptor) { }

  ngOnInit(): void {
    if (this.adal.isAuthenticated && !this.interceptor.accessToken) {
      this.adal.acquireToken(environment.webApiEndpoint).subscribe((t) => {
        this.interceptor.accessToken = t;
      });
    }
  }
}
