import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { RecordsTableDataSource } from './records-table-datasource';

@Component({
  selector: 'app-records-table',
  templateUrl: './records-table.component.html',
  styleUrls: ['./records-table.component.scss']
})
export class RecordsTableComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  dataSource: RecordsTableDataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['id', 'name'];

  ngOnInit() {
    this.dataSource = new RecordsTableDataSource(this.paginator, this.sort);
  }
}
