import { HttpParams } from '@angular/common/http';

export function objectToQueryParams(obj: any, prefix: string = ''): HttpParams {
    let params = new HttpParams();

    function buildParams(key: string, value: any, parentKey: string = ''): void {
        const fullKey = parentKey ? `${parentKey}.${key}` : key;

        if (value !== null && typeof value === 'object' && !Array.isArray(value)) {
            // If the value is an object, recursively process its properties
            for (const nestedKey in value) {
                if (value.hasOwnProperty(nestedKey)) {
                    buildParams(nestedKey, value[nestedKey], fullKey);
                }
            }
        } else if (Array.isArray(value)) {
            // If the value is an array, add each item as a separate parameter
            value.forEach((item, index) => {
                buildParams(index.toString(), item, fullKey);
            });
        } else {
            // If the value is a primitive, add it to the query parameters
            params = params.set(prefix ? `${prefix}.${fullKey}` : fullKey, value?.toString() || '');
        }
    }

    for (const key in obj) {
        if (obj.hasOwnProperty(key)) {
            buildParams(key, obj[key]);
        }
    }

    return params;
}
