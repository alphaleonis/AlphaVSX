using System;

namespace Alphaleonis.Vsx
{
   [Flags]
   public enum ServiceLocatorOptions
   {
      None = 0,
      PackageServiceProvider = 1,
      MEF = 2,
      All = 3
   }
}

