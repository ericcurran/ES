import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SortDirection } from '@angular/material';
import { RecordDoc } from './records-table/record';
import { RequestDoc } from './requests-table/request';
import { tap } from 'rxjs/operators';

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

    downloadFile(fileName: string) {

        const options = {
            responseType: 'blob' as 'json'

        };
        this.http.get(`api/Storage/${fileName}`, options)
            .subscribe((result) => {
                this.showFile(result, fileName);
            },
                e => {
                    console.log(e);
                });
    }

    showFile(x: any, fileName: string) {

        const blobType: string = this.GetBlobType(fileName);


        // It is necessary to create a new blob object with mime-type explicitly set
        // otherwise only Chrome works like it should
        const newBlob = new Blob([x], { type: blobType, });

        // IE doesn't allow using a blob object directly as link href
        // instead it is necessary to use msSaveOrOpenBlob
        if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(newBlob);
            return;
        }

        // For other browsers:
        // Create a link pointing to the ObjectURL containing the blob.
        const data = window.URL.createObjectURL(newBlob);
        window.open(data, '_blank');
    }
    GetBlobType(fileName: string): string {
        const nameParts = fileName.split('.');
        if (nameParts && nameParts.length > 1) {
            switch (nameParts[nameParts.length - 1]) {
                case 'pdf':
                    return 'application/pdf';

                case 'tiff':
                case 'ttf':
                    return 'image/tiff';

                case 'png':
                    return 'image/png';

                case 'jpg':
                case 'jpeg':
                    return 'image/jpeg';
            }
        }
        return 'application/octet-stream';
    }
}


