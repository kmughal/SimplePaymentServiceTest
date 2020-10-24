namespace SimplePaymentServiceTests.Services
{
    using SimplePaymentServiceTests.Types;

    public interface IPaymentService
    {
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
