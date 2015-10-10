using Alphaleonis.Vsx.IDE;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;

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
}