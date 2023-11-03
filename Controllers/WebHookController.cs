using Microsoft.AspNetCore.Mvc;
using OneFlowIntegration.DTO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using OneFlowDomain.Tables;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OneFlowIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {
        private readonly OneFlowDBContext _context;
        private readonly IConfiguration _config;
        public WebHookController(IConfiguration configuration,OneFlowDBContext context)
        {
            _config = configuration;
            _context = context;
        }

        /// <summary>
        /// WebHook Reciver
        /// </summary>
        /// <returns></returns>
        // POST: api/<WebHookController>/Reciver
        [HttpPost]
        public async Task<ActionResult> Reciver(WebHookResponse webHookResponse)
        {
            string  key = _config["OneFlowWebHookToken"];
            if (webHookResponse.ValidateResponse(webHookResponse,key))
            {
                //log request in file

                //log request in DB
                OneFlowLog log = new OneFlowLog();
                log.ContractId = webHookResponse.contract.Id;
                log.EventId=webHookResponse.events.Select(t=>t.Id).FirstOrDefault();
                log.CreatedTime=System.DateTime.Now;
                log.CallbackId = webHookResponse.callback_id;
                log.Type=webHookResponse.events.Select(t=>t.Type).FirstOrDefault();
                log.IsValid = true;
                _context.OneFlowLogs.Add(log);
                _context.SaveChanges();
                //process request
                //await 

            }
            else {
                //log the request before return

                OneFlowLog log = new OneFlowLog();
                log.ContractId = webHookResponse.contract.Id;
                log.EventId = webHookResponse.events.Select(t => t.Id).FirstOrDefault();
                log.CreatedTime = System.DateTime.Now;
                log.CallbackId = webHookResponse.callback_id;
                log.Type = webHookResponse.events.Select(t => t.Type).FirstOrDefault();
                log.IsValid = false;
                _context.OneFlowLogs.Add(log);
                _context.SaveChanges();
                return BadRequest(); 
            }
            return Ok();
        }

        ///// <summary>
        ///// /// contract:content_update > The contract's content has been updated after being sent to the counterparty.
        ///// </summary>
        ///// <returns></returns>
        //// GET: api/<WebHookController>
        //[HttpPost]
        //public IEnumerable<string> GetWebHookReturn()
        //{
        //    return new string[] { "value1", "value2" };
        //}


        ///// <summary>
        ///// /// contract:content_update > The contract's content has been updated after being sent to the counterparty.
        ///// </summary>
        ///// <returns></returns>
        //// GET: api/<WebHookController>
        //[HttpPost]
        //public IEnumerable<string> GetWebHook()
        //{
        //    return new string[] { "value1", "value2" };
        //}


        //// GET api/<WebHookController>/5
        //[HttpPost("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<WebHookController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<WebHookController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<WebHookController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
