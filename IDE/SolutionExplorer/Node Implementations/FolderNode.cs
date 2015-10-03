using Microsoft.VisualStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class FolderNode : ProjectItemNode, IFolderNode
   {
      private readonly Lazy<EnvDTE.ProjectItem> m_dteItem;

      public FolderNode(VsSolutionHierarchyNode hierarchyNode, Lazy<ISolutionExplorerNode> parentNode, ISolutionExplorerNodeFactory nodeFactory)
          : base(SolutionExplorerNodeKind.Folder, hierarchyNode, parentNode, nodeFactory)
      {
         m_dteItem = new Lazy<EnvDTE.ProjectItem>(() => (EnvDTE.ProjectItem)hierarchyNode.ExtenderObject);
      }

      public virtual IFolderNode CreateFolder(string name)
      {
         if (string.IsNullOrEmpty(name))
            throw new ArgumentException($"{nameof(name)} is null or empty.", nameof(name));

         m_dteItem.Value.ProjectItems.AddFolder(name);

         var newFolder = HierarchyNode.Children.Single(child => child.DisplayName == name);

         return CreateChildNode(newFolder) as IFolderNode;
      }

      public string FullPath
      {
         get { return m_dteItem.Value?.get_FileNames(1); }
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