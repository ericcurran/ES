import { createWriter, PDFStreamForResponse } from "hummus";
import * as fs from "fs";


export function callFromAsp(callback: any, tempDir: string) {
    getFileNames(tempDir).then((files) => {
        const fileName = `${new Date().getTime()}.pdf`;
        const target = `${tempDir}\\${fileName}`;
        const pdfDoc = createWriter(target);
        files.forEach(f => {
            addPdfPage(pdfDoc, `${tempDir}\\${f}`);
        });
        pdfDoc.end();
         callback(null,fileName);
    }, (err) => {        
        callback(err,null);
    });


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


