using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScoreClient.Data
{
    public class SqLiteDatabase
    {
        public static void Create(string dbfile)
        {
            SQLiteConnection.CreateFile(dbfile);
            using (var cn = new SQLiteConnection("Data Source=" + dbfile))
            {
                cn.Open();
                CreateTable(cn);
                cn.Close();
            }
        }

        private static void CreateTable(SQLiteConnection cn)
        {
            var sql = @"CREATE TABLE Teams (Id INTEGER PRIMARY KEY, Name VARCHAR(200), Coach VARCHAR(200), ShortName VARCHAR(200), LogoPath VARCHAR(200)); 
                        CREATE TABLE Players (Id INTEGER PRIMARY KEY, Name VARCHAR(200), PlayerNumber INTEGER, Position VARCHAR(40), PicturePath VARCHAR(200), PresentationVideoPath VARCHAR(200), 
                                              GoalVideoPath VARCHAR(200), TeamId INTEGER, FOREIGN KEY(TeamId) REFERENCES Teams(Id));";

            var cmd = new SQLiteCommand(sql, cn);
            cmd.ExecuteNonQuery();
        }
    }
}
