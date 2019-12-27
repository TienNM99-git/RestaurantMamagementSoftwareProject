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
    public class BSFood
    {
        private static BSFood instance;
        public static BSFood Instance
        {
            get
            {
                if (instance == null)
                    instance = new BSFood();
                return BSFood.instance;
            }

            private set
            {
                BSFood.instance = value;
            }
        }

        private BSFood() { }

        public List<Food> GetFood(int id)
        {
            List<Food> listFood = new List<Food>();

            string query = "select * from Food where idCategory = " + id;

            DataTable data = DBMain.Instance.ExcuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }

            return listFood;
        }
        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();
            string query = "select * from dbo.Food";
            DataTable data = DBMain.Instance.ExcuteQuery(query);
            foreach(DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
        public bool InsertFood(string fName, int fID, float fPrice)
        {
            string query = string.Format("Insert dbo.Food( name, idCategory, Price) values (N'{0}',{1},{2})", fName, fID, fPrice);
            int result = DBMain.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool ModifyFood (string fName, int fID, int catID,float fPrice)
        {
            string query = string.Format("Update dbo.Food set name = N'{0}', idCategory = {1}, price = {2} where id = {3}", fName, catID, fPrice, fID);
            int result = DBMain.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteFood(int fID)
        {
            BSBillinfo.Instance.DeleteBillInfoByFoodID(fID);
            string query = string.Format("Delete dbo.Food where id = {0}", fID);
            int result = DBMain.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public List<Food> searchFood(string fName)
        {
            List<Food> list = new List<Food>();
            //fName = convertToUnSign(fName);
            string query = string.Format("select * from dbo.Food where dbo.NormalizeString(name) like N'%'+dbo.NormalizeString(N'{0}')+'%'", fName);
            DataTable data = DBMain.Instance.ExcuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
        //public string convertToUnSign(string s)
        //{
        //    string stFormD = s.Normalize(NormalizationForm.FormD);
        //    StringBuilder sb = new StringBuilder();
        //    for (int ich = 0; ich < stFormD.Length; ich++)
        //    {
        //        System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
        //        if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
        //        {
        //            sb.Append(stFormD[ich]);
        //        }
        //    }
        //    sb = sb.Replace('Đ', 'D');
        //    sb = sb.Replace('đ', 'd');
        //    return (sb.ToString().Normalize(NormalizationForm.FormD));
        //}
    }
}
