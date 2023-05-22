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

namespace INVENTORY
{
    public partial class Frm_Inventory_managestocks : Form
    {
        public Frm_Inventory_managestocks()
        {
            InitializeComponent();
            stockLIST();
            item();
        }
        //My SQl Connection String
        string connectionString = Properties.Settings.Default.MyConnection;
        SqlConnection con = new SqlConnection();


        // DATA GRIDVIEW STOCKS UPDATES ----------------------------------------------------------------------------------start
        public void stockLIST()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            string select = @"SELECT    Stock_no, 
                                        ITEM_ID, 
                                        Stock_stockqty, 
                                        Stock_addeddate, 
                                        Stock_addedby 
                              FROM      tbl_INVENTORY_stockrecord 
                              WHERE     ITEM_ID ='" + cmb_itemID.Text + "'";
            cmd.CommandText = select;
            SqlDataAdapter da = new SqlDataAdapter(select, con);
            SqlCommandBuilder cbuilder = new SqlCommandBuilder(da);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);
            bindingSource_STOCK.DataSource = table;

            dtg_stockLIST.ReadOnly = true;
            dtg_stockLIST.DataSource = bindingSource_STOCK;
        }
        // DATA GRIDVIEW STOCKS UPDATES ----------------------------------------------------------------------------------end

        // combo box item id --
        public void item()
        {
            con.ConnectionString = connectionString;
            string select = "SELECT ITEM_ID FROM tbl_ITEMDATA; ";
            con.Open();
            SqlCommand cmd = new SqlCommand(select, con);

            SqlDataReader DR = cmd.ExecuteReader();
            while (DR.Read())
            {
                cmb_itemID.Items.Add(DR[0]);
            }
            con.Close();
        }
        // combo box item id -- end


        // set data --- start
        public void setDATA()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string selectemp = @"SELECT         ITEM_name, 
                                                ITEM_category, 
                                                ITEM_price
                                FROM            tbl_ITEMDATA
						        WHERE           ITEM_ID ='" + cmb_itemID.Text + "'";

            con.Open();
            cmd.CommandText = selectemp;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    txt_itemname.Text = myReader["ITEM_name"].ToString();
                    txt_itemcategory.Text = myReader["ITEM_category"].ToString();
                    txt_itemprice.Text = myReader["ITEM_price"].ToString();
                }
            }
            con.Close();
        }
        // set data --- end
        
        private void cmb_itemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            stockLIST();
            setDATA();
            txt_stock.ReadOnly = false;
            txt_stock.Focus();

            totalstock();
        }
        // total stock -- start
        private void totalstock() {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string total = @"SELECT             SUM(Stock_stockqty) 
                             AS                 TOTAL
                             FROM               tbl_INVENTORY_stockrecord
						     WHERE              ITEM_ID ='" + cmb_itemID.Text + "'";
            con.Open();
            cmd.CommandText = total;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    txt_total.Text = myReader["TOTAL"].ToString();
                }
            }
            con.Close();
        }
        // total stock -- end

            // input stock -----
        private void txt_stock_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_stock.Text, "[^0-9]"))
            {
                MessageBox.Show("Invalid input. Please enter numbers only.");
                txt_stock.Text = "";
                btn_addstock.Enabled = false;
            }
            if (txt_stock.Text.Length > 7)
            {
                MessageBox.Show("Please input valid data.");
                txt_stock.Text = "";
                btn_addstock.Enabled = false;
            }
            if ( txt_stock.Text.Length > 0)
            {
                btn_addstock.Enabled = true;
            }
            if (txt_stock.Text.Length < 0)
            {
                btn_addstock.Enabled = false;
            }
        }// input stock -----


        // ADD button --------------------------------------------------------- start
        private void btn_addstock_Click(object sender, EventArgs e)
        {
            if (dtg_toaddstock.Rows.Count == 0)
            {
                if (txt_stock.Text.Length.Equals(0))
                {
                    MessageBox.Show("Empty stck will not be added. Make sure to enter valid data.");
                }
                else
                {
                    toaddstock();
                    btn_DELETE.Enabled = true;
                    dtg_toaddstock.Enabled = true;
                }
            }
            else {
                MessageBox.Show("Multiple Item update is not allowed. Make sure to update each item one at a time.");
            }
            
        }

        // to add in stock -- start
        public void toaddstock() {
            string itemid = cmb_itemID.Text;
            string quantity = Convert.ToInt16(txt_stock.Text).ToString();
            string[] row = { itemid, quantity };
            dtg_toaddstock.Rows.Add(row);

        }
        // to add in stock -- end

        // clear --
        public void clear() {
            txt_stock.Text = "";
            txt_stock.ReadOnly = true;
            btn_addstock.Enabled = false;

            cmb_itemID.SelectedIndex = -1;
            txt_itemname.Text = "";
            txt_itemcategory.Text = "";
            txt_itemprice.Text = "";
        }

        // clear --end

        // to add datagrid cell click -- start
        private void dtg_toaddstock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                cmb_itemID.Text = dtg_toaddstock.Rows[e.RowIndex].Cells[0].Value.ToString();
                txt_stock.Text = dtg_toaddstock.Rows[e.RowIndex].Cells[1].Value.ToString();

                setDATA();
                btn_addstock.Enabled = false;
                txt_stock.ReadOnly = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Select at least 1 record.");
            }
        }

        // to add datagrid cell click -- end
        

        // remove --
        private void btn_REMOVE_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dtg_toaddstock.SelectedRows) {
                dtg_toaddstock.Rows.RemoveAt(item.Index);
            }

            txt_stock.Text = "";
            txt_stock.ReadOnly = false;
        }
        // remove -- end


        // SAVE STOCK TO DATABASE ??????????????????????????????????????????????????????????????????????????????????? START
        private void btn_SAVESTOCK_Click(object sender, EventArgs e)
        {
            if (dtg_toaddstock.RowCount.Equals(0))
            {
                MessageBox.Show("Empty stocks will not be saved.");
            }
            else {
                if (MessageBox.Show("New Stock record will be added. Do you want to continue?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    saveSTOCK();
                    stockLIST();
                    MessageBox.Show("New Stocks successfully added.");

                    dtg_toaddstock.Rows.Clear();

                    btn_SAVESTOCK.Visible = false;
                    btn_CANCELSTOCK.Visible = false;
                    totalstock();
                }
            }

        }
        // SQL // ADD STOC --------- start
        public void saveSTOCK() {
            Frm_dashboard userID = new Frm_dashboard();
            string updateby = userID.txt_employeeID.Text.ToString();

            for (int i = 0; i < dtg_toaddstock.Rows.Count; i++)
            {
                con.ConnectionString = connectionString;
                SqlCommand stock = new SqlCommand();
                stock.Connection = con;
                con.Open();

                string insert = @"INSERT INTO       tbl_INVENTORY_stockrecord
                                                (   Stock_stockqty, 
                                                    Stock_addeddate, 
                                                    Stock_addedby, 
                                                    ITEM_ID      ) 
                                VALUES      (       @Stock_stockqty, 
                                                    @Stock_addeddate, 
                                                    @Stock_addedby, 
                                                    @ITEM_ID     ); ";

                stock.Parameters.AddWithValue("@Stock_stockqty", dtg_toaddstock.Rows[i].Cells["qty"].Value);
                stock.Parameters.AddWithValue("@ITEM_ID", dtg_toaddstock.Rows[i].Cells["itemid"].Value);

                stock.Parameters.AddWithValue("@Stock_addedby", updateby);
                stock.Parameters.AddWithValue("@Stock_addeddate", SqlDbType.Date).Value = DateTime.Today;
                stock.CommandText = insert;
                stock.ExecuteNonQuery();
                stock.Parameters.Clear();
                con.Close();
            }
        }

        // SQL // ADD STOC --------- end
        // SAVE STOCK TO DATABASE ??????????????????????????????????????????????????????????????????????????????????? END

        // NEW ITEM STOCK ----------------------------------------------------------------------------------------------start
        private void btn_NEW_Click(object sender, EventArgs e)
        {
            if (cmb_itemID.Text.Length > 0)
            {
                if (MessageBox.Show("Previous activity will not be undo. Do you want to add new stock record?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (dtg_toaddstock.Rows.Count == 0)
                    {
                        enableTRUE();
                        clear();
                    }
                    else
                    {
                        enableTRUE();
                        dtg_toaddstock.Rows.Clear();
                        clear();
                    }
                }
            }
            else {
                enableTRUE();
                clear();
            }
        }
        private void enableTRUE() {
            cmb_itemID.Enabled = true;

            btn_SAVESTOCK.Visible = true;
            btn_CANCELSTOCK.Visible = true;
        }
        private void enableFALSE()
        {
            cmb_itemID.Enabled = false;

            btn_SAVESTOCK.Visible = false;
            btn_CANCELSTOCK.Visible = false;
            btn_DELETE.Enabled = false;
            
            txt_stock.ReadOnly = false;
            btn_addstock.Enabled = false;
        }
        // NEW ITEM STOCK ----------------------------------------------------------------------------------------------end

        // CANCEL ------------------------------------------------------------------------------------------------------ start
        private void btn_CANCELSTOCK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Record will not be saved. Do you want to cancel?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dtg_toaddstock.Rows.Clear();
                enableFALSE();
                clear();
            }
            else { }
        }

        // CANCEL ------------------------------------------------------------------------------------------------------ end
    }
}
