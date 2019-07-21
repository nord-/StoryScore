using System;
using System.Data.SQLite;
using System.IO;

namespace StoryScoreClient.Data
{
    public class SqLiteBaseRepository
    {
        public static string DbFile
        {
            get
            {
                var dbfile = Environment.CurrentDirectory + "\\StoryScoreData.sqlite";
                if (!File.Exists(dbfile))
                    //throw new FileNotFoundException("Database file not found.", dbfile);
                    SqLiteDatabase.Create(dbfile);

                return dbfile;
            }
        }

        public static SQLiteConnection StoryScoreDbConnection()
        {
            var cn = new SQLiteConnection("Data Source=" + DbFile);
            cn.Open();
            return cn;
        }
    }
}
