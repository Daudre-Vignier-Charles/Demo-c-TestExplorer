using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestExplorer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum FileType { File, Directory };

        /// <summary>
        /// Un objet FileUI représente un ligne dans la DataGrid
        /// Chaque propriété représente une colonne, le titre des colonnes c'est le nom des propriété
        /// </summary>
        public class FileUI
        {
            public string filename { get; set; }
            public string path { get; set; }

            public FileType type { get; set; }
        }

        /// <summary>
        /// Une liste d'objet FileUI, la liste représente toutes les lignes de la DataGrid
        /// </summary>
        private ObservableCollection<FileUI> fileList = new ObservableCollection<FileUI>();

        public MainWindow()
        {
            InitializeComponent();
            // Je dis que la source de ma DataGrid c'est ma liste d'objet FileUI
            UIExplorer.ItemsSource = fileList;
            currentPath.Text = Directory.GetCurrentDirectory();
            Update(currentPath.Text);
        }

        public void Update(string npath)
        {
            Directory.SetCurrentDirectory(npath);
            fileList.Clear();
            foreach (string file in Directory.GetDirectories(npath))
                fileList.Add(new FileUI() { filename = System.IO.Path.GetFileName(file), path = file, type = FileType.Directory });
            foreach (string file in Directory.GetFiles(npath))
                fileList.Add(new FileUI() { filename = System.IO.Path.GetFileName(file), path = file, type = FileType.File });
        }

        private void currenPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Update(currentPath.Text);
        }
    }
}
