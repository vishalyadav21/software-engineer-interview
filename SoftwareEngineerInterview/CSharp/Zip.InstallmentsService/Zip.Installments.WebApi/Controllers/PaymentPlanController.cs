using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Zip.Installments.DomainEntities;
using Zip.Installments.Repositories;

namespace Zip.Installments.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentPlanController : ControllerBase
    {
        private readonly ILogger _logger;
        private IPaymentPlanRepository _paymentPlanRepository;
        public PaymentPlanController(ILogger<PaymentPlanController> logger, IPaymentPlanRepository paymentPlanRepository)
        {
            _logger = logger;
            _paymentPlanRepository = paymentPlanRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        } 
         
        [HttpPost]
        public IActionResult Post([FromBody] PaymentPlan paymentPlan)
        {
            if (paymentPlan != null)
            {
                bool result = _paymentPlanRepository.CreatePaymentPlan(paymentPlan);
                if (result)
                {
                    _logger.LogInformation("created payment plan at :" + DateTime.Now);
                    return Ok(result);
                } 
            }
            _logger.LogError("Unable to create payment plan");
            return BadRequest(new { Message = "Unable to create payment plan" });
        } 
    }
}
