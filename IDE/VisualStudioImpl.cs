using Microsoft.Practices.ServiceLocation;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System;
using Alphaleonis.Vsx.IDE;

namespace Alphaleonis.Vsx
{   
   internal class VisualStudioImpl : IVisualStudio
   {
      private readonly Lazy<ISolutionExplorer> m_solutionExplorer;
      private readonly Lazy<IOutputWindow> m_outputWindow;

      public VisualStudioImpl(IServiceLocator serviceLocator, IDialogService dialogService, Lazy<ISolutionExplorer> solutionExplorer, Lazy<IOutputWindow> outputWindow)
      {
         ServiceLocator = serviceLocator;
         DialogService = dialogService;
         m_outputWindow = outputWindow;
         m_solutionExplorer = solutionExplorer;
      }

      public IDialogService DialogService { get; }

      public IServiceLocator ServiceLocator { get; }

      public ISolutionExplorer SolutionExplorer
      {
         get
         {
            return m_solutionExplorer.Value;
         }
      }

      public IOutputWindow OutputWindow
      {
         get
         {
            return m_outputWindow.Value;
         }
      }
   }
}
