using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alphaleonis.Vsx
{
   public class CSharpRoslynCodeGenerationContext
   {
      #region Construction

      private CSharpRoslynCodeGenerationContext(Document document, Compilation compilation, SyntaxTree syntaxTree, SemanticModel semanticModel)
      {
         if (document == null)
            throw new ArgumentNullException(nameof(document), $"{nameof(document)} is null.");

         Compilation = compilation;
         SyntaxTree = syntaxTree;
         SemanticModel = semanticModel;
         Document = document;
         Generator = SyntaxGenerator.GetGenerator(document);
      }

      public static async Task<CSharpRoslynCodeGenerationContext> CreateAsync(Document document, CancellationToken cancellationToken = default(CancellationToken))
      {
         Compilation compilation = await document.Project.GetCompilationAsync(cancellationToken);
         SyntaxTree syntaxTree = await document.GetSyntaxTreeAsync(cancellationToken);
         SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken);

         return new CSharpRoslynCodeGenerationContext(document, compilation, syntaxTree, semanticModel);
      }

      private SyntaxGenerator GetSyntaxGenerator()
      {
         return SyntaxGenerator.GetGenerator(Document);
      }

      #endregion

      public Document Document { get; }

      public Compilation Compilation { get; }

      public SyntaxTree SyntaxTree { get; }

      public SemanticModel SemanticModel { get; }

      public CompilationUnitSyntax CompilationUnit
      {
         get
         {
            return SyntaxTree.GetRoot() as CompilationUnitSyntax;
         }
      }

      public SyntaxGenerator Generator { get; }

      public CSharpRoslynCodeGenerationContext WithDocument(Document document)
      {
         return CreateAsync(document).Result;
      }

      public INamedTypeSymbol GetTypeByMetadataName(string fullyQualifiedMetadataName)
      {
         return Compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);
      }    
   }
}
