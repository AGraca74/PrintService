using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintService.Models;
using System.Drawing.Printing;
using System.Management;
using Microsoft.AspNetCore.Http;

namespace PrintService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Printers: ControllerBase{

        /// <summary>
        /// Get server printers information
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Printers information collected successfully</response>
        /// <response code="400">Error ocurred while processing de request</response>    
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<PrinterInfo>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List(){

            IActionResult rslt;
            try
            {
                rslt = await Task.Run(() =>{
                    var execRslt = new List<PrinterInfo>();
                    try
                    {
                        // string searchQuery = "SELECT * FROM Win32_Printer";
                        // ManagementObjectSearcher searchPrinters = 
                        //     new ManagementObjectSearcher(searchQuery);
                        // ManagementObjectCollection printerCollection = searchPrinters.Get();
                        // foreach(ManagementObject printer in printerCollection)
                        // {
                        //     execRslt.Add(new PrinterInfo{PrinterName = printer.Properties["Name"].Value.ToString().Replace(@"\\", @"\")} );
                        // }

                        ManagementScope objScope = new ManagementScope(ManagementPath.DefaultPath); //For the local Access
                        objScope.Connect();
            
                        SelectQuery selectQuery = new SelectQuery();
                        selectQuery.QueryString = @"Select * from win32_Printer";
                        ManagementObjectSearcher MOS = new ManagementObjectSearcher(objScope, selectQuery);
                        ManagementObjectCollection MOC = MOS.Get();
                        foreach (ManagementObject mo in MOC)
                        {
                            execRslt.Add(new PrinterInfo{
                                PrinterName = mo["Name"].ToString(),
                                PrinterLocation = (bool)mo["Network"] ? PrinterLocation.network : PrinterLocation.local
                            });
                        }
                    }
                    catch (System.Exception ex){
                        return StatusCode(500, new InternalServerErrorDetails{
                            Title = "Collecting system printers information",
                            Detail = ex.Message
                        });
                    }
                    return Ok(execRslt);
                });
            }
            catch (System.Exception ex){
                return StatusCode(500, new InternalServerErrorDetails{
                    Detail = ex.Message
                });
            }
            return rslt;
        }
    }
}