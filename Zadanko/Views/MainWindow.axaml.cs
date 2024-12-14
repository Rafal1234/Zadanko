using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Linq;

namespace Zadanko.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumbersOnly_KeyDown(object? sender, KeyEventArgs e)
        {
            bool isControlKey = e.Key == Key.Back || 
                              e.Key == Key.Delete || 
                              e.Key == Key.Left || 
                              e.Key == Key.Right ||
                              e.Key == Key.Tab;

            bool isDigitKey = (e.Key >= Key.D0 && e.Key <= Key.D9) || 
                             (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9);

            bool isModifiedKey = e.KeyModifiers != KeyModifiers.None;

            e.Handled = !(isControlKey || (isDigitKey && !isModifiedKey));
        }

        private void NumbersOnly_TextInput(object? sender, TextInputEventArgs e)
        {
            if (sender is not TextBox textBox) return;

            if (e.Text.Length > 1)
            {
                bool isAllDigits = e.Text.All(char.IsDigit);
                if (!isAllDigits)
                {
                    e.Handled = true;
                    return;
                }

                string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
                if (!int.TryParse(newText, out _))
                {
                    e.Handled = true;
                    return;
                }
            }
            else
            {
                if (!char.IsDigit(e.Text[0]))
                {
                    e.Handled = true;
                    return;
                }

                if (textBox.Text.Length == 0 && e.Text == "0")
                {
                    e.Handled = true;
                    return;
                }

                string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
                if (!int.TryParse(newText, out _))
                {
                    e.Handled = true;
                    return;
                }
            }
        }
    }
}