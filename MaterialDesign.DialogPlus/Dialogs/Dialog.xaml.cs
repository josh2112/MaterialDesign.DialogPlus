using System.Windows;

namespace Com.Josh2112.Libs.MaterialDesign.DialogPlus.Dialogs
{
    public partial class Dialog : IHasDialogResult<ButtonDef>
    {
        public string Title { get; }
        public string Message { get; private set; }
        public ButtonDef[] Buttons { get; private set; }

        public DialogResult<ButtonDef> Result { get; } = new DialogResult<ButtonDef>();

        /// <summary>
        /// Creates a dialog with the given title, question, and button list.
        /// Buttons will be laid out from left to right in the order given. The
        /// return value of the dialog will be the ButtonDef that was pressed.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="buttons"></param>
        public Dialog( string title, string message, params ButtonDef[] buttons )
        {
            Title = title;
            Message = message;
            Buttons = buttons;

            InitializeComponent();
        }

        private void Button_Click( object sender, RoutedEventArgs e ) =>
            Result.Set( (sender as FrameworkElement).DataContext as ButtonDef );
    }
}
