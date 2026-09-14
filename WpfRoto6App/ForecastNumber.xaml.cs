using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfRoto6App
{
    /// <summary>
    /// ForecastNumber.xaml の相互作用ロジック
    /// </summary>
    public partial class ForecastNumber : Window
    {
        public ObservableCollection<NewRoto6Info> CreateRotoNumber { get; set; }

        public ForecastNumber()
        {
            InitializeComponent();

            // 起動時に背景色を変更
            this.Background = Brushes.LightBlue;

            CreateRotoNumber = new ObservableCollection<NewRoto6Info>();
            DataContext = this;
        }

        //===============================================================================
        /// <summary>
        /// 作成（予想番号作成）ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //------------------------------------------------------------------------
            // 処理の流れ
            // ①１から６の本数字の作成
            // ②数字作成後、DB接続
            // ③過去に同じ数字が作成されていないか検索（SEARCH_NO_BONUS_NUMカラムを検索でOK）
            // 　※現状ボーナス数字を含めて、同じ数字は作成されていないが、1等は本数字のみ全て一致なのでSEARCH_NO_BONUS_NUMカラムでOK
            // ※不要処理　④ボーナス数字の作成
            // ※不要処理　⑤ボーナス数字が本数字内に存在しているかどうかの確認
            // ⑥存在しなければ、数字を表示。
            // 　存在した場合、再度④に戻る
            // 　→確認？？
            //------------------------------------------------------------------------

            //------------------------------------------------------------------------
            //  ① No.１～No.６の本数字の作成
            //------------------------------------------------------------------------
            //変数宣言
            bool rotoCheck = true;
            
            while (rotoCheck)
            {
                //配列・Listの宣言
                Random rand = new Random();
                int[] roto = new int[6];
                string[] result = new string[6];
                string searchNum = "";
                bool dbResult = true;
                bool errorFlag = false;

                // 本数字処理
                for (int i = 0; i < roto.Length; i++)
                {
                    int num = rand.Next(1,44);
                    roto[i] = num;
                }

                //配列内の重複を検知し、重複していたらやり直し
                var set = new HashSet<int>(); //重複確認用
                foreach (int duplicateCheck in roto)
                {
                    if (!set.Add(duplicateCheck))
                    {
                        errorFlag = true;
                    }
                }

                if (errorFlag == false) 
                {
                    //配列内を昇順にソートする
                    //roto.Length = 6
                    for (int n = 0; n < roto.Length - 1; n++)
                    {
                        for (int j = roto.Length - 1; j > n; j--)
                        {
                            if (roto[j - 1] > roto[j])
                            {
                                int box = roto[j];
                                roto[j] = roto[j - 1];
                                roto[j - 1] = box;
                            }
                        }
                    }

                    //一桁の数字の前に0を追加
                    //数字を文字列に変換する		
                    for (int i = 0; i < roto.Length; i++)
                    {
                        if (roto[i] < 10)
                        {
                            result[i] = '0' + Convert.ToString(roto[i]);
                        }
                        else
                        {
                            result[i] = Convert.ToString(roto[i]);
                        }
                    }

                    //List<string> list = new List<string>(result);

                    //検索用変数に格納
                    searchNum = result[0] + result[1] + result[2] + result[3] + result[4] + result[5];

                    //------------------------------------------------------------------------
                    //  ②数字作成後、DB接続
                    //  ③過去に同じ数字が作成されていないか検索（SEARCH_NO_BONUS_NUMカラムを検索でOK）
                    //------------------------------------------------------------------------
                    Roto6SearchDao dao = new Roto6SearchDao();
                    dbResult = dao.GetRoto6Dao(searchNum);

                    if (dbResult == false)
                    {
                        CreateRotoNumber.Add(new NewRoto6Info
                        {
                            FirstNum = result[0],
                            SecondNum = result[1],
                            ThirdNum = result[2],
                            FourthNum = result[3],
                            FifthNum = result[4],
                            SixthNum = result[5]
                        });

                        // 抜け出し用のフラグをfalseにする
                        rotoCheck = false;
                    }
                    else 
                    {
                        // 処理なし（ループをしなおす）
                    }

                }
            }
        }


        //===============================================================================
        /// <summary>
        /// 連番表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }


        //===============================================================================
        /// <summary>
        /// MainMenu画面へ戻るボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void Mainmenu_Return_Botton(object sender, RoutedEventArgs e)
        {
            // メインメニューへの遷移ボタン
            // Page2へ遷移
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Close();
        }
    }
}
