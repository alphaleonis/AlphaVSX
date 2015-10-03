using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class ProjectNode : SolutionExplorerNode, IProjectNode
   {
      private readonly Lazy<EnvDTE.Project> m_project;
     
      public ProjectNode(VsSolutionHierarchyNode hierarchyNode, Lazy<ISolutionExplorerNode> parentNode, ISolutionExplorerNodeFactory nodeFactory)
          : base(SolutionExplorerNodeKind.Project, hierarchyNode, parentNode, nodeFactory)
      {
         if (parentNode == null)
            throw new ArgumentNullException(nameof(parentNode), $"{nameof(parentNode)} is null.");

         m_project = new Lazy<EnvDTE.Project>(() => (EnvDTE.Project)hierarchyNode.ExtenderObject);
      }

      public EnvDTE.Project DteProject
      {
         get
         {
            return m_project.Value;
         }
      }

      public virtual IFolderNode CreateFolder(string name)
      {       
         DteProject.ProjectItems.AddFolder(name);

         var folder = HierarchyNode.Children.Single(child => child.DisplayName == name);

         return CreateChildNode(folder) as IFolderNode;
      }

      public virtual string FullPath
      {
         get
         {
            return DteProject?.FullName;
         }
      }

      public static bool operator ==(ProjectNode lhs, ProjectNode rhs)
      {
         return Equals(lhs, rhs);
      }

      public static bool operator !=(ProjectNode lhs, ProjectNode rhs)
      {
         return !Equals(lhs, rhs);
      }

      public bool Equals(IProjectNode other)
      {
         return Equals(this, other);
      }

      public override bool Equals(object obj)
      {
         return Equals(obj as IProjectNode);
      }

      private static bool Equals(IProjectNode lhs, IProjectNode rhs)
      {
         if (object.ReferenceEquals(lhs, rhs))
            return true;

         if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null))
            return false;

         
         if (lhs.GetType().Equals(rhs.GetType()) == false)
            return false;

         return lhs.FullPath.Equals(rhs.FullPath);
      }
      
      public override int GetHashCode()
      {
         return FullPath.GetHashCode();
      }

      public override void Accept(SolutionHierarchyVisitor visitor)
      {
         visitor.Visit(this);
      }
   }

   
}