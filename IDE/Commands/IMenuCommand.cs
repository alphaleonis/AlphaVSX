using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public interface IMenuCommand
   {
      int MatchedCommandID { get; }

      int CommandID { get; }

      Guid CommandSetId { get; }

      bool Enabled { get; set; }

      string Text { get; set; }

      bool Visible { get; set; }

      bool Checked { get; set; }

      int CommandIndex { get; }
   }
}

