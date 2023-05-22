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
    public partial class Frm_Inventory_records : Form
    {
        public Frm_Inventory_records()
        {
            InitializeComponent();
            itemLIST();
            totalITEM();
        }

        //My SQl Connection String
        string connectionString = Properties.Settings.Default.MyConnection;
        SqlConnection con = new SqlConnection();

        // DATA GRIDVIEW ----------------------------------------------------------------------------------start
        public void itemLIST()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            string select = @"SELECT        tbl_ITEMDATA.*, 
                                            tbl_ITEMDATA_supplier.ITEMSupplier_name,
                                            tbl_ITEMDATA_supplier.ITEMSupplier_address
                            FROM            tbl_ITEMDATA 
                            INNER JOIN      tbl_ITEMDATA_supplier 
                            ON              tbl_ITEMDATA.ITEM_ID = tbl_ITEMDATA_supplier.ITEM_ID; ";
            cmd.CommandText = select;
            SqlDataAdapter da = new SqlDataAdapter(select, con);
            SqlCommandBuilder cbuilder = new SqlCommandBuilder(da);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);
            bindingSource_itemLIST.DataSource = table;

            dtg_itemrecordLIST.ReadOnly = true;
            dtg_itemrecordLIST.DataSource = bindingSource_itemLIST;
        }
        // DATA GRIDVIEW ----------------------------------------------------------------------------------end

        // count total item -------start
        private void totalITEM()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string total = @"SELECT             COUNT(ITEM_ID) 
                             AS                 TOTAL
                             FROM               tbl_ITEMDATA";
            con.Open();
            cmd.CommandText = total;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    txt_totalitem.Text = myReader["TOTAL"].ToString();
                }
            }
            con.Close();
        }
        // count total item -------end


        // cell click -----------------------------------------------------------------------start
        private void dtg_itemrecordLIST_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                label_copytomonitoring.Visible = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Select at least 1 record.");
            }
        }

        private void label_copytomonitoring_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (DataGridViewRow item in this.dtg_itemrecordLIST.SelectedRows)
            {
                Frm_Inventory_records_monitoring monitor = new Frm_Inventory_records_monitoring();
                monitor.Show();
                monitor.txt_itemID.Text = dtg_itemrecordLIST.SelectedCells[0].Value.ToString();

                monitor.setDATA();
                monitor.totalstock();
                monitor.totaldamage();
                monitor.totalsold();
                monitor.totalavailable();
            }

        }
        // cell click -----------------------------------------------------------------------end





        private void txt_searchbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_PDF_Click(object sender, EventArgs e)
        {

        }
    }
}
