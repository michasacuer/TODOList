using System.Windows;

namespace TODOList
{
    /// <summary>
    /// Interaction logic for AddParticipantsWindow.xaml
    /// </summary>
    public partial class AddParticipantsWindow : Window
    {
        public AddParticipantsWindow(int index)
        {
            InitializeComponent();
            this.DataContext = new AddParticipantsViewModel(index);
        }
    }
}
