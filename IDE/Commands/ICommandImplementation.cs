using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public interface ICommandImplementation
   {
      void Execute(IMenuCommand command);
      void QueryStatus(IMenuCommand command);
   }
}

