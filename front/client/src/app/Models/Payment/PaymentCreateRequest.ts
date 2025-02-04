export interface PaymentCreateRequest {
    courseId: number;
    successUrl: string;
    cancelUrl: string;
}