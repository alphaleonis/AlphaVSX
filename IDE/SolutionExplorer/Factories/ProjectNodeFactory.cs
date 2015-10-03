using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   [Component("Part", true)]
   internal class ProjectNodeFactory : ISolutionExplorerNodeFactory
   {
      private readonly Lazy<ISolutionExplorerNodeFactory> m_nodeFactory;

      public ProjectNodeFactory([Import] Lazy<ISolutionExplorerNodeFactory> nodeFactory)
      {
         m_nodeFactory = nodeFactory;
      }

      public bool CanCreateFrom(VsSolutionHierarchyNode hierarchy)
      {
         var extenderObject = hierarchy.ExtenderObject;
         var project = extenderObject as EnvDTE.Project;

         return (extenderObject != null && extenderObject.GetType().FullName == "Microsoft.VisualStudio.Project.Automation.OAProject") ||
          (project != null && !(project.Object is EnvDTE80.SolutionFolder));
      }

      public ISolutionExplorerNode Create(VsSolutionHierarchyNode hierarchy, Lazy<ISolutionExplorerNode> parent)
      {
         if (hierarchy == null)
            throw new ArgumentNullException(nameof(hierarchy), $"{nameof(hierarchy)} is null.");

         if (parent == null)
            throw new ArgumentNullException(nameof(parent), $"{nameof(parent)} is null.");

         return CanCreateFrom(hierarchy) ? new ProjectNode(hierarchy, parent, m_nodeFactory.Value) : null;
      }
   }
}

