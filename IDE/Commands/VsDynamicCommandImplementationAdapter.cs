using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class DynamicCommandImplementationAdapter : CommandImplementationAdapter
   {
      public DynamicCommandImplementationAdapter(CommandID id, IDynamicCommandImplementation implementation)
          : base(id, implementation)
      {
      }

      private new IDynamicCommandImplementation Implementation
      {
         get
         {
            return base.Implementation as IDynamicCommandImplementation;
         }
      }

      protected override bool IsDynamic
      {
         get
         {
            return true;
         }
      }

      public override bool DynamicItemMatch(int cmdId)
      {
         if (cmdId >= CommandID.ID && cmdId < Implementation.GetDynamicCommandCount() + CommandID.ID)
         {
            Debug.WriteLine($"DynamicItemMatch({cmdId}) = true");
            MatchedCommandId = cmdId;
            return true;
         }
         else
         {
            Debug.WriteLine($"DynamicItemMatch({cmdId}) = false");
            MatchedCommandId = 0;
            return false;
         }
      }
   }
}

