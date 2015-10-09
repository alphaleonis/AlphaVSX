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
      public static IVisualStudio Initialize(IServiceProvider package, ServiceLocatorOptions serviceLocatorOptions = ServiceLocatorOptions.PackageServiceProvider)
      {
         IUnityContainer container = ConfigureContainer(package, serviceLocatorOptions);
         container.BuildUp(package);
         return container.Resolve<IVisualStudio>();
      }

      private static IUnityContainer ConfigureContainer(IServiceProvider package, ServiceLocatorOptions options)
      {
         if (package == null)
            throw new ArgumentNullException(nameof(package), $"{nameof(package)} is null.");

         IUnityContainer container = new UnityContainer();
         container.AddExtension(new ServiceProviderUnityExtension(package, options));

         container.RegisterType<IVisualStudio, VisualStudioImpl>(new ContainerControlledLifetimeManager());
         container.RegisterTypes(new SolutionExplorerNodeFactoryRegistrationConvention());
         container.RegisterType<IEnumerable<ISolutionExplorerNodeFactory>, ISolutionExplorerNodeFactory[]>();
         container.RegisterType<ISolutionExplorerNodeFactory, GlobalSolutionExplorerNodeFactory>();

         container.RegisterType<ISolutionExplorer, SolutionExplorer>();
         container.RegisterType<IOutputWindow, OutputWindow>();
         container.RegisterType<IDialogService, DialogService>();
         container.RegisterInstance<IServiceProvider>(package);

         UnityServiceLocator serviceLocator = new UnityServiceLocator(container);
         container.RegisterInstance<IServiceLocator>(serviceLocator);

         if (!ServiceLocator.IsLocationProviderSet)
            ServiceLocator.SetLocatorProvider(() => serviceLocator);

         return container;
      }
   }
}
