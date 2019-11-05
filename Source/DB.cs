using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace RateSpielGeografie
{
    class DB
    {
        private MySqlConnection sql;

        public DB()
        {
            sql = new MySqlConnection("server=localhost;uid=root;password=root;database=laender");
        }

        public void HScore(Score sc)
        {
            MySqlCommand comm = sql.CreateCommand();
            comm.CommandText = "INSERT INTO laender.highscore VALUES(NULL, \"" + sc.PNAME + "\"," + sc.PSCORE + ");";
            sql.Open();
            comm.ExecuteNonQuery();
            sql.Close();
        }

        public List<Score> Scores()
        {
            List<Score> LiSc = new List<Score>();

            MySqlCommand comm = sql.CreateCommand();
            MySqlDataReader DR;
            comm.CommandText = "SELECT * FROM laender.highscore ORDER BY highscoreplayerscore DESC;";
            sql.Open();
            DR = comm.ExecuteReader(); ;

            while (DR.Read())
            {
                LiSc.Add(new Score(DR.GetString(1), DR.GetInt32(2)));
            }
            DR.Close();
            sql.Close();

            return LiSc;
        }

        public List<Land> Lander()
        {
            List<Land> LiLa = new List<Land>();

            MySqlCommand comm = sql.CreateCommand();
            MySqlDataReader DR;
            comm.CommandText = "SELECT * FROM laender.laender;";
            sql.Open();
            DR = comm.ExecuteReader(); ;

            while(DR.Read())
            {
                LiLa.Add(new Land(DR.GetString(1), DR.GetString(2)));
            }
            DR.Close();
            sql.Close();

            return LiLa;
        }
    }
}
