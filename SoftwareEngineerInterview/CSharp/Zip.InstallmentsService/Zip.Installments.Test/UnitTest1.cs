using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        [SetUp]
        public void Setup()
        {
            var paymentPlan = new PaymentPlan
            {
                Id = System.Guid.NewGuid(),
                Installments = 4,
                PurchaseAmount = 100,
                PurchaseFrequency = 14
            }; 
            var myDbContextMock = new Mock<IApplicationDbContext>();
            //myDbContextMock.Setup(p => p.Entities).Returns(myDbContextMock.GetQueryableMockDbSet<PaymentPlan>(paymentPlan));
            //myDbContextMock.Setup(p => p.SaveChanges()).Returns(1);
        }

        [Test] 
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnTrue()
        { 
            var paymentPlan = new PaymentPlan
            {
                Id = System.Guid.NewGuid(),
                Installments = 4,
                PurchaseAmount = 100,
                PurchaseFrequency = 14
            };
            var mock = new Mock<IPaymentPlanRepository>();
            mock.Setup(p => p.CreatePaymentPlan(It.IsAny<PaymentPlan>())).Returns(true);
            var sut = new PaymentPlanRepository(mock.Object);

            var result = sut.CreatePaymentPlan(paymentPlan);

            Assert.IsTrue(result);
        }
    }
}