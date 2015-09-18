using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Alphaleonis.Vsx
{
   [ComVisible(true)]
   public abstract class RoslynCodeGeneratorBase : BaseTextCodeGenerator
   {
      protected sealed override void GenerateCode(string inputFilePath, string inputFileContent, TextWriter writer)
      {
         Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.Run(async () =>
         {
            IComponentModel componentModel = (IComponentModel)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SComponentModel));
            VisualStudioWorkspace workspace = componentModel.GetService<VisualStudioWorkspace>();

            if (workspace == null)
               throw new InvalidOperationException($"Unable to get the service {nameof(VisualStudioWorkspace)} from the host application.");

            Solution solution = workspace.CurrentSolution;
            if (solution == null)
               throw new InvalidOperationException($"No solution found in the current workspace.");

            DocumentId documentId = solution.GetDocumentIdsWithFilePath(inputFilePath).FirstOrDefault();

            if (documentId == null)
               throw new TextFileGeneratorException(String.Format("Unable to find a document matching the file path \"{0}\".", inputFilePath));

            Document document = solution.GetDocument(documentId);

            document = await GenerateCodeAsync(document);

            await writer.WriteLineAsync((await document.GetTextAsync()).ToString());
         });
      }

      protected abstract Task<Document> GenerateCodeAsync(Document inputDocument);
   }
}
