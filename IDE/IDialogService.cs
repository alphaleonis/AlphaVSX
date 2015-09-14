using System.Windows;

namespace Alphaleonis.Vsx
{
   public interface IDialogService
   {
      MessageBoxResult ShowMessageBox(string message, string title = "Microsoft Visual Studio", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information, MessageBoxResult defaultResult = MessageBoxResult.OK);
   }
}