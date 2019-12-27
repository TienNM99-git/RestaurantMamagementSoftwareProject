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
    public class BSAccount
    {
        private static BSAccount instance;

        public static BSAccount Instance
        {
            get
            {
                if (instance == null)
                    instance = new BSAccount();
                return BSAccount.instance;
            }

            private set
            {
                BSAccount.instance = value;
            }
        }

        private BSAccount() { }

        public bool Login(string userName, string passWord)
        {
            string query = "UPS_Login @userName , @passWord";

            DataTable result = DBMain.Instance.ExcuteQuery(query, new object[] { userName, passWord });

            return result.Rows.Count > 0;
        }
    }
}

