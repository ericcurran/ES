import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class RequestsService {

    private readonly baseUrl = 'https://localhost:5001';

    constructor(private http: HttpClient) {
    }

    getRequests(){
        return this.http.get('api/requests');
    }
}
