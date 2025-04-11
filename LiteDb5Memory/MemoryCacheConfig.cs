using System;

namespace LiteDB.Engine.Disk
{
    public class MemoryCacheConfig
    {
        public int MaxFreePages { get; set; } = 500;
        public TimeSpan MaxIdleTime { get; set; } = TimeSpan.FromMinutes(1);
        public int BatchSize { get; set; } = 100;
        public TimeSpan CleanupInterval { get; set; } = TimeSpan.FromMinutes(5);
    }
}
