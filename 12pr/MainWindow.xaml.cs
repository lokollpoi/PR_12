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
    }
}

