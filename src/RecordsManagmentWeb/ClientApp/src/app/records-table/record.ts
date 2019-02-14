import { RecordStatusEnum } from './record-status';

export class RecordDoc {
    id: number;
    fileName: string;
    status: RecordStatusEnum;
    inScope: boolean;
    requestPackageId: number;
    esRef?: number;
}

