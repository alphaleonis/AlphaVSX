using Microsoft.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class SolutionFolderNode : SolutionExplorerNode, ISolutionFolderNode
   {
      private readonly Lazy<EnvDTE80.SolutionFolder> m_dteSolutionFolder;

      public SolutionFolderNode(VsSolutionHierarchyNode hierarchyNode, Lazy<ISolutionExplorerNode> parent, ISolutionExplorerNodeFactory nodeFactory)
         : base(SolutionExplorerNodeKind.SolutionFolder, hierarchyNode, parent, nodeFactory)
      {
         m_dteSolutionFolder = new Lazy<EnvDTE80.SolutionFolder>(() => (EnvDTE80.SolutionFolder)((EnvDTE.Project)hierarchyNode.ExtenderObject).Object);
      }


      public virtual ISolutionFolderNode CreateSolutionFolder(string name)
      {
         if (string.IsNullOrEmpty(name))
            throw new ArgumentException($"{nameof(name)} is null or empty.", nameof(name));

         m_dteSolutionFolder.Value.AddSolutionFolder(name);

         var solutionfolder = HierarchyNode.Children.Single(child => child.DisplayName == name);

         return (ISolutionFolderNode)CreateChildNode(solutionfolder);
      }

      public override void Accept(SolutionHierarchyVisitor visitor)
      {
         visitor.Visit(this);
      }
   }
}