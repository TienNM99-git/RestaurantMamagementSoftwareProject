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
    public class BSMenu
    {
        private static BSMenu instance;

        public static BSMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new BSMenu();
                return BSMenu.instance;
            }
            private set
            {
                BSMenu.instance = value;
            }
        }
        
        private BSMenu() { }

        public List<Menu> GetListMenuInfo(int id)
        {
            List<Menu> listMenu = new List<Menu>();

            string query = "SELECT f.name, bi.count, f.Price, f.Price * bi.count AS Total FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.stat = 0 AND b.idTable = " + id;
            DataTable data = DBMain.Instance.ExcuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }

    }
}
