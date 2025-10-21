
export interface IEmployee {
  id: number;
    name: string;
    email: string;
    phone: string;
    jobTitle: string;
    gender: Gender;
    departmentId: number;
    dateOfBirth: string; // Use string or Date depending on your usage
    joiningDate: string; // Use string or Date depending on your usage
    lastWorkingDate: string; // Use string or Date depending on your usage
    salary: number;
}

export enum Gender{
      Male = 1,
      Female = 2
}
