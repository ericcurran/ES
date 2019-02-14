import { AppHttpService } from '../app-http.service';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { RecordsTableDataSource } from './records-table-datasource';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-records-table',
  templateUrl: './records-table.component.html',
  styleUrls: ['./records-table.component.scss']
})
export class RecordsTableComponent implements OnInit, AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  dataSource: RecordsTableDataSource;
  requestId: number;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['id', 'fileName', 'status', 'inScope', 'esRef'];

  constructor(private http: AppHttpService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.getRequestIdFromRoute();
    this.dataSource = new RecordsTableDataSource(this.paginator, this.sort, this.http);
  }


  ngAfterViewInit(): void {
    this.dataSource.loadRecords(this.requestId, this.sort.direction, this.paginator.pageIndex, this.paginator.pageSize);
    this.paginator.page
      .pipe(
        tap(() => this.dataSource.loadRecords(this.requestId, this.sort.direction, this.paginator.pageIndex, this.paginator.pageSize))
      )
      .subscribe();
  }

  onDetailsLinkCLick(fileName: string) {
    window.open(`https://casedocuments.blob.core.windows.net/documents-test/${fileName}`, '_blank');
  }


  private getRequestIdFromRoute() {
    let id = Number.parseInt(this.route.snapshot.params['id']);
    if (Number.isNaN(id)) {
      id = 0;
    }
    this.requestId = id;
  }


}
