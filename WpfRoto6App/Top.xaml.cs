using System.Reflection.Emit;
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

namespace WpfRoto6App
{
    /// <summary>
    /// Interaction logic for Top.xaml
    /// </summary>
    public partial class Top : Window
    {
        public Top()
        {
            InitializeComponent();

            // 起動時に背景色を変更
            this.Background = Brushes.LightBlue;
        }


        //===============================================================================
        /// <summary>
        /// STARTボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void Start_Botton(object sender, RoutedEventArgs e)
        {
            // メインメニューへの遷移ボタン
            // Page2へ遷移
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Close();
        }


        //===============================================================================
        /// <summary>
        /// ENDボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void End_Botton(object sender, RoutedEventArgs e)
        {
            // システム終了ボタン
            Close();
        }
    }
}