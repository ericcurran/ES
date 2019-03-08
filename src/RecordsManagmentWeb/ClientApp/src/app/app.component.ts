import { MsAdalAngular6Service } from 'microsoft-adal-angular6';
import { Component, OnInit } from '@angular/core';
import { environment } from '../environments/environment';
import { TokenInterceptor } from './token.interceptor';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  
  title = 'app';
  constructor() {

  }

}
