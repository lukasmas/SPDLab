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
        List<DanePlik> danePliks = new List<DanePlik>();
        int mv = 0;


        public MainWindow()
        {
            InitializeComponent();
            

        }

        public void MainWindow_Load()
        {
            
            
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
            if (dane != null)
            {
                StreamReader sr = new StreamReader(dane);
                int indeks = 0;


                while (!sr.EndOfStream)
                {

                    if (sr.ReadLine().Contains("ta"))
                    {
                        string nazwa = "ta";
                        if (indeks < 10)
                        {
                            nazwa += "00" + (indeks).ToString();
                        }
                        else if (indeks < 100)
                        {
                            nazwa += "0" + (indeks).ToString();
                        }
                        else
                        {
                            nazwa += (indeks).ToString();
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

                        DanePlik temp = new DanePlik(nazwa, h, w, arr);

                        danePliks.Add(temp);

                        indeks++;


                    }



                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            UseData();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Rectangle rec = new Rectangle()
            {
                Width = 50,
                Height = 20,
                Fill = Brushes.AliceBlue,
                
            };


            canvas.Children.Add(rec);
            Canvas.SetTop(rec, 20+mv);
            Canvas.SetLeft(rec, 20+mv);

            Label lab = new Label()
            {
                Content = "1",
               // Width = 20,
               // Height = 20,
                FontSize = 20,


            };
            canvas.Children.Add(lab);
            Canvas.SetTop(lab, 10+mv);
            Canvas.SetLeft(lab, 35+mv);

            mv += 22;
        }
    }

        
}
