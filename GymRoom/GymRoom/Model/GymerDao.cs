using GymRoom.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRoom.Model
{
    public class GymerDao
    {
        GymDbContext db = null;
        public GymerDao()
        {
            db = new GymDbContext();
        }

        public List<GYMER> GetListGymer()
        {
            return db.GYMERs.OrderBy(x=>x.dateExpired).ToList();
        }
        
        public bool updateGymer(long idG,string name, string adress, string note)
        {
            object[] param =
            {
                new SqlParameter("@id",idG),
                new  SqlParameter("@name",name),
                new  SqlParameter("@modi",DateTime.Now),
                new  SqlParameter("@adress",adress),
                new  SqlParameter("@note",note)
                 

            };
            var rows = db.Database.ExecuteSqlCommand("updateGymer @id,@name,@modi,@adress,@note", param);
            db = new GymDbContext();
            return rows == 0 ? false : true;
        }
        public bool extendGymer(long idG,DateTime dateRegistraion,DateTime dateExpired,int numMonth)
        {

            object[] param =
            {
                new SqlParameter("@id",idG),
                new  SqlParameter("@dateR",dateRegistraion),
                new  SqlParameter("@dateE",dateExpired),
                 new  SqlParameter("@month",numMonth),
                 new  SqlParameter("@modi",DateTime.Now),
            };
            var rows = db.Database.ExecuteSqlCommand("extendGymer @id,@dateR,@dateE,@month,@modi",param);
            db = new GymDbContext();
            return rows == 0 ? false : true;
        }
        public List<GYMER> findGymer(string s)
        {
            return db.GYMERs.SqlQuery("findGymer N'"+s+"'").OrderBy(x=>x.dateExpired).ToList();
        }

        public bool deleteGymer(long idG)
        {
          
          var rows=  db.Database.ExecuteSqlCommand("deleteGymer "+idG);
            return rows == 0 ? false : true;
        }
        public bool addGymer(GYMER gymer)
        {
           GYMER check = db.GYMERs.Add(gymer);
            db.SaveChanges();
            return check==null ? false:true;
        }
    }
}
