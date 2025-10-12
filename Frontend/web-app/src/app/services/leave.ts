import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ApplyLeave } from '../types/ILeave';
import { environment } from '../../environments/environment';
import { ILeave } from '../types/Leave';
import { IPagedData } from '../types/IPagedData';

@Injectable({
  providedIn: 'root'
})
export class Leaves {
  http = inject(HttpClient)
  applyLeave(Leave : ApplyLeave){
    return this.http.post(`${environment.apiUrl}/api/Leave/apply`, Leave)
  }

  getLeave(filter : any){
    let params = new HttpParams({ fromObject: filter });
    return this.http.get<IPagedData<ILeave>>(`${environment.apiUrl}/api/Leave?`, { params });
  }
}
