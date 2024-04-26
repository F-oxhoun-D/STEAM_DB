using System.Windows;
using System.Windows.Controls;

namespace STEAM_DB
{
    /// <summary>
    /// Логика взаимодействия для PersonalAccountWindow.xaml
    /// </summary>
    public partial class PersonalAccountWindow : Window
    {
        // список, хранящий страницы
        public static readonly List<Page> listPage = [];
        // индексы страниц
        public const int indxGame = 0;
        public const int indxPurchase = 1;
        public const int indxWishlist = 2;

        public PersonalAccountWindow()
        {
            InitializeComponent();
            Load();
        }
        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            // по нажатию кнопки обращяемся к конкретной странице из списка
            // делается для того, чтобы экономить память (не создавать каждый раз новый экземпляр без необходимости)
            PersonalAccountFrame.Content = listPage[indxGame];
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            PersonalAccountFrame.Content = listPage[indxPurchase];
        }

        private void WishlistButton_Click(object sender, RoutedEventArgs e)
        {
            PersonalAccountFrame.Content = listPage[indxWishlist];
        }

        private static void Load()
        {
            if (listPage.Count != 0)
                listPage.Clear();

            // добавляем в список каждую страницу
            listPage.Add(new PageGame());
            listPage.Add(new PagePurchase());
            listPage.Add(new PageWishlist());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Покинуть личный кабинет?", "", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Close();
                    break;
                default:
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Global.user = new();
        }
    }
}
