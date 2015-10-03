using Microsoft.Practices.ServiceLocation;
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
      public static IVisualStudio Initialize(IServiceProvider package, bool satisfyImportsOfPackage = true, bool includeVsComponentModel = false)
      {
         // Allow dependencies of VS exported services.
         IComponentModel vsComponentModel = includeVsComponentModel ? package.TryGetService<SComponentModel, IComponentModel>() : null;

         AssemblyCollection assemblies = new AssemblyCollection();

         assemblies.Add(Assembly.GetExecutingAssembly());
         assemblies.TryAdd(package.GetType().Assembly);

         ComposablePartCatalog catalog = new AggregateCatalog(assemblies.Select(asm => new ComponentCatalog(asm)));
         
         ExportProvider[] providers;
         if (vsComponentModel != null)
         {
            providers = new ExportProvider[] { new VsxExportProvider(package), vsComponentModel.DefaultExportProvider };
         }
         else
         {
            providers = new ExportProvider[] { new VsxExportProvider(package) };
         }

         CompositionContainer container = new CompositionContainer(catalog, providers);

         MefServiceLocator serviceLocator = new MefServiceLocator(container);
         container.ComposeExportedValue<IServiceLocator>(serviceLocator);
         container.ComposeExportedValue<IServiceProvider>(serviceLocator);

         if (satisfyImportsOfPackage)
            container.ComposeParts(package);

         return serviceLocator.GetInstance<IVisualStudio>();
      }

      private class AssemblyCollection : KeyedCollection<string, Assembly>
      {         
         public AssemblyCollection()
            : base(StringComparer.OrdinalIgnoreCase)
         {
         }

         public bool TryAdd(Assembly assembly)
         {
            if (!Contains(assembly.Location))
            {
               Add(assembly);
               return true;
            }

            return false;
         }

         protected override string GetKeyForItem(Assembly item)
         {
            return item.Location;
         }
      }
   }
}
