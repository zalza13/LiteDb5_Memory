using LiteDB.Engine;
using LiteDB.Engine.Disk;
using LiteDB5;
using LiteDB5.Engine;
using System.Collections.Concurrent;

/// <summary>
/// This test simulates heavy concurrent access to the MemoryCache system.
/// It creates multiple threads performing random reads and writes on dummy pages,
/// verifying that all pages are properly released after the operations.
/// 
/// The goal is to stress the cache system and confirm that:
/// - No memory leaks occur (PagesInUse == 0)
/// - Writable pages are properly discarded or recycled
/// - ShareCounter is correctly managed even under high concurrency
/// 
/// This test is intentionally chaotic to detect concurrency bugs in real-world scenarios.
/// </summary>
public class MemoryCacheStressTest
{
    [Fact]
    public void Should_Not_Leak_Memory_When_Accessed_Concurrently()
    {
        var threadCount = 20;
        var iterationsPerThread = 1000;
        var rnd = new Random();
        var cache = new MemoryCacheV2(new[] { 128 }, new LiteDB.Engine.Disk.MemoryCacheConfig());

        var tasks = new List<Task>();

        for (int i = 0; i < threadCount; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                for (int j = 0; j < iterationsPerThread; j++)
                {
                    var pageID = rnd.Next(1, 100);
                    var isWrite = rnd.Next(0, 2) == 0;

                    if (isWrite)
                    {
                        var page = cache.GetWritablePage(pageID, LiteDB5.Engine.FileOrigin.Data, DummyLoad);
                        Thread.Sleep(rnd.Next(1, 3));
                        cache.DiscardPage(page);
                    }
                    else
                    {
                        var page = cache.GetReadablePage(pageID, LiteDB5.Engine.FileOrigin.Data, DummyLoad);
                        Thread.Sleep(rnd.Next(1, 3));
                        page.Release();
                    }
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        Assert.Equal(0, cache.PagesInUse);
    }

    [Fact]
    public void MemoryCache_Should_Survive_Ultra_Concurrency_Stress()
    {
        var threadCount = 100;
        var iterationsPerThread = 500;
        var rnd = new Random();

        var cache = new MemoryCacheV2(new[] { 256, 512, 1024 }, new MemoryCacheConfig()); 

        var tasks = new List<Task>();

        for (int i = 0; i < threadCount; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                var localRnd = new Random(Guid.NewGuid().GetHashCode()); 

                for (int j = 0; j < iterationsPerThread; j++)
                {
                    var pageID = localRnd.Next(1, 500);
                    var isWrite = localRnd.Next(0, 2) == 0;

                    try
                    {
                        if (isWrite)
                        {
                            var page = cache.GetWritablePage(pageID, FileOrigin.Data, DummyLoad);
                            Thread.SpinWait(localRnd.Next(10, 100)); 
                            cache.DiscardPage(page);
                        }
                        else
                        {
                            var page = cache.GetReadablePage(pageID, FileOrigin.Data, DummyLoad);
                            Thread.SpinWait(localRnd.Next(10, 100)); 
                            page.Release();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Error: {ex.Message}");
                        throw;
                    }
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        Assert.Equal(0, cache.PagesInUse);
    }

    /// <summary>
    /// This test creates maximum chaos in the memory cache system.
    /// It uses 200 threads doing 1000 operations each with a lot of collisions,
    /// artificial CPU pressure, memory pressure, and even forced GC to simulate extreme scenarios.
    /// </summary>
    [Fact]
    public void MemoryCache_Should_Handle_Maximum_Chaos()
    {
        var threadCount = 200;
        var iterationsPerThread = 1000;
        var cache = new MemoryCacheV2(new[] { 128, 256, 512 }, new MemoryCacheConfig());
        var errors = new ConcurrentBag<string>();

        var tasks = new List<Task>();

        for (int i = 0; i < threadCount; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                var rnd = new Random(Guid.NewGuid().GetHashCode());

                for (int j = 0; j < iterationsPerThread; j++)
                {
                    var pageID = rnd.Next(1, 30); // Force collisions
                    var isWrite = rnd.Next(0, 2) == 0;

                    try
                    {
                        if (isWrite)
                        {
                            var page = cache.GetWritablePage(pageID, FileOrigin.Data, SlowDummyLoad);
                            if (rnd.Next(0, 4) == 0) Thread.Sleep(rnd.Next(1, 5)); // simulate slow I/O
                            else Thread.SpinWait(rnd.Next(10, 100));
                            cache.DiscardPage(page);
                        }
                        else
                        {
                            var page = cache.GetReadablePage(pageID, FileOrigin.Data, SlowDummyLoad);
                            if (rnd.Next(0, 4) == 0) Thread.Sleep(rnd.Next(1, 5));
                            else Thread.SpinWait(rnd.Next(10, 100));
                            page.Release();
                        }

                        // Occasionally force garbage collection
                        if (j % 500 == 0) GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"[Thread {Thread.CurrentThread.ManagedThreadId}] {ex.Message}");
                    }
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        if (!errors.IsEmpty)
        {
            foreach (var err in errors) Console.WriteLine(err);
            Assert.True(false, $"Chaos test failed with {errors.Count} errors");
        }

        Assert.Equal(0, cache.PagesInUse);
    }

    private static void SlowDummyLoad(long pos, BufferSlice buffer)
    {
        Thread.SpinWait(500); // simulate slow disk
        buffer.Fill((byte)(pos % 255));
    }

    private static void DummyLoad(long pos, BufferSlice buffer)
    {
        buffer.Fill((byte)(pos % 255));
    }
}
