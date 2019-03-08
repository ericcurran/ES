import { RequestStatusEnum } from './request-status';

export class RequestDoc {
    id: number;
    zipFileName: string;
    deatilsFileName: string;
    detailsRecordId: string;
    esRef?: number;
    pdfPackName: string;
    status: RequestStatusEnum;
    claimNumber: string;
    insuredName: string;
    dateOfLoss: string;
    dateOfService: string;
    phase: string;
}
