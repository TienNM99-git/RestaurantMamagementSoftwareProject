using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using QuanLyQuanAn.DB_Layer;

namespace QuanLyQuanAn.BS_Layer
{
    public class BSBill
    {
        private static BSBill instance;

        public static BSBill Instance
        {
            get
            {
                if (instance == null)
                    instance = new BSBill();
                return BSBill.instance;
            }
            private set
            {
                BSBill.instance = value;
            }
        }

        public BSBill() { }

        /// <summary>
        /// Success: bill.ID
        /// Fail: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>


        public int GetUncheckBillID(int id)
        {
            DataTable data = DBMain.Instance.ExcuteQuery("Select * from dbo.Bill where idTable = " + id + "and stat = 0");

            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id;
            }

            return -1;
        }
        public void CheckOut(int id, float totalPrice)
        {
            string query = "update dbo.Bill Set DateCheckOut = GetDate() , stat = 1, "+ " totalPrice = "+ totalPrice + " Where id = "+id;
            DBMain.Instance.ExcuteNonQuery(query);

        }
        public void InsertBill(int id)
        {
            DBMain.Instance.ExcuteNonQuery("exec USP_InserBill @idTable", new object[] { id });
        }
        public DataTable GetBillListByDate(DateTime checkIn,DateTime checkOut)
        {
            return DBMain.Instance.ExcuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[]{checkIn,checkOut});

        }
        public DataTable GetTopBill()
        {
            DataTable topBill = new DataTable();
            string query = "select top(5) id,idTable,totalPrice from dbo.Bill order by totalPrice desc";
            topBill = DBMain.Instance.ExcuteQuery(query);
            return topBill;
        }
        public int GetMaxIdBill()
        {
            try
            {
                return (int)DBMain.Instance.ExcuteScalar("Select MAX(id) from dbo.Bill");
            }
            catch
            {
                return 1;
            }
        }
    }
}
