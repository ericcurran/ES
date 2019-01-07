import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { RequestsTableDataSource } from './requests-table-datasource';
import { RequestsService } from './requests-table.service';

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
  displayedColumns = ['id', 'name'];

  constructor(private http:RequestsService) {    

  }

  ngOnInit() {
    this.dataSource = new RequestsTableDataSource(this.paginator, this.sort, this.http);
  }
}
