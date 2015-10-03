using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public static class EnumerableExtensions
   {
      public static IEnumerable<T> BreadthFirstTraversal<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childSequenceRetriever)
      {
         Queue<T> queue = new Queue<T>(source);

         while (queue.Count > 0)
         {
            var current = queue.Dequeue();
            yield return current;

            var children = childSequenceRetriever(current);
            if (children != null)
            {
               foreach (var child in children)
               {
                  queue.Enqueue(child);
               }
            }
         }
      }

      public static IEnumerable<T> DepthFirstTraversal<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childSequenceRetriever)
      {
         var stack = new Stack<T>(source);

         while (stack.Count > 0)
         {
            var current = stack.Pop();
            yield return current;

            var children = childSequenceRetriever(current);
            if (children != null)
            {
               foreach (var child in children)
               {
                  stack.Push(child);
               }
            }
         }
      }
   }
}

