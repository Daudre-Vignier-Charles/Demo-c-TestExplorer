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
using System.Drawing;
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
            public string filename { get; set; } // Pour lier avec une DataGrid, le get et set est obligatoire !
            public string path { get; set; }
            public FileType type { get; set; }
            public ImageSource icon { get; set; }

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

        /// <summary>
        /// Mets à jour la DataGrid
        /// </summary>
        /// <param name="npath"></param>
        public void Update(string npath)
        {
            Directory.SetCurrentDirectory(npath);
            fileList.Clear();
            // récupération des fichiers
            foreach (string file in Directory.GetDirectories(npath))
                fileList.Add(new FileUI() {
                    filename = System.IO.Path.GetFileName(file),
                    path = file,
                    type = FileType.Directory,
                });
            // récupération des dossiers
            foreach (string file in Directory.GetFiles(npath))
                fileList.Add(
                    new FileUI()
                    {
                        filename = System.IO.Path.GetFileName(file),
                        path = file,
                        type = FileType.File,
                        icon = System.Drawing.Icon.ExtractAssociatedIcon(file).ToImageSource()
                    }); ; ;
        }


        /// <summary>
        /// se déclenche quand on tappe dans la TextBox en haut,
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void currenPath_KeyDown(object sender, KeyEventArgs e)
        {
            // Vérifie si la touche est la touche "Entrée", si oui, mets à jour la table
            if (e.Key == Key.Enter)
                Update(currentPath.Text);
        }
    }

    /// <summary>
    /// Transforme l'icone d'un fichier en une image utilisable en WPF, si tu veux la récupérer,
    /// copie le code tel quel
    /// </summary>
    internal static class IconUtilities
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ToImageSource(this Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }

            return wpfBitmap;
        }
    }
}
