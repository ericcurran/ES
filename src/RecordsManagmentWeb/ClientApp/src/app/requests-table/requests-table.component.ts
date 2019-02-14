import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { RequestsTableDataSource } from './requests-table-datasource';
import { AppHttpService } from '../app-http.service';

@Component({
  selector: 'app-requests-table',
  templateUrl: './requests-table.component.html',
  styleUrls: ['./requests-table.component.scss']
})
export class RequestsTableComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  dataSource: RequestsTableDataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['id', 'deatilsFileName', 'zipFileName', 'status', 'esRef', 'requestPack', 'pdfPackName'];

  constructor(private http: AppHttpService) {
  }

  ngOnInit() {
    this.dataSource = new RequestsTableDataSource(this.paginator, this.sort, this.http);
  }

  onDetailsLinkCLick(deatilsFileName: string) {
    window.open(`https://casedocuments.blob.core.windows.net/documents-test/${deatilsFileName}`, '_blank');
  }

  onGenerateClick(id: number) {
    this.http.generateReport(id)
      .subscribe(() => {
        window.open(`https://casedocuments.blob.core.windows.net/documents-test/input.pdf`, '_blank');
      });
  }

  onGetPdfPack(fileName: string) {
    window.open(`https://casedocuments.blob.core.windows.net/documents-test/${fileName}`, '_blank');
  }
}
