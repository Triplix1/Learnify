import { RoleRequest } from "./RoleRequest";

export interface ReqisterRequest {
    email: string;
    username: string;
    password: string;
    confirmPassword: string;
    role: RoleRequest;
}