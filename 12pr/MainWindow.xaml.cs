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
using Microsoft.Win32;

namespace _12pr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Classes.Author> AllAuthors = Classes.Author.AllAuthors();

        List<Classes.Genre> AllGenres = Classes.Genre.AllGenres();

        List<Classes.Book> AllBooks = Classes.Book.AllBook();

        List<int> AllYears = new List<int>();

        public void AddAuthors()
        {
            cbAuthors.Items.Add("Все авторы");

            foreach (Classes.Author Author in AllAuthors)
                cbAuthors.Items.Add(Author.FIO);

            cbAuthors.SelectedIndex = 0;
        }


        public void AddGenres()
        {

            cbGenres.Items.Add("Все жанры");

            foreach (Classes.Genre Genre in AllGenres)
                cbGenres.Items.Add(Genre.Name);

            cbGenres.SelectedIndex = 0;
        }

        public void AddYears()
        {

            cbYear.Items.Add("Все годы");

            foreach (Classes.Book Book in AllBooks)
            {
 
                if (AllYears.Find(x => x == Book.Year) == 0)
                    AllYears.Add(Book.Year);
            }

            foreach (int Year in AllYears)
                cbYear.Items.Add(Year);

            cbYear.SelectedIndex = 0;
        }

        public MainWindow()
        {
            InitializeComponent();

            AddAuthors();

            AddGenres();

            AddYears();

            CreateUI(AllBooks);
        }

        public void CreateUI(List<Classes.Book> Books)
        {
            parent.Children.Clear();

            foreach (Classes.Book Book in Books)
                parent.Children.Add(new Elements.Element(Book));
        }

        private void Search_Book(object sender, KeyEventArgs e) =>
            Search();

        private void SelectAuthor(object sender, SelectionChangedEventArgs e) =>
            Search();

        public void Search()
        {
            List<Classes.Book> FindBook = AllBooks.FindAll(
                x => x.Name.ToLower().Contains(tbSearch.Text.ToLower())
            );

            if (cbAuthors.SelectedIndex > 0)
            {
                Classes.Author SelectAuthor = AllAuthors.Find(
                    x => x.FIO == cbAuthors.SelectedItem.ToString()
                );
                FindBook = FindBook.FindAll(
                    x => x.Authors.Find(y => y.Id == SelectAuthor.Id) != null
                );
            }

            if (cbGenres.SelectedIndex > 0)
            {
                Classes.Genre SelectGender = AllGenres.Find(
                    x => x.Name == cbGenres.SelectedItem.ToString()
                );

                FindBook = FindBook.FindAll(
                    x => x.Genres.Find(y => y.Id == SelectGender.Id) != null
                );
            }

            if (cbYear.SelectedIndex > 0)
                FindBook = FindBook.FindAll(
                    x => x.Year == Convert.ToInt32(cbYear.SelectedItem.ToString())
                );

            CreateUI(FindBook);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveBtn(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if(sfd.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    foreach (Classes.Book Book in AllBooks)
                    {
                        sw.WriteLine($"{Book.Id};{Book.Name};{Book.ToGenres()};{Book.ToAuthors()};{Book.Year}");
                    }
                }
            }
        }

        private void ImportBtn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                using (StreamReader sr = new StreamReader(ofd.FileName))
                {  
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(';');
                        int id = int.Parse(parts[0]);
                        string name = parts[1];
                        List<Classes.Genre> genres = new List<Classes.Genre>();
                        foreach (string genreName in parts[2].Split(','))
                        {
                            Classes.Genre genre = AllGenres.Find(x => x.Name == genreName.Trim());
                            if (genre != null)
                                genres.Add(genre);
                        }
                        List<Classes.Author> authors = new List<Classes.Author>();
                        foreach (string authorName in parts[3].Split(','))
                        {
                            Classes.Author author = AllAuthors.Find(x => x.FIO == authorName.Trim());
                            if (author != null)
                                authors.Add(author);
                        }
                        int year = int.Parse(parts[4]);
                        AllBooks.Add(new Classes.Book(id, name, genres, authors, year));
                    }
                }
                CreateUI(AllBooks);
            }

        }
    }
}

