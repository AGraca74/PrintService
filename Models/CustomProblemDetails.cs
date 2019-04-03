using Microsoft.AspNetCore.Mvc;

namespace PrintService.Models
{
    /// <summary>
    /// Status code 500 - Internal server error
    /// </summary>
    public class  InternalServerErrorDetails: ProblemDetails{
        /// <summary>
        /// Status code 500 - Internal server error
        /// </summary>
        public InternalServerErrorDetails(): base(){
            this.Type = @"https://httpstatuses.com/500";
            this.Title= "Internal Server Error";
            this.Status = 500;
        }
    }

    /// <summary>
    /// Status code 408 - Request Timeout error
    /// </summary>
    public class  TimeoutErrorDetails: ProblemDetails{
        /// <summary>
        /// Status code 408 - Request Timeout error
        /// </summary>
        public TimeoutErrorDetails(): base(){
            this.Type = @"https://httpstatuses.com/408";
            this.Title= "Request Timeout";
            this.Status = 408;
        }
    }

    /// <summary>
    /// Status code 400 - Bad request error
    /// </summary>
    public class  BadRequestDetails: ProblemDetails{
        /// <summary>
        /// Status code 400 - Bad request error
        /// </summary>
        public BadRequestDetails(): base(){
            this.Type = @"https://httpstatuses.com/400";
            this.Title= "Bad Request";
            this.Status = 400;
        }
    }
}