import {provide} from 'angular2/core';
import {Http, BaseRequestOptions, RequestOptions} from 'angular2/http';
import {ConnectionBackend, Connection} from 'angular2/http';
import {ReadyState, RequestMethod, ResponseType} from 'angular2/http';
import {Request} from 'angular2/http';
import {Response} from 'angular2/http';
import {Headers} from 'angular2/http';
import {ResponseOptions, BaseResponseOptions} from 'angular2/http';
import {Injectable} from 'angular2/core';
import {BrowserXhr} from 'angular2/http';
import {isPresent} from 'angular2/src/facade/lang';
import {Observable} from 'rxjs/Observable';


//Drop in replacement for XHRConnection from ng2.  The original class has governance issues

function getResponseURL(xhr: any): string {
    if ('responseURL' in xhr) {
        return xhr.responseURL;
    }
    if (/^X-Request-URL:/m.test(xhr.getAllResponseHeaders())) {
        return xhr.getResponseHeader('X-Request-URL');
    }
    return;
}

export class XHRConnection2 implements Connection {
    request: Request;
    /**
     * Response {@link EventEmitter} which emits a single {@link Response} value on load event of
     * `XMLHttpRequest`.
     */
    response: Observable<Response>;
    readyState: ReadyState;
    constructor(req: Request, browserXHR: BrowserXhr, baseResponseOptions?: ResponseOptions) {
        this.request = req;
        this.response = new Observable(responseObserver => {
            let _xhr: XMLHttpRequest = browserXHR.build();

            _xhr.open(RequestMethod[req.method].toUpperCase(), req.url);
            // load event handler
            let onLoad = () => {
                // responseText is the old-school way of retrieving response (supported by IE8 & 9)
                // response/responseType properties were introduced in XHR Level2 spec (supported by
                // IE10)
                let body = isPresent(_xhr.response) ? _xhr.response : _xhr.responseText;

                let headers = Headers.fromResponseHeaderString(_xhr.getAllResponseHeaders());

                let url = getResponseURL(_xhr);

                // normalize IE9 bug (http://bugs.jquery.com/ticket/1450)
                let status: number = _xhr.status === 1223 ? 204 : _xhr.status;

                // fix status code when it is 0 (0 status is undocumented).
                // Occurs when accessing file resources or on Android 4.1 stock browser
                // while retrieving files from application cache.
                if (status === 0) {
                    status = body ? 200 : 0;
                }
                var responseOptions = new ResponseOptions({ body, status, headers, url });
                if (isPresent(baseResponseOptions)) {
                    responseOptions = baseResponseOptions.merge(responseOptions);
                }
                let response = new Response(responseOptions);

                responseObserver.next(response);
                // TODO(gdi2290): defer complete if array buffer until done
                responseObserver.complete();
                return;

            };
            // error event handler
            let onError = (err) => {
                var responseOptions = new ResponseOptions({ body: err, type: ResponseType.Error });
                if (isPresent(baseResponseOptions)) {
                    responseOptions = baseResponseOptions.merge(responseOptions);
                }
                responseObserver.error(new Response(responseOptions));
            };

            if (isPresent(req.headers)) {
                req.headers.forEach((values, name) => _xhr.setRequestHeader(name, values.join(',')));
            }

            _xhr.addEventListener('load', onLoad);
            _xhr.addEventListener('error', onError);

            _xhr.send(this.request.text());

            return () => {
                _xhr.removeEventListener('load', onLoad);
                _xhr.removeEventListener('error', onError);
                _xhr.abort();
            };
        });
    }
}

/**
 * Creates {@link XHRConnection} instances.
 *
 * This class would typically not be used by end users, but could be
 * overridden if a different backend implementation should be used,
 * such as in a node backend.
 *
 * ### Example
 *
 * ```
 * import {Http, MyNodeBackend, HTTP_PROVIDERS, BaseRequestOptions} from 'angular2/http';
 * @Component({
 *   viewProviders: [
 *     HTTP_PROVIDERS,
 *     provide(Http, {useFactory: (backend, options) => {
 *       return new Http(backend, options);
 *     }, deps: [MyNodeBackend, BaseRequestOptions]})]
 * })
 * class MyComponent {
 *   constructor(http:Http) {
 *     http.request('people.json').subscribe(res => this.people = res.json());
 *   }
 * }
 * ```
 *
 **/
@Injectable()
export class XHRBackend2 implements ConnectionBackend {

    constructor(private _browserXHR: BrowserXhr, private _baseResponseOptions: ResponseOptions) { }

    createConnection(request: Request): XHRConnection2 {
        return new XHRConnection2(request, this._browserXHR, this._baseResponseOptions);
    }
}


export const HTTP_PROVIDERS: any[] = [
    provide(Http,
        {
            useFactory: (xhrBackend, requestOptions) => new Http(xhrBackend, requestOptions),
            deps: [XHRBackend2, RequestOptions]
        }),
    BrowserXhr,
    provide(RequestOptions, { useClass: BaseRequestOptions }),
    provide(ResponseOptions, { useClass: BaseResponseOptions }),
    XHRBackend2
];