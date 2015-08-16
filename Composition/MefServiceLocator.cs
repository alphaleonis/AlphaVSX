using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace Alphaleonis.Vsx
{
   internal sealed class MefServiceLocator : ServiceLocatorImplBase
   {
      readonly private ExportProvider m_provider;

      public MefServiceLocator(ExportProvider provider)
      {
         m_provider = provider;
      }

      protected override object DoGetInstance(Type serviceType, string key)
      {
         if (key == null)
            key = AttributedModelServices.GetContractName(serviceType);

         IEnumerable<Lazy<object>> exports = m_provider.GetExports<object>(key);

         if (exports.Any())
         {
            object service = exports.First().Value;
            return service;
         }

         throw new ActivationException(string.Format("Could not locate any instances of contract {0}", key));
      }

      protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
      {
         return m_provider.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
      }
   }
}
