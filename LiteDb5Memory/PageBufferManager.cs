using LiteDB5.Engine;
using System.Collections.Concurrent;

namespace LiteDB.Engine.Disk
{
    internal class PageBufferManager
    {
        private readonly MemoryCacheConfig _config;
        private readonly ConcurrentQueue<PageBuffer> _free;
        private DateTimeOffset _lastCleanupRun = DateTimeOffset.MinValue;

        public PageBufferManager(MemoryCacheConfig config, ConcurrentQueue<PageBuffer> free)
        {
            _config = config;
            _free = free;
        }

        /// <summary>
        /// Clean up old pages from the free queue based on the maximum idle time.
        /// Only runs if at least 5 minutes have passed since the last cleanup.
        /// Processes pages in batches of up to "batchSize" per run.
        /// </summary>
        public void Cleanup()
        {
            if ((_free?.Count ?? 0) == 0)
            {
                return;
            }

            if ((DateTimeOffset.UtcNow - _lastCleanupRun) < _config.CleanupInterval &&
                _free.Count <= _config.MaxFreePages)
            {
                return;
            }

            _lastCleanupRun = DateTimeOffset.UtcNow;
            var now = DateTime.UtcNow.Ticks;
            var itemsToKeep = new List<PageBuffer>();

            int processed = 0;
            while (_free.TryDequeue(out var page))
            {
                if ((now - (page.TimestampFree ?? now)) < _config.MaxIdleTime.Ticks)
                {
                    itemsToKeep.Add(page);
                }
                else
                {
                    page.Clear();
                    page = null;
                }

                processed++;
                if (processed >= _config.BatchSize) 
                    break;
            }

            foreach (var page in itemsToKeep)
            {
                _free.Enqueue(page);
            }
        }
    }
}
