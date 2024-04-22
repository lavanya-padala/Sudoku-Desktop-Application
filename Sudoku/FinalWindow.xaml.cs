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

namespace Sudoku
{

    public partial class FinalWindow : Window
    {
        public string PromptMessage { get; set; }
        public FinalWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}