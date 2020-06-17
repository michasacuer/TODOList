using System.Windows;

namespace TODOList
{
    /// <summary>
    /// Interaction logic for NewTask.xaml
    /// </summary>
    public partial class NewTask : Window
    {
        public NewTask()
        {
            InitializeComponent();
            this.DataContext = MainViewModel.Instance;
        }
    }
}
