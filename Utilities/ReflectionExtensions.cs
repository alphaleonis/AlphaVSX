using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alphaleonis.Vsx
{
   internal static class ReflectionExtensions
   {
      public static bool IsDefined<T>(this MemberInfo member) where T : Attribute
      {
         return member.IsDefined(typeof(T));
      }

      public static bool IsDefined<T>(this MemberInfo member, bool inherit) where T : Attribute
      {
         return member.IsDefined(typeof(T), inherit);
      }
   }
}
