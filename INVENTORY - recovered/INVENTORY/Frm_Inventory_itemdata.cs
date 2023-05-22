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
    public partial class Frm_Inventory_itemdata : Form
    {
        public Frm_Inventory_itemdata()
        {
            InitializeComponent();
            panel_itemSCROLL.AutoScroll = false;
            category();
            itemLIST();
            categoryLIST();
        }


        //My SQl Connection String
        string connectionString = Properties.Settings.Default.MyConnection;
        SqlConnection con = new SqlConnection();

        // combo box category --
        public void category()
        {
            con.ConnectionString = connectionString;
            string select = "SELECT ITEMCategory_name FROM tbl_ITEMDATA_category; ";
            con.Open();
            SqlCommand cmd = new SqlCommand(select, con);

            SqlDataReader DR = cmd.ExecuteReader();
            while (DR.Read())
            {
                cmb_itemcategory.Items.Add(DR[0]);
            }
            con.Close();
        }
        // combo box category -- end

        // combo box category refresh--
        public void categoryrefresh()
        {
            con.ConnectionString = connectionString;
            string select = "SELECT TOP 1 ITEMCategory_name FROM tbl_ITEMDATA_category ORDER BY ITEMCategory_no DESC; ";
            con.Open();
            SqlCommand cmd = new SqlCommand(select, con);

            SqlDataReader DR = cmd.ExecuteReader();
            while (DR.Read())
            {
                cmb_itemcategory.Items.Add(DR[0]);
            }
            con.Close();
        }
        // combo box category -- end

        // DATA GRIDVIEW ----------------------------------------------------------------------------------start
        public void itemLIST()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            string select = "SELECT * FROM tbl_ITEMDATA; ";
            cmd.CommandText = select;
            SqlDataAdapter da = new SqlDataAdapter(select, con);
            SqlCommandBuilder cbuilder = new SqlCommandBuilder(da);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);
            bindingSource_ITEM.DataSource = table;

            dtg_itemLIST.ReadOnly = true;
            dtg_itemLIST.DataSource = bindingSource_ITEM;
        }
        // DATA GRIDVIEW ----------------------------------------------------------------------------------end
        
        

        // NEW MENU BUTTON ///////////////////////////////////////////////////////////////////////////start
        private void btn_NEW_Click(object sender, EventArgs e)
        {
            if (txt_itemid.Text.Length > 1)
            {
                if (MessageBox.Show("Previews activity will not be undone. Do you want to continue?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    btn_NEW.Enabled = false;
                    readyonlyFALSE();

                    btn_ADDitem.Visible = true;
                    btn_itemCANCEL.Visible = true;

                    dtg_itemLIST.Enabled = false;
                    txt_searchbox.Visible = false;

                    ITEMID();
                }
                else { }
            }
            else
            {
                if (MessageBox.Show("Do you want to add new Item record?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    btn_NEW.Enabled = false;
                    readyonlyFALSE();

                    btn_ADDitem.Visible = true;
                    btn_itemCANCEL.Visible = true;

                    dtg_itemLIST.Enabled = false;
                    txt_searchbox.Visible = false;

                    ITEMID();
                }
                else { }
            }
        }

        // read only == false *********************************************************start
        public void readyonlyFALSE()
        {
            txt_itemname.ReadOnly = false;
            cmb_itemcategory.Enabled = true;
            txt_itemprice.ReadOnly = false;

            txt_itemmanu.ReadOnly = false;
            txt_itemmanuadd.ReadOnly = false;
            txt_itemmanucontact.ReadOnly = false;
        }
        // read only == false *********************************************************end
        // NEW MENU BUTTON ///////////////////////////////////////////////////////////////////////////end



        // read only == false *********************************************************start
        public void readyonlyTRUE()
        {
            txt_itemname.ReadOnly = true;
            cmb_itemcategory.Enabled = false;
            txt_itemprice.ReadOnly = true;

            txt_itemmanu.ReadOnly = true;
            txt_itemmanuadd.ReadOnly = true;
            txt_itemmanucontact.ReadOnly = true;
        }
        // read only == false *********************************************************end


        //generate ITEM ID --
        public void ITEMID()
        {
            con.ConnectionString = connectionString;
            SqlCommand empid = new SqlCommand();
            empid.Connection = con;

            con.Open();
            string id = "SELECT COUNT(ITEM_ID) +1 FROM tbl_ITEMDATA;";
            empid.CommandText = id;
            SqlDataReader dr = empid.ExecuteReader();
            while (dr.Read())
            {
                int value = int.Parse(dr[0].ToString());
                if (value.ToString().Length == 1) { txt_itemid.Text = "0000000" + String.Format(value.ToString()); }
                if (value.ToString().Length == 2) { txt_itemid.Text = "000000" + String.Format(value.ToString()); }
                if (value.ToString().Length == 3) { txt_itemid.Text = "00000" + String.Format(value.ToString()); }
                if (value.ToString().Length == 4) { txt_itemid.Text = "0000" + String.Format(value.ToString()); }
                if (value.ToString().Length == 5) { txt_itemid.Text = "000" + String.Format(value.ToString()); }
                if (value.ToString().Length == 6) { txt_itemid.Text = "00" + String.Format(value.ToString()); }
            }
            con.Close();
        }

        //generate ITEM ID --


        //define new category--
        private void cmb_itemcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_itemcategory.SelectedIndex.Equals(0))
            {
                panel_newcategory.Visible = true;
                panel_itemdata.Enabled = false;
                panel_empgrid.Visible = false;
            }
        }
        //define new category--


        // if empty containers --
        public void error() {
            if (txt_itemname.Text.Length.Equals(0) || txt_itemname.Text.Length > 100) { error_itemname.Visible = true; }
            if (cmb_itemcategory.SelectedIndex.Equals(0)) { error_itemcategory.Visible = true; }
            if (txt_itemprice.Text.Length > 7 ) { error_itemprice.Visible = true; }
            if (txt_itemmanu.Text.Length > 150) { error_itemmanu.Visible = true; }
            if (txt_itemmanuadd.Text.Length > 150) { error_itemaddress.Visible = true; }
            if (txt_itemmanucontact.Text.Length > 150) { error_itemcontact.Visible = true; }
        }
        // if empty containers -- end

        // NULL CONTENT / N/A --
        public void NULL()
        {
            if (txt_itemmanu.Text.Length.Equals(0)) { txt_itemmanu.Text = "N/A"; }
            if (txt_itemmanuadd.Text.Length.Equals(0)) { txt_itemmanuadd.Text = "N/A"; }
            if (txt_itemmanucontact.Text.Length.Equals(0)) { txt_itemmanucontact.Text = "N/A"; }
        }
        // NULL CONTENT / N/A -- end

        // ERROR HIDE --
        public void errorHIDE()
        {
            error_itemname.Visible = false ; 
            error_itemcategory.Visible = false; 
            error_itemprice.Visible = false; 
            error_itemmanu.Visible = false; 
            error_itemaddress.Visible = false; 
            error_itemcontact.Visible = false; 
        }
        // ERROR HIDE -- end

        // CLEAR DATA --
        public void CLEAR()
        {
            txt_itemid.Text = "";
            txt_itemname.Text = "";
            cmb_itemcategory.SelectedIndex = -1;
            txt_itemprice.Text = "";
            txt_itemmanu.Text = "";
            txt_itemmanuadd.Text = "";
            txt_itemmanucontact.Text = "";
        }
        // CLEAR DATA -- end


        // SQl // ADD TO DATABASE ---------------------------------------------------------start
        public void addITEMDATA()
        {
            Frm_dashboard userID = new Frm_dashboard();
            string regby = userID.txt_employeeID.Text.ToString();

            con.ConnectionString = connectionString;
            SqlCommand itemdata = new SqlCommand();
            itemdata.Connection = con;
            con.Open();
            string insert1 = @"INSERT INTO      tbl_ITEMDATA
                                            (   ITEM_ID, 
                                                ITEM_name, 
                                                ITEM_category, 
                                                ITEM_price, 
                                                ITEM_regdate, 
                                                ITEM_regby      ) 
                                VALUES      (   @ITEM_ID, 
                                                @ITEM_name, 
                                                @ITEM_category, 
                                                @ITEM_price, 
                                                @ITEM_regdate, 
                                                @ITEM_regby      ); ";

            itemdata.Parameters.AddWithValue("@ITEM_ID", txt_itemid.Text);
            itemdata.Parameters.AddWithValue("@ITEM_name", txt_itemname.Text);
            itemdata.Parameters.AddWithValue("@ITEM_category", cmb_itemcategory.Text);
            itemdata.Parameters.AddWithValue("@ITEM_price", txt_itemprice.Text);
            itemdata.Parameters.AddWithValue("@ITEM_regdate", SqlDbType.Date).Value = DateTime.Today;
            itemdata.Parameters.AddWithValue("@ITEM_regby", regby);
            itemdata.CommandText = insert1;
            itemdata.ExecuteNonQuery();
            itemdata.Parameters.Clear();
            con.Close();
        }

        public void addITEMDATA_manufacturer()
        {
            Frm_dashboard userID = new Frm_dashboard();
            string regby = userID.txt_employeeID.Text.ToString();

            con.ConnectionString = connectionString;
            SqlCommand itemdata_manu = new SqlCommand();
            itemdata_manu.Connection = con;
            con.Open();
            string insert2 = @"INSERT INTO      tbl_ITEMDATA_supplier
                                            (   ITEMSupplier_name, 
                                                ITEMSupplier_address, 
                                                ITEMSupplier_contact, 
                                                ITEM_ID    ) 
                                VALUES      (   @ITEMSupplier_name, 
                                                @ITEMSupplier_address, 
                                                @ITEMSupplier_contact, 
                                                @ITEM_ID         ); ";

            itemdata_manu.Parameters.AddWithValue("@ITEMSupplier_name", txt_itemmanu.Text);
            itemdata_manu.Parameters.AddWithValue("@ITEMSupplier_address", txt_itemmanuadd.Text);
            itemdata_manu.Parameters.AddWithValue("@ITEMSupplier_contact", txt_itemmanucontact.Text);
            itemdata_manu.Parameters.AddWithValue("@ITEM_ID", txt_itemid.Text);
            itemdata_manu.CommandText = insert2;
            itemdata_manu.ExecuteNonQuery();
            itemdata_manu.Parameters.Clear();
            con.Close();
        }
        // SQl // ADD TO DATABASE ---------------------------------------------------------end


        // SAVE NEW ITEM RECORD ????????????????????????????????????????????????????????????????? START
        private void btn_ADDitem_Click(object sender, EventArgs e)
        {
            error();

            if (txt_itemname.Text.Length.Equals(0) || cmb_itemcategory.SelectedIndex.Equals(-1) ||
                txt_itemprice.Text.Length.Equals(0))
            {
                NULL();

                error();
                MessageBox.Show("Record couldn't be save. Make sure to input valid data.");
            }
            else
            {
                if (MessageBox.Show("New item record will be added. Do you want to continue?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    NULL();
                    addITEMDATA();
                    addITEMDATA_manufacturer();

                    errorHIDE();
                    itemLIST();
                    readyonlyTRUE();
                    MessageBox.Show("New Item record succesfully added.");

                    CLEAR();
                    btn_ADDitem.Visible = false;
                    btn_itemCANCEL.Visible = false;

                    dtg_itemLIST.Enabled = true;
                }
            }
        }
        // SAVE NEW ITEM RECORD ????????????????????????????????????????????????????????????????? END

        // CANCEL NEW BUTTON ???????????????????????????????????????????????????????????????????????? START
        private void btn_itemCANCEL_Click(object sender, EventArgs e)
        {
            if (txt_itemid.Text.Length > 1)
            {
                if (MessageBox.Show("Record will not be save. Are you sure to cancel?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CLEAR();
                    errorHIDE();
                    readyonlyTRUE();
                    dtg_itemLIST.Enabled = true;

                    btn_ADDitem.Visible = false;
                    btn_itemCANCEL.Visible = false;
                    
                }
                else { }

            }
            else
            {
                CLEAR();
                errorHIDE();
                readyonlyTRUE();
                dtg_itemLIST.Enabled = true;

                btn_ADDitem.Visible = false;
                btn_itemCANCEL.Visible = false;
            }
        }
        // CANCEL NEW BUTTON ???????????????????????????????????????????????????????????????????????? END



        // ======================================== UPDATE ITEM DATA =========================================


        // DATA GRID VIEW CELLCLICK ????????????????????????????????????????????????????????????????? START

        // set data using INNER JOIN  -- start
        public void setDATA()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string selectemp = @"SELECT         tbl_ITEMDATA.*, 
                                                tbl_ITEMDATA_supplier.ITEMSupplier_name, 
                                                tbl_ITEMDATA_supplier.ITEMSupplier_address, 
                                                tbl_ITEMDATA_supplier.ITEMSupplier_contact
                                FROM            tbl_ITEMDATA 
                                INNER JOIN      tbl_ITEMDATA_supplier 
                                ON              tbl_ITEMDATA.ITEM_ID = tbl_ITEMDATA_supplier.ITEM_ID
						        WHERE           tbl_ITEMDATA.ITEM_ID ='" + txt_itemid.Text + "'";

            con.Open();
            cmd.CommandText = selectemp;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    txt_itemname.Text = myReader["ITEM_name"].ToString();
                    cmb_itemcategory.Text = myReader["ITEM_category"].ToString();
                    txt_itemprice.Text = myReader["ITEM_price"].ToString();

                    txt_itemregdate.Text = Convert.ToDateTime(myReader["ITEM_regdate"]).ToString();
                    txt_itemregby.Text = myReader["ITEM_regby"].ToString();
                    
                    txt_itemmanu.Text = myReader["ITEMSupplier_name"].ToString();
                    txt_itemmanuadd.Text = myReader["ITEMSupplier_address"].ToString();
                    txt_itemmanucontact.Text = myReader["ITEMSupplier_contact"].ToString();
                }
            }
            con.Close();
        }
        // set data using INNER JOIN  -- end

        // cell click --
        private void dtg_itemLIST_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txt_itemid.Text = dtg_itemLIST.Rows[e.RowIndex].Cells[0].Value.ToString();

                setDATA();
                label_update.Visible = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Select at least 1 record.");
            }
        }

        // DATA GRID VIEW CELLCLICK ????????????????????????????????????????????????????????????????? END



        // UPDATE RECORD ???????????????????????????????????????????????????????????????????????????? START
        private void label_update_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("Do you want to update this record?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                readyonlyFALSE();

                btn_updateSAVE.Visible = true;
                btn_updateCANCEL.Visible = true;

                btn_ADDitem.Visible = false;
                btn_itemCANCEL.Visible = false;

                dtg_itemLIST.Enabled = false;
                txt_searchbox.Visible = false;
            }
            else { }
        }

        // SQl // UPDATE TO DATABASE ---------------------------------------------------------start
        public void UPDATEempdata()
        {
            Frm_dashboard userID = new Frm_dashboard();
            string regby = userID.txt_employeeID.Text.ToString();

            con.ConnectionString = connectionString;
            SqlCommand itemdata = new SqlCommand();
            itemdata.Connection = con;
            con.Open();
            string update1 = @"UPDATE      tbl_ITEMDATA 
                               SET         ITEM_name = @ITEM_name, 
                                           ITEM_category = @ITEM_category, 
                                           ITEM_price = @ITEM_price, 
                                           ITEM_regdate = @ITEM_regdate, 
                                           ITEM_regby = @ITEM_regby
                                WHERE      ITEM_ID = '" + txt_itemid.Text + "'";
            
            itemdata.Parameters.AddWithValue("@ITEM_name", txt_itemname.Text);
            itemdata.Parameters.AddWithValue("@ITEM_category", cmb_itemcategory.Text);
            itemdata.Parameters.AddWithValue("@ITEM_price", txt_itemprice.Text);
            itemdata.Parameters.AddWithValue("@ITEM_regdate", SqlDbType.Date).Value = DateTime.Today;
            itemdata.Parameters.AddWithValue("@ITEM_regby", regby);
            itemdata.CommandText = update1;
            itemdata.ExecuteNonQuery();
            itemdata.Parameters.Clear();
            con.Close();
        }
        public void UPDATEempdata_manufacturer()
        {
            con.ConnectionString = connectionString;
            SqlCommand itemdata_manu = new SqlCommand();
            itemdata_manu.Connection = con;
            con.Open();
            string update2 = @"UPDATE      tbl_ITEMDATA_manufacturer 
                               SET         ITEMManu_name = @ITEMManu_name, 
                                           ITEMManu_address = @ITEMManu_address, 
                                           ITEMManu_contact = @ITEMManu_contact
                                WHERE      ITEM_ID = '" + txt_itemid.Text + "'";

            itemdata_manu.Parameters.AddWithValue("@ITEMManu_name", txt_itemmanu.Text);
            itemdata_manu.Parameters.AddWithValue("@ITEMManu_address", txt_itemmanuadd.Text);
            itemdata_manu.Parameters.AddWithValue("@ITEMManu_contact", txt_itemmanucontact.Text);
            itemdata_manu.CommandText = update2;
            itemdata_manu.ExecuteNonQuery();
            itemdata_manu.Parameters.Clear();
            con.Close();
        }

        // SQl // UPDATE TO DATABASE --------------------------------------------------------- end

        // update --
        private void btn_updateSAVE_Click(object sender, EventArgs e)
        {
            error();

            if (txt_itemname.Text.Length.Equals(0) || cmb_itemcategory.SelectedIndex.Equals(-1) ||
                txt_itemprice.Text.Length.Equals(0))
            {
                NULL();
                error();
                MessageBox.Show("Record couldn't be updated. Make sure to input valid data.");
            }
            else
            {
                if (MessageBox.Show("Changes will be saved. Do you want to continue?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    NULL();
                    UPDATEempdata();
                    UPDATEempdata_manufacturer();

                    errorHIDE();
                    itemLIST();
                    readyonlyTRUE();
                    MessageBox.Show("Item record succesfully updated.");

                    CLEAR();
                    btn_updateSAVE.Visible = false;
                    btn_updateCANCEL.Visible = false;

                    dtg_itemLIST.Enabled = true;
                }
            }
        }

        // update --

        // cancel update --
        private void btn_updateCANCEL_Click(object sender, EventArgs e)
        {
            if (txt_itemid.Text.Length > 1)
            {
                if (MessageBox.Show("Changes will not be save. Are you sure to cancel?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CLEAR();
                    errorHIDE();
                    readyonlyTRUE();
                    dtg_itemLIST.Enabled = true;
                    
                    btn_updateSAVE.Visible = false;
                    btn_updateCANCEL.Visible = false;

                }
                else { }

            }
            else
            {
                CLEAR();
                errorHIDE();
                readyonlyTRUE();
                dtg_itemLIST.Enabled = true;

                btn_updateSAVE.Visible = false;
                btn_updateCANCEL.Visible = false;
            }
        }

        // cancel update --

        // BARCODE --------------------------------------------start
        private void btn_generateBARCODE_Click(object sender, EventArgs e)
        {
        }

        // BARCODE --------------------------------------------end


        // NEW CATEGORY ???????????????????????????????????????????????????????????????????START
        private void btn_SAVEcategory_Click(object sender, EventArgs e)
        {
            if (txt_category.Text.Length.Equals(0) || txt_category.Text.Length > 20)
            {
                MessageBox.Show("Could not be saved. Make sure to input valid data.");
            }
            else
            {
                if (MessageBox.Show("New Item Category will be added. Do you want to continue?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    addCATEGORY();
                    MessageBox.Show("New Item Category successfully added.");
                    categoryLIST();

                    txt_category.Text = " ";
                }
                else { }
            }
        }
        // data grid view --
        public void categoryLIST()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            string select = "SELECT * FROM tbl_ITEMDATA_category; ";
            cmd.CommandText = select;
            SqlDataAdapter da = new SqlDataAdapter(select, con);
            SqlCommandBuilder cbuilder = new SqlCommandBuilder(da);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);
            bindingSource_CATEGORY.DataSource = table;

            dtg_category.ReadOnly = true;
            dtg_category.DataSource = bindingSource_CATEGORY;
        }
        //SQL COMMAND --
        public void addCATEGORY()
        {
            con.ConnectionString = connectionString;
            SqlCommand category = new SqlCommand();
            category.Connection = con;
            con.Open();
            string insert = @"INSERT INTO      tbl_ITEMDATA_category
                                            (   ITEMCategory_name   ) 
                                VALUES      (   @ITEMCategory_name   ); ";

            category.Parameters.AddWithValue("@ITEMCategory_name", txt_category.Text);
            category.CommandText = insert;
            category.ExecuteNonQuery();
            category.Parameters.Clear();
            con.Close();
        }
        
        private void btn_EXIT_Click_1(object sender, EventArgs e)
        {
            if (txt_category.Text.Length >= 0)
            {
                if (MessageBox.Show("Input will not be save. Do you want to Exit?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    panel_newcategory.Visible = false;
                    panel_empgrid.Visible = true;
                    panel_itemdata.Enabled = true;
                    categoryrefresh();
                }
            }
            else {
                panel_newcategory.Visible = false;
                panel_empgrid.Visible = true;
                panel_itemdata.Enabled = true;
                categoryrefresh();
            }
        }
        // NEW CATEGORY ???????????????????????????????????????????????????????????????????END
    }
}
