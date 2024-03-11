using System.Windows;

namespace STEAM_DB
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            string sql = textBoxSQLQuery.Text;

            try // в данном блоке выполняется какая-либо инструкция(метод)
            {
                queryGrid.ItemsSource =  ProcessRequest.SQLQuery(sql);
            }
            catch (Exception ex) // если в блоке try возникает ошибка или исключение, то программа ищет блок catch
            {   // набор инструкций при ошибке
                MessageBox.Show(ex.Message); // в данном случаем просто вывод сообщения с данными об ошибке из-за некоректного запроса
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxSQLQuery.Clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Global.user = new ();
        }
    }
}
