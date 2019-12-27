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
    public class BSCategory
    {
        private static BSCategory instance;
        public static BSCategory Instance
        {
            get
            {
                if (instance == null)
                    instance = new BSCategory();
                return BSCategory.instance;
            }

            private set
            {
                BSCategory.instance = value;
            }
        }

        private BSCategory() { }

        public List<Category> GetListCategory()
        {
            List<Category> listCategory = new List<Category>();

            string query = "select * from FoodCategory";

            DataTable data = DBMain.Instance.ExcuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Category category = new Category(item);
                listCategory.Add(category);
            }

            return listCategory;
        }
        public Category GetCategoryByID(int id)
        {
            Category category = null;
            string query = "select * from dbo.FoodCategory where id =" +id;
            DataTable data = DBMain.Instance.ExcuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }
            return category;
        }
        public bool InsertCategory(string catName)
        {
            string query = string.Format("Insert dbo.FoodCategory( name) values (N'{0}')", catName);
            int result = DBMain.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool ModifyCategory(string catName, int catID)
        {
            string query = string.Format("Update dbo.FoodCategory set name = N'{0}' where id = {1}", catName,catID);
            int result = DBMain.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteCategory(int catID)
        {
            BSBillinfo.Instance.DeleteBillInfoByFoodID(catID);
            string query = string.Format("Delete dbo.FoodCategory where id = {0}", catID);
            int result = DBMain.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
    }
}
