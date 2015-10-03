using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal static class VsHierarchyExtensions
   {
      public static T GetProperty<T>(this IVsHierarchy hierarchy, __VSHPROPID propId, uint itemid)
      {
         object value = null;         
         int hr = hierarchy.GetProperty(itemid, (int)propId, out value);
         if (hr != VSConstants.S_OK || value == null)
         {
            return default(T);
         }
         return (T)value;
      }
   }
}

