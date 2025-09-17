import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IDepartment } from '../types/IDepartment';
import { IEmployee } from '../types/IEmployee';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  http = inject(HttpClient)

   getDepartments() {
    return this.http.get<IDepartment[]>(environment.apiUrl+'/api/Department');
  }
  addDepartment(name:string){
    return this.http.post(environment.apiUrl+'/api/Department',{name});
  }
  updateDepartment(id: number, name: string) {
  return this.http.put(environment.apiUrl + `/api/Department/${id}`, { id, name });
}
deleteDepartment(id: number) {
  return this.http.delete(environment.apiUrl + `/api/Department/${id}`); 
}
getEmployees(){
  return this.http.get<IEmployee[]>(environment.apiUrl+`/api/Employee`)
}
addEmployee(employee: IEmployee){
    return this.http.post(environment.apiUrl+'/api/Employee',employee);
  }
updateEmployee(id: number, employee: IEmployee) {
    return this.http.put(`${environment.apiUrl}/api/Employee/${id}`, employee);
  }
deleteEmployee(id: number) {
  return this.http.delete(environment.apiUrl + `/api/Employee/${id}`);  
}
getEmployeeById(id:number){
  return this.http.get(environment.apiUrl + `/api/Employee/${id}`)
}
}
