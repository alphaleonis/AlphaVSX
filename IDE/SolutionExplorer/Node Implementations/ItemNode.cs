using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal sealed class ItemNode : ProjectItemNode, IItemNode
   {
      internal readonly Lazy<EnvDTE.ProjectItem> m_dteProjectItem;

      public ItemNode(VsSolutionHierarchyNode hierarchyNode, Lazy<ISolutionExplorerNode> parentNode, ISolutionExplorerNodeFactory nodeFactory)
          : base(SolutionExplorerNodeKind.Item, hierarchyNode, parentNode, nodeFactory)
      {
         m_dteProjectItem = new Lazy<EnvDTE.ProjectItem>(() => (EnvDTE.ProjectItem)hierarchyNode.ExtenderObject);
         Properties = new ItemProperties(this);
      }

      public string FullPath
      {
         get { return m_dteProjectItem.Value?.get_FileNames(1); }
      }

      public bool IsDirty
      {
         get
         {
            return m_dteProjectItem.Value.IsDirty;
         }
      }

      public EnvDTE.ProjectItem DTEProjectItem
      {
         get
         {
            return m_dteProjectItem.Value;
         }
      }

      public dynamic Properties { get; }

      public void Delete()
      {
         m_dteProjectItem.Value.Delete();
      }

      public override void Accept(SolutionHierarchyVisitor visitor)
      {         
         visitor.Visit(this);
      }
   }

   
}