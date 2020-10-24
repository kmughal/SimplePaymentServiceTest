namespace SimplePaymentServiceTests.Tests.PaymentSchemesTests
{
    using SimplePaymentServiceTests.Services;
    using SimplePaymentServiceTests.Stores;
    using SimplePaymentServiceTests.Types;
    using Moq;
    using System;
    using Xunit;

    public class FasterPaymentsTests
    {
        [Fact]
        public void ShouldNotSucceedIfPaymentSchemeIsNotFasterPayments()
        {
            // Arramge
            var mockDataStore = new Mock<IDataStore>();
            var fakeAccount = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
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
        public void ShouldNotSucceedIfPaymentSchemeIsFasterPaymentsAndBalanceIsLessThanAmount()
        {
            // Arramge
            var mockDataStore = new Mock<IDataStore>();
            var fakeAccount = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 100.0m
            };
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(fakeAccount);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs, Amount = 300 };

            // Act
            var actual = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.False(actual.Success);
        }

        [Fact]
        public void ShouldSucceedIfPaymentSchemeIsFasterPaymentsAndBalanceIsGreaterThanAmount()
        {
            // Arramge
            var mockDataStore = new Mock<IDataStore>();
            var fakeAccount = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 250.0m,
            };
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(fakeAccount);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.FasterPayments,
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
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 250.0m
            };
            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(fakeAccount);
            var paymentService = new PaymentService(mockDataStore.Object);
            var paymentRequest = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.FasterPayments,
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