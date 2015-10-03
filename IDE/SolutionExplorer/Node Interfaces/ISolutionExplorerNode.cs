using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public interface ISolutionExplorerNode
   {
      SolutionExplorerNodeKind Kind { get; }
      ISolutionNode Solution { get; }
      string DisplayName { get; }
      bool IsSelected { get; }
      bool IsExpanded { get; }
      bool IsVisible { get; }
      bool IsHidden { get; }

      ISolutionExplorerNode Parent { get; }
      IEnumerable<ISolutionExplorerNode> Children { get; }

      void Accept(SolutionHierarchyVisitor visitor);

      //void Collapse();
      //void Expand(bool recursively = false);
      //void Select(bool allowMultiple = false);
   }
}

