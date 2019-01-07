import { RecordStatusEnum } from './record-status';

export class RecordDoc {
    id: number;
    fileName: string;
    status: RecordStatusEnum;
    requestPackageId: number;
    esRef?: number;
}

