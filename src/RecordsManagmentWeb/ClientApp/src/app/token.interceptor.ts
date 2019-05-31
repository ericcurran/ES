import { Injectable } from '@angular/core';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor
} from '@angular/common/http';
import { MsAdalAngular6Service } from 'microsoft-adal-angular6';

import { Observable } from 'rxjs';
import { mergeMap } from 'rxjs/operators';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(private adal: MsAdalAngular6Service) { }

    public accessToken: string;

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
                
        const resource = this.adal.GetResourceForEndpoint(request.url);
        if (!resource || !this.adal.isAuthenticated) {
            return next.handle(request);
        }
        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ${this.accessToken}`
            }
        });
        return this.adal.acquireToken(resource)
            .pipe(
                mergeMap((token: string) => {
                    const authorizedRequest = request.clone({
                        headers: request.headers.set('Authorization', `Bearer ${token}`),
                    });
                    return next.handle(authorizedRequest);
                }));
    }
}
