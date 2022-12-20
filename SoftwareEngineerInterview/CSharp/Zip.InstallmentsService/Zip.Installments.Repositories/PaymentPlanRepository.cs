using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Zip.Installments.DomainEntities;
using Zip.Installments.Repositories.AppContext;
using Zip.Installments.Repositories.Helpers;

namespace Zip.Installments.Repositories
{
    public interface IPaymentPlanRepository
    {
        bool CreatePaymentPlan(PaymentPlan paymentPlanModel);
    }

    public class PaymentPlanRepository : IPaymentPlanRepository
    {
        private readonly ILogger _logger; 

        private readonly ApplicationDbContext _applicationDbContext;

        private readonly AppSettings _appSettings;

        public PaymentPlanRepository(ApplicationDbContext applicationDbContext, IOptions<AppSettings> appSettings, ILogger<PaymentPlanRepository> logger)
        {
            _applicationDbContext = applicationDbContext;
            _appSettings = appSettings.Value;
            _logger = logger;
        } 

        public bool CreatePaymentPlan(PaymentPlan paymentPlanModel)
        {
            try
            {
                if (paymentPlanModel != null)
                { 
                    PaymentPlan paymentPlan = new PaymentPlan();
                    paymentPlan.Id = Guid.NewGuid();
                    _applicationDbContext.tblPaymentPlan.Add(paymentPlanModel);
                    _applicationDbContext.SaveChanges(); 
                    for (int i = 0; i < paymentPlanModel.Installments; i++)
                    {
                        var installment = new Installment();
                        installment.Amount = paymentPlanModel.PurchaseAmount / paymentPlanModel.Installments;
                        installment.DueDate = (i == 0) ? DateTime.Today.AddDays(0) :DateTime.Today.AddDays(paymentPlanModel.PurchaseFrequency*i); 
                        installment.Id = new Guid();
                        installment.PurchaseId = paymentPlan.Id.ToString();  

                        _applicationDbContext.tblInstallment.Add(installment);
                        _applicationDbContext.SaveChanges();
                        _logger.LogInformation("Added installment details");
                    }  
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
            return false; 
        }
    }
}
