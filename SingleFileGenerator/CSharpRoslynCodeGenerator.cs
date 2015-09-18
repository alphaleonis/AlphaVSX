using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Simplification;
using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Alphaleonis.Vsx
{

   public abstract class CSharpRoslynCodeGenerator : RoslynCodeGeneratorBase
   {
      protected sealed override async Task<Document> GenerateCodeAsync(Document inputDocument)
      {
         ReportProgress(0, 100);
         CompilationUnitSyntax compilationUnit = await GenerateCompilationUnit(await CSharpRoslynCodeGenerationContext.CreateAsync(inputDocument));
         Document resultDocument = inputDocument.Project.AddDocument(Guid.NewGuid().ToString() + ".tmp.g.cs", compilationUnit);
         resultDocument = await Simplifier.ReduceAsync(resultDocument);
         ReportProgress(100, 100);
         return resultDocument;
      }

      protected abstract Task<CompilationUnitSyntax> GenerateCompilationUnit(CSharpRoslynCodeGenerationContext context);
   }
}
