using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class SolutionNode : SolutionExplorerNode, ISolutionNode
   {
      public SolutionNode(VsSolutionHierarchyNode hierarchyNode, ISolutionExplorerNodeFactory nodeFactory)
         : base(SolutionExplorerNodeKind.Solution, hierarchyNode, null, nodeFactory)
      {
      }

      public override void Accept(SolutionHierarchyVisitor visitor)
      {
         visitor.Visit(this);
      }
   }
}
