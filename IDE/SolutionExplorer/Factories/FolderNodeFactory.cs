using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   [Component("Part", true)]
   internal class FolderNodeFactory : ISolutionExplorerNodeFactory
   {

      private readonly Lazy<ISolutionExplorerNodeFactory> m_nodeFactory;

      public FolderNodeFactory([Import] Lazy<ISolutionExplorerNodeFactory> nodeFactory)
      {
         m_nodeFactory = nodeFactory;
      }

      public bool CanCreateFrom(VsSolutionHierarchyNode hierarchyNode)
      {
         EnvDTE.ProjectItem projectItem = hierarchyNode.ExtenderObject as EnvDTE.ProjectItem;
         return projectItem != null && projectItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder;
      }

      public ISolutionExplorerNode Create(VsSolutionHierarchyNode hierarchy, Lazy<ISolutionExplorerNode> parent)
      {
         return CanCreateFrom(hierarchy) ? new FolderNode(hierarchy, parent, m_nodeFactory.Value) : null;
      }
   }
}

