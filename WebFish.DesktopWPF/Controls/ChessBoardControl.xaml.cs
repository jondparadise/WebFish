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

namespace WebFish.DesktopWPF.Controls
{
    /// <summary>
    /// Interaction logic for ChessBoardControl.xaml
    /// </summary>
    public partial class ChessBoardControl : UserControl
    {


        public SolidColorBrush LightBrush
        {
            get { return (SolidColorBrush)GetValue(LightBrushProperty); }
            set { SetValue(LightBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LightBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LightBrushProperty =
            DependencyProperty.Register("LightBrush", typeof(SolidColorBrush), typeof(ChessBoardControl));

        public SolidColorBrush DarkBrush
        {
            get { return (SolidColorBrush)GetValue(DarkBrushProperty); }
            set { SetValue(DarkBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DarkBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DarkBrushProperty =
            DependencyProperty.Register("DarkBrush", typeof(SolidColorBrush), typeof(ChessBoardControl));


        public ChessBoardControl()
        {
            InitializeComponent();
        }
    }
}
