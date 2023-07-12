# `SimdJsonSharp` vs `System.Text.Json`
## Summary
Comparing using [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview) and [SimdJsonSharp](https://github.com/EgorBo/SimdJsonSharp) 
to deserialize a json file to a list of `Rec` objects.

## Results
![BarPlot](./images/SimdJsonBench.Benchmarks-barplot.png)

```

BenchmarkDotNet v0.13.6, Arch Linux
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.107
  [Host]     : .NET 7.0.7 (7.0.723.32201), X64 RyuJIT AVX2
  Job-QAAPEU : .NET 7.0.7 (7.0.723.32201), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|         Method |     Mean |   Error |  StdDev | Ratio |       Gen0 |       Gen1 |      Gen2 | Allocated | Alloc Ratio |
|--------------- |---------:|--------:|--------:|------:|-----------:|-----------:|----------:|----------:|------------:|
|  SimdJsonSharp | 264.3 ms | 1.78 ms | 1.39 ms |  1.00 | 10000.0000 | 10000.0000 |         - | 127.67 MB |        1.00 |
| SystemTextJson | 573.7 ms | 2.57 ms | 2.14 ms |  2.17 | 10000.0000 | 10000.0000 | 2000.0000 | 131.57 MB |        1.03 |

