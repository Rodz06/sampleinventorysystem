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
    public partial class Frm_Inventory_qualityinspection : Form
    {
        public Frm_Inventory_qualityinspection()
        {
            InitializeComponent();
            damageLIST();
            item();
        } 
        //My SQl Connection String
        string connectionString = Properties.Settings.Default.MyConnection;
        SqlConnection con = new SqlConnection();

        // DATA GRIDVIEW STOCKS UPDATES ----------------------------------------------------------------------------------start
        public void damageLIST()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            string select = @"SELECT    QualityInspect_no, 
                                        ITEM_ID, 
                                        QualityInspect_damageqty,
                                        QualityInspect_date, 
                                        QualityInspect_by
                              FROM      tbl_INVENTORY_qualityinspect 
                              WHERE     ITEM_ID ='" + cmb_itemid.Text + "'";
            cmd.CommandText = select;
            SqlDataAdapter da = new SqlDataAdapter(select, con);
            SqlCommandBuilder cbuilder = new SqlCommandBuilder(da);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);
            bindingSource_DAMAGED.DataSource = table;

            dtg_damageLIST.ReadOnly = true;
            dtg_damageLIST.DataSource = bindingSource_DAMAGED;
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
                cmb_itemid.Items.Add(DR[0]);
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
						        WHERE           ITEM_ID ='" + cmb_itemid.Text + "'";

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

        
        private void cmb_itemid_SelectedIndexChanged(object sender, EventArgs e)
        {
            damageLIST();
            setDATA();
            txt_damageqty.ReadOnly = false;
            txt_damageqty.Focus();

            totaldamage();
            totalstock();

            //if (txt_instock.Text.Length > 1) {
            //    calculate()
            //}
        }

        // calculate good items --
        private void calculate() {
            int stock = Convert.ToInt32(txt_instock.Text);
            int damage = Convert.ToInt32(txt_damagetotal.Text);

            int good = stock - damage;
            txt_good.Text = good.ToString();
        }

        // total damage -- start
        private void totaldamage()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string total = @"SELECT             SUM(QualityInspect_damageqty) 
                             AS                 TOTALDAMAGE
                             FROM               tbl_INVENTORY_qualityinspect
						     WHERE              ITEM_ID ='" + cmb_itemid.Text + "'";
            con.Open();
            cmd.CommandText = total;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    txt_damagetotal.Text = myReader["TOTALDAMAGE"].ToString();
                }
            }
            con.Close();
        }
        // total damage -- end
        // total stock -- start
        private void totalstock()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string total = @"SELECT             SUM(Stock_stockqty) 
                             AS                 TOTAL
                             FROM               tbl_INVENTORY_stockrecord
						     WHERE              ITEM_ID ='" + cmb_itemid.Text + "'";
            con.Open();
            cmd.CommandText = total;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    txt_instock.Text = myReader["TOTAL"].ToString();
                }
            }
            con.Close();
        }
        // total stock -- end



        //input damage ------ start
        private void txt_damageqty_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_damageqty.Text, "[^0-9]"))
            {
                MessageBox.Show("Invalid input. Please enter numbers only.");
                txt_damageqty.Text = "";
                btn_damageADD.Enabled = false;
            }
            if (txt_damageqty.Text.Length > 7)
            {
                MessageBox.Show("Please input valid data.");
                txt_damageqty.Text = "";
                btn_damageADD.Enabled = false;
            }
            if (txt_damageqty.Text.Length > 0)
            {
                btn_damageADD.Enabled = true;
            }
            if (txt_damageqty.Text.Length < 0)
            {
                btn_damageADD.Enabled = false;
            }
        }
        //input damage ------ end

        // ADD button --------------------------------------------------------- start
        private void btn_damageADD_Click(object sender, EventArgs e)
        {
            if (dtg_toadddamage.Rows.Count == 0)
            {
                if (txt_damageqty.Text.Length.Equals(0))
                {
                    MessageBox.Show("Empty quantity will not be added. Make sure to enter valid data.");
                }
                else
                {
                    toadddamage();
                    btn_DELETE.Enabled = true;
                    dtg_toadddamage.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Multiple Item update is not allowed. Make sure to update each item one at a time.");
            }
        }
        // to add in damage -- start
        public void toadddamage()
        {
            string itemid = cmb_itemid.Text;
            string quantity = Convert.ToInt16(txt_damageqty.Text).ToString();
            string[] row = { itemid, quantity };
            dtg_toadddamage.Rows.Add(row);
        }
        // to add in damage -- end

        // clear --
        public void clear()
        {
            txt_damageqty.Text = "";
            txt_damageqty.ReadOnly = true;
            btn_damageADD.Enabled = false;

            cmb_itemid.SelectedIndex = -1;
            txt_itemname.Text = "";
            txt_itemcategory.Text = "";
            txt_itemprice.Text = "";
        }

        // clear --end





        // SAVE DAMAGE TO DATABASE ??????????????????????????????????????????????????????????????????????????????????? START
        private void btn_ADD_Click(object sender, EventArgs e)
        {
            if (dtg_toadddamage.RowCount.Equals(0))
            {
                MessageBox.Show("Empty table will not be saved.");
            }
            else
            {
                if (MessageBox.Show("Damage Stock record will be added. Do you want to continue?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    addDAMAGE();
                    damageLIST();
                    MessageBox.Show("New Stocks successfully added.");

                    dtg_toadddamage.Rows.Clear();

                    btn_ADD.Visible = false;
                    btn_CANCEL.Visible = false;
                    totalstock();
                    totaldamage();
                }
            }
        }
        // SQL // ADD DAMAGE --------- start
        public void addDAMAGE()
        {
            Frm_dashboard userID = new Frm_dashboard();
            string updateby = userID.txt_employeeID.Text.ToString();

            for (int i = 0; i < dtg_toadddamage.Rows.Count; i++)
            {
                con.ConnectionString = connectionString;
                SqlCommand stock = new SqlCommand();
                stock.Connection = con;
                con.Open();

                string insert = @"INSERT INTO       tbl_INVENTORY_qualityinspect
                                                (   QualityInspect_damageqty, 
                                                    QualityInspect_date, 
                                                    QualityInspect_by, 
                                                    ITEM_ID      ) 
                                VALUES      (       @QualityInspect_damageqty, 
                                                    @QualityInspect_date, 
                                                    @QualityInspect_by, 
                                                    @ITEM_ID     ); ";

                stock.Parameters.AddWithValue("@QualityInspect_damageqty", dtg_toadddamage.Rows[i].Cells["qty"].Value);
                stock.Parameters.AddWithValue("@ITEM_ID", dtg_toadddamage.Rows[i].Cells["itemid"].Value);

                stock.Parameters.AddWithValue("@QualityInspect_by", updateby);
                stock.Parameters.AddWithValue("@QualityInspect_date", SqlDbType.Date).Value = DateTime.Today;
                stock.CommandText = insert;
                stock.ExecuteNonQuery();
                stock.Parameters.Clear();
                con.Close();
            }
        }
        // SQL // ADD DAMAGE --------- end
        // SAVE DAMAGE TO DATABASE ??????????????????????????????????????????????????????????????????????????????????? END



        // CANCEL ------------------------------------------------------------------------------------------------------start
        private void btn_CANCEL_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Record will not be saved. Do you want to cancel?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dtg_toadddamage.Rows.Clear();
                btn_ADD.Visible = false;
                btn_CANCEL.Visible = false;
                
                clear();
            }
            else { }
        }

        // CANCEL ------------------------------------------------------------------------------------------------------end


        // NEW ------------------------------------------------------------------------------------------------------start
        private void btn_NEW_Click(object sender, EventArgs e)
        {
            if (cmb_itemid.Text.Length > 0)
            {
                if (MessageBox.Show("Previous activity will not be undo. Do you want to add new stock record?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (dtg_toadddamage.Rows.Count == 0)
                    {
                        enableTRUE();
                        clear();
                    }
                    else
                    {
                        enableTRUE();
                        dtg_toadddamage.Rows.Clear();
                        clear();
                    }
                }
            }
            else
            {
                enableTRUE();
                clear();
            }
        }
        private void enableTRUE()
        {
            cmb_itemid.Enabled = true;

            btn_ADD.Visible = true;
            btn_CANCEL.Visible = true;
        }
        private void enableFALSE()
        {
            cmb_itemid.Enabled = false;

            btn_ADD.Visible = false;
            btn_CANCEL.Visible = false;
            btn_DELETE.Enabled = false;

            txt_damageqty.ReadOnly = false;
            btn_ADD.Enabled = false;
        }

        // NEW ------------------------------------------------------------------------------------------------------end


        // CELL CLICK   ---- 
        private void dtg_toadddamage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                cmb_itemid.Text = dtg_toadddamage.Rows[e.RowIndex].Cells[0].Value.ToString();
                txt_damageqty.Text = dtg_toadddamage.Rows[e.RowIndex].Cells[1].Value.ToString();

                setDATA();
                btn_damageADD.Enabled = false;
                txt_damageqty.ReadOnly = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Select at least 1 record.");
            }
        }

        // CELL CLICK   ---- 


        private void btn_DELETE_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dtg_toadddamage.SelectedRows)
            {
                dtg_toadddamage.Rows.RemoveAt(item.Index);
            }

            txt_damageqty.Text = "";
            txt_damageqty.ReadOnly = false;
        }
    }
}
