import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { RequestsTableDataSource } from './requests-table-datasource';
import { AppHttpService } from '../app-http.service';
import { RequestHelper } from './request-helper';

@Component({
  selector: 'app-requests-table',
  templateUrl: './requests-table.component.html',
  styleUrls: ['./requests-table.component.scss']
})
export class RequestsTableComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  
  dataSource: RequestsTableDataSource;
  helper:RequestHelper;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['request', 'created', 'esRef', 'status', 'requestId', 'claimNumber',
  'insured', 'dateOfLoss', 'dateOfService', 'amount', 'phase', 'requestPack', 'pdfPackName',
   'open', 'recordsLink'];

  constructor(private http: AppHttpService) {
  }

  ngOnInit() {
    this.dataSource = new RequestsTableDataSource(this.paginator, this.sort, this.http);
    this.helper = new RequestHelper();
  }

  onDetailsLinkCLick(deatilsFileName: string) {
    window.open(`https://casedocuments.blob.core.windows.net/documents-test/${deatilsFileName}`, '_blank');
  }

  onGenerateClick(id: number) {
    this.http.generateReport(id)
      .subscribe((fileName) => {
        window.open(`https://casedocuments.blob.core.windows.net/documents-test/${fileName}`, '_blank');
      });
  }

  onGetPdfPack(fileName: string) {
    window.open(`https://casedocuments.blob.core.windows.net/documents-test/${fileName}`, '_blank');
  }
}
