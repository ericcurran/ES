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
      tenant: '1ad5568a-c025-4749-b1d4-868d82da39eb',
      clientId: '9e9241f6-2af7-4f7e-a22b-c732099ffa82',
      redirectUri: 'http://recordsmanagement-sys.azurewebsites.net/requests',
      endpoints: { },
      navigateToLoginRequestUrl: false,
      cacheLocation: 'localStorage',
    }),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthenticationGuard] },
      { path: 'requests', component: RequestsTableComponent, pathMatch: 'full', canActivate: [AuthenticationGuard] },
      { path: 'records', component: RecordsTableComponent , canActivate: [AuthenticationGuard]},
      { path: 'request/:id', component: RequestComponent , canActivate: [AuthenticationGuard]},
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
