using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.ComponentModelHost;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Composition;
using System.Linq;
using System.Text.RegularExpressions;

namespace Alphaleonis.Vsx.Unity
{

   internal class ServiceProviderBuildStrategy : BuilderStrategy
   {
      private readonly IServiceProvider m_serviceProvider;
      private readonly IComponentModel m_componentModel;
      private readonly ServiceLocatorOptions m_options;

      public ServiceProviderBuildStrategy(IServiceProvider serviceProvider, ServiceLocatorOptions options)
      {
         if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider), $"{nameof(serviceProvider)} is null.");

         m_serviceProvider = serviceProvider;
         m_componentModel = serviceProvider.GetService<SComponentModel, IComponentModel>();
         m_options = options;
      }

      public static Regex s_typeNameRegex = new Regex("^(?<ns>Microsoft\\.VisualStudio\\.(?:\\w+\\.)*)(?<typeName>.+)$", RegexOptions.CultureInvariant | RegexOptions.Compiled);

      public override void PreBuildUp(IBuilderContext context)
      {
         string name = context.BuildKey.Type.Name;

         if (context.Existing == null && context.OriginalBuildKey.Equals(context.BuildKey))
         {
            var svc = m_serviceProvider.GetService(context.OriginalBuildKey.Type);
            if (svc != null)
            {
               context.Existing = svc;
               context.BuildComplete = true;
               return;
            }

            if (m_options.HasFlag(ServiceLocatorOptions.PackageServiceProvider))
            {
               Match match = s_typeNameRegex.Match(context.BuildKey.Type.FullName);
               if (match.Success)
               {
                  string ns = match.Groups["ns"].Value;
                  string typeName = match.Groups["typeName"].Value;

                  if (typeName.StartsWith("IVs"))
                  {
                     Type newType = context.BuildKey.Type.Assembly.GetType($"{ns}S{typeName.Substring(1)}", false);
                     if (newType != null)
                     {
                        svc = m_serviceProvider.GetService(newType);
                        if (svc != null)
                        {
                           context.Existing = svc;
                           context.BuildComplete = true;
                           return;
                        }
                     }                     
                  }
               }
            }

            if (m_options.HasFlag(ServiceLocatorOptions.MEF))
            {
               var mefSvc = TryGetInstanceFromComponentModel(context.BuildKey.Type);
               if (mefSvc != null)
               {
                  context.Existing = mefSvc;
                  context.BuildComplete = true;
                  return;
               }
            }
         }
      }

      private object TryGetInstanceFromComponentModel(Type serviceType)
      {
         if (m_componentModel != null)
         {
            var importDefinition = new ContractBasedImportDefinition(AttributedModelServices.GetContractName(serviceType), null, null, ImportCardinality.ExactlyOne, false, true, CreationPolicy.Any);
            IEnumerable<Export> exports;
            if (m_componentModel.DefaultExportProvider.TryGetExports(importDefinition, null, out exports) && exports.Any())
            {
               return exports.First().Value;
            }
         }

         return null;
      }
   }
}

