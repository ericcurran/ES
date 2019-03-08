import { createWriter } from "hummus";
import * as fs from "fs";
import { PdfItem } from "./pdfItem";

export function callFromAsp(callback: any, tempDir: string, fontFile:string) {
    const pdfData: Array<PdfItem> = require(`${tempDir}\\pdf-data.json`);
    const fileName = `${new Date().getTime()}.pdf`;
    const target = `${tempDir}\\${fileName}`;
    const pdfDoc = createWriter(target);
    addPdfPage(pdfDoc, `${tempDir}\\${pdfData[0].fileName}`);
    addDetailsList(pdfDoc, pdfData, fontFile);

    for (let i = 1; i < pdfData.length; i++) {
        const file = pdfData[i].fileName;
        addPdfPage(pdfDoc, `${tempDir}\\${file}`);
    }

    pdfDoc.end();
    callback(null, fileName);
}

function addDetailsList(pdfDoc, pdfData:Array<PdfItem>, fontFile:string) {
    const page = pdfDoc.createPage(0, 0, 210, 297);
    var cxt = pdfDoc.startPageContentContext(page);
    var textOptions = {
        font: pdfDoc.getFontForFile(fontFile),
        size: 3,
        colorspace: 'gray',
        color: 0x00
    };
    let height = 290;
    let count = 1;
    for (let i = 0; i < pdfData.length; i++) {
        const file = pdfData[i];
        if (file.inLog) {
            cxt.writeText(`${count}. ${file.fileName};`, 10, height, textOptions);
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

function getFileNames(dir: string): Promise<Array<string>> {
    return new Promise((resolve, reject) => {
            fs.readdir(dir, (err, fileNames) => {
            const files = [];
            if (err) {
                reject(err);
            }
            fileNames.forEach(file => {
                files.push(file);
            });
            resolve(files);
        });       
    });
}


// function GeneratePdf() {
//     const pdfDoc = createWriter("input.pdf");
//     //addDetails(pdfDoc);
//     addTiff(pdfDoc);
//     addJpg(pdfDoc);
//     pdfDoc.end();
// }

// function addTiff(pdfDoc) {
//     const page = pdfDoc.createPage(0, 0, 210, 297);
//     var cxt = pdfDoc.startPageContentContext(page);
//     cxt.drawImage(0, 0, 'content/record1.tiff', { transformation: { width: 210, height: 290 } })
//     pdfDoc.writePage(page);
// }

// function addJpg(pdfDoc) {
//     const page = pdfDoc.createPage(0,0,210,297);
//     var cxt = pdfDoc.startPageContentContext(page);
//     cxt.drawImage(0, 0, 'content/record2.jpg', { transformation: { width: 210, height: 290 } })
//     pdfDoc.writePage(page);
// }


