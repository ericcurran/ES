import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MsAdalAngular6Module, AuthenticationGuard } from 'microsoft-adal-angular6';

import { AppComponent } from './app.component';

import { AppNavComponent } from './app-nav/app-nav.component';
import { LayoutModule } from '@angular/cdk/layout';

import { MatToolbarModule, MatButtonModule, MatSidenavModule, MatIconModule,
         MatListModule, MatTableModule, MatPaginatorModule, MatSortModule,
         MatGridListModule, MatCardModule, MatMenuModule, MatCheckboxModule,
         MatInputModule, MatOptionModule, MatSelectModule, MatRadioModule, 
         MatDatepickerModule, MatNativeDateModule} from '@angular/material';

import { RequestsTableComponent } from './requests-table/requests-table.component';
import { RecordsTableComponent } from './records-table/records-table.component';
import { HomeComponent } from './home/home.component';
import { AppHttpService } from './app-http.service';
import { environment } from '../environments/environment';
import { TokenInterceptor } from './token.interceptor';
import { RecordComponent } from './records-table/record/record.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AppNavComponent,
    RequestsTableComponent,
    RecordsTableComponent,
    RecordComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MsAdalAngular6Module.forRoot({
      tenant: environment.azureAdTenantId,
      clientId: environment.azureAdClientId,
      redirectUri: window.location.origin,
      endpoints: environment.endpoints,
      navigateToLoginRequestUrl: false,
      cacheLocation: 'localStorage',
    }),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthenticationGuard] },
      { path: 'requests', component: RequestsTableComponent, pathMatch: 'full', canActivate: [AuthenticationGuard] },
      { path: 'records', component: RecordsTableComponent , canActivate: [AuthenticationGuard]},
      { path: 'record/:id', component: RecordComponent , canActivate: [AuthenticationGuard]},
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
    MatMenuModule,
    MatCheckboxModule,
    MatInputModule,
    MatOptionModule,
    MatSelectModule, 
    MatRadioModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  providers: [AuthenticationGuard, AppHttpService, TokenInterceptor,
    {
      provide: HTTP_INTERCEPTORS,
      useExisting: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
