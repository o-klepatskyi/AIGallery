using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class ImageView : UserControl
    {
        public ImageView(ImageViewModel model)
        {
            InitializeComponent();
            DataContext = model;
            model.ImageDeleted += OnImageDeleted;
        }

        private void OnImageDeleted(object sender, EventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}
