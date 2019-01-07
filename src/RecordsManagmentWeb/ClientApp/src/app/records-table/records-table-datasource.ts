import { AppHttpService } from '../app-http.service';
import { DataSource } from '@angular/cdk/collections';
import { MatPaginator, MatSort, SortDirection } from '@angular/material';
import { Observable, of as observableOf, merge, BehaviorSubject, of } from 'rxjs';
import { RecordDoc } from './record';
import { catchError, finalize } from 'rxjs/operators';


export class RecordsTableDataSource extends DataSource<RecordDoc> {
  private recordsData = new BehaviorSubject<RecordDoc[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);


  constructor(private paginator: MatPaginator, private sort: MatSort, private http: AppHttpService) {
    super();
  }

  connect(): Observable<RecordDoc[]> {
    return this.recordsData.asObservable();
  }

  disconnect() { }

  loadRecords(id: number, sortDir: SortDirection, page: number, pageSize: number) {
    this.loadingSubject.next(true);

    return this.http.getRecords(id, sortDir, page, pageSize)
      .pipe(
        catchError(() => of([])),
        finalize(() => this.loadingSubject.next(false))
      )
      .subscribe((response: { data: RecordDoc[], count: number }) => {
        this.paginator.length = response.count;
        this.recordsData.next(response.data);
      });
    }

}
