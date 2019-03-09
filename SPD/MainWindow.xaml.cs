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
        DanePlik[] danePliks = new DanePlik[121];


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
            int indeks = 0;
            

            while (!sr.EndOfStream )
            {
                
                if(sr.ReadLine().Contains("ta"))
                {
                    string nazwa = "ta";
                    if(indeks <10)
                    {
                        nazwa +="00" + (indeks).ToString();
                    }
                    else if (indeks < 100)
                    {
                        nazwa += "0" + (indeks).ToString();
                    }
                    else
                    {
                        nazwa +=  (indeks).ToString();
                    }

                    int w, h;
                    string[] wart = sr.ReadLine().Split();

                    h = int.Parse(wart[0]);
                    w = int.Parse(wart[1]);
                    int[,] arr = new int[h, w];

                    
                   
                    


                    for (int i = 0; i < h; i++)
                     {
                        string[] vs = sr.ReadLine().Split();

                        

                        int x = 0;
                         for (int j = 0; j < vs.Length; j++)
                         {
                            try
                            {
                                if (vs[j] != "")
                                    arr[i, x++] = int.Parse(vs[j]);
                            }
                            catch (Exception)
                            {

                            }

                         }
                     }

                   





                    danePliks[indeks] = new DanePlik(nazwa, h,w, arr);

                    indeks++;


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
