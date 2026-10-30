using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using AGPC.FixedLayout.Tests.DTO;

namespace AGPC.FixedLayout.Tests
{
    public class FixedLayoutPerformanceTests
    {
        private const int ItemCount = 300000;
        private readonly ITestOutputHelper _output;

        public FixedLayoutPerformanceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        [Trait("Category", "Performance")]
        public void ToConcatString_CompareNuGet100WithCurrent_300000Items()
        {
            string legacyAssemblyPath = Path.Combine(AppContext.BaseDirectory, "legacy", "AGPC.FixedLayout.dll");
            Assert.True(File.Exists(legacyAssemblyPath), "The NuGet 1.0.0 assembly was not copied to the test output.");

            var legacyContext = new FixedLayoutLoadContext(legacyAssemblyPath);
            try
            {
                Assembly legacyDtos = legacyContext.LoadFromAssemblyPath(typeof(ReportOrdersDTO).Assembly.Location);
                Type legacyReportType = legacyDtos.GetType(typeof(ReportOrdersDTO).FullName, true);
                Type legacyOrderType = legacyDtos.GetType(typeof(OrderDTO).FullName, true);
                Type legacyProductType = legacyDtos.GetType(typeof(ProductDTO).FullName, true);
                MethodInfo legacySerialize = legacyReportType.GetMethod("ToConcatString", Type.EmptyTypes);

                // START Warming both paths so this comparison does not mostly measure first-use JIT and metadata setup.
                CreateCurrentReport(100).ToConcatString();
                legacySerialize.Invoke(CreateLegacyReport(legacyReportType, legacyOrderType, legacyProductType, 100), null);
                // END Warming both paths so this comparison does not mostly measure first-use JIT and metadata setup.

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                object legacyReport = CreateLegacyReport(legacyReportType, legacyOrderType, legacyProductType, ItemCount);
                Measurement legacy = Measure(() => (string)legacySerialize.Invoke(legacyReport, null));
                legacyReport = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                ReportOrdersDTO currentReport = CreateCurrentReport(ItemCount);
                Measurement current = Measure(currentReport.ToConcatString);

                _output.WriteLine("Records per run: {0:N0}", ItemCount);
                _output.WriteLine("NuGet 1.0.0: {0:N0} ms; {1:N0} bytes allocated on the calling thread.", legacy.ElapsedMilliseconds, legacy.AllocatedBytes);
                _output.WriteLine("Current source: {0:N0} ms; {1:N0} bytes allocated on the calling thread.", current.ElapsedMilliseconds, current.AllocatedBytes);
                _output.WriteLine("Output length: {0:N0} characters.", current.OutputLength);

                Assert.Equal(legacy.OutputLength, current.OutputLength);
                Assert.Equal(legacy.OutputSha256, current.OutputSha256);
            }
            finally
            {
                legacyContext.Unload();
            }
        }


        [Fact]
        [Trait("Category", "Performance")]
        public void ToConcatString_CompareNuGet100WithCurrent_ParallelLoad()
        {
            const int warmUpOperations = 10000;
            const int measuredOperations = 300000;
            
            int maxDegreeOfParallelism = Math.Max(2, Math.Min(Environment.ProcessorCount, 8));
            
            string testAssemblyPath = typeof(ProductDTO).Assembly.Location;
            string legacyAssemblyPath = Path.Combine(AppContext.BaseDirectory, "legacy", "AGPC.FixedLayout.dll");
            string currentAssemblyPath = Path.Combine(AppContext.BaseDirectory, "AGPC.FixedLayout.dll");

            var legacyContext = new FixedLayoutLoadContext(legacyAssemblyPath);
            var currentContext = new FixedLayoutLoadContext(currentAssemblyPath);
            try
            {
                Func<string> legacySerialize = CreateProductSerializer(legacyContext, testAssemblyPath);
                Func<string> currentSerialize = CreateProductSerializer(currentContext, testAssemblyPath);
                string expected = "Benchmark item".PadRight(50)
                    + 9.85M.ToString().PadRight(10)
                    + 298.ToString().PadRight(5)
                    + "Foods".PadRight(25);

                //The first concurrent pass exercises cold metadata initialization and records errors without assuming the old package must fail on every runtime.
                ParallelMeasurement legacyWarmUp = MeasureParallel(legacySerialize, expected, warmUpOperations, maxDegreeOfParallelism);
                ParallelMeasurement currentWarmUp = MeasureParallel(currentSerialize, expected, warmUpOperations, maxDegreeOfParallelism);

                ParallelMeasurement legacy = MeasureParallel(legacySerialize, expected, measuredOperations, maxDegreeOfParallelism);
                ParallelMeasurement current = MeasureParallel(currentSerialize, expected, measuredOperations, maxDegreeOfParallelism);

                _output.WriteLine("Concurrent workers: {0}", maxDegreeOfParallelism);
                _output.WriteLine("Initial concurrent pass: {0:N0} calls per version.", warmUpOperations);
                _output.WriteLine("NuGet 1.0.0 initial pass: {0} exceptions, {1} incorrect outputs.", legacyWarmUp.Errors, legacyWarmUp.Mismatches);
                _output.WriteLine("Current initial pass: {0} exceptions, {1} incorrect outputs.", currentWarmUp.Errors, currentWarmUp.Mismatches);
                _output.WriteLine("Measured pass: {0:N0} calls per version.", measuredOperations);
                _output.WriteLine("NuGet 1.0.0: {0:N0} ms; {1:N0} bytes allocated; {2} exceptions; {3} incorrect outputs.", legacy.ElapsedMilliseconds, legacy.AllocatedBytes, legacy.Errors, legacy.Mismatches);
                _output.WriteLine("Current source: {0:N0} ms; {1:N0} bytes allocated; {2} exceptions; {3} incorrect outputs.", current.ElapsedMilliseconds, current.AllocatedBytes, current.Errors, current.Mismatches);

                Assert.Equal(0, currentWarmUp.Errors);
                Assert.Equal(0, currentWarmUp.Mismatches);
                Assert.Equal(0, current.Errors);
                Assert.Equal(0, current.Mismatches);
            }
            finally
            {
                legacyContext.Unload();
                currentContext.Unload();
            }
        }

        private static Func<string> CreateProductSerializer(FixedLayoutLoadContext context, string testAssemblyPath)
        {
            Assembly testAssembly = context.LoadFromAssemblyPath(testAssemblyPath);
            Type productType = testAssembly.GetType(typeof(ProductDTO).FullName, true);
            object product = Activator.CreateInstance(productType);
            SetProperty(product, "Name", "Benchmark item");
            SetProperty(product, "Price", 9.85M);
            SetProperty(product, "IdCategory", 298);
            SetProperty(product, "CategoryDescription", "Foods");

            MethodInfo serialize = productType.GetMethod("ToConcatString", Type.EmptyTypes);
            return (Func<string>)serialize.CreateDelegate(typeof(Func<string>), product);
        }

        private static ParallelMeasurement MeasureParallel(Func<string> serialize, string expected, int operationCount, int maxDegreeOfParallelism)
        {
            long errors = 0;
            long mismatches = 0;
            long allocatedBefore = GC.GetTotalAllocatedBytes(true);
            var options = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
            var stopwatch = Stopwatch.StartNew();

            Parallel.For(0, operationCount, options, _ =>
            {
                try
                {
                    if (!String.Equals(serialize(), expected, StringComparison.Ordinal))
                        Interlocked.Increment(ref mismatches);
                }
                catch (Exception)
                {
                    Interlocked.Increment(ref errors);
                }
            });

            stopwatch.Stop();
            long allocatedBytes = GC.GetTotalAllocatedBytes(true) - allocatedBefore;
            return new ParallelMeasurement(stopwatch.ElapsedMilliseconds, allocatedBytes, errors, mismatches);
        }
        private static ReportOrdersDTO CreateCurrentReport(int count)
        {
            var orders = new List<OrderDTO>(count);
            DateTime orderDate = new DateTime(2024, 5, 25);

            for (int i = 0; i < count; i++)
            {
                orders.Add(new OrderDTO
                {
                    OrderId = i,
                    CreateDate = orderDate,
                    Product = new ProductDTO
                    {
                        Name = "Benchmark item",
                        Price = 9.85M,
                        IdCategory = 298,
                        CategoryDescription = "Foods"
                    },
                    Notes = "Performance comparison"
                });
            }

            return new ReportOrdersDTO
            {
                Name = "Performance test",
                Sponsor = "AGPC.FixedLayout",
                OrderList = orders,
                Code = 8728,
                CreateDate = orderDate
            };
        }

        private static object CreateLegacyReport(Type reportType, Type orderType, Type productType, int count)
        {
            var orders = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(orderType));
            DateTime orderDate = new DateTime(2024, 5, 25);

            for (int i = 0; i < count; i++)
            {
                object product = Activator.CreateInstance(productType);
                SetProperty(product, "Name", "Benchmark item");
                SetProperty(product, "Price", 9.85M);
                SetProperty(product, "IdCategory", 298);
                SetProperty(product, "CategoryDescription", "Foods");

                object order = Activator.CreateInstance(orderType);
                SetProperty(order, "OrderId", i);
                SetProperty(order, "CreateDate", orderDate);
                SetProperty(order, "Product", product);
                SetProperty(order, "Notes", "Performance comparison");
                orders.Add(order);
            }

            object report = Activator.CreateInstance(reportType);
            SetProperty(report, "Name", "Performance test");
            SetProperty(report, "Sponsor", "AGPC.FixedLayout");
            SetProperty(report, "OrderList", orders);
            SetProperty(report, "Code", 8728);
            SetProperty(report, "CreateDate", orderDate);
            return report;
        }

        private static void SetProperty(object instance, string propertyName, object value)
        {
            instance.GetType().GetProperty(propertyName).SetValue(instance, value, null);
        }

        private static Measurement Measure(Func<string> serialize)
        {
            long allocatedBefore = GC.GetAllocatedBytesForCurrentThread();
            var stopwatch = Stopwatch.StartNew();
            string output = serialize();
            stopwatch.Stop();
            long allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - allocatedBefore;

            int outputLength = output.Length;
            string outputSha256 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(output)));
            return new Measurement(stopwatch.ElapsedMilliseconds, allocatedBytes, outputLength, outputSha256);
        }

        private sealed class ParallelMeasurement
        {
            internal ParallelMeasurement(long elapsedMilliseconds, long allocatedBytes, long errors, long mismatches)
            {
                ElapsedMilliseconds = elapsedMilliseconds;
                AllocatedBytes = allocatedBytes;
                Errors = errors;
                Mismatches = mismatches;
            }

            internal long ElapsedMilliseconds { get; }
            internal long AllocatedBytes { get; }
            internal long Errors { get; }
            internal long Mismatches { get; }
        }
        private sealed class Measurement
        {
            internal Measurement(long elapsedMilliseconds, long allocatedBytes, int outputLength, string outputSha256)
            {
                ElapsedMilliseconds = elapsedMilliseconds;
                AllocatedBytes = allocatedBytes;
                OutputLength = outputLength;
                OutputSha256 = outputSha256;
            }

            internal long ElapsedMilliseconds { get; }
            internal long AllocatedBytes { get; }
            internal int OutputLength { get; }
            internal string OutputSha256 { get; }
        }

        private sealed class FixedLayoutLoadContext : AssemblyLoadContext
        {
            private readonly string _legacyAssemblyPath;

            internal FixedLayoutLoadContext(string legacyAssemblyPath) : base(isCollectible: true)
            {
                _legacyAssemblyPath = legacyAssemblyPath;
            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                if (assemblyName.Name == "AGPC.FixedLayout")
                    return LoadFromAssemblyPath(_legacyAssemblyPath);

                return null;
            }
        }
    }
}