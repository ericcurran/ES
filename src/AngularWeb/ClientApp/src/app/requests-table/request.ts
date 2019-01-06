import { RequestStatusEnum } from './request-status';

export class RequestDoc {
    id: number;
    zipFileName: string;
    deatilsFileName: string;
    detailsRecordId: string;
    esRef?: number;
    status: RequestStatusEnum;
}
