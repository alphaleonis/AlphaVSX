using System;

namespace Alphaleonis.Vsx.IDE
{
   internal class ReferencesFolderNodeFactory : ISolutionExplorerNodeFactory
   {
      private readonly Lazy<ISolutionExplorerNodeFactory> m_nodeFactory;

      public ReferencesFolderNodeFactory(Lazy<ISolutionExplorerNodeFactory> nodeFactory)
      {
         m_nodeFactory = nodeFactory;
      }

      public bool CanCreateFrom(VsSolutionHierarchyNode hierarchyNode)
      {
         // Can we do something better here?
         return hierarchyNode.ExtenderObject == null && hierarchyNode.DisplayName == "References";
      }

      public ISolutionExplorerNode Create(VsSolutionHierarchyNode hierarchy, Lazy<ISolutionExplorerNode> parent)
      {
         return CanCreateFrom(hierarchy) ? new ReferencesFolderNode(hierarchy, parent, m_nodeFactory.Value) : null;
      }
   }
}

