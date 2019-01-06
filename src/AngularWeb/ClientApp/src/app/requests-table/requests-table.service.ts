import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class RequestsService {

    private readonly baseUrl= 'https://';

    constructor(private http: HttpClient) {
    }

    getRequests(){

        return this.http.get()

    }

    
}
