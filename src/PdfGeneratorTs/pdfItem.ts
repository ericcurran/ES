export class PdfItem {
    fileName: string;
    inLog: boolean;
    log:string;
}

export class PdfJsonData {
    logTitle: Array<string>;
    logItems: Array<PdfItem>;
}