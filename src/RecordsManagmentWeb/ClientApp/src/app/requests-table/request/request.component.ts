import { Component, OnInit } from '@angular/core';
import { RequestDoc } from '../request';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppHttpService } from 'src/app/app-http.service';

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.scss']
})
export class RequestComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private http: AppHttpService) { }

  request: RequestDoc;
  requestForm: FormGroup;
  statuses = [
    { v: 0, t: 'Unknown' },
    { v: 1, t: 'New' },
    { v: 2, t: 'Unzipped' },
    { v: 3, t: 'Saved to Azure' }];


  ngOnInit() {
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
      status: r.status,
      esRef: r.esRef,
      insuredName: r.insuredName,
      dateOfLoss: r.dateOfLoss,
      dateOfService: r.dateOfService,
      phase: r.phase
    });
  }

  onSubmit() {
    if (this.requestForm) {
      this.request.status = this.requestForm.controls['status'].value;
      this.request.esRef = this.requestForm.controls['esRef'].value;
      this.request.insuredName = this.requestForm.controls['insuredName'].value;
      this.request.dateOfLoss = this.requestForm.controls['dateOfLoss'].value;
      this.request.dateOfService = this.requestForm.controls['dateOfService'].value;
      this.request.phase = this.requestForm.controls['phase'].value;
      this.http.putRequest(this.request)
        .subscribe(() => {
          this.router.navigate(['/requests']);
        });

    }
  }

}
