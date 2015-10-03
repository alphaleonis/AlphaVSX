using System;

namespace Alphaleonis.Vsx
{
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
   public sealed class ComponentAttribute : Attribute
   {
      public ComponentAttribute()
      {
      }

      public ComponentAttribute(string name, bool isSingleton)         
      {
         Name = name;
         IsSingleton = isSingleton;
      }

      public ComponentAttribute(bool isSingleton)
         : this(null, isSingleton)
      {
      }

      

      public string Name
      {
         get; set;
      }

      public bool IsSingleton
      {
         get; set;
      }
   }
}
