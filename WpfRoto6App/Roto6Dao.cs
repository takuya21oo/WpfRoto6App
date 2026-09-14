using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace WpfRoto6App
{
    class Roto6Dao
    {
        private readonly string _connectionString = "server=localhost;user=root;password=root;database=roto6;";

        //===============================================================================
        // 表示用　検索処理
        //===============================================================================
        public List<Roto6InfoDto> DisplayGetRoto6Dao()
        {
            var roto6List = new List<Roto6InfoDto>();

            string sql = "SELECT * " +
                         "FROM ROTO6_INFO " +
                         "ORDER BY ROTO6_NO DESC ";

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var roto6InfoDto = new Roto6InfoDto
                            {
                                Roto6No = reader.GetInt32("ROTO6_NO"),
                                ReleaseDate = reader.GetDateTime("RELEASE_DATE"),
                                FirstNum = reader.GetString("FIRST_NUM"),
                                SecondNum = reader.GetString("SECOND_NUM"),
                                ThirdNum = reader.GetString("THIRD_NUM"),
                                FourthNum = reader.GetString("FOURTH_NUM"),
                                FifthNum = reader.GetString("FIFTH_NUM"),
                                SixthNum = reader.GetString("SIXTH_NUM"),
                                BonusNum = reader.GetString("BONUS_NUM"),
                                SearchNoBonusNum = reader.GetString("SEARCH_NO_BONUS_NUM"),
                                SearchNum = reader.GetString("SEARCH_NUM")
                            };
                            roto6List.Add(roto6InfoDto);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("エラーのため画面表示に失敗しました。", ex);
                }
            }

            return roto6List;

        }


        //===============================================================================
        // 登録用　登録処理
        //===============================================================================
        public bool InsertRoto6Dao(Roto6InfoDto roto6Info)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                string query = @"   INSERT INTO ROTO6.ROTO6_INFO (
                                    ROTO6_NO, RELEASE_DATE, FIRST_NUM, SECOND_NUM, THIRD_NUM,
                                    FOURTH_NUM, FIFTH_NUM, SIXTH_NUM, BONUS_NUM, SEARCH_NO_BONUS_NUM, SEARCH_NUM
                                    ) VALUES (
                                    @roto6No, @releaseDate, @firstNum, @secondNum, @thirdNum,
                                    @fourthNum, @fifthNum, @sixthNum, @bonusNum, @searchNoBonusNum, @searchNum
                                )";

                using var command = new MySqlCommand(query, connection, transaction);

                // VARCHAR(50) 型としてパラメータを追加
                command.Parameters.Add("@roto6No", MySqlDbType.VarChar, 50).Value = roto6Info.Roto6No;
                command.Parameters.Add("@releaseDate", MySqlDbType.Date).Value = roto6Info.ReleaseDate;
                command.Parameters.Add("@firstNum", MySqlDbType.VarChar, 50).Value = roto6Info.FirstNum;
                command.Parameters.Add("@secondNum", MySqlDbType.VarChar, 50).Value = roto6Info.SecondNum;
                command.Parameters.Add("@thirdNum", MySqlDbType.VarChar, 50).Value = roto6Info.ThirdNum;
                command.Parameters.Add("@fourthNum", MySqlDbType.VarChar, 50).Value = roto6Info.FourthNum;
                command.Parameters.Add("@fifthNum", MySqlDbType.VarChar, 50).Value = roto6Info.FifthNum;
                command.Parameters.Add("@sixthNum", MySqlDbType.VarChar, 50).Value = roto6Info.SixthNum;
                command.Parameters.Add("@bonusNum", MySqlDbType.VarChar, 50).Value = roto6Info.BonusNum;
                command.Parameters.Add("@searchNoBonusNum", MySqlDbType.VarChar, 50).Value = roto6Info.SearchNoBonusNum;
                command.Parameters.Add("@searchNum", MySqlDbType.VarChar, 50).Value = roto6Info.SearchNum;

                int result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("ROTO6_INFOの登録に失敗しました。", ex);
            }
        }


        //===============================================================================
        // 削除用　削除処理
        //===============================================================================
        public bool DeleteRoto6Dao(int roto6No)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                string query = @"DELETE FROM ROTO6_INFO WHERE ROTO6_NO = @roto6No";

                using var command = new MySqlCommand(query, connection, transaction);
                // VARCHAR(50) 型としてパラメータを追加
                command.Parameters.Add("@roto6No", MySqlDbType.Int32).Value = roto6No;
                //command.Parameters.Add("@roto6No", MySqlDbType.VarChar, 50).Value = roto6No;

                int result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("ROTO6_INFOの削除に失敗しました。", ex);
            }
        }
    }
}
