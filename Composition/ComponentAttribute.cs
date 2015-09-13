using System;

namespace Alphaleonis.Vsx
{
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
   public sealed class ComponentAttribute : Attribute
   {
      public ComponentAttribute()
         : this(true)
      {
      }

      public ComponentAttribute(bool isSingleton)
      {
         IsSingleton = isSingleton;
      }


      public bool IsSingleton
      {
         get; set;
      }
   }
}
