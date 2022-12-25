using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;
using Zip.Installments.DomainEntities;
using Zip.Installments.Repositories;
using Zip.Installments.Repositories.AppContext;

namespace Zip.Installments.Test
{
    public static class DbContextMock
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            return dbSet.Object;
        }
    }
    public class Tests
    {
        protected Mock<IPaymentPlanRepository> paymentPlanRepository;
        protected Mock<ILogger> logger; 
        private PaymentPlan paymentPlan;

        [SetUp]
        public void Setup()
        {
            paymentPlan = new PaymentPlan
            {
                Id = System.Guid.NewGuid(),
                Installments = 4,
                PurchaseAmount = 100,
                PurchaseFrequency = 14
            };  
        }

        [Test] 
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnTrue()
        {
            var testObject = new PaymentPlan();
            var context = new Mock<ApplicationDbContext>();
            var dbSetMock = new Mock<DbSet<PaymentPlan>>();
            var mock = new Mock<IPaymentPlanRepository>();
            var _mockLogger = new Mock<ILogger<IPaymentPlanRepository>>();
            context.Setup(x => x.Set<PaymentPlan>()).Returns(dbSetMock.Object); 
            mock.Setup(p => p.CreatePaymentPlan(It.IsAny<PaymentPlan>())).Returns(new Task<bool>(() => true));
            context.Setup(p => p.SaveChanges()).Returns(1);

            var sut = new PaymentPlanRepository(context.Object, _mockLogger.Object); 
            var result = sut.CreatePaymentPlan(paymentPlan);  
            Assert.IsTrue(result.Result);
        }
    }
}