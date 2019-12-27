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
    public class BSTable
    {
        private static BSTable instance;

        public static BSTable Instance
        {
            get
            {
                if (instance == null)
                    instance = new BSTable();
                return BSTable.instance;
            }

            private set
            {
                BSTable.instance = value;
            }
        }

        private BSTable() { }
        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();
            string query = "select * from dbo.CusTable";
            DataTable data = DBMain.Instance.ExcuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }
            return tableList; 
        }
        public Table GetTableByID(int id)
        {
            Table table = null;
            string query = "select * from dbo.CusTable where id =" + id;
            DataTable data = DBMain.Instance.ExcuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                table = new Table(item);
                return table;
            }
            return table;
        }
    }
}
