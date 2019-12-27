using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace QuanLyQuanAn
{
    public class Table
    {
        public Table(int id, string name, string stat)
        {
            this.ID = id;
            this.Name = name;
            this.Stat = stat;
        }

        public Table(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Stat = row["stat"].ToString();
        }

        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private string name;
        public string Name
        {
            get{ return name; }
            set{ name = value;}
        }

        private string stat;
        public string Stat
        {
            get { return stat; }
            set { stat = value; }
        }

        public static int TableWidth = 60;
        public static int TableHeight = 60;
    }
}
