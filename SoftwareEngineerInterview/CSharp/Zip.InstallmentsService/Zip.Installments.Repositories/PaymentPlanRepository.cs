using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Zip.Installments.DomainEntities;
using Zip.Installments.Repositories.AppContext;

namespace Zip.Installments.Repositories
{
    public interface IPaymentPlanRepository
    {
        Task<bool> CreatePaymentPlan(PaymentPlan paymentPlanModel);
    }

    public class PaymentPlanRepository : IPaymentPlanRepository
    {
        private readonly ILogger _logger; 

        private readonly ApplicationDbContext _applicationDbContext;
         
        public PaymentPlanRepository(ApplicationDbContext applicationDbContext, ILogger<IPaymentPlanRepository> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        } 

        /// <summary>
        /// Created playment plan with Installments. Installments computed based on input 
        /// </summary>
        /// <param name="paymentPlanModel"></param>
        /// <returns></returns>
        public async Task<bool> CreatePaymentPlan(PaymentPlan paymentPlanModel)
        {
            try
            { 
                if (paymentPlanModel != null)
                { 

                    PaymentPlan paymentPlan = new PaymentPlan();
                    paymentPlan.Id = Guid.NewGuid();
                    await _applicationDbContext.tblPaymentPlan.AddAsync(paymentPlanModel);
                    await _applicationDbContext.SaveChangesAsync();  //Inserted payment record into table

                    // Logic to create installments.
                    for (int i = 0; i < paymentPlanModel.Installments; i++)
                    {
                        var installment = new Installment();
                        installment.Amount = paymentPlanModel.PurchaseAmount / paymentPlanModel.Installments;
                        installment.DueDate = (i == 0) ? DateTime.Today.AddDays(0) : DateTime.Today.AddDays(paymentPlanModel.PurchaseFrequency * i); 
                        installment.Id = new Guid();
                        installment.PurchaseId = paymentPlan.Id.ToString();

                        await _applicationDbContext.tblInstallment.AddAsync(installment);
                        await _applicationDbContext.SaveChangesAsync();
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
