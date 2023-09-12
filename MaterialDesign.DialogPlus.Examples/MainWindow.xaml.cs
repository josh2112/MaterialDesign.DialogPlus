using System;
using System.Threading;
using System.Windows;

using MaterialDesignThemes.Wpf;

using Com.Josh2112.Libs.MaterialDesign.DialogPlus;
using Dialogs = Com.Josh2112.Libs.MaterialDesign.DialogPlus.Dialogs;

namespace MaterialDesign.DialogPlus.Examples
{
    public partial class MainWindow : Window
    {
        public SnackbarMessageQueue Messages { get; } = new();

        public MainWindow() => InitializeComponent();

        private async void BasicDialogButton_Click( object sender, RoutedEventArgs e )
        {
            var result = await this.ShowDialogForResultAsync( new Dialogs.Dialog( "Question",
                "Your confrobulator has unsaved changes. What do you want to do?",
                ButtonDef.Neutral( "Keep working", isCancel: true ),
                ButtonDef.Negative( "Delete it" ),
                ButtonDef.Positive( "Save it", isDefault: true ) ) );

            Messages.Enqueue( $"Dialog closed with button '{result.Text}'" );
        }

        private async void BasicDialogButtonWithCancellation_Click( object sender, RoutedEventArgs e )
        {
            var canceler = new CancellationTokenSource( TimeSpan.FromSeconds( 5 ) );

            try
            {
                var result = await this.ShowDialogForResultAsync( new Dialogs.Dialog(
                    "Alert", "This dialog will self-destruct in 5 seconds!" ), canceler.Token );

                Messages.Enqueue( $"Dialog closed with button '{result.Text}'" );
            }
            catch( OperationCanceledException )
            {
                Messages.Enqueue( $"Dialog closed by cancellation token" );
            }
        }
    }
}
