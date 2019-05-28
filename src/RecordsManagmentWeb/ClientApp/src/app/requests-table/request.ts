import { RequestStatusEnum } from './request-status';

export class RequestDoc {
    id: number;
    requestId: string;
    type: number;
    claimNumber: string;
    insuredName: string;
    created: string;
    amount: number;
    dateOfLoss: string;
    dateOfService: string;
    phase: string;
    deatilsFileName: string;
    esRef?: number;
    status: RequestStatusEnum;
}
