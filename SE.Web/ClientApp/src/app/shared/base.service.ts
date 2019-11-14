import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable} from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class BaseService{

  domain = 'https://localhost:44362/api';
  constructor(private httpClient: HttpClient) { }

 

  public getAll<T>(url: string){
    return this.httpClient
      .get<T>(`${this.domain}/${url}`);
  }

  public getById(url: string,id: number) {
    return this.httpClient
      .get(`${this.domain}/${url}/${id}`);
  }

  public post<T>(url: string, item: any): Observable<any> {
    return this.httpClient
      .post<T>(`${this.domain}/${url}`, JSON.stringify(item));
  }

  public update(url: string, item: any) {
    return this.httpClient
      .put<any>(`${this.domain}/${url}`, item.json())
      .pipe(map(data => data.json()));
  }

  public delete(url:string, id: number) {
    return this.httpClient
      .delete(`${this.domain}/${url}/${id}`);
  }


}
