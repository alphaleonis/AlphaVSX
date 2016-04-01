using Alphaleonis.Vsx.IDE;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alphaleonis.Vsx
{
   public interface IToolkit : IDisposable
   {
      ICommandManager CommandManager { get; }

      IDialogService DialogService { get; }

      IOutputWindow OutputWindow { get; }

      IServiceLocator ServiceLocator { get; }

      ISolutionExplorer SolutionExplorer { get; }

      
   }

   [Serializable]
   public class ProgressInfo 
   {
      public int TotalSteps { get; set; }
      public int CurrentStep { get; set; }
      public string WaitText { get; set; }      
      public string ProgressText { get; set; }
   }
}