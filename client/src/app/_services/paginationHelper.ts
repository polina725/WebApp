import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PaginatedResult } from "../_models/pagination";

export function getPaginatedResults<T>(url, params, http: HttpClient) {
    const paginatedRes: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
        map(response => {
            paginatedRes.result = response.body;
            if (response.headers.get('Pagination') !== null) {
                paginatedRes.pagination = JSON.parse(response.headers.get('Pagination'));
            }
            return paginatedRes;
        })
    );
}

export function getPaginationHeaders(pageNumber: number, pageSize: number){
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    
    return params;
}