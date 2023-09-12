using Com.Josh2112.Libs.MaterialDesign.DialogPlus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace MaterialDesign.DialogPlus.Examples
{
    public class Form : ObservableValidator
    {
        private string? name, email;
        private int age;

        [Required]
        [MinLength( 3 )]
        public string? Name
        {
            get => name;
            set => SetProperty( ref name, value, true );
        }

        [Required]
        [Range( 18, 120 )]
        public int Age
        {
            get => age;
            set => SetProperty( ref age, value, true );
        }

        [Required]
        [EmailAddress]
        public string? Email
        {
            get => email;
            set => SetProperty( ref email, value, true );
        }

        public bool Validate()
        {
            ValidateAllProperties();
            return !HasErrors;
        }
    }

    public partial class FormInputDialog : UserControl, IHasDialogResult<Form?>
    {
        public DialogResult<Form?> Result { get; } = new();

        public Form Form { get; } = new();

        public FormInputDialog() => InitializeComponent();

        [RelayCommand]
        private void Submit()
        {
            if( Form.Validate() )
                Result.Set( Form );
        }

        [RelayCommand]
        private void Cancel() => Result.Set( null );
    }
}
