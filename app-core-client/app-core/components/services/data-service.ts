import {Inject, Injectable, Injector, Component, provide} from 'angular2/core';
import {ControlGroup} from 'angular2/common';
//import 'breeze-client';

import {IConfigService, ConfigService} from './config-service';
import {ILogService} from './log-service';
import {IAuthService} from './auth-service';
import {ModelErrorContainer} from '../interfaces';
import {Q} from './q';
import {LoadCollectionArgs} from "../annotations";
import FilterQueryOpSymbol = breeze.FilterQueryOpSymbol;

var OData;
OData = breeze.core.requireLib("OData", "Needed to support remote OData services");


export abstract class IDataService {
    public isReady: boolean;
    public servicePath: string;
    abstract getEntity(sender: any, setName: string, id: number, expand: string, clearCache: boolean, success: (sender: any, data: breeze.QueryResult) => any, failure: (sender: any, data: any) => any);
    abstract getDetached(sender: any, setName: string, id: number, expand: string, success: (sender: any, data: breeze.QueryResult) => any, failure: (sender: any, data: any) => any);
    abstract createEntity(entityType, initialValues): any;
    abstract saveChanges(sender: any, success: (sender: any, data: breeze.SaveResult) => any, failure: (sender: any, data: any) => any);
    abstract detachEntity(entity);
    abstract query(sender: any, query: breeze.EntityQuery, success: (sender: any, data: breeze.QueryResult) => any, failure: (sender: any, data: any) => any);
    abstract reset();
    abstract cacheEntities(sender: any, setName: string, expand: string, success: (sender: any, data: breeze.QueryResult) => any, failure: (sender: any, data: any) => any);
    abstract getCollection(sender: any, setName: string, expand: string, success: (sender: any, data: breeze.QueryResult) => any, failure: (sender: any, data: any) => any);
    abstract loadNavigationGraph(sender: any, entity: breeze.Entity, path: breeze.NavigationProperty, expand: string, failure: (sender: any, data: any) => any);
    abstract getCachedEntities(typeName: string);
    abstract decodeServerModelState(serverState: any): any[];
    abstract validate(validationKey: string, validationContext: ValidationContext): ValidationResponse;
    abstract loadCollections(target: any);
    abstract loadCollection(target: any, property: string);
    abstract createManager() : breeze.EntityManager;
}

function initEntity(entity) {
    entity.init();
}

@Injectable()
export abstract class DataService implements IDataService {


    private rootManager: breeze.EntityManager;
    private manager: breeze.EntityManager;
    private authService: IAuthService;
    private logService : ILogService;
    private configService: IConfigService;

    public isReady: boolean;

    constructor(
        @Inject(IConfigService) configService: IConfigService,
        @Inject(ILogService) logService: ILogService,
        @Inject(IAuthService) authService: IAuthService
    ) {
        var self = this;
        this.isReady = false;
        this.authService = authService;
        this.logService = logService;
        this.configService = configService;
        this.servicePath = this.getServerPath();
        this.configureAdaptor();
        var customMetadata = self.buildCustomMetadata(this.servicePath, self.getNamespace());

        if (customMetadata) {
            this.rootManager = new breeze.EntityManager({serviceName: this.servicePath, metadataStore: customMetadata});
            self.modifyMetadata(self.rootManager.metadataStore);
            self.manager = self.rootManager.createEmptyCopy();
            self.isReady = true;
        } else {
            this.rootManager = new breeze.EntityManager(this.servicePath);
            this.rootManager.fetchMetadata().then((value) => {
                self.modifyMetadata(self.rootManager.metadataStore);
                self.manager = self.rootManager.createEmptyCopy();
                self.isReady = true;
            }, (reason) => {
            });
        }

        var valOpts = this.rootManager.validationOptions.using({ validateOnAttach: false, validateOnPropertyChange: false, validateOnSave: false });
        this.rootManager.setProperties({ validationOptions: valOpts });

    }
    
    protected getServerPath() {
        return this.configService.getSettings().api + this.getRelativePath();
    }

    protected modifyMetadata(metadataStore: breeze.MetadataStore) {

    }

    protected buildCustomMetadata(serviceName: string, namespace: string): breeze.MetadataStore {
        return null;
    }

    protected getNamespace() : string {
        return "";
    }

    protected getBackendFormat(): string {
        return "OData";
    }

    protected configureAdaptor() {
        var self = this;
        breeze.config.setQ(<breeze.promises.IPromiseService>Q);
        breeze.config.initializeAdapterInstance("modelLibrary", "backingStore", true);
        breeze.config.initializeAdapterInstance('dataService', this.getBackendFormat(), true);

        var oldClient = OData.defaultHttpClient;
        OData.defaultHttpClient = {
            request: function(request, success, error) {
                //console.log(JSON.stringify(request));
                var uri: string = request.requestUri;
                if (uri.endsWith("$metadata"))
                    request.headers["Accept"] = "application/xml";

                if (self.authService.userInfo.activeTenant) {
                    request.headers["TenantID"] = self.authService.userInfo.activeTenant.ID;
                }

                return oldClient.request(request, success, error);
            }
        };
    }

    protected abstract getRelativePath(): string;

    reset() {
        this.manager = this.rootManager.createEmptyCopy();
    }

    getEntity(sender: any, setName: string, id: number, expand: string, clearCache: boolean, success: (sender: any, data: breeze.QueryResult) => any, failure: (sender: any, data: any) => any) {
        var query = breeze.EntityQuery.from(setName)
            .where('ID', 'eq', id);

        if (expand)
            query = query.expand(expand);

        if (clearCache)
            this.manager.clear();

        query.using(this.manager).execute()
            .then(function(data) {
                success(sender, data);
            })
            .catch((reason) => {
                failure(sender, reason);
            });
    }

    getDetached(sender: any, setName: string, id: number, expand: string, success: (sender: any, data: breeze.QueryResult) => any, failure: (sender: any, data: any) => any) {
        var query = breeze.EntityQuery.from(setName)
            .where('ID', 'eq', id);

        if (expand)
            query = query.expand(expand);

        query.noTracking().using(this.manager).execute()
            .then(function(data) {
                success(sender, data);
            })
            .catch((reason) => {
                failure(sender, reason);
            });
    }


    createEntity(entityType, initialValues) {
        return this.manager.createEntity(entityType, initialValues);
    }

    detachEntity(entity) {
        this.manager.detachEntity(entity);
    }

    query(sender: any, query: breeze.EntityQuery, success: (sender: any, data: breeze.QueryResult) => any, failure: (sender: any, data: any) => any) {
        query.noTracking().using(this.manager).execute()
            .then(function(data) {
                success(sender, data);
            })
            .catch((reason) => {
                failure(sender, reason);
            });
    }

    saveChanges(sender: any, success: (sender: any, data: breeze.SaveResult) => any, failure: (sender: any, data: any) => any) {
        this.manager.saveChanges()
            .then(function(data) {
                success(sender, data);
            })
            .catch((reason) => {
                failure(sender, reason);
            });
    }

    cacheEntities(sender: any, setName: string, expand: string, success: (sender: any, data: breeze.QueryResult) => any, failure: (sender: any, data: any) => any) {
        var query = breeze.EntityQuery.from(setName)

        if (expand)
            query = query.expand(expand);

        query.using(this.manager).execute()
            .then(function(data) {
                success(sender, data);
            })
            .catch((reason) => {
                failure(sender, reason);
            });
    }

    getCollection(sender: any, setName: string, expand: string, success: (sender: any, data: breeze.QueryResult) => any, failure: (sender: any, data: any) => any) {

        var query = breeze.EntityQuery.from(setName);

        if (expand)
            query = query.expand(expand);

        query.using(this.manager).execute()
            .then(function(data) {
                success(sender, data);
            })
            .catch((reason) => {
                failure(sender, reason);
            });
    }


    loadNavigationGraph(sender: any, entity: breeze.Entity, path: breeze.NavigationProperty, expand: string, failure: (sender: any, data: any) => any) {

        var navQuery = breeze.EntityQuery
            .fromEntityNavigation(entity, path);

        if (expand)
            navQuery = navQuery.expand(expand);

        this.manager.executeQuery(navQuery)
            .catch((reason) => { failure(sender, reason) });

    }

    getCachedEntities(typeName: string) {
        return this.manager.getEntities(typeName);
    }

    decodeServerModelState(serverState: any): [] {
        var result = [];
        if (serverState.ModelState) {
            for (var p in serverState.ModelState) {
                var fname = p.replace("model.", "");
                for (var i in serverState.ModelState[p]) {
                    result.push({errorKey: "server", fieldName: fname, message: serverState.ModelState[p][i]});
                }
            }
        }
        return result;
    }

    validate(validationKey: string, validationContext: ValidationContext): ValidationResponse {
        var result = new ValidationResponse();
        result.valid = true;
        result.message = null;

        //if (this.entity) {
        //    if (this.entity.entityAspect) {
        //        var errors = this.entity.entityAspect.getValidationErrors(this.name);
        //        return (errors.length > 0);
        //    }
        //}

        if (validationContext.errorContainer) {
            validationContext.errorContainer.forEach((err) => {
                if (err.fieldName == validationKey) {
                    result.valid = false;
                    result.message = err.message;
                }
            });

            //result.valid = validationContext.form.find(validationKey).valid;// && this.form.find(this.name).touched;
            //var e = validationContext.form.find(validationKey).errors;
            //if (e) {
            //    if (e.length > 0)
            //        result.message = validationContext.form.find(validationKey).errors[0];
            //}

        }

        return result;
    }

    public loadCollections(target: any) {
        for (var p in target) {
            this.loadCollection(target, p);
        }
    }

    public loadCollection(target: any, property: string) {
        var loader: LoadCollectionArgs = Reflect.getMetadata("ac:loadCollection", target, property);
        if (loader) {

            var query = breeze.EntityQuery.from(loader.entitySet);

            if (loader.filter) {
                var filters = target[loader.filter];
                if (filters) {
                    for (let predicate of filters)
                        query = query.where(breeze.Predicate.create(predicate.property, predicate.operator, predicate.value));
                }
            }

            if (loader.expand)
                query = query.expand(loader.expand);

            if (loader.orderBy)
                query = query.orderBy([loader.orderBy]);

            query.using(this.manager).execute()
                .then(function(data) {
                    target[property] = data.results;
                })
                .catch((reason) => {
                    console.log("error " + reason);
                });

        }
    }

    public createManager() : breeze.EntityManager {
        return this.rootManager.createEmptyCopy();
    }
}

export class ValidationContext {
    errorContainer: ModelErrorContainer[] = [];
    form: ControlGroup;
}

export class ValidationResponse {
    valid: boolean;
    message: string;
}

