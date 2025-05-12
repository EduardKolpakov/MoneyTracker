using MoneyTrackerApp.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyTrackerApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as MainViewModel;
        viewModel?.AddTransaction();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox.Text == "Описание" || textBox.Text == "Сумма")
        {
            textBox.Text = "";
            textBox.Foreground = Brushes.Black;
        }
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (string.IsNullOrWhiteSpace(textBox.Text))
        {
            if (textBox.Name == "DescriptionTextBox")
                textBox.Text = "Описание";
            else
                textBox.Text = "Сумма";

            textBox.Foreground = Brushes.Gray;
        }
    }
}