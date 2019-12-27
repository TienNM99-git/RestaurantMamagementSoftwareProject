using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyQuanAn
{
    public class Bill
    {
        private int id;
        public int Id
        {
            get{ return id; }
            set {id = value; }
        }

        private DateTime? dateCheckIn; //allow null
        public DateTime? DateCheckIn
        {
            get { return dateCheckIn; }
            set { dateCheckIn = value; }
        }

        private DateTime? dateCheckOut;

        public DateTime? DateCheckOut
        {
            get { return dateCheckOut; }
            set { dateCheckOut = value; }
        }

        private int stat;
        public int Stat
        {
            get { return stat; }
            set { stat = value; }
        }
        
        public Bill(int id, DateTime? dateCheckIn, DateTime? dateCheckOut, int stat)
        {
            this.Id = id;
            this.DateCheckIn = dateCheckIn;
            this.DateCheckOut = dateCheckOut;
            this.Stat = stat;
        }
        
        public Bill(DataRow row)
        {
            this.Id = (int)row["id"];
            this.DateCheckIn = (DateTime?)row["dateCheckIn"];

            var dateCheckOutTemp = row["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;
            this.Stat = (int)row["stat"];
        }    
    }
}
