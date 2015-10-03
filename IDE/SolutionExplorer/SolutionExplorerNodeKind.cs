using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alphaleonis.Vsx.IDE
{
   public enum SolutionExplorerNodeKind
   {
      Solution,
      SolutionFolder,
      SolutionItem,
      Project,
      Folder,
      Item,
      Reference,
      ReferencesFolder,
      Custom,
   }
}