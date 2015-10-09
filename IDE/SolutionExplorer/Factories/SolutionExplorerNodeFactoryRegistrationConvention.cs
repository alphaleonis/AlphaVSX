using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Vsx.IDE
{
   internal class SolutionExplorerNodeFactoryRegistrationConvention : RegistrationConvention
   {
      public override Func<Type, IEnumerable<Type>> GetFromTypes()
      {
         return t => new[] { typeof(ISolutionExplorerNodeFactory) };
      }

      public override Func<Type, IEnumerable<InjectionMember>> GetInjectionMembers()
      {
         return null;
      }

      public override Func<Type, LifetimeManager> GetLifetimeManager()
      {
         return WithLifetime.ContainerControlled;
      }

      public override Func<Type, string> GetName()
      {
         return WithName.TypeName;
      }

      public override IEnumerable<Type> GetTypes()
      {
         return Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.Equals(typeof(GlobalSolutionExplorerNodeFactory)) && type.IsClass && !type.IsAbstract && type.Implements<ISolutionExplorerNodeFactory>());
      }
   }
}

