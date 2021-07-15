
namespace POSUNO.Pages
{
    using System;
    using System.Threading.Tasks;
    using POSUNO.Helpers;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = await ValidForm();

            if (isValid)
            {
                MessageDialog messageDialog = new MessageDialog("Vamos bien", "OK");

                await messageDialog.ShowAsync();
            }
        }

        private async Task<bool> ValidForm()
        {
            bool validateForm = true;

            MessageDialog messageDialog;

            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar tu correo", "Error");

                await messageDialog.ShowAsync();

                validateForm = false;
            }

            if (!RegexUtilities.IsValidEmail(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar un correo válido", "Error");

                await messageDialog.ShowAsync();

                validateForm = false;
            }


            if (!RegexUtilities.IsValidEmail(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar tu contraseña", "Error");

                await messageDialog.ShowAsync();

                validateForm = false;
            }

            return validateForm;
        }
    }
}