using Shouldly;
using Xunit;

namespace Zip.InstallmentsService.Test
{
    public class PaymentPlanFactoryTests
    {
        [Fact]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnValidPaymentPlan()
        {
            // Arrange
            var paymentPlanRepository = new PaymentPlanRepository();
            var mock = new Mock<IPaymentPlanRepository>();
            mock.Setup(p => p.DoSomething(It.IsAny<string>())).Returns(true);
            var sut = new Service(mock.Object);

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);

            // Assert
            paymentPlan.ShouldNotBeNull();
        }

        [Fact]
        public void WhenCreatePaymentPlanWithInvalidPaymentAmount_ShouldReturnErrorMessage()
        {
            // Arrange
            var PaymentPlan = new PaymentPlan();

            // Act
            var paymentPlan = PaymentPlan.CreatePaymentPlan(123.45M);

            // Assert
            paymentPlan.ShouldNotBeNull();
        }
    }
}
