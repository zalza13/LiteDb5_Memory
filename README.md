# MemoryCacheV2 Updates

## Overview
This update introduces a **cleanup process** to improve memory management in `MemoryCacheV2`. The changes aim to optimize RAM usage and prevent spikes by efficiently managing free pages.

---

## Key Changes

### 1. Cleaner Process

The newly added `PageBufferManager` manages the cleanup process, which:

- Runs periodically based on a configurable `CleanupInterval`.
- Evaluates pages in the free queue (`_free`) and removes those that exceed the `MaxIdleTime` threshold.
- Processes pages in batches, limited by the configurable `BatchSize`.

### 2. Configurable Behavior

A new configuration class, `MemoryCacheConfig`, was introduced to control the behavior:

- **MaxFreePages**: Limits the maximum number of pages in the free queue.
- **MaxIdleTime**: Specifies the maximum idle time for a page before it is removed.
- **BatchSize**: Defines how many pages are processed in each cleanup cycle.
- **CleanupInterval**: Controls how often the cleanup process is triggered.
- **AllowExtendSegments**: Enables or disables automatic memory segment expansion.
- **ReuseOldPages**: Allows reuse of pages that are not in use.
- **LogAllocations**: Enables logging when pages/segments are allocated.

---

## Benefits

- **Memory Optimization**: Reduces the risk of RAM spikes by cleaning up unused pages.
- **Scalability**: Ensures the cache remains responsive under high load.
- **Configurable**: Allows fine-tuning for different workloads and environments.

---

## How to Use

1. **Configure `MemoryCacheConfig`:**

   ```csharp
   var config = new MemoryCacheConfig
   {
       MaxFreePages = 500,
       MaxIdleTime = TimeSpan.FromMinutes(1),
       BatchSize = 100,
       CleanupInterval = TimeSpan.FromMinutes(5),
       AllowExtendSegments = true,
       ReuseOldPages = true,
       LogAllocations = false
   };
   ```

2. **Initialize `MemoryCacheV2`:**

   ```csharp
   var segmentSizes = new[] { 1024, 2048, 4096 }; // Example segment sizes
   var memoryCache = new MemoryCacheV2(segmentSizes, config);
   ```

---

## 🧪 Stress Testing

To ensure the robustness of `MemoryCacheV2` under extreme conditions, this repo includes **brutal stress tests**:

- **High Concurrency**: Up to 200 threads, each performing 1,000 random read/write operations.
- **Page ID Collisions**: Pages restricted to a small range (e.g., IDs 1–30) to force contention.
- **Forced Garbage Collection**: `GC.Collect()` calls during operations to simulate memory pressure and force finalizer execution.
- **Artificial Delays**: Use of `Thread.Sleep` and `Thread.SpinWait` to simulate I/O bottlenecks and CPU-bound chaos.
- **Segment Expansion**: Segments expand dynamically under load to test scalability.

These tests validate:

- No memory leaks (`PagesInUse == 0` after all operations).
- Correct use and release of `PageBuffer` instances.
- Stability under pressure.

> 💥 If it survives this, it’s ready for war.

---

## 🧟 Internal Classes from LiteDB

To allow these tests to run independently, this repo includes **copied internal components** from [LiteDB v5](https://github.com/mbdavid/LiteDB):

- `PageBuffer`
- `BufferSlice`
- `FileOrigin`
- `Constants`
- `LiteException` (minimal)
- `ENSURE()` / `DEBUG()` helpers

> ⚠️ These files are only for testing. Do not use them in production or other apps.

---

## 🧹 .gitignore

This repo uses the default `.gitignore` from `dotnet new gitignore`, which excludes:

- `/bin`, `/obj`, `.vs/`, `.vscode/`
- `*.user`, `*.suo`, `TestResults/`, etc.

---

## ✅ Requirements

- .NET 6 SDK or later
- `xUnit` test runner
- Console or IDE to run the tests (e.g., VSCode or JetBrains Rider)

---

## 💬 License

This repo is for educational and experimental purposes.  
All original LiteDB source code belongs to the [LiteDB project](https://github.com/mbdavid/LiteDB).
