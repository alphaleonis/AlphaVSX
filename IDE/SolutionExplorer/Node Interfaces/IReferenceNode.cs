using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public interface IReferenceNode : ISolutionExplorerNode
   {
      string Description { get; }
      bool CopyLocal { get; set; }

   }
}

