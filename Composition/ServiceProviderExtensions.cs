using System;

namespace Alphaleonis.Vsx
{
   internal static class ServiceProviderExtensions
   {
      public static T TryGetService<T>(this IServiceProvider provider)
      {
         return (T)provider.GetService(typeof(T));
      }

      public static T GetService<T>(this IServiceProvider provider)
      {
         var service = (T)provider.GetService(typeof(T));

         if (service == null)
            throw new InvalidOperationException($"Required service '{typeof(T).FullName}' not found.");

         return service;
      }
      
      public static TService TryGetService<TRegistration, TService>(this IServiceProvider provider)
      {
         return (TService)provider.GetService(typeof(TRegistration));
      }

      public static TService GetService<TRegistration, TService>(this IServiceProvider provider)
      {
         var service = (TService)provider.GetService(typeof(TRegistration));

         if (service == null)
            throw new InvalidOperationException($"Required service '{typeof(TRegistration).FullName}' not found.");

         return service;
      }
   }
}
