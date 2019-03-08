import { RecordStatusEnum } from './record-status';

export class RecordDoc {
    id: number;
    fileName: string;
    status: RecordStatusEnum;
    inScope: boolean;
    inLog: boolean;
    requestPackageId: number;
    esRef?: number;
    claimNumber: string;
    bundleNumber: string;
    pageNumber: number;
    orderNumber: number;
    startDate: string;
    log: string;
    phase: string;
}

