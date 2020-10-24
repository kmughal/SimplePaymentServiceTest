namespace SimplePaymentServiceTests.Tests.PaymentSchemesTests
{
    using SimplePaymentServiceTests.Services;
    using SimplePaymentServiceTests.Stores;
    using SimplePaymentServiceTests.Types;
    using Moq;
    using System;
    using Xunit;

    public class ChapsTests
    {
        [Fact]
        public void ShouldNotSucceedIfPaymentSchemeIsNotChapsPayments()
        {
            // Arramge
            var mockDataStore = new Mock<IDataStore>();
            var fakeAccount = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
            };
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(fakeAccount);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };

            // Act
            var actual = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.False(actual.Success);
        }

        [Fact]
        public void ShouldNotSucceedIfPaymentSchemeIsChapsAndAccountStatusIsNotLive()
        {
            // Arramge
            var mockDataStore = new Mock<IDataStore>();
            var fakeAccount = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Disabled
            };
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(fakeAccount);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };

            // Act
            var actual = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.False(actual.Success);
        }

        [Fact]
        public void ShouldSucceedIfPaymentSchemeIsChapsPaymentsAndAccountIsLive()
        {
            // Arramge
            var mockDataStore = new Mock<IDataStore>();
            var fakeAccount = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Balance = 250.0m,
                Status = AccountStatus.Live
            };
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(fakeAccount);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Chaps,
                Amount = 200,
                CreditorAccountNumber = "_fake_creditor_account_number",
                DebtorAccountNumber = "_fake_debtor_account_number",
                PaymentDate = DateTime.UtcNow,
            };

            // Act
            var actual = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.True(actual.Success);
        }

        [Fact]
        public void ShouldDeductAmountIfPaymentSucceeds()
        {
            // Arramge
            var mockDataStore = new Mock<IDataStore>();
            var fakeAccount = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Balance = 250.0m,
                Status = AccountStatus.Live
            };
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(fakeAccount);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Chaps,
                Amount = 200,
                CreditorAccountNumber = "_fake_creditor_account_number",
                DebtorAccountNumber = "_fake_debtor_account_number",
                PaymentDate = DateTime.UtcNow,
            };

            // Act
            var actual = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.Equal(50.0m, fakeAccount.Balance);
            mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }
    }
}