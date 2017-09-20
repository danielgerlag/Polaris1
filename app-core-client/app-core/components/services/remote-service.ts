import {Inject, Injectable, Injector, Component, provide} from 'angular2/core';
import {BaseRequestOptions, Http, Headers, Response} from 'angular2/http';
import {Observable, Subscription} from 'rxjs/Rx';
import {IConfigService, ConfigService} from './config-service';
import {TransientInfo} from './auth-service';


export abstract class IRemoteService {
    abstract post(sender: any, url: string, data: any, callback: (sender: any, data: any, status: number) => any): Subscription<any>;
    abstract put(sender: any, url: string, data: any, callback: (sender: any, data: any, status: number) => any): Subscription<any>;
    abstract get(sender: any, url: string, callback: (sender: any, data: any, status: number) => any): Subscription<any>;
    abstract fetchBlob(sender: any, url: string, data: any, callback: (sender: any, data: any, status: number) => any);

    public userInfo: TransientInfo;

}

@Injectable()
export class RemoteService implements IRemoteService {

    public userInfo: TransientInfo;
    private http: Http;
    private apiPath: string;

    constructor(
        @Inject(Http) http: Http,
        @Inject(IConfigService) configService: IConfigService) {
                
        this.http = http;
        this.apiPath = configService.getSettings().api;
        this.userInfo = new TransientInfo();
    }

    post(sender: any, url: string, data: any, callback: (sender: any, data: any, status: number) => any): Subscription<any> {
        var self = this;
        var headers = new Headers();
        headers.append('Accept', 'application/json, text/plain, */*');
        headers.append('Content-Type', 'application/json');

        if (self.userInfo.activeTenant) {
            headers.append("TenantID", self.userInfo.activeTenant.ID.toString());
        }

        var observable = self.http.post(self.apiPath + url, JSON.stringify(data), { headers: headers });
        var subscription = observable.subscribe(
            response => {                
                var responseData: any;
                try {
                    if (response.status != 204)
                        responseData = self.decodeResponse(response);
                }
                catch (ex) {
                    //alert(ex);
                    console.error(ex);
                    //logging
                }
                
                callback(sender, responseData, response.status);
                //subscription.unsubscribe();
            }
        );
        return subscription;
    }

    put(sender: any, url: string, data: any, callback: (sender: any, data: any, status: number) => any): Subscription<any> {
        var self = this;
        var headers = new Headers();
        headers.append('Accept', 'application/json, text/plain, */*');
        headers.append('Content-Type', 'application/json');

        if (self.userInfo.activeTenant) {
            headers.append("TenantID", self.userInfo.activeTenant.ID.toString());
        }

        if (data["odata.etag"])
            headers.append("If-Match", data["odata.etag"]);

        var observable = self.http.put(self.apiPath + url, JSON.stringify(data), { headers: headers });        
        var subscription = observable.subscribe(
            response => {
                var responseData: any;
                try {
                    if (response.status != 204)
                        responseData = self.decodeResponse(response);
                }
                catch (ex) {
                    //logging
                    console.error(ex);
                }
                callback(sender, responseData, response.status);
                //subscription.unsubscribe();
            }
        );
        return subscription;
    }

    get(sender: any, url: string, callback: (sender: any, data: any, status: number) => any): Subscription<any> {
        var self = this;
        var headers = new Headers();
        headers.append('Accept', 'application/json, text/plain, */*');
        headers.append('Content-Type', 'application/json');
        if (self.userInfo.activeTenant) {
            headers.append("TenantID", self.userInfo.activeTenant.ID.toString());
        }

        var observable = self.http.get(self.apiPath + url, { headers: headers });
        var subscription = observable.subscribe(
            response => {
                var responseData: any;
                try {
                    if (response.status != 204)
                        responseData = self.decodeResponse(response);
                }
                catch (ex) {
                    //logging
                    console.error(ex);
                }
                callback(sender, responseData, response.status);
                //subscription.unsubscribe();
            }
        );
        return subscription;
    }


    fetchBlob(sender: any, url: string, data: any, callback: (sender: any, data: any, status: number) => any) {
        var xhr = new XMLHttpRequest();
        xhr.open('POST', this.apiPath + url, true);
        xhr.responseType = 'blob';
        xhr.setRequestHeader('Content-Type', 'application/json');

        if (this.userInfo.activeTenant) {
            xhr.setRequestHeader("TenantID", this.userInfo.activeTenant.ID.toString());
        }

        xhr.onload = function (e) {
            if (this.status == 200) {
                callback(sender, this.response, this.status);
            }
        };
        xhr.send(JSON.stringify(data));
    }




    private decodeResponse(response: Response) {
        var contentType = response.headers.get('Content-Type');

        if (contentType) {

            if (contentType.startsWith("application/json")) {
                return response.json();
            }

            if (contentType.startsWith("application/pdf")) {
                return response.text;
            }
        }

        return response.text();
    }
}
