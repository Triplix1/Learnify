export interface Message {
    id: number;
    senderName?: string;
    senderId?: number;
    content: string;
    messageSent: Date;
}