export interface GoogleCodeRequest {
    redirectUrl: string;
    clientId: string;
    scope: string;
    state: string;
    codeChallange: string;
    codeChalangeMethod: string;
}