using Autodesk.AutoCAD.Runtime;

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

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    /// <summary>
    /// Логика взаимодействия для UserControl2.xaml
    /// </summary>
    public partial class UserControl2 : Window
    {
        public List<string> LayList = new List<string>();
        public  char []separator = { '\n','\r' };
        string[] h;
        public UserControl2()
        {
            InitializeComponent();
        }

        private void Layers_text_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Diam_text_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    

    private void Start_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Layers_text.LineCount; i++) 
            {
               
                h =  Layers_text.Text.Split(separator);
               
            }
            Class_add_Layer _Layer = new Class_add_Layer();
            _Layer.AddLayer(h);
        }
    }
}
