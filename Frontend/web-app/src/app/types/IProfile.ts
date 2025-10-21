export interface IProfile {
  userId: string;
  name: string;
  phone: string;
  profileImage: string ;
  oldPassword: string;
  newPassword: string;
}


export interface IProfileResponse {
  name: string;
  phone: string;
  profileImage: string;
  salary : number
}