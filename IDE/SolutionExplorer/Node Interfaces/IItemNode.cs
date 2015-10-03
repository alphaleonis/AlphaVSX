using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public interface IItemNode : ISolutionExplorerNode
   {
      void Delete();

      bool IsDirty { get; }

      string FullPath { get; }
   }
}

