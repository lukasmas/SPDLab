using Microsoft.Win32;
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
using System.IO;


namespace SPD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileStream dane;
        DanePlik[] danePliks = new DanePlik[120];


        public MainWindow()
        {
            InitializeComponent();
            

        }

        public void LoadData()
        {


            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == true)
            {
         
                dane = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read);
            }

        }

        public void UseData()
        {
            StreamReader sr = new StreamReader(dane);
            int i = 0;
            

            while (!sr.EndOfStream)
            {
                
                if(sr.ReadLine().Contains("ta"))
                {
                    string numer = i.ToString;
                    string nazwa = "ta" + numer;
                    //int w, h;



                    //danePliks[i] = new DanePlik();
                }
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            UseData();
        }
    }

        
}
