# LiteDb 5 MemoryCacheV2 Updates

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
- **AllowExtendSegments**: Controls if new memory segments can be allocated.
- **ReuseOldPages**: Controls whether pages with ShareCounter == 0 can be reused.
- **LogAllocations**: Enables logging when new pages or segments are allocated.

---

## Benefits

- **Memory Optimization**: Reduces the risk of RAM spikes by cleaning up unused pages.
- **Scalability**: Ensures the cache remains responsive under high load.
- **Configurable**: Allows fine-tuning for different workloads.

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
