using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintService.Models;

namespace PrintService.Controllers
{
    // [Route("api/[controller]")]
    // [ApiController]
    // public class ApiInfoController: ControllerBase
    // {
    //     [HttpGet]
    //     [ProducesResponseType(StatusCodes.Status200OK)]
    //     [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //     public async Task<IActionResult> ApiInfo(){

    //         IActionResult rslt;
    //         try
    //         {
    //             rslt = await Task.Run(() =>{
    //                 try
    //                 {
    //                     return Ok(new {
    //                         AppCodeExecutiedUsing = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
    //                         IsUserAuthenticated = User.Identity.IsAuthenticated.ToString(),
    //                         AuthenticationType = User.Identity.AuthenticationType,
    //                         UserName = User.Identity.Name
    //                     });
    //                 }
    //                 catch (System.Exception ex){
    //                     return StatusCode(500, new InternalServerErrorDetails{
    //                         Title = "Collecting system printers information",
    //                         Detail = ex.Message
    //                     });
    //                 }
    //             });
    //         }
    //         catch (System.Exception ex){
    //             return StatusCode(500, new InternalServerErrorDetails{
    //                 Detail = ex.Message
    //             });
    //         }
    //         return rslt;
    //     }
    // }
}