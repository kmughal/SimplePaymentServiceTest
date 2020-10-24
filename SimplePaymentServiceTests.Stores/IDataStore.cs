namespace SimplePaymentServiceTests.Stores
{
    using SimplePaymentServiceTests.Types;

    public interface IDataStore
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}
