using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyQuanAn
{
    public class BillInfo
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int billID;
        public int BillID
        {
            get { return billID; }
            set { billID = value; }
        }

        private int foodID;
        public int FoodID
        {
            get { return foodID; }
            set { foodID = value; }
        }
        private int foodCount;
        public int FoodCount
        {
            get { return foodCount; }
            set { foodCount = value; }
        }

        public BillInfo(int id, int billID, int foodID, int foodCount)
        {
            this.Id = id;
            this.BillID = billID;
            this.FoodID = foodID;
            this.FoodCount = foodCount;
        }

        public BillInfo(DataRow row)
        {
            this.Id = (int)row["id"];
            this.BillID = (int)row["idBill"];
            this.FoodID = (int)row["idFood"];
            this.FoodCount = (int)row["count"];
        }
       
    }
}
