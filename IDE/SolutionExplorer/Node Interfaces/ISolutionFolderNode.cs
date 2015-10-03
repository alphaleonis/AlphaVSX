using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public interface ISolutionFolderNode : ISolutionExplorerNode
   {
      ISolutionFolderNode CreateSolutionFolder(string name);
   }
}

