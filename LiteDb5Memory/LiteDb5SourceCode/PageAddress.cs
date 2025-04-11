using System.Diagnostics;

namespace LiteDB5.Engine
{
    /// <summary>
    /// Represents a page address inside a page structure - index could be byte offset position OR index in a list (6 bytes)
    /// </summary>
    [DebuggerStepThrough]
    public struct PageAddress
    {
        public const int SIZE = 5;
    }
}