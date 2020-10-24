namespace SimplePaymentServiceTests.Tests.PaymentSchemesTests
{
    using SimplePaymentServiceTests.Services;
    using SimplePaymentServiceTests.Stores;
    using SimplePaymentServiceTests.Types;
    using Moq;
    using Xunit;

    public class BacsPaymentTests
    {
        [Fact]
        public void PaymentShouldNotSucceed()
        {
            // Arramge
            var mockDataStore = new Mock<IDataStore>();
            var fakeAccount = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(fakeAccount);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };

            // Act
            var actual = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.False(actual.Success);
        }
    }
}