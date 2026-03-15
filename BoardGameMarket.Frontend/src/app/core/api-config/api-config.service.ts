import { InjectionToken } from "@angular/core";
import { devEnvironment } from "../../../environments/environment.development";
import { ApiConfig } from "./api-config.interface";

export const API_CONFIG_SERVICE = new InjectionToken<ApiConfig>('app.config');

export const DEV_API_CONFIG: ApiConfig = {
    apiEndpoint: devEnvironment.apiAddress,
}