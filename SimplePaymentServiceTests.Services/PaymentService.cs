namespace SimplePaymentServiceTests.Services
{
    using SimplePaymentServiceTests.Stores;
    using SimplePaymentServiceTests.Types;

    public class PaymentService : IPaymentService
    {
        private readonly IDataStore _dataStore;

        public PaymentService(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            if (IsPaymentRequestNotValid(request)) return MakePaymentResult.CreateVoidPaymentResult();

            var account = _dataStore.GetAccount(request.DebtorAccountNumber);
            if (account is null) return MakePaymentResult.CreateVoidPaymentResult();

            var isAllowedToPay = CanPaymentSucceed(account, request);

            if (!isAllowedToPay) return MakePaymentResult.CreateMakePaymentResultForFailure();

            DeductPayment(account, request.Amount);
            return MakePaymentResult.CreateMakePaymentResultForSuccess();
        }

        private bool CanPaymentSucceed(Account account, MakePaymentRequest request)
        {
            var (balance, accountStatus, allowedPaymentScheme) = (account.Balance, account.Status, account.AllowedPaymentSchemes);
            var (amount, paymentScheme) = (request.Amount, request.PaymentScheme);
            var accountIsLive = (accountStatus == AccountStatus.Live);
            var accountHasEnoughBalance = balance > amount;

            return paymentScheme switch
            {
                PaymentScheme.FasterPayments => allowedPaymentScheme.HasFlag(AllowedPaymentSchemes.FasterPayments) && accountHasEnoughBalance,
                PaymentScheme.Chaps => allowedPaymentScheme.HasFlag(AllowedPaymentSchemes.Chaps) && accountIsLive,
                _ => false
            };
        }

        private bool IsPaymentRequestNotValid(MakePaymentRequest request) => (request is null ||
               string.IsNullOrWhiteSpace(request.CreditorAccountNumber) ||
               string.IsNullOrWhiteSpace(request.DebtorAccountNumber) ||
               request.Amount == default ||
               request.PaymentDate == default
               );

        private void DeductPayment(Account account, decimal amount)
        {
            account.Balance -= amount;
            _dataStore.UpdateAccount(account);
        }
    }
}