using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class ItemNode : ProjectItemNode, IItemNode
   {
      private readonly Lazy<EnvDTE.ProjectItem> m_dteItem;

      public ItemNode(VsSolutionHierarchyNode hierarchyNode, Lazy<ISolutionExplorerNode> parentNode, ISolutionExplorerNodeFactory nodeFactory)
          : base(SolutionExplorerNodeKind.Item, hierarchyNode, parentNode, nodeFactory)
      {
         m_dteItem = new Lazy<EnvDTE.ProjectItem>(() => (EnvDTE.ProjectItem)hierarchyNode.ExtenderObject);
      }

      public string FullPath
      {
         get { return m_dteItem.Value?.get_FileNames(1); }
      }

      public bool IsDirty
      {
         get
         {
            return m_dteItem.Value.IsDirty;
         }
      }

      public void Delete()
      {
         m_dteItem.Value.Delete();
      }

      public override void Accept(SolutionHierarchyVisitor visitor)
      {         
         visitor.Visit(this);
      }
   }

   
}