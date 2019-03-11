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
        
        public System.Windows.ShutdownMode ShutdownMode { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            
    

        }

        private void Siatka()
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    Rectangle rec = new Rectangle()
                    {
                        Width = 20,
                        Height = 20,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,

                    };





                    canvas.Children.Add(rec);
                    Canvas.SetTop(rec, i * 20);
                    Canvas.SetLeft(rec, j * 20);

                    if (i == 0) { 

                        Label lab = new Label
                        {
                            FontSize = 20,
                            Content = j.ToString(),

                        };

                        canvas.Children.Add(lab);
                        Canvas.SetTop(lab, 10);
                        Canvas.SetLeft(lab, 10 + j * 20);
                    }

                }
            }
        }

        public void LoadData()
        {

            danePliks.Clear();
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

                sr.Close();
                
            }
            

            dane.Close();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            UseData();
            FillCombo();
            
            
        }

        private void FillCombo()
        {
            if (dane != null && danePliks.Count > 0)
            {
                xd.Text = danePliks[0].nazwa;
                //danePliks[0].Draw123();
                foreach (var item in danePliks)
                {
                    combo1.Items.Add(item.nazwa);

                }
                combo1.SelectedIndex = 0;

            }

            else
            {
                combo1.Items.Clear();
                xd.Text = "";
                
            }
        
        }

        public void Draw123()
        {

            DanePlik temp = danePliks[0];
            int t_czas;

            int[] t_zwolnienia = new int[temp.maszyny];
            byte r, g, b;

            Array.Clear(t_zwolnienia, 0, t_zwolnienia.Length);

            Random rnd = new Random();


            for (int z = 0; z < temp.zadania; z++)
            {
                t_czas = 0;
                int mv = 0;
                r = (byte)rnd.Next(255);
                g = (byte)rnd.Next(255);
                b = (byte)rnd.Next(255);

                for (int i = 0; i < temp.maszyny; i++)
                {

                   
                    t_czas = (temp.JohnsonNaSztywno()[z, i]);
                    
                    
                    

                    int t_czas2 = t_czas * 20 ;

                    Rectangle rec = new Rectangle()
                    {
                        Width = t_czas2,
                        Height = 40,
                        Fill = new SolidColorBrush(Color.FromRgb(r, g, b)),
                        Stroke = Brushes.Black,

                    };


                    canvas.Children.Add(rec);
                    Canvas.SetTop(rec, 20 + mv);
                    if(i == 0)
                    {
                        
                        Canvas.SetLeft(rec, 20 + t_zwolnienia[i]);
                        
                    }
                    else
                    {
                        if(t_zwolnienia[i] >= t_zwolnienia[i-1])
                            Canvas.SetLeft(rec, 20 + t_zwolnienia[i]);
                        else
                            Canvas.SetLeft(rec, 20 + t_zwolnienia[i-1]);


                    }


                    Label lab = new Label()
                    {
                        Content = (z + 1).ToString(),

                        FontSize = 20,
                        Foreground = new SolidColorBrush(Color.FromRgb((byte)(255 - r), (byte)(255 - g), (byte)(255 - b))),


                    };
                    canvas.Children.Add(lab);
                    Canvas.SetTop(lab, 10 + mv);
                    if(i ==0)
                        Canvas.SetLeft(lab, 10 + t_czas2/2 + t_zwolnienia[i]);
                    else
                    {
                        if (t_zwolnienia[i] >= t_zwolnienia[i - 1])
                            Canvas.SetLeft(lab, 10 + t_czas2 / 2 + t_zwolnienia[i]);
                        else
                            Canvas.SetLeft(lab, 10 + t_czas2 / 2 + t_zwolnienia[i-1]);
                    }


                    if (i == 0)
                    {
                        t_zwolnienia[i] += t_czas2;
                    }
                    else
                    {
                        if(t_zwolnienia[i] >= t_zwolnienia[i-1])
                            t_zwolnienia[i] += (t_czas2);
                        else
                            t_zwolnienia[i] = t_zwolnienia[i-1] + (t_czas2);
                    }
                    

                    mv += 60;
                }
            }
            xd.Text = (t_zwolnienia[temp.maszyny - 1]/20).ToString();
        }

        private void combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Draw123();
            //xd.Text = combo1.SelectedValue.ToString();
        }

        private void Rysuj(object sender, RoutedEventArgs e)
        {
            //Draw123();
            canvas.Children.Clear();
            Siatka();
            kupa.Text = FunkcjaLiczacaCzas();
            // danePliks[0].Johnson();
           
        }
        public string FunkcjaLiczacaCzas()
        {

            DanePlik temp = danePliks[0];
            int t_czas;

            int[] t_zwolnienia = new int[temp.maszyny];
          


            for (int z = 0; z < temp.zadania; z++)
            {
                t_czas = 0;
                

                for (int i = 0; i < temp.maszyny; i++)
                {


                    t_czas = (temp.JohnsonNaSztywno()[z, i]);




                    int t_czas2 = t_czas;

                   


                    if (i == 0)
                    {
                        t_zwolnienia[i] += t_czas2;
                    }
                    else
                    {
                        if (t_zwolnienia[i] >= t_zwolnienia[i - 1])
                            t_zwolnienia[i] += (t_czas2);
                        else
                            t_zwolnienia[i] = t_zwolnienia[i - 1] + (t_czas2);
                    }


                    
                }
            }
            int cmax;
            cmax = (t_zwolnienia[temp.maszyny - 1] );
            return cmax.ToString();
            //xd.Text = (t_zwolnienia[temp.maszyny - 1] / 20).ToString();
        }

    }

        
}
