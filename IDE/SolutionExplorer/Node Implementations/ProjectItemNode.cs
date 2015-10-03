using Microsoft.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal abstract class ProjectItemNode : SolutionExplorerNode, ISolutionExplorerNode
   {
      #region Private Fields

      private Lazy<IProjectNode> m_parentProject;

      #endregion

      #region Constructors

      public ProjectItemNode(SolutionExplorerNodeKind kind, VsSolutionHierarchyNode hierarchyNode, Lazy<ISolutionExplorerNode> parent, ISolutionExplorerNodeFactory nodeFactory)
         : base(kind, hierarchyNode, parent, nodeFactory)
      {
         m_parentProject = new Lazy<IProjectNode>(() =>
         {
            VsSolutionHierarchyNode parentProjectHierarchy = new VsSolutionHierarchyNode(hierarchyNode.VsHierarchy, VSConstants.VSITEMID_ROOT);
            return NodeFactory.Create(parentProjectHierarchy, GetParent(parentProjectHierarchy)) as IProjectNode;
         });
      }

      #endregion

      #region Properties

      public virtual IProjectNode ParentProject
      {
         get { return m_parentProject.Value; }
      }
     
      #endregion

      #region Methods

      private Lazy<ISolutionExplorerNode> GetParent(VsSolutionHierarchyNode hierarchy)
      {
         return hierarchy.Parent == null ? null : new Lazy<ISolutionExplorerNode>(() => NodeFactory.Create(hierarchy.Parent, GetParent(hierarchy.Parent)));
      }

      public static bool operator ==(ProjectItemNode lhs, ProjectItemNode rhs)
      {
         return Equals(lhs, rhs);
      }

      public static bool operator !=(ProjectItemNode lhs, ProjectItemNode rhs)
      {
         return !Equals(lhs, rhs);
      }

      public bool Equals(ProjectItemNode other)
      {
         return Equals(this, other);
      }

      public override bool Equals(object obj)
      {
         return Equals(this, obj as ProjectItemNode);
      }

      private static bool Equals(ProjectItemNode lhs, ProjectItemNode rhs)
      {
         if (object.ReferenceEquals(lhs, rhs))
            return true;

         if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null) || Object.ReferenceEquals(lhs.ParentProject, null) || Object.ReferenceEquals(rhs.ParentProject, null))
            return false;

         if (lhs.GetType().Equals(rhs.GetType()) == false)
            return false;

         return lhs.ParentProject.Equals(rhs.ParentProject) && lhs.HierarchyNode.ItemId.Equals(rhs.HierarchyNode.ItemId);
      }

      public override int GetHashCode()
      {
         return ParentProject.GetHashCode() ^ HierarchyNode.ItemId.GetHashCode();
      }

      #endregion
   }
}