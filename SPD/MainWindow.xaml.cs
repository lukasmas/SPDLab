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
        DanePlik temp;
        int co;
        double width;
        int sekunda;
        double f_size;
       
        
        public System.Windows.ShutdownMode ShutdownMode { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            width = okno.Width;
            sekunda = 20; //px
            f_size = (12 * sekunda) / 20;
            temp = null;
            Siatka();
            co = 0;
            
            
        }
       

        private void Siatka()
        {
             
            if(temp != null)
            {
                int temp_s = temp.Czas(temp.czasy);
                if (temp_s > 60)
                {
                    sekunda = (int)(20*60)/temp_s;
                    if (sekunda == 0)
                        sekunda = 1;
                }
                else
                    sekunda = 20;
            }
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < width / sekunda; j++)
                {
                    Rectangle rec = new Rectangle()
                    {
                        Width = sekunda,
                        Height = 20,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,

                    };

                    canvas.Children.Add(rec);
                    Canvas.SetTop(rec, i * 20);
                    Canvas.SetLeft(rec, j * sekunda);

                    if (i == 2)
                    {
                        
                        Label lab = new Label
                        {
                            
                            FontSize = f_size,
                            Content = j.ToString(),

                        };

                        canvas.Children.Add(lab);
                        Canvas.SetTop(lab, -5);
                        Canvas.SetLeft(lab, j * sekunda);
                    }
                    if (sekunda < 5) j += 9;
                }
            }
        }

        public void LoadData()
        {
            co = 0;
            danePliks.Clear();

            dane = null;

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
                dane.Close();
            }
            
                
            
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

            int[,] temp_czasy;
            string numery = string.Empty;

            switch (co)
            {
                case 0:
                    {
                        temp_czasy = (temp.czasy);
                        xd.Text = temp.Czas(temp.czasy).ToString();
                        break;
                    }

                case 1:
                    {
                        temp_czasy = (temp.JohnsonNaSztywno());
                        numery = temp.bestOptJ;
                        xd.Text = temp.Czas(temp.JohnsonNaSztywno()).ToString();
                        break;
                    }

                case 2:
                    {
                        temp_czasy = (temp.prez_wy);
                        numery = temp.bestOpt;
                        xd.Text = temp.Czas(temp.prez_wy).ToString();
                        break;
                    }
                default:
                    {
                        temp_czasy = (temp.czasy);
                        xd.Text = temp.Czas(temp.czasy).ToString();
                        break;
                    }
            }

           

            int[] t_zwolnienia = new int[temp.maszyny];
            byte r, g, b;

            Array.Clear(t_zwolnienia, 0, t_zwolnienia.Length);

            Random rnd = new Random();


            for (int z = 0; z < temp.zadania; z++)
            {
                t_czas = 0;
                int mv = 0;
                string numer;
                if (numery != string.Empty)
                {
                    numer = (numery[z] - 48).ToString();
                }
                else
                {
                    numer = (z + 1).ToString();
                }
                

                r = (byte)rnd.Next(255);
                g = (byte)rnd.Next(255);
                b = (byte)rnd.Next(255);

                

                for (int i = 0; i < temp.maszyny; i++)
                {
                    t_czas = temp_czasy[z, i];

                    int t_czas2 = t_czas * sekunda ;

                    Rectangle rec = new Rectangle()
                    {
                        Width = t_czas2,
                        Height = 40,
                        Fill = new SolidColorBrush(Color.FromRgb(r,g,b)),
                        Stroke = Brushes.Black,

                    };


                    canvas.Children.Add(rec);
                    Canvas.SetTop(rec, 20 + mv);
                    if(i == 0)
                    {
                        
                        Canvas.SetLeft(rec, sekunda + t_zwolnienia[i]);
                        
                    }
                    else
                    {
                        if(t_zwolnienia[i] >= t_zwolnienia[i-1])
                            Canvas.SetLeft(rec, sekunda + t_zwolnienia[i]);
                        else
                            Canvas.SetLeft(rec, sekunda + t_zwolnienia[i-1]);


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
                        Canvas.SetLeft(lab, sekunda/2 + t_czas2/2 + t_zwolnienia[i]);
                    else
                    {
                        if (t_zwolnienia[i] >= t_zwolnienia[i - 1])
                            Canvas.SetLeft(lab, sekunda/2 + t_czas2 / 2 + t_zwolnienia[i]);
                        else
                            Canvas.SetLeft(lab, sekunda/2 + t_czas2 / 2 + t_zwolnienia[i-1]);
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
            //xd.Text = (t_zwolnienia[temp.maszyny - 1]/sekunda).ToString();
            
        }

        private void combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // temp = null;
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

        private void okno_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            if (Math.Abs(okno.Width - width) > 100)
            {
                width = okno.Width;
                Clear();
                if (temp != null)
                    Draw123();

            }

        }

        private void Button_Click_Generuj(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            int max_maszyn = 3;
            int max_zadan = 6;
            int max_time = 15;
            int indeks = 0;


            using (FileStream fs = File.Create("score.txt")) //Creates Scores.txt
            {
                StreamWriter sw = new StreamWriter(fs);
                for (int h = 2; h <= max_zadan; h++)
                {
                    
                    for (int i = 2; i <= max_maszyn; i++)
                    {
                        if(indeks < 10 )
                            sw.WriteLine("ta00" + indeks);
                        else if(indeks < 100)
                            sw.WriteLine("ta0" + indeks);
                        else
                            sw.WriteLine("ta" + indeks);

                        sw.WriteLine(h + " " + i);
                        for (int m = 0; m < h; m++)
                        {

                            for (int z = 0; z < i; z++)
                            {
                                if(z == 0)
                                    sw.Write((rnd.Next(1, max_time)).ToString().PadLeft(2));
                                else
                                    sw.Write((rnd.Next(1, max_time)).ToString().PadLeft(3));
                            }
                            sw.WriteLine();
                            

                        }
                        indeks++;
                        sw.WriteLine();
                    }


                }
                sw.Close();

            }
            MessageBox.Show("Dane zostały wygenerowane","Dane");
        }
    }

        
}
