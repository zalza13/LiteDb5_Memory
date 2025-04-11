namespace LiteDB5
{
    public static class BufferSliceExtensions
    {
        public static UInt32 ReadUInt32(this BufferSlice buffer, int offset)
        {
            return BitConverter.ToUInt32(buffer.Array, buffer.Offset + offset);
        }
    }
}