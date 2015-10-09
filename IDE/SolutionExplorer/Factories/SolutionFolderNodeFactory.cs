using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class SolutionFolderNodeFactory : ISolutionExplorerNodeFactory
   {
      private Lazy<ISolutionExplorerNodeFactory> m_childNodeFactory;

      public SolutionFolderNodeFactory(Lazy<ISolutionExplorerNodeFactory> childNodeFactory)
      {
         if (childNodeFactory == null)
            throw new ArgumentNullException(nameof(childNodeFactory), $"{nameof(childNodeFactory)} is null.");

         m_childNodeFactory = childNodeFactory;
      }

      public bool CanCreateFrom(VsSolutionHierarchyNode hierarchyNode)
      {
         var project = hierarchyNode.ExtenderObject as EnvDTE.Project;
         return project != null && project.Object is EnvDTE80.SolutionFolder;
      }

      public ISolutionExplorerNode Create(VsSolutionHierarchyNode hierarchy, Lazy<ISolutionExplorerNode> parent)
      {
         if (hierarchy == null)
            throw new ArgumentNullException(nameof(hierarchy), $"{nameof(hierarchy)} is null.");

         if (parent == null)
            throw new ArgumentNullException(nameof(parent), $"{nameof(parent)} is null.");

         return CanCreateFrom(hierarchy) ? new SolutionFolderNode(hierarchy, parent, m_childNodeFactory.Value) : null;
      }
   }
}

