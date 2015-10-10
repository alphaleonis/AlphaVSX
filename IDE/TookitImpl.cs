using Microsoft.Practices.ServiceLocation;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System;
using Alphaleonis.Vsx.IDE;
using Microsoft.Practices.Unity;

namespace Alphaleonis.Vsx
{   
   internal sealed class TookitImpl : IToolkit
   {
      private readonly IUnityContainer m_container;
      private readonly Lazy<ISolutionExplorer> m_solutionExplorer;
      private readonly Lazy<IOutputWindow> m_outputWindow;

      public TookitImpl(IServiceLocator serviceLocator, IDialogService dialogService, Lazy<ISolutionExplorer> solutionExplorer, Lazy<IOutputWindow> outputWindow, ICommandManager commandManager, IUnityContainer container)
      {
         m_container = container;
         CommandManager = commandManager;
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

      public ICommandManager CommandManager { get; }

      public void Dispose()
      {
         if (m_container != null)
         {
            m_container.Dispose();
         }  
      }
   }
}
