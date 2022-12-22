using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Zip.Installments.WebApp.Models;

namespace Zip.Installments.WebApp.Controllers
{
    public class PaymentPlanController : Controller
    {
        private readonly ILogger<PaymentPlanController> _logger;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        public PaymentPlanController(ILogger<PaymentPlanController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _Configure = configuration; 
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }
        public IActionResult Index()
        {
            var paymentPlan = new List<PaymentPlanViewModel>();
            return View("Index", paymentPlan); 
        }

        [HttpGet]
        public IActionResult Create()
        {
            var paymentPlan = new PaymentPlanViewModel();
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "7 days", Value = "7" });
            items.Add(new SelectListItem { Text = "14 days", Value = "14" });
            items.Add(new SelectListItem { Text = "21 days", Value = "21" });  
            ViewBag.PurchaseFrequency = items; 
            return View("Create", paymentPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PaymentPlanViewModel paymentPlan)  
        {
            if (ModelState.IsValid)
            { 
                var client = new HttpClient();
                client.BaseAddress = new Uri(apiBaseUrl + string.Format("/paymentplan/"));
                var json = JsonConvert.SerializeObject(paymentPlan);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(client.BaseAddress, content);
                response.Wait();
                var result = response.Result; 
            }
            return RedirectToAction(nameof(Index));
        } 
    } 
}
