namespace SimplePaymentServiceTests.Tests.ServicesTests
{
    using SimplePaymentServiceTests.Services;
    using SimplePaymentServiceTests.Stores;
    using SimplePaymentServiceTests.Types;
    using Moq;
    using System;
    using Xunit;

    public class PaymentServiceTests
    {
        [Fact]
        public void ShouldNotSucceedIfPaymentRequestIsNull()
        {
            // Arrange
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns<Account>(null);
            var paymentService = new PaymentService(mockDataStore.Object);

            // Act
            var actual = paymentService.MakePayment(null);

            // Assert
            Assert.False(actual.Success);
        }

        [Fact]
        public void ShouldNotSucceedIfCreditorAccountNumberIsNotProvided()
        {
            // Arrange
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns<Account>(null);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest
            {
                Amount = 50.0m,
                DebtorAccountNumber = "_fake_debtor_account_number",
                PaymentDate = DateTime.UtcNow,
                PaymentScheme = PaymentScheme.Chaps
            };

            // Act
            var actual = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.False(actual.Success);
        }

        [Fact]
        public void ShouldNotSucceedIfDebtorAccountNumberIsNotProvided()
        {
            // Arrange
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns<Account>(null);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest
            {
                Amount = 50.0m,
                CreditorAccountNumber = "_fake_creditor_account_number",
                PaymentDate = DateTime.UtcNow,
                PaymentScheme = PaymentScheme.Chaps
            };

            // Act
            var actual = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.False(actual.Success);
        }

        [Fact]
        public void ShouldNotSucceedIfDebtorAmountIsNotProvided()
        {
            // Arrange
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns<Account>(null);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest
            {
                CreditorAccountNumber = "_fake_creditor_account_number",
                DebtorAccountNumber = "_fake_debtor_account_number",
                PaymentDate = DateTime.UtcNow,
                PaymentScheme = PaymentScheme.Chaps
            };

            // Act
            var actual = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.False(actual.Success);
        }

        [Fact]
        public void ShouldNotSucceedIfDebtorPaymentDatetIsNotProvided()
        {
            // Arrange
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns<Account>(null);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest
            {
                CreditorAccountNumber = "_fake_creditor_account_number",
                DebtorAccountNumber = "_fake_debtor_account_number",
                PaymentScheme = PaymentScheme.Chaps,
                Amount = 100.0m
            };

            // Act
            var actual = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.False(actual.Success);
        }
    }
}