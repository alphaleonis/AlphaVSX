using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   [DebuggerDisplay("{Kind} {DisplayName}")]
   internal abstract class SolutionExplorerNode : ISolutionExplorerNode
   {
      #region Private Fields

      private readonly VsSolutionHierarchyNode m_hierarchyNode;
      private readonly Lazy<ISolutionExplorerNode> m_parent;
      private readonly Lazy<ISolutionNode> m_solutionNode;
      private readonly Lazy<IVsUIHierarchyWindow> m_window;
      private readonly Lazy<bool> m_isHidden;

      #endregion

      #region Constructor

      public SolutionExplorerNode(SolutionExplorerNodeKind kind, VsSolutionHierarchyNode hierarchyNode, Lazy<ISolutionExplorerNode> parentNode, ISolutionExplorerNodeFactory nodeFactory)
      {
         if (nodeFactory == null)
            throw new ArgumentNullException(nameof(nodeFactory), $"{nameof(nodeFactory)} is null.");

         if (hierarchyNode == null)
            throw new ArgumentNullException(nameof(hierarchyNode), $"{nameof(hierarchyNode)} is null.");

         Kind = kind;
         NodeFactory = nodeFactory;
         m_hierarchyNode = hierarchyNode;
         m_window = new Lazy<IVsUIHierarchyWindow>(() => GetWindow(hierarchyNode.ServiceProvider));
         m_parent = parentNode ?? new Lazy<ISolutionExplorerNode>(() => null);

         Func<bool> getHiddenProperty = () => hierarchyNode.VsHierarchy.GetProperty<bool?>(__VSHPROPID.VSHPROPID_IsHiddenItem, m_hierarchyNode.ItemId).GetValueOrDefault();

         m_isHidden = parentNode != null ?
                new Lazy<bool>(() => getHiddenProperty() || parentNode.Value.IsHidden) :
                new Lazy<bool>(() => getHiddenProperty());

         m_solutionNode = new Lazy<ISolutionNode>(() =>
         {
            var solutionHierarchy = new VsSolutionHierarchyNode(
                (IVsHierarchy)m_hierarchyNode.ServiceProvider.GetService<SVsSolution, IVsSolution>(),
                VSConstants.VSITEMID_ROOT);

            return new SolutionNode(solutionHierarchy, NodeFactory);
         });
      }

      #endregion

      #region Properties

      public IEnumerable<ISolutionExplorerNode> Children
      {
         get
         {
            return m_hierarchyNode.Children.Select(hierarchy => NodeFactory.Create(hierarchy, new Lazy<ISolutionExplorerNode>(() => this))).Where(c => c != null);            
         }
      }

      public string DisplayName
      {
         get
         {
            return m_hierarchyNode.DisplayName;
         }
      }

      public VsSolutionHierarchyNode HierarchyNode
      {
         get
         {
            return m_hierarchyNode;
         }
      }

      public virtual bool IsVisible
      {
         get
         {
            return !IsHidden && (Parent == null || (Parent.IsVisible && Parent.IsExpanded));
         }
      }

      public virtual bool IsHidden
      {
         get { return m_isHidden.Value; }
      }

      public virtual bool IsSelected
      {
         get
         {
            uint state;
            ErrorHandler.ThrowOnFailure(m_window.Value.GetItemState(m_hierarchyNode.VsHierarchy as IVsUIHierarchy, m_hierarchyNode.ItemId, (uint)__VSHIERARCHYITEMSTATE.HIS_Selected, out state));
            return state == (uint)__VSHIERARCHYITEMSTATE.HIS_Selected;
         }
      }

      public virtual bool IsExpanded
      {
         get
         {
            if (Parent == null)
               return true;

            uint state;
            ErrorHandler.ThrowOnFailure(m_window.Value.GetItemState(
                m_hierarchyNode.VsHierarchy as IVsUIHierarchy, m_hierarchyNode.ItemId, (uint)__VSHIERARCHYITEMSTATE.HIS_Expanded, out state));

            return state == (uint)__VSHIERARCHYITEMSTATE.HIS_Expanded;
         }
      }

      public virtual ISolutionExplorerNode Parent { get { return m_parent.Value; } }

      public ISolutionNode Solution
      {
         get
         {
            return m_solutionNode.Value;
         }
      }

      public SolutionExplorerNodeKind Kind { get; }

      #endregion

      #region Methods

      private IVsUIHierarchyWindow GetWindow(IServiceProvider serviceProvider)
      {
         var uiShell = serviceProvider.GetService<SVsUIShell, IVsUIShell>();
         object pvar = null;
         IVsWindowFrame frame;
         var persistenceSlot = new Guid(EnvDTE.Constants.vsWindowKindSolutionExplorer);
         if (ErrorHandler.Succeeded(uiShell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref persistenceSlot, out frame)))
            ErrorHandler.ThrowOnFailure(frame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out pvar));

         return (IVsUIHierarchyWindow)pvar;
      }

      protected ISolutionExplorerNodeFactory NodeFactory { get; }

      protected virtual ISolutionExplorerNode CreateChildNode(VsSolutionHierarchyNode hierarchy)
      {
         return NodeFactory.Create(hierarchy, new Lazy<ISolutionExplorerNode>(() => this));
      }

      public abstract void Accept(SolutionHierarchyVisitor visitor);

      #endregion
   }
}