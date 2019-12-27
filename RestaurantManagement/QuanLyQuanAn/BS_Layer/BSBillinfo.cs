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
    public class BSBillinfo
    {
        private static BSBillinfo instance;

        public static BSBillinfo Instance
        {
            get
            {
                if (instance == null)
                    instance = new BSBillinfo();
                return BSBillinfo.instance;
            }
            private set
            {
                BSBillinfo.instance = value;
            }
        }

        private BSBillinfo() { }

        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            DataTable data = DBMain.Instance.ExcuteQuery("SELECT * FROM dbo.BillInfo WHERE idBill = " + id);

            foreach(DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DBMain.Instance.ExcuteNonQuery("USP_InserBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
        public void DeleteBillInfoByFoodID(int id)
        {
            DBMain.Instance.ExcuteQuery("Delete FROM dbo.BillInfo WHERE idFood = " + id);
        }
    }
}
