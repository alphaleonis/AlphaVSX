using System;

namespace Alphaleonis.Vsx
{
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
   public sealed class ToolkitComponentAttribute : Attribute
   {
      public ToolkitComponentAttribute()
         : this(true)
      {
      }

      public ToolkitComponentAttribute(bool isSingleton)
      {
         IsSingleton = isSingleton;
      }


      public bool IsSingleton
      {
         get; set;
      }
   }
}
