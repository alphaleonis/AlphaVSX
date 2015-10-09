using Alphaleonis.Vsx.IDE;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Alphaleonis.Vsx
{
   public interface IVisualStudio
   {      
      IServiceLocator ServiceLocator { get; }

      IDialogService DialogService { get; }

      ISolutionExplorer SolutionExplorer { get; }

      IOutputWindow OutputWindow { get; }
   }
}