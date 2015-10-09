using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alphaleonis.Vsx.Unity
{
   public class ServiceProviderUnityExtension : UnityContainerExtension
   {
      private IServiceProvider m_serviceProvider;
      private readonly ServiceLocatorOptions m_options;

      public ServiceProviderUnityExtension(IServiceProvider serviceProvider, ServiceLocatorOptions options)
      {
         if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider), $"{nameof(serviceProvider)} is null.");

         m_options = options;
         m_serviceProvider = serviceProvider;
      }

      protected override void Initialize()
      {
         var strategy = new ServiceProviderBuildStrategy(m_serviceProvider, m_options);

         Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
      }
   }
}