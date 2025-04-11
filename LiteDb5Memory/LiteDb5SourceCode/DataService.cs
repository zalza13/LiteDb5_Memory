using static LiteDB5.Constants;

namespace LiteDB5.Engine
{
    public class DataService
    {
        /// <summary>
        /// Bytes used in each offset slot (to store segment position (2) + length (2))
        /// </summary>
        public const int SLOT_SIZE = 4;

        /// <summary>
        /// Get fixed part of DataBlock (6 bytes)
        /// </summary>
        public const int DATA_BLOCK_FIXED_SIZE = 1 + // Extend
                                                 PageAddress.SIZE; // NextBlock

        /// <summary>
        /// Get maximum data bytes[] that fit in 1 page = 8150
        /// </summary>
        public const int MAX_DATA_BYTES_PER_PAGE =
            PAGE_SIZE - // 8192
            PAGE_HEADER_SIZE - // [32 bytes]
            SLOT_SIZE - // [4 bytes]
            DATA_BLOCK_FIXED_SIZE; // [6 bytes];
    }
}
