using Alphaleonis.Vsx.IDE;
using Alphaleonis.Vsx.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.ComponentModelHost;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Reflection.Context;

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

         container.RegisterType<IToolkit, TookitImpl>(new ContainerControlledLifetimeManager());
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
         container.RegisterInstance<IServiceLocator>(serviceLocator);

         if (!ServiceLocator.IsLocationProviderSet)
            ServiceLocator.SetLocatorProvider(() => serviceLocator);

         return container;
      }
   }

   
}
