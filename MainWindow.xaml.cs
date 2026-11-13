using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FilmekFeladat
{
    
    public class Film
    {
        public string Cim { get; set; }
        public string Mufaj { get; set; }
        public int Hossz { get; set; }
        public double Ertekeles { get; set; }
    }

    public partial class MainWindow : Window
    {
        private List<Film> filmekLista = new List<Film>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnBetoltes_Click(object sender, RoutedEventArgs e)
        {
            filmekLista.Clear();

            if (File.Exists("filmek.txt"))
            {
                var sorok = File.ReadAllLines("filmek.txt");
                foreach (var sor in sorok.Skip(1)) 
                {
                    var adatok = sor.Split(';');
                    if (adatok.Length >= 4)
                    {
                        filmekLista.Add(new Film
                        {
                            Cim = adatok[0],
                            Mufaj = adatok[1],
                            Hossz = int.Parse(adatok[2]),
                            Ertekeles = double.Parse(adatok[3].Replace('.', ','))
                        });
                    }
                }
            }
            else
            {
                
                filmekLista.Add(new Film { Cim = "Csillagok háborúja", Mufaj = "sci-fi", Hossz = 121, Ertekeles = 8.6 });
                filmekLista.Add(new Film { Cim = "Titanic", Mufaj = "dráma", Hossz = 195, Ertekeles = 7.8 });
                filmekLista.Add(new Film { Cim = "A Gyűrűk Ura", Mufaj = "fantasy", Hossz = 201, Ertekeles = 8.8 });
                filmekLista.Add(new Film { Cim = "Mátrix", Mufaj = "sci-fi", Hossz = 136, Ertekeles = 8.7 });
                filmekLista.Add(new Film { Cim = "Shrek", Mufaj = "animáció", Hossz = 90, Ertekeles = 7.9 });
            }

            dgFilmek.ItemsSource = filmekLista;
            dgFilmek.Items.Refresh();
            pbHaladas.Value = 100; 
        }

        private void BtnHanyFilm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"A filmek száma: {filmekLista.Count}", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnLeghosszabb_Click(object sender, RoutedEventArgs e)
        {
            if (filmekLista.Count == 0) return;
            var leghosszabb = filmekLista.OrderByDescending(f => f.Hossz).First();
            MessageBox.Show($"A leghosszabb film: {leghosszabb.Cim} ({leghosszabb.Hossz} perc)", "Leghosszabb", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAtlag_Click(object sender, RoutedEventArgs e)
        {
            if (filmekLista.Count == 0) return;
            double atlag = filmekLista.Average(f => f.Ertekeles);
            MessageBox.Show($"Átlagos értékelés: {atlag:F2}", "Átlag", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnScifi_Click(object sender, RoutedEventArgs e)
        {
            var scifi = filmekLista.Where(f => f.Mufaj.ToLower().Contains("sci-fi")).ToList();
            dgFilmek.ItemsSource = scifi;
        }

        private void BtnCimek_Click(object sender, RoutedEventArgs e)
        {
            string cimek = string.Join("\n", filmekLista.Select(f => f.Cim));
            MessageBox.Show(cimek, "Filmek címei", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnRendezes_Click(object sender, RoutedEventArgs e)
        {
            var rendezett = filmekLista.OrderBy(f => f.Cim).ToList();
            dgFilmek.ItemsSource = rendezett;
        }

        private void DgFilmek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgFilmek.SelectedItem is Film kivalasztott)
            {
                MessageBox.Show($"Kiválasztott film: {kivalasztott.Cim}\nÉrtékelés: {kivalasztott.Ertekeles}", "Film adatai", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
