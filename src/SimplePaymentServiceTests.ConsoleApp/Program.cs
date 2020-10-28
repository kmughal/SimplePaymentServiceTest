namespace SimplePaymentServiceTests.ConsoleApp
{
    using SimplePaymentServiceTests.Infrastructure;
    using SimplePaymentServiceTests.Services;
    using static System.Console;

    public record Person(string FirstName, string LastName);

    class Program
    {
        static void Main(string[] args)
        {
            var paymentService = (IPaymentService)IocProvider.ServiceProvider.GetService(typeof(IPaymentService));
            var result = paymentService.MakePayment(null);
            WriteLine($"Payment Result : {result.Success}");
            var p = new Person("Khurram", "Mughal");
            var (firstname, lastname) = (p.FirstName, p.LastName);
            var p1 = p with { LastName = "mughal" };
            WriteLine($"{p1.FirstName} {p1.LastName}");

            WriteLine($"{firstname} {lastname}");
            ReadLine();
        }
    }


}

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit { }
}
