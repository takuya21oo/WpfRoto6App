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

namespace WpfRoto6App
{
    /// <summary>
    /// MainMenu.xaml の相互作用ロジック
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();

            // 起動時に背景色を変更
            this.Background = Brushes.LightBlue;
        }


        //===============================================================================
        /// <summary>
        /// Roto6管理画面ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void Roto6_Control_Botton(object sender, RoutedEventArgs e)
        {
            // Roto6管理画面への遷移ボタン
            // Page2へ遷移
            Roto6Control roto6Control = new Roto6Control();
            roto6Control.Show();
            this.Close();
        }


        //===============================================================================
        /// <summary>
        /// 予想番号作成画面ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void Forecast_Number_Botton(object sender, RoutedEventArgs e)
        {
            // 予想番号作成画面への遷移ボタン
            // Page2へ遷移
            ForecastNumber forecastNumber = new ForecastNumber();
            forecastNumber.Show();
            this.Close();
        }


        //===============================================================================
        /// <summary>
        /// TOP画面へ戻るボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void Top_Return_Botton(object sender, RoutedEventArgs e)
        {
            // Topへの遷移ボタン
            // Page2へ遷移
            Top topReturn = new Top();
            topReturn.Show();
            this.Close();
        }
    }
}
