using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alphaleonis.Vsx.IDE
{
   internal interface ISolutionExplorerNodeFactory
   {
      bool CanCreateFrom(VsSolutionHierarchyNode hierarchyNode);
      ISolutionExplorerNode Create(VsSolutionHierarchyNode hierarchy, Lazy<ISolutionExplorerNode> parent);
   }

}
