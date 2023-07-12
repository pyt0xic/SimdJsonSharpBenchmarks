using System.Text.Json;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
// using SimdJsonBench.Generated.Simdjson;
using SimdJsonSharp;
// using static SimdJsonBench.Generated.Simdjson.Dom.Parser;
using System.IO;
using System;
using BrotliSharpLib;

namespace SimdJsonBench;

[MemoryDiagnoser]
[RPlotExporter]
public class Benchmarks
{
    private byte[] _bytes;
    private List<Rec> _recs;

    [IterationSetup]
    public void Setup()
    {
        _recs = new List<Rec>(767660);
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        using var compressed = File.OpenRead("data.json.br");
        using var jsonStream = new MemoryStream();
        using var brotliStream = new BrotliStream(compressed, System.IO.Compression.CompressionMode.Decompress);
        brotliStream.CopyTo(jsonStream);
        _bytes = jsonStream.ToArray();
        _recs = new List<Rec>(767660);
    }

    [IterationCleanup]
    public void Cleanup()
    {
        _recs = null;
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _bytes = null;
        _recs = null;
    }

    [Benchmark(Baseline = true)]
    public unsafe void SimdJsonSharp()
    {
        var jobno = "jobno"u8;
        var invoiced = "invoiced"u8;
        var invoiceno = "invoiceno"u8;
        var email = "email"u8;
        var oemail = "oemail"u8;
        var vin = "vin"u8;
        var branch = "branch"u8;
        fixed (byte* ptr = _bytes)
        {
            // SimdJsonN -- N stands for Native, it means we are using Bindings for simdjson native lib
            // SimdJson -- fully managed .NET Core 3.0 port
            using var iterator = SimdJson.ParseJsonAndOpenIterator(ptr, _bytes.Length);
            //open iterator:
            bool lastWasKey = false;
            Span<byte> key = stackalloc byte[0];
            var rec = new Rec(default, default, default, default, default, default, default);
            // iterate over document and determine whether the token is a key or value then print
            while (iterator.MoveForward())
            {
                var jsonTokenType = iterator.GetTokenType();
                if (jsonTokenType is JsonTokenType.EndObject)
                {
                    _recs.Add(rec);
                    continue;
                }
                if (iterator.IsString && !lastWasKey)
                {
                    // Console.Write($"Key: {iterator.GetUtf16String()}");
                    key = new Span<byte>(
                        iterator.GetUtf8String(),
                        iterator.GetUtf8StringLength()
                    );
                    lastWasKey = true;
                    continue;
                }
                if (!iterator.IsString)
                {
                    continue;
                }
                if (key.SequenceEqual(jobno))
                {
                    rec.Jobno = iterator.GetUtf16String();
                }
                else if (key.SequenceEqual(invoiced))
                {
                    rec.Invoiced = iterator.GetUtf16String();
                }
                else if (key.SequenceEqual(invoiceno))
                {
                    rec.Invoiceno = iterator.GetUtf16String();
                }
                else if (key.SequenceEqual(email))
                {
                    rec.Email = iterator.GetUtf16String();
                }
                else if (key.SequenceEqual(oemail))
                {
                    rec.Oemail = iterator.GetUtf16String();
                }
                else if (key.SequenceEqual(vin))
                {
                    rec.Vin = iterator.GetUtf16String();
                }
                else if (key.SequenceEqual(branch))
                {
                    rec.Branch = iterator.GetUtf16String();
                }
                lastWasKey = false;
            }
        }
    }

    [Benchmark()]
    public void SystemTextJson()
    {
        _recs = JsonSerializer.Deserialize<List<Rec>>(_bytes);
    }

    // [Benchmark()]
    // public void NewtonsoftJson()
    // {
    //     using var stream = new MemoryStream(_bytes);
    //     using var reader = new StreamReader(stream);
    //     using var jsonReader = new JsonTextReader(reader);
    //     _recs = Newtonsoft.Json.JsonSerializer.CreateDefault().Deserialize<List<Rec>>(jsonReader);
    // }
}
