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

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public D3D.RenderHost RenderHost { get { return renderHost.Child as D3D.RenderHost; } }

        public MainWindow()
        {
            InitializeComponent();

            renderHost.Child = new D3D.RenderHost();
            Instance = this;

            renderHost.Child.HandleCreated += (sender, args) => D3D.RenderManager.Instance.init();
        }
    }
}
