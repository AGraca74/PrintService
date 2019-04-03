using System.ComponentModel.DataAnnotations;

namespace PrintService.Models
{
    /// <summary>
    /// Request print PDF model
    /// </summary>
    /// <example>
    /// {
    ///     "pdfUrl": "http://server/rwms/PDFS/SomePDF.pdf",
    ///     "printerName": "PDFCreator"
    /// }
    /// </example>
    public class PrintPDFRequest{
        /// <summary>
        /// PDF Url
        /// </summary>
        [Required(ErrorMessage = "PDF Url is required")]
        public string pdfUrl { get; set; }
        /// <summary>
        /// Server Printer Name
        /// </summary>
        /// <remarks>
        /// You can collect all possible printer names by calling server:port/api/Printers url
        /// </remarks>
        [Required(ErrorMessage = "Printer name is required")]
        public string printerName { get; set; }
    }
}