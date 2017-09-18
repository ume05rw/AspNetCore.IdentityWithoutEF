using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthNoneEf.Models
{
    public class DbProvider : AppModel
    {
        private const string SqliteFileName = "authNoneEf.sqlite3";

        private static readonly string DbPath = System.IO.Path.Combine(
            System.IO.Directory.GetCurrentDirectory(), 
            DbProvider.SqliteFileName
        );

        private static Xb.Db.Sqlite _db;

        public static Xb.Db.Sqlite Db
        {
            get
            {
                if (DbProvider._db != null)
                    return DbProvider._db;

                if (!System.IO.File.Exists(DbProvider.DbPath))
                    DbProvider.CreateDb();

                DbProvider._db = new Xb.Db.Sqlite(DbProvider.DbPath);
                return DbProvider._db;
            }
        }

        private static void CreateDb()
        {
            var tableModels = new AppDbModel[]
            {
                (new AuthUserStore())
            };
            var db = new Xb.Db.Sqlite(DbProvider.DbPath);

            foreach (var model in tableModels)
                model.FormatDb(db);

            db.Dispose();
        }
    }
}
