using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.EventSourceClassGenerator
{
   internal static class SyntaxNodeExtensions
   {
      public static T WithPrependedLeadingTrivia<T>(
            this T node,
            params SyntaxTrivia[] trivia) where T : SyntaxNode
      {
         if (trivia.Length == 0)
         {
            return node;
         }

         return node.WithPrependedLeadingTrivia((IEnumerable<SyntaxTrivia>)trivia);
      }

      public static T WithPrependedLeadingTrivia<T>(this T node, SyntaxTriviaList trivia) where T : SyntaxNode
      {
         if (trivia.Count == 0)
         {
            return node;
         }

         return node.WithLeadingTrivia(trivia.Concat(node.GetLeadingTrivia()));
      }

      public static T WithPrependedLeadingTrivia<T>(this T node, IEnumerable<SyntaxTrivia> trivia) where T : SyntaxNode
      {
         return node.WithPrependedLeadingTrivia(trivia.ToSyntaxTriviaList());
      }

      public static T WithAppendedTrailingTrivia<T>(this T node, params SyntaxTrivia[] trivia) where T : SyntaxNode
      {
         if (trivia.Length == 0)
         {
            return node;
         }

         return node.WithAppendedTrailingTrivia((IEnumerable<SyntaxTrivia>)trivia);
      }

      public static T WithAppendedTrailingTrivia<T>(this T node, SyntaxTriviaList trivia) where T : SyntaxNode
      {
         if (trivia.Count == 0)
         {
            return node;
         }

         return node.WithTrailingTrivia(node.GetTrailingTrivia().Concat(trivia));
      }

      public static T WithAppendedTrailingTrivia<T>(this T node, IEnumerable<SyntaxTrivia> trivia) where T : SyntaxNode
      {
         return node.WithAppendedTrailingTrivia(trivia.ToSyntaxTriviaList());
      }

      
   }
}
