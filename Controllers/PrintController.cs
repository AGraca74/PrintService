using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PrintService.Models;
using PrintService.Services;

namespace PrintService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintController: ControllerBase{
        private readonly IOptions<AppConfigurations> appConfigurations;

        public PrintController(IOptions<AppConfigurations> appConfigurations): base(){
            this.appConfigurations = appConfigurations;
        }

        /// <summary>
        /// Send PDF (URL-addressed) to a printer
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /PDF
        ///     {
        ///         "pdfUrl": "http://server/rwms/PDFS/SomePDF.pdf",
        ///         "printerName": "PDFCreator"
        ///     }
        /// </remarks>
        /// <param name="printRequest">Print PDF request information</param>
        /// <returns>Service action result</returns>
        /// <response code="200">PDF sent to printer successfully</response>
        /// <response code="400">Error ocurred while processing de request</response>    
        /// <response code="408">Request timeout reached</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(SrvActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PDF(PrintPDFRequest printRequest){
            IActionResult rslt;
            try {
                //To get the location 
                var path = new Uri(System.IO.Path.GetDirectoryName(
                                   System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
                 
                rslt = await Task.Run(() => {
                    try
                    {
                        var pdfFile = printRequest.pdfUrl;
                        var downloadFilePath = path + @"\" + DateTime.Now.ToString("ddMMyyyy_hhmmssf") + ".pdf";

                        var downloadFileError = DownloadFile(pdfFile, downloadFilePath);
                        try
                        {
                            // Check for download file errors
                            if(!string.IsNullOrEmpty(downloadFileError)){
                                return StatusCode(400, new BadRequestDetails{
                                    Title = $"Error downloading file '{pdfFile}'!",
                                    Detail = downloadFileError
                                });
                            }
                            // Check PDF File
                            else if (!System.IO.File.Exists(downloadFilePath)) {
                                downloadFileError = "Downloaded file '" + downloadFilePath + "' not found";


                                return StatusCode(500, new InternalServerErrorDetails{
                                    Title = "Downloaded file not found",
                                    Detail = $"File '{pdfFile}', downloaded to '"+downloadFilePath+"' was not found!"
                                });
                            }
                            else{
                                var printerName = printRequest.printerName;
                                // This uses gsprint (mind the paths)
                                var gsPrintExecutable = path + @"\GhostscriptBin\gsprint.exe";
                                var gsExecutable = path + @"\GhostscriptBin\gswin32c.exe";
                                var processArgs = $"-ghostscript \"{gsExecutable}\" -copies=1 -dPDFFitPage -all -printer \"{printerName}\" \"{downloadFilePath}\"";

                                var gsProcessInfo = new ProcessStartInfo {
                                    WindowStyle = ProcessWindowStyle.Hidden,
                                    FileName = gsPrintExecutable,
                                    Arguments = processArgs
                                };

                                // Printing timeout
                                var printTimeout = appConfigurations.Value.PrintTimeout;

                                using (var gsProcess = Process.Start(gsProcessInfo)) {
                                    // Start printing process
                                    if (!gsProcess.WaitForExit(printTimeout)) {

                                        return StatusCode(408, new TimeoutErrorDetails{
                                            Title = "Print PDF File",
                                            Detail = $"Timeout {printTimeout}mls reached, on printing '{pdfFile}'!"
                                        });
                                    }
                                }
                            }
                        }
                        finally{
                             if(string.IsNullOrEmpty(downloadFileError))
                                System.IO.File.Delete(downloadFilePath);
                        }
                        
                    }
                    catch (System.Exception ex){
                        return StatusCode(500, new InternalServerErrorDetails{
                            Detail = "Unhandled exception: " + ex.Message
                        });
                    } 

                    return Ok(new SrvActionResult());
                });
            }
            catch (System.Exception ex) {
                return StatusCode(500, new InternalServerErrorDetails{
                    Detail = "Unhandled exception: " + ex.Message
                });
            }

            return rslt;
        }


        internal static string DownloadFile(String remoteFilename, String localFilename) {
            // Function will return the number of bytes processed
            // to the caller. Initialize to 0 here.
            int bytesProcessed = 0;

            // Assign values to these objects here so that they can
            // be referenced in the finally block
            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;

            // Use a try/catch/finally block as both the WebRequest and Stream
            // classes throw exceptions upon error
            try {
                // Create a request for the specified remote file name
                var request = WebRequest.Create(remoteFilename);
                request.Timeout = 60000;
                ((HttpWebRequest)request).ReadWriteTimeout = 60000;
                if (request != null) {
                    // Send the request to the server and retrieve the
                    // WebResponse object 
                    response = request.GetResponse();
                    if (response != null) {
                        // Once the WebResponse object has been retrieved,
                        // get the stream object associated with the response's data
                        remoteStream = response.GetResponseStream();

                        // Create the local file
                        localStream = System.IO.File.Create(localFilename);

                        // Allocate a 1k buffer
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        // Simple do/while loop to read from stream until
                        // no bytes are returned
                        do {
                            // Read data (up to 1k) from the stream
                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                            // Write the data to the local file
                            localStream.Write(buffer, 0, bytesRead);

                            // Increment total bytes processed
                            bytesProcessed += bytesRead;
                        } while (bytesRead > 0);
                    }
                }
            }
            catch (Exception e) {
                return e.Message;
            }
            finally {
                // Close the response and streams objects here 
                // to make sure they're closed even if an exception
                // is thrown at some point
                if (response != null) response.Close();
                if (remoteStream != null) remoteStream.Close();
                if (localStream != null) localStream.Close();
            }
            return "";
        }
    }
    
}