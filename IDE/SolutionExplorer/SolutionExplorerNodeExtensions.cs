using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public static class SolutionExplorerNodeExtensions
   {
      public static IEnumerable<ISolutionExplorerNode> DescendantNodes(this ISolutionExplorerNode node, Predicate<ISolutionExplorerNode> traverseIntoChildren)
      {
         return DescendantNodes(node, traverseIntoChildren, false);
      }

      public static IEnumerable<ISolutionExplorerNode> DescendantNodesAndSelf(this ISolutionExplorerNode node, Predicate<ISolutionExplorerNode> traverseIntoChildren)
      {
         return DescendantNodes(node, traverseIntoChildren, true);
      }

      private static IEnumerable<ISolutionExplorerNode> DescendantNodes(ISolutionExplorerNode root, Predicate<ISolutionExplorerNode> descendIntoChildren, bool includeSelf)
      {
         if (includeSelf)
            yield return root;

         Stack<ISolutionExplorerNode> nodes = new Stack<ISolutionExplorerNode>();
         nodes.Push(root);

         while (nodes.Count > 0)
         {
            var current = nodes.Pop();
            if (descendIntoChildren == null || descendIntoChildren(current))
            {
               foreach (var child in current.Children)
               {
                  nodes.Push(child);
                  yield return child;
               }
            }
         }

      }
   }
}

