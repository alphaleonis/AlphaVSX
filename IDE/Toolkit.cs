using Alphaleonis.Vsx.IDE;
using Alphaleonis.Vsx.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alphaleonis.Vsx
{
   public static class Toolkit
   {
      public static IToolkit Initialize(IServiceProvider package, ServiceLocatorOptions serviceLocatorOptions = ServiceLocatorOptions.PackageServiceProvider, bool buildupPackage = true)
      {
         IUnityContainer container = ConfigureContainer(package, serviceLocatorOptions);

         if (buildupPackage)
            container.BuildUp(package);

         return container.Resolve<IToolkit>();
      }

      private class SomeProgress : IProgress<ProgressInfo>
      {
         private readonly CommonMessagePump m_messagePump;

         public SomeProgress(CommonMessagePump messagePump)
         {
            m_messagePump = messagePump;
         }

         public void Report(ProgressInfo value)
         {
            if (value.TotalSteps <= 0 || value.CurrentStep < 0)
            {
               m_messagePump.EnableRealProgress = false;
            }
            else
            {
               m_messagePump.EnableRealProgress = true;
               m_messagePump.CurrentStep = value.CurrentStep;
               m_messagePump.TotalSteps = value.TotalSteps;               
            }

            m_messagePump.WaitText = value.WaitText;
            m_messagePump.ProgressText = value.ProgressText;
         }
      }

      public static void RunWithProgress(Func<IProgress<ProgressInfo>, CancellationToken, System.Threading.Tasks.Task> taskFactory, string title)
      {
         CommonMessagePump msgPump = new CommonMessagePump();
         msgPump.AllowCancel = true;
         msgPump.EnableRealProgress = true;
         msgPump.WaitTitle = title;
         msgPump.WaitText = "Please stand by...";

         CancellationTokenSource cts = new CancellationTokenSource();
         SomeProgress progress = new SomeProgress(msgPump);

         System.Threading.Tasks.Task task = taskFactory(progress, cts.Token);

         var exitCode = msgPump.ModalWaitForHandles(((IAsyncResult)task).AsyncWaitHandle);

         if (exitCode == CommonMessagePumpExitCode.UserCanceled || exitCode == CommonMessagePumpExitCode.ApplicationExit)
         {
            cts.Cancel();
            msgPump = new CommonMessagePump();
            msgPump.AllowCancel = false;
            msgPump.EnableRealProgress = false;
            // Wait for the async operation to actually cancel.
            msgPump.ModalWaitForHandles(((IAsyncResult)task).AsyncWaitHandle);
         }

         if (!task.IsCanceled)
         {
            task.GetAwaiter().GetResult();
         }
      }

      private static void RegisterCommands(IUnityContainer container, IEnumerable<Type> types)
      {
         if (types == null)
            throw new ArgumentNullException(nameof(types), $"{nameof(types)} is null.");
         
         types = types.Where(type => type.Implements<ICommandImplementation>() && type.IsDefined<CommandAttribute>(false));

         foreach (var type in types)
         {
            container.RegisterType(typeof(ICommandImplementation), type, $"{type.FullName}", new ContainerControlledLifetimeManager());
         }
      }

      private static IUnityContainer ConfigureContainer(IServiceProvider package, ServiceLocatorOptions options)
      {
         if (package == null)
            throw new ArgumentNullException(nameof(package), $"{nameof(package)} is null.");

         IUnityContainer container = new UnityContainer();
         container.AddExtension(new ServiceProviderUnityExtension(package, options));

         container.RegisterType<IToolkit, ToolkitImpl>(new ExternallyControlledLifetimeManager());
         container.RegisterTypes(new SolutionExplorerNodeFactoryRegistrationConvention());
         container.RegisterType<IEnumerable<ISolutionExplorerNodeFactory>, ISolutionExplorerNodeFactory[]>();
         container.RegisterType<ISolutionExplorerNodeFactory, GlobalSolutionExplorerNodeFactory>();

         container.RegisterType<ISolutionExplorer, SolutionExplorer>();
         container.RegisterType<IOutputWindow, OutputWindow>(new ContainerControlledLifetimeManager());
         container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());

         container.RegisterType<IEnumerable<ICommandImplementation>, ICommandImplementation[]>();

         container.RegisterInstance<IServiceProvider>(package);

         container.RegisterType<ICommandManager, CommandManager>(new ContainerControlledLifetimeManager());

         UnityServiceLocator serviceLocator = new UnityServiceLocator(container);
         // The service locator will dispose the container when it is disposed leading to a StackOverflowException, so we
         // simply tell the container not to dispose it.
         container.RegisterInstance<IServiceLocator>(serviceLocator, new ExternallyControlledLifetimeManager()); 

         if (!ServiceLocator.IsLocationProviderSet)
            ServiceLocator.SetLocatorProvider(() => serviceLocator);

         return container;
      }
   }

   
}
