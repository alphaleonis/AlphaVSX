using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public class SolutionHierarchyWalker : SolutionHierarchyVisitor
   {
      protected override void DefaultVisit(ISolutionExplorerNode node)
      {
         if (node != null)
         {            
            if (node.Children != null)
            {
               foreach (var childNode in node.Children)
               {
                  childNode.Accept(this);
               }
            }
         }
      }
   }
}

