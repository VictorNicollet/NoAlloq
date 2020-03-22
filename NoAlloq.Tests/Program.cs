using BenchmarkDotNet.Running;

namespace NoAlloq.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
                .Run(new[] { "--filter", "*" });
        }
    }
}
