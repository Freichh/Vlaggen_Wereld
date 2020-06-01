using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace Vlaggen_Wereld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    ///
    /// Opdracht 2: Vlaggen van de wereld
    /// Maak een programma waarmee de gebruiker(a) alle vlaggen van de wereld kan bekijken(b) zich in
    /// het herkennen van die vlaggen kan oefenen.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<ImageSource> vlaggen = new List<ImageSource>();
            
            List<Country> countries = new List<Country>();

            LoadJson();



            /// Haal alle images op als stream met pack URI's, zet om naar bitmaps en voeg toe aan List Vlaggen.
            string[] files = GetResourcesUnder("Resources");
            Array.Sort(files);
            foreach (var file in files)
            {
                string uriPath = "pack://application:,,,/Resources/" + file;
                //Console.WriteLine(file);
                Uri uri = new Uri(uriPath, UriKind.RelativeOrAbsolute);
                StreamResourceInfo info = Application.GetResourceStream(uri);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = info.Stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                vlaggen.Add(bitmapImage);
            }

            /// Bind vlaggen List aan ItemsControl in XAML
            //flagsOverview.ItemsSource = Vlaggen;

        }


        // Loop door alle resources en bewaar filenames 
        // (https://stackoverflow.com/questions/5869249/loop-through-visual-studio-resources)
        public static string[] GetResourcesUnder(string folder)
        {
            folder = folder.ToLower() + "/";

            var assembly = Assembly.GetCallingAssembly();
            var resourcesName = assembly.GetName().Name + ".g.resources";
            var stream = assembly.GetManifestResourceStream(resourcesName);
            var resourceReader = new ResourceReader(stream);

            var resources =
                from p in resourceReader.OfType<DictionaryEntry>()
                let theme = (string)p.Key
                where theme.StartsWith(folder)
                select theme.Substring(folder.Length);

            return resources.ToArray();
        }




        // Laad alle landen namen uit json resource bestand
        public void LoadJson()
        {
            // Convert van byte[] naar stream. Json format: "Code", "Name"
            MemoryStream stream = new MemoryStream(Properties.Resources.country_data_json);
            using (StreamReader r = new StreamReader(stream))
            {
                string json = r.ReadToEnd();
                List<Country> countries = JsonConvert.DeserializeObject<List<Country>>(json);
                foreach (Country country in countries)
                {
                    Console.WriteLine(country.Name);
                }

            }
        }

        public class Country
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public ImageSource Vlag { get; set; }
        }


    }

}
