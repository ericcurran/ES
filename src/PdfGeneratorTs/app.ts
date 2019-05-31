import { createWriter } from "hummus";
import { PdfJsonData } from "./pdfItem";

export function callFromAsp(callback: any, tempDir: string, fontFile:string) {
    const pdfData: PdfJsonData = require(`${tempDir}\\pdf-data.json`);
    const fileName = `${new Date().getTime()}.pdf`;
    const target = `${tempDir}\\${fileName}`;
    const pdfDoc = createWriter(target);
    addPdfPage(pdfDoc, `${tempDir}\\${pdfData.logItems[0].fileName}`);
    addDetailsList(pdfDoc, pdfData, fontFile);

    for (let i = 1; i < pdfData.logItems.length; i++) {
        const file = pdfData.logItems[i].fileName;
        addPdfPage(pdfDoc, `${tempDir}\\${file}`);
    }

    pdfDoc.end();
    callback(null, fileName);
}

function addDetailsList(pdfDoc, pdfData:PdfJsonData, fontFile:string) {
    let addedSheets = 0;
    for (let i = 0; i < pdfData.logItems.length; i++) {
        if (i === 0) {
            const countMax = pdfData.logItems.length < 45 ? pdfData.logItems.length : 46;
            addRecordListPage(pdfDoc, fontFile, pdfData, i, countMax);
        }
        if (i === (45 + 50 * addedSheets)) {
            addedSheets++;
            const countMax = pdfData.logItems.length < (45 + (50 * i)) ? pdfData.logItems.length : 45 + (50 * i) + 1;
            addRecordListPage(pdfDoc, fontFile, pdfData, i, countMax);
        }
    }
    
}

function addRecordListPage(pdfDoc: any, fontFile: string, pdfData: PdfJsonData, recordStart: number, recordEnd: number) {
    const page = pdfDoc.createPage(0, 0, 210, 297);
    var cxt = pdfDoc.startPageContentContext(page);
    var textOptions = {
        font: pdfDoc.getFontForFile(fontFile),
        size: 3,
        colorspace: 'gray',
        color: 0x00
    };
    let height = 290;
    if (recordStart === 0) {
        for (let i = 0; i < pdfData.logTitle.length; i++) {
            const titleString = pdfData.logTitle[i];
            cxt.writeText(`${titleString};`, 10, height, textOptions);
            height -= 5;
        }
        height -= 20;
    }
    let count = recordStart + 1;
    for (let i = recordStart; i < recordEnd; i++) {
        const file = pdfData.logItems[i];
        if (file.inLog) {
            cxt.writeText(`${count}. ${file.log}`, 10, height, textOptions);
            height -= 5;
            count++;
        }
    }
    pdfDoc.writePage(page);
}

export function testCall(callback: any){
    callback(null, 'ok');
}

function addPdfPage(pdfDoc, fileName) {
    const page = pdfDoc.createPage(0,0,210,297);
    var cxt = pdfDoc.startPageContentContext(page);
    cxt.drawImage(1, 1, fileName, { transformation: { width: 209, height: 296 } })
    pdfDoc.writePage(page);
}