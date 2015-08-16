using System.Windows;

namespace Alphaleonis.Vsx
{
   public interface IDialogService
   {
      bool? ShowMessageBox(string message, string title = "Microsoft Visual Studio", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.OK);
   }
}