import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AppHttpService } from 'src/app/app-http.service';
import { RecordDoc } from '../record';
import { RecordStatusEnum } from '../record-status';

@Component({
  selector: 'app-record-form',
  templateUrl: './record.component.html',
  styleUrls: ['./record.component.scss']
})
export class RecordComponent implements OnInit {
  record:RecordDoc;
  recordForm:FormGroup;
  statuses = [1, 2, 3];

  constructor(private fb: FormBuilder, 
    private route: ActivatedRoute, 
    private router: Router, 
    private http: AppHttpService) { }

  ngOnInit(): void {
    const id = this.route.snapshot.params.id;
    if (id) {
      this.http.getRecord(id)
        .subscribe((r: RecordDoc) => {
          this.record = r;
          this.initForm(r);
        },
          () => {

          });
    }
  }

  initForm(r: RecordDoc): any {
    this.recordForm = this.fb.group({
      status: r.status,
      inScope: r.inScope,
      inLog: r.inLog,
      esRef: r.esRef,
      startDate: r.startDate,
      log: r.log,
      phase: r.phase
    });
  }

  onSubmit() {
    if (this.recordForm) {
      this.record.status = this.recordForm.controls['status'].value;
      this.record.inScope = this.recordForm.controls['inScope'].value;
      this.record.inLog = this.recordForm.controls['inLog'].value;
      this.record.esRef = this.recordForm.controls['esRef'].value;
      this.record.startDate = this.recordForm.controls['startDate'].value;
      this.record.log = this.recordForm.controls['log'].value;
      this.record.phase = this.recordForm.controls['phase'].value;
      this.http.putRecord(this.record)
        .subscribe(() => {
          this.router.navigate(['/records', { id: this.record.requestPackageId }]);
        });

    }
  }
}
