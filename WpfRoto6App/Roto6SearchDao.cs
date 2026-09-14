using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace WpfRoto6App
{
    class Roto6SearchDao
    {
        private readonly string _connectionString = "server=localhost;user=root;password=root;database=roto6;";

        //===============================================================================
        // ロト６数値検索（検索用カラムで検索）
        //===============================================================================
        public bool GetRoto6Dao(string roto6Num)
        {
            string sql = "SELECT * FROM ROTO6_INFO WHERE SEARCH_NO_BONUS_NUM = @searchNoBonusNum";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    // パラメータ追加（部分一致検索）
                    cmd.Parameters.AddWithValue("@searchNoBonusNum", roto6Num);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        // 1件以上あれば true、なければ false
                        return reader.HasRows;
                    }
                }
            }
        }


        //===============================================================================
        // ロト６回別検索
        //===============================================================================
        public bool GetRoto6RoundDao(int roto6RoundNum)
        {
            string sql = "SELECT * FROM ROTO6_INFO WHERE ROTO6_NO = @searchRoto6RoundNum";

            using (var conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    // パラメータ追加（部分一致検索）
                    cmd.Parameters.AddWithValue("@searchRoto6RoundNum", roto6RoundNum);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        // 1件以上あれば true、なければ false
                        return reader.HasRows;
                    }
                }
            }
        }
    }
}
