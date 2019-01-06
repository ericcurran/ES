import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';

import { AppNavComponent } from './app-nav/app-nav.component';
import { LayoutModule } from '@angular/cdk/layout';

import { MatToolbarModule, MatButtonModule, MatSidenavModule, MatIconModule,
         MatListModule, MatTableModule, MatPaginatorModule, MatSortModule,
         MatGridListModule, MatCardModule, MatMenuModule} from '@angular/material';

import { RequestsTableComponent } from './requests-table/requests-table.component';
import { RecordsTableComponent } from './records-table/records-table.component';
import { HomeComponent } from './home/home.component';
import { RequestsService } from './requests-table/requests-table.service';


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
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'requests', component: RequestsTableComponent, pathMatch: 'full' },
      { path: 'records', component: RecordsTableComponent, pathMatch: 'full' }
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
  providers: [RequestsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
