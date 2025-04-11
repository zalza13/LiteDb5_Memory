using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDb5Memory.LiteDb5SourceCode
{
    public class BasePage
    {
        internal enum PageType { Empty = 0, Header = 1, Collection = 2, Index = 3, Data = 4 }
    }
}
