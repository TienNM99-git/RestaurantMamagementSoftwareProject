using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyQuanAn.BS_Layer;
using QuanLyQuanAn.DB_Layer;

namespace QuanLyQuanAn
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }
        #region Methods
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;
            LoadListBillByDate(dtpkFrom.Value, dtpkTo.Value);
            LoadListFood();
            LoadCategory();
            LoadListTable();
            LoadFoodCategoryIntoComboBox(cmbFoodCategory);
            AddFoodBinding();         
            AddCategoryBinding();
            AddTableBinding();
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource= BSBill.Instance.GetBillListByDate(checkIn, checkOut);
        }
        void AddFoodBinding()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true,DataSourceUpdateMode.Never));
            txtFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));        
            nbrFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        void AddCategoryBinding()
        {
            txtCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
        }
        void AddTableBinding()
        {
            txtTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Stat", true, DataSourceUpdateMode.Never));
        }
        void LoadListFood()
        {
            foodList.DataSource = BSFood.Instance.GetListFood();
        }
        void LoadFoodCategoryIntoComboBox(ComboBox cmb)
        {
            cmb.DataSource = BSCategory.Instance.GetListCategory();
            cmb.DisplayMember = "Name";
        }
        
        void LoadCategory()
        {
            categoryList.DataSource = BSCategory.Instance.GetListCategory();
        }
        void LoadListTable()
        {
            tableList.DataSource = BSTable.Instance.LoadTableList();
        }
        List<Food> searchFood(string fName)
        {
            List<Food> searchList = BSFood.Instance.searchFood(fName);
            return searchList;
        }
        #endregion
        #region Events
        private void btnView_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFrom.Value,dtpkTo.Value);
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadCategory();
        }
        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }
        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                    Category category = BSCategory.Instance.GetCategoryByID(id);
                    cmbFoodCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cmbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cmbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }
     

        private void btnAddFood_Click(object sender, EventArgs e)
        {       
            string name = txtFoodName.Text;
            int id = (cmbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nbrFoodPrice.Value;
            if (BSFood.Instance.InsertFood(name, id, price))
            {
                MessageBox.Show("Add food successful");
                LoadListFood();
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Adding Unsuccessful");
            }
        }

        private void btnModifyFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int catId = (cmbFoodCategory.SelectedItem as Category).ID;
            int fId = Convert.ToInt32(txtFoodID.Text);
            float price = (float)nbrFoodPrice.Value;
            if (BSFood.Instance.ModifyFood(name, fId, catId, price))
            {
                MessageBox.Show("Modify food successful");
                LoadListFood();
                if (modifyFood != null)
                {
                    modifyFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Modifying Unsuccessful");
            }
        }

        private void btnDelFood_Click(object sender, EventArgs e)
        {
            int fId = Convert.ToInt32(txtFoodID.Text);
            if (BSFood.Instance.DeleteFood(fId))
            {
                MessageBox.Show("Delete food successful");
                LoadListFood();
                if(deleteFood!=null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Deleting Unsuccessful");
            }
        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string catName = txtCategoryName.Text;
            if (BSCategory.Instance.InsertCategory(catName))
            {
                MessageBox.Show("Add category successful");
                LoadCategory();
                if (insertCategory != null)
                {
                    insertCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Adding Unsuccessful");
            }
        }
        private void btnDelCategory_Click(object sender, EventArgs e)
        {
            int catID = Convert.ToInt32(txtCategoryID.Text);
            if (BSCategory.Instance.DeleteCategory(catID))
            {
                MessageBox.Show("Delete category successful");
                LoadCategory();
                if (deleteCategory != null)
                {
                    deleteCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Deleting Unsuccessful");
            }
        }
        private void btnModifyCategory_Click(object sender, EventArgs e)
        {
            string catName = txtCategoryName.Text;
            int catID = Convert.ToInt32(txtCategoryID.Text);
            if (BSCategory.Instance.ModifyCategory(catName,catID))
            {
                MessageBox.Show("Modify category successful");
                LoadCategory();
                if (modifyCategory != null)
                {
                    modifyCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Modifying Unsuccessful");
            }
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = searchFood(txtSearchFoodName.Text);
        }
        private void btnTopBill_Click(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BSBill.Instance.GetTopBill();
        }
        #endregion
        #region Events Handler
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }
        private event EventHandler modifyFood;
        public event EventHandler ModifyFood
        {
            add { modifyFood += value; }
            remove { modifyFood -= value; }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }
        private event EventHandler modifyCategory;
        public event EventHandler ModifyCategory
        {
            add { modifyCategory += value; }
            remove { modifyCategory -= value; }
        }
        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }

        #endregion

        private void fAdmin_Load(object sender, EventArgs e)
        {

        }

        private void fAdmin_Load_1(object sender, EventArgs e)
        {

        }

        private void fAdmin_Load_2(object sender, EventArgs e)
        {

        }
    }
}
