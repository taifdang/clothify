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

namespace WpfApp2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public class Person
{
    public bool IsSelected { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
   
}
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var data = new List<Person>
        {
            new Person { Name = "Ngọc", IsSelected = false },
            new Person { Name = "Hà", IsSelected = true },
            new Person { Name = "Ngọc", IsSelected = false },
            new Person { Name = "Hà", IsSelected = true },
            new Person { Name = "Ngọc", IsSelected = false },
            new Person { Name = "Hà", IsSelected = true }
        };

        MyDataGrid.ItemsSource = data;
    }
}