import { RoleRequest } from "./RoleRequest";

export interface GoogleAuthRequest {
    code: string;
    codeVerifier: string;
    redirectUrl: string;
    role: RoleRequest;
}