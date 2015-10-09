using System;

namespace Alphaleonis.Vsx.IDE
{
   internal interface ISolutionExplorerNodeFactory
   {
      bool CanCreateFrom(VsSolutionHierarchyNode hierarchyNode);
      ISolutionExplorerNode Create(VsSolutionHierarchyNode hierarchy, Lazy<ISolutionExplorerNode> parent);
   }

}
