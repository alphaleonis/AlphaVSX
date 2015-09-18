using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaVSX.Roslyn
{
   public static class SyntaxGeneratorExtensions
   {
      /// <summary>
      /// Creates a statement that checks if the specified identifier is equal to null, and if so throws an ArgumentNullException.
      /// </summary>
      public static SyntaxNode ThrowIfNullStatement(this SyntaxGenerator generator, string identifier)
      {
         
         return generator.IfStatement(generator.ReferenceEqualsExpression(generator.IdentifierName(identifier), generator.NullLiteralExpression()),
            new SyntaxNode[]
            {
               generator.ThrowStatement(
                  generator.ObjectCreationExpression(
                     generator.QualifiedName(generator.IdentifierName("System"), generator.IdentifierName("ArgumentNullException")),
                     generator.InvocationExpression(generator.IdentifierName("nameof"), generator.IdentifierName(identifier))                     
                     
                  )
               )                        
            }
         );
      }
   }
}
