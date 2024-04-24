using System.Diagnostics;
using System.Reflection.Metadata;
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

namespace AIGallery
{

    public partial class MainWindow : Window
    {
        AppDBContext db = new AppDBContext();

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}