using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

using QuanLyQuanAn.BS_Layer;

namespace QuanLyQuanAn
{
    public partial class fCusTable : Form
    {
        public fCusTable()
        {
            InitializeComponent();

            LoadTable();

            LoadCategory();
        }

        #region Method
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = BSTable.Instance.LoadTableList();
            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = Table.TableWidth, Height = Table.TableHeight};
                btn.Text = item.Name + Environment.NewLine + item.Stat;
                btn.Click += Btn_Click;
                btn.Tag = item;

                if (item.Stat == "Empty")
                {
                    btn.BackColor = Color.Green;
                }
                else
                    btn.BackColor = Color.Red;

                flpTable.Controls.Add(btn);
            }
        }

        void LoadCategory()
        {
            List<Category> listCategory = BSCategory.Instance.GetListCategory();
            cmbCategory.DataSource = listCategory;
            cmbCategory.DisplayMember = "name";
        }

        void LoadListFood(int id)
        {
            List<Food> listFood = BSFood.Instance.GetFood(id);
            cmbFood.DataSource = listFood;
            cmbFood.DisplayMember = "name";
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<Menu> listBillInfo = BSMenu.Instance.GetListMenuInfo(id);
            float totalPrice = 0;
            foreach(Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.FoodCount.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");

            Thread.CurrentThread.CurrentCulture = culture;

            txtTotalPrice.Text = totalPrice.ToString("c");
        }

        #endregion

        #region Events
        private void Btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.ModifyFood += f_ModifyFood;
            f.InsertCategory += f_InsertCategory;
            f.DeleteCategory += f_DeleteCategory;
            f.ModifyCategory += f_ModifyCategory;
            f.ShowDialog();
        }

        private void f_ModifyCategory(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void f_DeleteCategory(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void f_InsertCategory(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void f_ModifyFood(object sender, EventArgs e)
        {
            LoadListFood((cmbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
        }

        private void f_DeleteFood(object sender, EventArgs e)
        {
            LoadListFood((cmbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
            LoadTable();
        }

        private void f_InsertFood(object sender, EventArgs e)
        {
            LoadListFood((cmbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;

            id = selected.ID;
            LoadListFood(id);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Please choose table first!!");
                return;
            }
            int idBill = BSBill.Instance.GetUncheckBillID(table.ID);
            int idFood = (cmbFood.SelectedItem as Food).ID;
            int count = (int)nbrFoodCount.Value;

            if(idBill == -1)
            {
                BSBill.Instance.InsertBill(table.ID);
                BSBillinfo.Instance.InsertBillInfo(BSBill.Instance.GetMaxIdBill(), idFood, count);
            }
            else
            {
                BSBillinfo.Instance.InsertBillInfo(idBill, idFood, count);
            }

            ShowBill(table.ID);
            LoadTable();
        }
        private void btnCheckout_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int idbill = BSBill.Instance.GetUncheckBillID(table.ID);
            double totalPrice = Convert.ToDouble(txtTotalPrice.Text.Split(',')[0]);
            if (idbill != -1)
            {
                if (MessageBox.Show("Are you sure that you want to check out?" + table.Name, "Notice", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BSBill.Instance.CheckOut(idbill, (float)totalPrice);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }
        }
        #endregion
        private void fCusTable_Load(object sender, EventArgs e)
        {

        }

      
    }
}
