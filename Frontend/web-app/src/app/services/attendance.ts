import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IPagedData } from '../types/IPagedData';
import { IAttendance } from '../types/IAttendance';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {

  http = inject(HttpClient)

  markAttendance() {
  return this.http.post(`${environment.apiUrl}/api/Attendance/mark-present`,{});

}

getAttendanceHistory(filter:any) {
    let params = new HttpParams({ fromObject: filter });
    return this.http.get<IPagedData<IAttendance>>(`${environment.apiUrl}/api/Attendance`, { params });
  }
}
