namespace SimplePaymentServiceTests.Types
{
    public class MakePaymentResult
    {
        public bool Success { get; set; }

        public static MakePaymentResult CreateMakePaymentResultForFailure() => new MakePaymentResult();

        public static MakePaymentResult CreateMakePaymentResultForSuccess() => new MakePaymentResult { Success = true };

        public static MakePaymentResult CreateVoidPaymentResult() => new MakePaymentResult();
    }
}
