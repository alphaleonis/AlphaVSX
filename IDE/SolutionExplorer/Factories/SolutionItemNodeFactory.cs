using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class SolutionItemNodeFactory : ISolutionExplorerNodeFactory
   {
      private readonly Lazy<ISolutionExplorerNodeFactory> m_nodeFactory;

      public SolutionItemNodeFactory(Lazy<ISolutionExplorerNodeFactory> nodeFactory)
      {
         m_nodeFactory = nodeFactory;
      }

      public bool CanCreateFrom(VsSolutionHierarchyNode hierarchy)
      {
         if (hierarchy.Parent == null)
            return false;

         var item = hierarchy.ExtenderObject as EnvDTE.ProjectItem;
         var project = hierarchy.Parent.ExtenderObject as EnvDTE.Project;

         return project != null && item != null && project.Object is EnvDTE80.SolutionFolder;
      }

      public ISolutionExplorerNode Create(VsSolutionHierarchyNode hierarchy, Lazy<ISolutionExplorerNode> parent)
      {
         if (hierarchy == null)
            throw new ArgumentNullException(nameof(hierarchy), $"{nameof(hierarchy)} is null.");

         if (parent == null)
            throw new ArgumentNullException(nameof(parent), $"{nameof(parent)} is null.");

         return CanCreateFrom(hierarchy) ? new SolutionItemNode(hierarchy, parent, m_nodeFactory.Value) : null;
      }
   }
}

