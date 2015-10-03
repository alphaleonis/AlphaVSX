using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public interface ISolutionItemNode : ISolutionExplorerNode
   {
      string FullPath { get; }
      ISolutionFolderNode ParentSolutionFolder { get; }
   }
}

