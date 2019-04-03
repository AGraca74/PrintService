using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PrintService.Models
{
    /// <summary>
    /// Printer information
    /// </summary>
    public class PrinterInfo{
        /// <summary>
        /// Printer name
        /// </summary>
        public string PrinterName { get; set; }
        
        /// <summary>
        /// Printer location
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PrinterLocation PrinterLocation { get; set; }

        /// <summary>
        /// Printer information constructor
        /// </summary>
        public PrinterInfo(){
            this.PrinterName = "";
            this.PrinterLocation = PrinterLocation.local;
        }
    }

    /// <summary>
    /// Printer location types
    /// </summary>
    public enum PrinterLocation{
        /// <summary>
        /// Local printer
        /// </summary>
        local,
        /// <summary>
        /// Network printer>
        /// </summary>
        network
    }
}