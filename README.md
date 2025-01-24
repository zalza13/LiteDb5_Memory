Cleaner Process for Memory Management

To optimize memory usage and prevent unnecessary RAM spikes, a cleanup process has been introduced in MemoryCacheV2. The cleanup is managed by the newly added PageBufferManager and works as follows:

    Configurable Cleanup:
        The cleanup process runs periodically based on a configurable CleanupInterval.
        It evaluates pages in the free queue (_free) and removes those that exceed the MaxIdleTime threshold.

    Batch Processing:
        Pages are processed in batches, limited by the configurable BatchSize, to maintain performance even under high load.

    Reusability:
        Pages that are still valid are retained in the free queue, ensuring efficient memory reuse without frequent allocations.

This change reduces the likelihood of memory spikes while keeping the cache responsive and scalable.
