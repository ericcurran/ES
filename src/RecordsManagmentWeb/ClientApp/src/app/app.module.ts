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
         MatDatepickerModule, MatDialogModule } from '@angular/material';
import { MatMomentDateModule, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { RequestsTableComponent } from './requests-table/requests-table.component';
import { RecordsTableComponent } from './records-table/records-table.component';
import { HomeComponent } from './home/home.component';
import { AppHttpService } from './app-http.service';
import { environment } from '../environments/environment';
import { TokenInterceptor } from './token.interceptor';
import { RecordComponent } from './records-table/record/record.component';
import { RequestComponent, ConfirmComponent } from './requests-table/request/request.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AppNavComponent,
    RequestsTableComponent,
    RecordsTableComponent,
    RecordComponent,
    RequestComponent,
    ConfirmComponent
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
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [] },
      { path: 'requests', component: RequestsTableComponent, pathMatch: 'full', canActivate: [] },
      { path: 'records', component: RecordsTableComponent , canActivate: []},
      { path: 'request/:id', component: RequestComponent , canActivate: []},
      { path: 'record/:id', component: RecordComponent , canActivate: []},
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
    MatMomentDateModule,
    MatDialogModule
  ],
  providers: [AuthenticationGuard, AppHttpService, TokenInterceptor,
    {
      provide: HTTP_INTERCEPTORS,
      useExisting: TokenInterceptor,
      multi: true
    },
    {provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true }}
  ],
  bootstrap: [AppComponent],
  entryComponents: [ConfirmComponent]
})
export class AppModule { }
