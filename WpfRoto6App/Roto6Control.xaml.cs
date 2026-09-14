using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfRoto6App
{
    /// <summary>
    /// Roto6Control.xaml の相互作用ロジック
    /// </summary>
    public partial class Roto6Control : Window
    {
        private Roto6ViewModel _viewModel;

        public Roto6Control()
        {
            InitializeComponent();             // 初期化

            // 起動時に背景色を変更
            this.Background = Brushes.LightBlue;

            _viewModel = new Roto6ViewModel(); // ViewModelを作成
            DataContext = _viewModel;          // バインド
            DataObject.AddPastingHandler(txtRound, OnPaste);
            DataObject.AddPastingHandler(dpDate, OnPaste);
            DataObject.AddPastingHandler(txtNum1, OnPaste);
            DataObject.AddPastingHandler(txtNum2, OnPaste);
            DataObject.AddPastingHandler(txtNum3, OnPaste);
            DataObject.AddPastingHandler(txtNum4, OnPaste);
            DataObject.AddPastingHandler(txtNum5, OnPaste);
            DataObject.AddPastingHandler(txtNum6, OnPaste);
            DataObject.AddPastingHandler(txtBonus, OnPaste);
        }


        //===============================================================================
        /// <summary>
        /// クリアボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            txtRound.Text = string.Empty;
            dpDate.SelectedDate = null;
            txtNum1.Text = string.Empty;
            txtNum2.Text = string.Empty;
            txtNum3.Text = string.Empty;
            txtNum4.Text = string.Empty;
            txtNum5.Text = string.Empty;
            txtNum6.Text = string.Empty;
            txtBonus.Text = string.Empty;
        }


        //===============================================================================
        // ※XAMLでCommandバインディングしているので以下不要
        /// <summary>
        /// 表示ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        //private void DisplayButton_Click(object sender, RoutedEventArgs e)
        //{
        //    _viewModel.LoadRoto6(); // ← ViewModelの検索処理を呼び出す
        //}


        //===============================================================================
        /// <summary>
        /// 登録ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            int numRound;
            if (int.TryParse(txtRound.Text, out numRound))
            {
                //変換成功 → numRound に値が入る
            }
            else 
            {
                // 空白チェック（空白の場合、int.TryParse = 0となる）
                if (numRound == 0)
                {
                    MessageBox.Show($"回別に半角の数字を入力してください。");
                    return;
                }
                else 
                {
                    // 数値チェック
                    // 変換失敗 → 入力が数値でない
                    MessageBox.Show($"回別に半角の数字を入力してください。");
                    return;
                }
            }
                
            var num1 = txtNum1.Text;
            var num2 = txtNum2.Text;
            var num3 = txtNum3.Text;
            var num4 = txtNum4.Text;
            var num5 = txtNum5.Text;
            var num6 = txtNum6.Text;
            var num7 = txtBonus.Text;

            // 配列にまとめる
            var nums = new[] { num1, num2, num3, num4, num5, num6, num7 };

            string searchNum = "";
            bool dbResult = true;
            bool result = true;

            //--------------------------------------------
            // nullチェック
            //--------------------------------------------
            // 日付
            if (!dpDate.SelectedDate.HasValue)
            {
                MessageBox.Show("空白の項目があります。");
                return;
            }

            for (int i = 0; i < nums.Length; i++)
            {
                // その他項目
                if (string.IsNullOrWhiteSpace(nums[i]))
                {
                    MessageBox.Show("空白の項目があります。");
                    return;
                }
            }

            //--------------------------------------------
            // 数値チェック
            // ※リアルタイム制御だけでは、
            // 　途中の数値以外項目を制御できなかったため
            //--------------------------------------------
            for (int i = 0; i < nums.Length; i++)
            {
                if (!int.TryParse(nums[i], out int number))
                {
                    MessageBox.Show($"半角の数字を入力してください。");
                    return;
                }
            }

            //--------------------------------------------
            // 日付チェック（入力制御しているので、あまり引っ掛からないかも）
            //--------------------------------------------
            if (!DateTime.TryParse(dpDate.Text, out DateTime parsedDate))
            {
                MessageBox.Show("有効な日付を入力してください。");
            }

            //--------------------------------------------
            // 桁数チェック
            // 　回別：桁数なし
            // 　数値1～6：2桁
            // 　ボーナス数値：2桁
            //--------------------------------------------
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i].Length > 2)
                {
                    MessageBox.Show("本数字とボーナス数字は、\r\n２桁以内で４３以下の数字を入力してください。");
                    return;
                }
            }

            //--------------------------------------------
            // 範囲制限チェック
            // 　本数字・ボーナス数字：1～43
            // 　44以上の数字が無いかをチェック
            //--------------------------------------------
            for (int i = 0; i < nums.Length; i++)
            {
                // 数値変換
                if (int.TryParse(nums[i], out int number))
                {
                    // 44以上の数値の場合
                    if (number >= 44)
                    {
                        MessageBox.Show("本数字とボーナス数字は、\r\n１～４３の数字を入力してください。");
                        return;
                    }
                }
            }

            //--------------------------------------------
            // 一桁の場合、0を10の位に入れる　
            //--------------------------------------------
            for (int i = 0; i < nums.Length; i++)
            {
                // 数値変換
                if (int.TryParse(nums[i], out int number))
                {
                    // 10以下の数値の場合
                    if (number < 10)
                    {
                        nums[i] = '0' + Convert.ToString(number);
                    }
                }
            }

            //--------------------------------------------
            // 重複チェック（回別）
            //--------------------------------------------
            //  DB接続
            //  存在している回別か検索
            Roto6SearchDao dao = new Roto6SearchDao();
            dbResult = dao.GetRoto6RoundDao(numRound);
            
            if (dbResult == true)
            {
                MessageBox.Show("既に存在している回別のため登録できません。");
                return;
            }
            
            //--------------------------------------------
            // 重複チェック（数値）
            //--------------------------------------------
            //検索用変数に格納
            searchNum = nums[0] + nums[1] + nums[2] + nums[3] + nums[4] + nums[5];

            //  数字作成後、DB接続
            //  過去に同じ数字が作成されていないか検索（SEARCH_NO_BONUS_NUMカラムを検索でOK）
            dbResult = dao.GetRoto6Dao(searchNum);

            if (dbResult == true)
            {
                MessageBox.Show("既に存在しているパターンのため登録できません。");
                return;
            }
            
            //--------------------------------------------
            // 登録処理
            //--------------------------------------------
            var dto = new Roto6InfoDto
            {
                Roto6No = numRound,
                ReleaseDate = DateTime.Parse(dpDate.Text),
                FirstNum = nums[0],
                SecondNum = nums[1],
                ThirdNum = nums[2],
                FourthNum = nums[3],
                FifthNum = nums[4],
                SixthNum = nums[5],
                BonusNum = nums[6],
                SearchNoBonusNum = nums[0] + nums[1] + nums[2] + nums[3] + nums[4] + nums[5],
                SearchNum = nums[0] + nums[1] + nums[2] + nums[3] + nums[4] + nums[5] + nums[6],
            };

            result = _viewModel.RegisterItemFromDatabase(dto);

            if (result == true)
            {
                MessageBox.Show("登録しました。");
            }
            else
            {
                MessageBox.Show("登録に失敗しました。");
            }
        }


        //===============================================================================
        /// <summary>
        /// 削除ボタン押下
        /// </summary>
        /// sender は「どのコントロールが押されたか」を表す引数
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = true;

            // sender を Button 型にキャストします。
            var button = sender as Button;
            // その Button の Tag プロパティには、DataGridの行にバインドされた1件の DTO（Roto6InfoDto）が入っているはず。
            // DTO が null なら何もせず終了
            var dto = button.Tag as Roto6InfoDto;
            if (dto == null) return;

            // 🔔 メッセージボックスで確認を取る
            var mbResult = MessageBox.Show(
                "このデータを削除してもよろしいですか？",
                "削除確認",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (mbResult != MessageBoxResult.Yes)
            {
                return; // No または閉じる → 削除しない
            }

            result = _viewModel.DeleteItemFromDatabase(dto);

            if (result == true)
            {
                MessageBox.Show("削除しました。");
            }
            else
            {
                MessageBox.Show("削除に失敗しました。");
            }
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


        //===============================================================================
        /// <summary>
        /// 手入力でのリアルタイム数値制御（最初の1文字しか制御できていない？）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }


        //===============================================================================
        /// <summary>
        /// ペーストでのリアルタイム数値制御
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pasteText = (string)e.DataObject.GetData(typeof(string));
                if (!Regex.IsMatch(pasteText, "^[0-9]+$"))
                {
                    e.CancelCommand();
                }
            }
        }


        //===============================================================================
        /// <summary>
        /// 自動的にカレンダーを開くことで、ユーザーに選択を促すことができます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void dpDate_GotFocus(object sender, RoutedEventArgs e)
        {
            dpDate.IsDropDownOpen = true;
        }


        //===============================================================================
        /// <summary>
        /// 日付リアルタイム制御
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //===============================================================================
        private void dpDate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 数字とスラッシュのみ許可（例: 2025/08/23）
            e.Handled = !Regex.IsMatch(e.Text, @"[\d/]");
        }
    }
}
