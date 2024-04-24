using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
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
        private ImageProviderViewModel ViewModel = new();

        public MainWindow()
        {
            InitializeComponent();
            entityComboBox.ItemsSource = ViewModel.GetProviderNames();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void EntityComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (entityComboBox.SelectedItem != null)
            {
                ViewModel.SetActiveProvider(entityComboBox.SelectedItem.ToString() ?? "");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text.Length == 0)
                return;
            ViewModel.ProcessQuery(searchTextBox.Text).ContinueWith(t =>
            {
                string? errorMsg = null;
                try
                {
                    if (t.IsFaulted)
                    {
                        errorMsg = $"{t.Exception?.Message}\nStack Trace:{t.Exception?.StackTrace}";
                    }
                    else if (t.Result != null && t.Result.ImageData != null)
                    {
                        var image = new BitmapImage();
                        using (var ms = new MemoryStream(t.Result.ImageData))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = ms;
                            image.EndInit();
                        }
                        image.Freeze();
                        Dispatcher.Invoke(() =>
                        {
                            displayedImage.Source = image;
                        });
                    }
                    else
                    {
                        errorMsg = "No image was returned";
                    }
                }
                catch (Exception e)
                {
                    errorMsg = $"{e.Message}\nStack Trace:{e.StackTrace}";
                }

                if (errorMsg is not null)
                {
                    Dispatcher.Invoke(() => // Ensure that the following code runs on UI thread
                    {
                        MessageBox.Show(errorMsg, "Image query failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            });
        }
    }
}