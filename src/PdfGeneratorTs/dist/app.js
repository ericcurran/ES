"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var hummus_1 = require("hummus");
var fs = require("fs");
function callFromAsp(callback, tempDir) {
    getFileNames(tempDir).then(function (files) {
        var fileName = new Date().getTime() + ".pdf";
        var target = tempDir + "\\" + fileName;
        var pdfDoc = hummus_1.createWriter(target);
        files.forEach(function (f) {
            addPdfPage(pdfDoc, tempDir + "\\" + f);
        });
        pdfDoc.end();
        callback(null, fileName);
    }, function (err) {
        callback(err, null);
    });
}
exports.callFromAsp = callFromAsp;
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