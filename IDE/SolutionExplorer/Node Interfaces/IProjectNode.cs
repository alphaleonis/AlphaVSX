using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public interface IProjectNode : ISolutionExplorerNode, IEquatable<IProjectNode>
   {
      EnvDTE.Project DteProject { get; }

      string FullPath { get; }
   }
}

