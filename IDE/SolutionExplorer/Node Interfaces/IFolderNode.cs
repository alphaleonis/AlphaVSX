using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public interface IFolderNode : ISolutionExplorerNode
   {
      void Delete();

      string FullPath { get; }

      IFolderNode CreateFolder(string name);
   }
}

