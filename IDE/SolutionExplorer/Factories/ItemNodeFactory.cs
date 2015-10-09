using System;

namespace Alphaleonis.Vsx.IDE
{
   internal class ItemNodeFactory : ISolutionExplorerNodeFactory
   {
      private readonly Lazy<ISolutionExplorerNodeFactory> m_nodeFactory;

      public ItemNodeFactory(Lazy<ISolutionExplorerNodeFactory> nodeFactory)
      {
         m_nodeFactory = nodeFactory;
      }

      public bool CanCreateFrom(VsSolutionHierarchyNode hierarchy)
      {
         var projectItem = hierarchy.ExtenderObject as EnvDTE.ProjectItem;

         return projectItem != null && projectItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile && !(projectItem.ContainingProject.Object is EnvDTE80.SolutionFolder);
      }

      public ISolutionExplorerNode Create(VsSolutionHierarchyNode hierarchy, Lazy<ISolutionExplorerNode> parent)
      {
         return CanCreateFrom(hierarchy) ? new ItemNode(hierarchy, parent, m_nodeFactory.Value) : null;
      }
   }
}

