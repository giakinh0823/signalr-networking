using Microsoft.Win32;
using Newtonsoft.Json;
using PE_FA_WPF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace PE_FA_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private PE_PRN_Fall22B1Context _context;
        private static List<Star>? _list = null;

        public MainWindow(PE_PRN_Fall22B1Context context)
        {
            InitializeComponent();
            _context = context;
            _list = _context.Stars.ToList();
            listStars.ItemsSource = _list;
            List<String> nationalities = new List<string>
            {
                    "USA",
                    "UK",
                    "Japan",
                    "China"
            };
            Nationalities.ItemsSource = nationalities;
        }

        private void UpdateGridView()
        {
            if(_list != null )
            {
                listStars.ItemsSource = _list.ToList();
            }
        }

        private void listStars_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = ((ListView)sender).SelectedItem;
            if (item != null)
            {
                var gender = ((Star)item).Male;
                var nationality = ((Star)item).Nationality;
                if (gender != null)
                {
                    if (gender.Value)
                    {
                        Male.IsChecked = true;
                    }
                    else
                    {
                        Female.IsChecked = true;
                    }
                }

                if(nationality != null)
                {
                    Nationalities.SelectedItem = nationality;
                }
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (GetInputDirectorObject != null)
            {
                if(_list == null) _list = new List<Star>();
                Star? star = GetInputDirectorObject();
                if(star != null)
                {
                    _list.Add(star);
                }
            }
            UpdateGridView();
        }

        private Star? GetInputDirectorObject()
        {
            Star? star = null;
            try
            {
                star = new Star
                {
                    Id = 0,
                    FullName = FullName.Text,
                    Male = Male.IsChecked == true ? true : false,
                    Dob = DtpDateOfBirth != null && DtpDateOfBirth.SelectedDate !=null ? DtpDateOfBirth.SelectedDate.Value : null,
                    Nationality = Nationalities !=null && Nationalities.SelectedItem !=null ? Nationalities.SelectedItem.ToString() : null,
                    Description = Description.Text
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error get stars");
            }
            return star;
        }

        private void import_Click(object sender, RoutedEventArgs e)
        {
            if(_list ==null) _list = new List<Star>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                List<Star> stars = ReadStarsFromFile(openFileDialog.FileName);
                foreach (Star s in stars)
                {
                    _list.Add(s);
                }
            }
            UpdateGridView();
        }

        private List<Star> ReadStarsFromFile(string filePath)
        {
            string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();

            switch (fileExtension)
            {
                case ".json":
                    string json = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<List<Star>>(json);
                case ".xml":
                    XElement xml = XElement.Load(filePath);
                    return xml.Elements("Star").Select(s => new Star
                    {
                        FullName = s.Element("FullName").Value,
                        Male = bool.Parse(s.Element("Male").Value),
                        Dob = DateTime.Parse(s.Element("Dob").Value),
                        Description = s.Element("Description").Value,
                        Nationality = s.Element("Nationality").Value

                    }).ToList();
                default:
                    throw new ArgumentException("Unsupported file format");
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Star s in _list)
                {
                    if (s.Id == 0)
                    {
                        _context.Stars.Add(s);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Star");
            }

            MessageBox.Show(" add stars successfull");
        }
    }
}
