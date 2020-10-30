import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BaseService{

  constructor(private httpClient: HttpClient) { }

  public getAll<T>(url: string){
    return this.httpClient
      .get<T>(url);
  }
  public get<T>(url: string,query: any) {
    return this.httpClient
      .get<T>(`${url}${query}`);
  }
  public post<T>(url: string, item: any): Observable<any> {
    return this.httpClient
      .post<T>(`${url}`, JSON.stringify(item));
  }
  public update(url: string, item: any) {
    return this.httpClient
      .put<any>(`${url}`, item.json())
      .pipe(map(data => data.json()));
  }
  public delete(url:string, id: number) {
    return this.httpClient
      .delete(`${url}${id}`);
  }


}
