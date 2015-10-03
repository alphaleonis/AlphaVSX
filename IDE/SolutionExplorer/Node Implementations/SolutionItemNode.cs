using Microsoft.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class SolutionItemNode : SolutionExplorerNode, ISolutionItemNode
   {
      private readonly Lazy<EnvDTE.ProjectItem> m_dteItem;

      private Lazy<ISolutionFolderNode> owningFolder;

      public SolutionItemNode(VsSolutionHierarchyNode hierarchyNode, Lazy<ISolutionExplorerNode> parentNode, ISolutionExplorerNodeFactory nodeFactory)
          : base(SolutionExplorerNodeKind.SolutionItem, hierarchyNode, parentNode, nodeFactory)
      {
         m_dteItem = new Lazy<EnvDTE.ProjectItem>(() => (EnvDTE.ProjectItem)hierarchyNode.ExtenderObject);

         owningFolder = new Lazy<ISolutionFolderNode>(() =>
         {
            var owningHierarchy = new VsSolutionHierarchyNode(hierarchyNode.VsHierarchy, VSConstants.VSITEMID_ROOT);
            return NodeFactory.Create(owningHierarchy, GetParent(owningHierarchy)) as ISolutionFolderNode;
         });
      }

      public virtual string FullPath
      {
         get { return m_dteItem.Value.get_FileNames(1); }
      }

      public virtual ISolutionFolderNode ParentSolutionFolder
      {
         get { return owningFolder.Value; }
      }

      private Lazy<ISolutionExplorerNode> GetParent(VsSolutionHierarchyNode hierarchy)
      {
         return hierarchy.Parent == null ? null : new Lazy<ISolutionExplorerNode>(() => NodeFactory.Create(hierarchy.Parent, GetParent(hierarchy.Parent)));
      }

      public override void Accept(SolutionHierarchyVisitor visitor)
      {
         visitor.Visit(this);
      }
   }
}

