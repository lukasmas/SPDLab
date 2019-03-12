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
using System.Collections;

namespace SPD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileStream dane;
        List<DanePlik> danePliks = new List<DanePlik>();
        DanePlik temp;
        int co;
        
        public System.Windows.ShutdownMode ShutdownMode { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            Siatka();
            co = 0;


        }

        private void Siatka()
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 1280 / 20; j++)
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

                    if (i == 2)
                    {

                        Label lab = new Label
                        {
                            FontSize = 12,
                            Content = j.ToString(),

                        };

                        canvas.Children.Add(lab);
                        Canvas.SetTop(lab, -5);
                        Canvas.SetLeft(lab, j * 20);
                    }

                }
            }
        }

        public void LoadData()
        {
            co = 0;
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
                danePliks.Clear();


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
            
            if(dane!= null)
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
            combo1.Items.Clear();
            if (dane != null && danePliks.Count > 0)
            {
               // xd.Text = danePliks[0].nazwa;
                //danePliks[0].Draw123();
                foreach (var item in danePliks)
                {
                    combo1.Items.Add(item.nazwa);

                }
                combo1.SelectedIndex = 0;

            }
        }

        public void Draw123()
        {

            //DanePlik temp = danePliks[0];
            int t_czas;

            int[] t_zwolnienia = new int[temp.maszyny];
            byte r, g, b;

            Array.Clear(t_zwolnienia, 0, t_zwolnienia.Length);

            Random rnd = new Random();


            for (int z = 0; z < temp.zadania; z++)
            {
                t_czas = 0;
                int mv = 0;
                string numer = "";
                r = (byte)rnd.Next(255);
                g = (byte)rnd.Next(255);
                b = (byte)rnd.Next(255);

                for (int i = 0; i < temp.maszyny; i++)
                {

                    switch (co)
                    {
                        case 0:
                            {
                                t_czas = (temp.czasy[z, i]);
                                numer = (z+1).ToString();
                                break;
                            }

                        case 1:
                            {
                                t_czas = (temp.JohnsonNaSztywno()[z, i]);
                                numer = temp.bestOptJ[z].ToString();
                                break;
                            }

                        case 2:
                            {
                                t_czas = (temp.prez_wy[z, i]);
                                numer = temp.bestOpt[z].ToString();
                                break;
                            }
                        default:
                            {
                                t_czas = (temp.czasy[z, i]);
                                numer = (z + 1).ToString();
                                break;
                            }
                    }
                    
                    
                    
                    

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
                        
                        Content = numer,

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
            temp = null;
            foreach (var item in danePliks)
            {
                try
                {
                    if (item.nazwa == combo1.SelectedValue.ToString())
                    {
                        temp = item;
                    }
                }
                catch
                {

                }
            }
            if (temp == null)
                if (danePliks.Count > 0)
                    temp= danePliks[0];

            Clear(0);
            //kupa.Text = FunkcjaLiczacaCzas();

        }

        private void Clear(int x = -1)
        {
            canvas.Children.Clear();
            Siatka();
            if (x != -1)
            {
                co = x;
                Draw123();
            }
        }

        private void Rysuj(object sender, RoutedEventArgs e)
        {

            Clear();
            
        }
        public string FunkcjaLiczacaCzas()
        {
            return temp.Czas(temp.JohnsonNaSztywno()).ToString();
            //return danePliks[0].Czas(danePliks[0].JohnsonNaSztywno()).ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //DanePlik temp = danePliks[0];
            if (temp.bestOpt == null)
            {
                temp.Permutacja();
                temp.PermutacjaCzas();

                kupa.Text = "";
                for (int i = 0; i < temp.permutacje.Count; i++)
                {
                    kupa.Text += temp.permutacje[i] + " : " + temp.czasy_permutacje[i].ToString() + " \n";
                }
            }
            Clear(2);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Clear(1);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Clear(0);
        }

        public void CreateData()
        {
            Random rnd = new Random();
            int a = int.Parse(iloscMaszyn.Text);
            int b= int.Parse(iloscZadan.Text);
            string [,] temp = new string[a, b];
          
            if ((!File.Exists("scores.txt")))
                using (FileStream fs = File.Create("D:/data.txt")) //Creates Scores.txt
        {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write("ta001\n");
                    sw.Write(a + " " + b + "\n");
                     for (int i = 0; i < a; i++)
                     {
                         for (int j = 0; j < b; j++)
                         {
                           
                            temp[i, j] = rnd.Next(1, 100)+" ";
                             sw.Write(temp[i, j]);
                          
                        }
                        sw.Write("\n");
                       
                    }

                    sw.Close();
                }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            CreateData();
        }
    }

        
}
