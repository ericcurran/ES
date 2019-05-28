import { Component, OnInit } from '@angular/core';
import { RequestDoc } from '../request';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppHttpService } from 'src/app/app-http.service';
import { RequestsTableDataSource } from '../requests-table-datasource';
import { RequestHelper } from '../request-helper';

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.scss']
})
export class RequestComponent implements OnInit {

  helper:RequestHelper;

  constructor(private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private http: AppHttpService) { }

  request: RequestDoc;
  requestForm: FormGroup;
  statuses: Array<{ value: number; text: string }>;


  ngOnInit() {
    this.helper = new RequestHelper();
    this.statuses = this.helper.requestStatuses;

    const id = this.route.snapshot.params.id;
    if (id) {
      this.http.getRequest(id)
        .subscribe((r: RequestDoc) => {
          this.request = r;
          this.initForm(r);
        },
          () => {

          });
    }
  }

  initForm(r: RequestDoc): any {
    this.requestForm = this.fb.group({
      deatilsFileName: new FormControl({ value: r.deatilsFileName, disabled: true }),
      requestId: new FormControl({ value: r.requestId, disabled: true }),
      claimNumber: new FormControl({ value: r.claimNumber, disabled: true }),
      created: new FormControl({ value: this.helper.formatDate(r.created), disabled: true }),
      esRef: r.esRef,
      status: r.status,
      insuredName: r.insuredName,
      dateOfLoss: r.dateOfLoss,
      dateOfService: r.dateOfService,
      phase: r.phase,
      amount: r.amount
    });
  }

  onSubmit() {
    if (this.requestForm) {

      this.request.esRef = this.requestForm.controls['esRef'].value;
      this.request.status = this.requestForm.controls['status'].value;
      this.request.insuredName = this.requestForm.controls['insuredName'].value;
      this.request.phase = this.requestForm.controls['phase'].value;
      this.request.amount = this.requestForm.controls['amount'].value;

      this.request.dateOfLoss = this.helper.getIsoDateString(this.requestForm.controls['dateOfLoss'].value);
      this.request.dateOfService = this.helper.getIsoDateString(this.requestForm.controls['dateOfService'].value);

      this.http.putRequest(this.request)
        .subscribe(() => {
          this.router.navigate(['/requests']);
        });

    }
  }

}
