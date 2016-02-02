using Homebase.Business.Model;
using Homebase.Phone.ViewModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Homebase.Phone.Pages
{
    public sealed partial class TasksPage : PageBase
    {
        public TasksPage()
            : base()
        {
            InitializeComponent();
        }

        private async void AddTapped(object sender, RoutedEventArgs e)
        {
            var configureActionDialog = new ConfigureActionDialog();
            await configureActionDialog.ShowAsync();

            await (DataContext as ConvenienceViewModelBase).Load();
        }

        private async void ListView_ItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            var action = (e.ClickedItem as ActionDescription);
            
            var configureActionDialog = new ConfigureActionDialog(action.ActionIdentifier);
            await configureActionDialog.ShowAsync();
        }

        private void TaskHolding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }
    }
}
