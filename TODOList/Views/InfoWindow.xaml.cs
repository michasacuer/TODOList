using System.Windows;

namespace TODOList
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
            this.DataContext = new InfoViewModel();
        }
    }
}
