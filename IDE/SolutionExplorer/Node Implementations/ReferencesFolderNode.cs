using Microsoft.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class ReferencesFolderNode : ProjectItemNode, IReferencesFolderNode
   {
      private readonly Lazy<VSLangProj.References> m_dteReferences;

      public ReferencesFolderNode(VsSolutionHierarchyNode hierarchyNode, Lazy<ISolutionExplorerNode> parentNode, ISolutionExplorerNodeFactory nodeFactory)
          : base(SolutionExplorerNodeKind.ReferencesFolder, hierarchyNode, parentNode, nodeFactory)
      {
         m_dteReferences = new Lazy<VSLangProj.References>(() => ((VSLangProj.VSProject)((EnvDTE.Project)hierarchyNode.VsHierarchy.GetProperty<object>(Microsoft.VisualStudio.Shell.Interop.__VSHPROPID.VSHPROPID_ExtObject, VSConstants.VSITEMID_ROOT)).Object).References);
      }

      public override void Accept(SolutionHierarchyVisitor visitor)
      {         
         visitor.Visit(this);
      }
   }
}