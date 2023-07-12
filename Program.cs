using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace SimdJsonBench
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = DefaultConfig.Instance;
            // config.WithOptions(ConfigOptions.DisableOptimizationsValidator);
            config = config.WithOption(ConfigOptions.DisableOptimizationsValidator, true);
            var summary = BenchmarkRunner.Run<Benchmarks>(config, args);

            // Use this to select benchmarks from the console:
            // var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
        }
    }
}
