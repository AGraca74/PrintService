using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace PrintService.Models
{
    /// <summary>
    /// Execution result types
    /// </summary>
    public enum ExecResult{
        /// <summary>
        /// Action executed successfully
        /// </summary>
        success,
        /// <summary>
        /// Action executed successfully with warnings
        /// </summary>
        warning
    }

    /// <summary>
    /// Service action result model
    /// </summary>
    public class SrvActionResult{
        /// <summary>
        /// Execution result
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ExecResult Result { get; set; }

        /// <summary>
        /// Result code
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// Result message
        /// </summary>
        public string ResultMessage { get; set; } 

        /// <summary>
        /// Service action result
        /// </summary>
        public SrvActionResult(){
            this.Result = ExecResult.success;
            this.ResultCode = "";
            this.ResultMessage = "";
        }  
    }

    /// <summary>
    /// Service action value result model 
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    public class SrvActionResult<TValue>: SrvActionResult{
        /// <summary>
        /// Result value
        /// </summary>
        public TValue Value { get; private set; }   

        /// <summary>
        /// Service action value result model constructor
        /// </summary>
        public SrvActionResult() : base(){
            this.Value = (TValue)Activator.CreateInstance(typeof(TValue));
        }
    }
}