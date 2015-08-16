using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Alphaleonis.Vsx
{
   internal class ServiceProviderExportProviderAdapter : ExportProvider
   {
      private static readonly Regex s_numberSuffixRegex = new Regex(@"^(.*)(\d+)$", RegexOptions.Compiled);
      private static readonly Regex s_iVsPrefixRegex = new Regex(@"(.*\.)(IVs)(.*?)(\d*)$", RegexOptions.Compiled);
      private const string s_iVsPrefixReplacement = "$1SVs$3";

      private readonly ConcurrentDictionary<string, IEnumerable<Export>> m_exportsCache = new ConcurrentDictionary<string, IEnumerable<Export>>();
      private readonly IServiceProvider m_serviceProvider;

      public ServiceProviderExportProviderAdapter(IServiceProvider services)
      {
         m_serviceProvider = services;
      }

      protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
      {
         return m_exportsCache.GetOrAdd(definition.ContractName, ResolveExports);
      }

      private IEnumerable<Export> ResolveExports(string contractName)
      {
         IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic).SelectMany(asm => TryGetExportedTypes(asm));

         Type serviceType = types.FirstOrDefault(t => t.FullName.Equals(contractName));

         if (serviceType == null)
            return Enumerable.Empty<Export>();

         string serviceTypeName = contractName;

         // Try getting the service without it's version suffix.
         Match versionMatch = s_numberSuffixRegex.Match(contractName);
         if (versionMatch.Success)
         {
            var noVersionName = versionMatch.Groups[1].Value;
            serviceType = types.FirstOrDefault(t => t.FullName.Equals(noVersionName));
            if (serviceType != null)
            {
               serviceTypeName = noVersionName;      
            }
         }

         // Okay, then let's see if this is a service whose type name starts with IVs... If so, try getting the service
         // by using the SVs-prefix instead.                  
         if (s_iVsPrefixRegex.IsMatch(contractName))
         {
            var sVsServiceName = s_iVsPrefixRegex.Replace(contractName, s_iVsPrefixReplacement);
            serviceType = types.FirstOrDefault(t => t.FullName.Equals(sVsServiceName));
            if (serviceType != null)
            {
               serviceTypeName = sVsServiceName;
            }
         }

         if (serviceType != null)
         {
            var service = m_serviceProvider.GetService(serviceType);
            if (service != null)
               return CreateExport(contractName, serviceType);
         }

         return Enumerable.Empty<Export>();
      }

      private Export[] CreateExport(string contractName, Type serviceType)
      {         
         return new Export[] { new Export(contractName, () => m_serviceProvider.GetService(serviceType)) };
      }

      private static IEnumerable<Type> TryGetExportedTypes(Assembly asm)
      {
         try
         {
            return asm.GetExportedTypes();
         }
         catch (Exception)
         {
            return Enumerable.Empty<Type>();
         }
      }
   }
}
