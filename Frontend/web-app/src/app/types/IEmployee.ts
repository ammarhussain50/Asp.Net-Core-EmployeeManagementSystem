import { IDepartment } from "./IDepartment";

export interface IEmployee {
  id: number;
  name: string;
  email: string;
  phone: string;
  jobTitle: string;
  gender: Gender;
  departmentId: number;   // optional
//   department?: IDepartment; // nested interface
  joiningDate: string;     // API se aayega ISO string
  lastWorkingDate: string;
  dateOfBirth: string;     // DateOnly bhi API se string me aata hai
}

export enum Gender{
      Male = 1,
      Female = 2
}
