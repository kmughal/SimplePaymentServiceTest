namespace SimplePaymentServiceTests.ConsoleApp
{
    using SimplePaymentServiceTests.Infrastructure;
    using SimplePaymentServiceTests.Services;
    using static System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            var paymentService = (IPaymentService) IocProvider.ServiceProvider.GetService(typeof(IPaymentService));
            var result = paymentService.MakePayment(null);
            WriteLine($"Payment Result : {result.Success}");
            ReadLine();
        }
    }
}
