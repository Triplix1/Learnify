import { RoleRequest } from "../RoleRequest";

export interface ReqisterRequest {
    email: string;
    name: string;
    surname: string;
    username: string;
    password: string;
    confirmPassword: string;
    role: RoleRequest;
}