"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var hummus_1 = require("hummus");
var fs = require("fs");
function callFromAsp(callback, tempDir, fontFile) {
    var pdfData = require(tempDir + "\\pdf-data.json");
    var fileName = new Date().getTime() + ".pdf";
    var target = tempDir + "\\" + fileName;
    var pdfDoc = hummus_1.createWriter(target);
    addPdfPage(pdfDoc, tempDir + "\\" + pdfData[0].fileName);
    addDetailsList(pdfDoc, pdfData, fontFile);
    for (var i = 1; i < pdfData.length; i++) {
        var file = pdfData[i].fileName;
        addPdfPage(pdfDoc, tempDir + "\\" + file);
    }
    pdfDoc.end();
    callback(null, fileName);
}
exports.callFromAsp = callFromAsp;
function addDetailsList(pdfDoc, pdfData, fontFile) {
    var page = pdfDoc.createPage(0, 0, 210, 297);
    var cxt = pdfDoc.startPageContentContext(page);
    var textOptions = {
        font: pdfDoc.getFontForFile(fontFile),
        size: 3,
        colorspace: 'gray',
        color: 0x00
    };
    var height = 290;
    var count = 1;
    for (var i = 0; i < pdfData.length; i++) {
        var file = pdfData[i];
        if (file.inLog) {
            cxt.writeText(count + ". " + file.log + ";", 10, height, textOptions);
            height -= 5;
            count++;
        }
    }
    pdfDoc.writePage(page);
}
function testCall(callback) {
    callback(null, 'ok');
}
exports.testCall = testCall;
function addPdfPage(pdfDoc, fileName) {
    var page = pdfDoc.createPage(0, 0, 210, 297);
    var cxt = pdfDoc.startPageContentContext(page);
    cxt.drawImage(1, 1, fileName, { transformation: { width: 209, height: 296 } });
    pdfDoc.writePage(page);
}
function getFileNames(dir) {
    return new Promise(function (resolve, reject) {
        fs.readdir(dir, function (err, fileNames) {
            var files = [];
            if (err) {
                reject(err);
            }
            fileNames.forEach(function (file) {
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
//# sourceMappingURL=app.js.map