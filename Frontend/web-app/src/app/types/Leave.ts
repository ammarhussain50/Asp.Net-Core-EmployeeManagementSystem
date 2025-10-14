export interface ILeave{
     id: number;
      type : LeaveType,
      Reason : string,
      LeaveDate : string,
      status : LeaveStatus
       employeeId: number;
  employee?: any;
}

export enum LeaveType
{
    Sick = 1,
    Casual = 2,
    Earned = 3
}

export enum LeaveStatus
{
    Pending = 0,
    Accepted = 1,
    Rejected = 2,
    Cancelled = 3
}