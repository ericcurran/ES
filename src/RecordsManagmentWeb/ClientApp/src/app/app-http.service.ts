import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SortDirection } from '@angular/material';
import { RecordDoc } from './records-table/record';
import { RequestDoc } from './requests-table/request';

@Injectable()
export class AppHttpService {

    constructor(private http: HttpClient) {
    }

    getRequests() {
        return this.http.get('/api/request');
    }

    getRecords(requestId: number = 0, sortDir: SortDirection, page: number, size: number) {

        const params = new HttpParams()
            .set('requestId', requestId.toString())
            .set('sortDir', sortDir)
            .set('page', page.toString())
            .set('size', size.toString());
        return this.http.get('/api/record', { params });
    }

    generateReport(id: number) {
        return this.http.post(`/api/pack/${id}`, undefined);
    }

    getRecord(id: any): any {
        return this.http.get(`/api/record/${id}`);
    }

    getRequest(id: any): any {
        return this.http.get(`/api/request/${id}`);
    }

    testNodeService() {
        return this.http.get(`/api/pack/test`);
    }

    putRecord(row: RecordDoc) {
        return this.http.put(`/api/record/${row.id}`, row);
    }

    putRequest(request: RequestDoc, updateEf: boolean) {
        return this.http.put(`/api/request/${request.id}`, { request: request, updateEf: updateEf });
    }
}


