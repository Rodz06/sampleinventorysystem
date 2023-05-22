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
    public partial class Frm_Inventory_records_monitoring : Form
    {
        public Frm_Inventory_records_monitoring()
        {
            InitializeComponent();
        }

        //My SQl Connection String
        string connectionString = Properties.Settings.Default.MyConnection;
        SqlConnection con = new SqlConnection();


        // set data --------------------------------------------start
        public void setDATA() {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string selectemp = @"SELECT        tbl_ITEMDATA.ITEM_name, 
                                               tbl_ITEMDATA.ITEM_category, 
                                               tbl_ITEMDATA.ITEM_price, 
                                               tbl_ITEMDATA_supplier.ITEMSupplier_name
                                FROM           tbl_ITEMDATA 
                                INNER JOIN     tbl_ITEMDATA_supplier 
                                ON             tbl_ITEMDATA.ITEM_ID = tbl_ITEMDATA_supplier.ITEM_ID
						        WHERE          tbl_ITEMDATA.ITEM_ID ='" + txt_itemID.Text + "'";

            con.Open();
            cmd.CommandText = selectemp;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    txt_itemname.Text = myReader["ITEM_name"].ToString();
                    txt_itemcategory.Text = myReader["ITEM_category"].ToString();
                    txt_itemprice.Text = myReader["ITEM_price"].ToString();
                    txt_itemsuplier.Text = myReader["ITEMSupplier_name"].ToString();
                }
            }
            con.Close();
        }
        // set data --------------------------------------------start


        // total stock -- start
        public void totalstock()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string total = @"SELECT             SUM(Stock_stockqty) 
                             AS                 TOTAL
                             FROM               tbl_INVENTORY_stockrecord
						     WHERE              ITEM_ID ='" + txt_itemID.Text + "'";
            con.Open();
            cmd.CommandText = total;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    string value = myReader["TOTAL"].ToString();

                    if (value.Length == 1) { txt_totalstock.Text = "00000" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 2) { txt_totalstock.Text = "0000" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 3) { txt_totalstock.Text = "000" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 4) { txt_totalstock.Text = "00" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 5) { txt_totalstock.Text = "0" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 6) { txt_totalstock.Text =  myReader["TOTAL"].ToString(); }
                }
            }
            con.Close();
        }
        // total stock -- end

        // total damage -- start
        public void totaldamage()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string total = @"SELECT             SUM(QualityInspect_damageqty) 
                             AS                 TOTAL
                             FROM               tbl_INVENTORY_qualityinspect
						     WHERE              ITEM_ID ='" + txt_itemID.Text + "'";
            con.Open();
            cmd.CommandText = total;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    string value = myReader["TOTAL"].ToString();

                    if (value.Length == 1) { txt_totaldamage.Text = "00000" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 2) { txt_totaldamage.Text = "0000" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 3) { txt_totaldamage.Text = "000" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 4) { txt_totaldamage.Text = "00" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 5) { txt_totaldamage.Text = "0" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 6) { txt_totaldamage.Text = myReader["TOTAL"].ToString(); }
                }
            }
            con.Close();
        }
        // total damage -- end


        //total available ----- start
        public void totalavailable() {
            int stock = Convert.ToInt32(txt_totalstock.Text);
            int damage = Convert.ToInt32(txt_totaldamage.Text);
            int sold = Convert.ToInt32(txt_totalsold.Text);
            int ds = damage + sold;
            string available = Convert.ToInt32(stock - ds).ToString();

            if (available.Length == 1) { txt_totalavailable.Text = "00000" + available; }
            if (available.Length == 2) { txt_totalavailable.Text = "0000" + available; }
            if (available.Length == 3) { txt_totalavailable.Text = "000" + available; }
            if (available.Length == 4) { txt_totalavailable.Text = "00" + available; }
            if (available.Length == 5) { txt_totalavailable.Text = "0" + available; }
            if (available.Length == 6) { txt_totalavailable.Text = available; }
        }
        //total available ----- end


        //total sold -------- start
        public void totalsold() {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string total = @"SELECT             SUM(SOItem_qty) 
                             AS                 TOTAL
                             FROM               tbl_SALESORDER_item
						     WHERE              ITEM_ID ='" + txt_itemID.Text + "'";
            con.Open();
            cmd.CommandText = total;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    string value = myReader["TOTAL"].ToString();

                    if (value.Length == 1) { txt_totalsold.Text = "00000" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 2) { txt_totalsold.Text = "0000" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 3) { txt_totalsold.Text = "000" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 4) { txt_totalsold.Text = "00" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 5) { txt_totalsold.Text = "0" + myReader["TOTAL"].ToString(); }
                    if (value.Length == 6) { txt_totalsold.Text = myReader["TOTAL"].ToString(); }
                }
            }
            con.Close();
        }
        //total sold -------- end

        




        private void txt_itemID_Click(object sender, EventArgs e)
        {

        }
    }
}
