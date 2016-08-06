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
using System.Windows.Shapes;

namespace RoboGUI.Views
{
    /// <summary>
    /// Interaction logic for MapCreatorDialog.xaml
    /// </summary>
    public partial class MapCreatorDialog : Window
    {
        private int width;

        private int height;

        public MapCreatorDialog()
        {
            InitializeComponent();

            this.width = 0;
            this.height = 0;

            this.DataContext = this;
        }

        public int MapWidth
        {
            get
            {
                return this.width;
            }

            set
            {
                this.width = value;
            }
        }

        public int MapHeight
        {
            get
            {
                return this.height;
            }

            set
            {
                this.height = value;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
