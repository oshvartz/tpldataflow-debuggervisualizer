using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace TPLDataFlowDebuggerVisualizer.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ToolWindow : Window
    {
        public ToolWindow()
        {
            InitializeComponent();
           
        }
        DispatcherTimer _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(.5) };

        private void expanderHasCollapsed(object sender, RoutedEventArgs args)
        {
            RemoveOverlapOfVertexs();
        }

        private void OnItemExpanded(object sender, RoutedEventArgs e)
        {
            RemoveOverlapOfVertexs();

        }

        private void RemoveOverlapOfVertexs()
        {
            _timer.Tick += delegate
            {
                Graph.RecalculateOverlapRemoval();
                _timer.Stop();
            };
            _timer.Start();
        }


   
    }
}
