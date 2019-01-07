import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MsAdalAngular6Module, AuthenticationGuard } from 'microsoft-adal-angular6';

import { AppComponent } from './app.component';

import { AppNavComponent } from './app-nav/app-nav.component';
import { LayoutModule } from '@angular/cdk/layout';

import { MatToolbarModule, MatButtonModule, MatSidenavModule, MatIconModule,
         MatListModule, MatTableModule, MatPaginatorModule, MatSortModule,
         MatGridListModule, MatCardModule, MatMenuModule} from '@angular/material';

import { RequestsTableComponent } from './requests-table/requests-table.component';
import { RecordsTableComponent } from './records-table/records-table.component';
import { HomeComponent } from './home/home.component';
import { AppHttpService } from './app-http.service';
import { environment } from '../environments/environment';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AppNavComponent,
    RequestsTableComponent,
    RecordsTableComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    MsAdalAngular6Module.forRoot({
      tenant: environment.azureAdTenantId,
      clientId: environment.azureAdClientId,
      redirectUri: window.location.origin,
      endpoints: {
        'https://localhost:5001/api/': '10204d32-ea7b-4608-81eb-e35ff52d4428'
      },
      navigateToLoginRequestUrl: false,
      cacheLocation: 'localStorage',
    }),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthenticationGuard] },
      { path: 'requests', component: RequestsTableComponent, pathMatch: 'full' },
      { path: 'records', component: RecordsTableComponent }
    ]),
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule
  ],
  providers: [AuthenticationGuard, AppHttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }
