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
    public partial class Frm_Sales_record : Form
    {
        public Frm_Sales_record()
        {
            InitializeComponent();
            SOLIST();
        }
        //My SQl Connection String
        string connectionString = Properties.Settings.Default.MyConnection;
        SqlConnection con = new SqlConnection();




        // DATA GRIDVIEW SALES ORDER ----------------------------------------------------------------------------------start
        public void SOLIST()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            string select = @"SELECT        dbo.tbl_SALESORDER.*, 
                                            dbo.tbl_SALESORDER_payment.SOPayment_subtotal, 
                                            dbo.tbl_SALESORDER_payment.SOPayment_vat, 
                                            dbo.tbl_SALESORDER_payment.SOPayment_discount, 
                                            dbo.tbl_SALESORDER_payment.SOPayment_totalpayable, 
                                            dbo.tbl_SALESORDER_payment.SOPayment_totalcash, 
                                            dbo.tbl_SALESORDER_payment.SOPayment_totalchange, 
                                            dbo.tbl_SALESORDER_payment.SOPayment_date
                            FROM            dbo.tbl_SALESORDER
                            INNER JOIN      dbo.tbl_SALESORDER_payment 
                            ON              dbo.tbl_SALESORDER.SalesOrder_no = dbo.tbl_SALESORDER_payment.SalesOrder_no ; ";
            cmd.CommandText = select;
            SqlDataAdapter da = new SqlDataAdapter(select, con);
            SqlCommandBuilder cbuilder = new SqlCommandBuilder(da);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);
            bindingSource_SALESORDER.DataSource = table;

            dtg_SOLIST.ReadOnly = true;
            dtg_SOLIST.DataSource = bindingSource_SALESORDER;
        }
        public void SOITEM()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            string select = @"SELECT        dbo.tbl_SALESORDER.SalesOrder_no, 
                                            dbo.tbl_SALESORDER_item.ITEM_ID, 
                                            dbo.tbl_SALESORDER_item.SOItem_qty, 
                                            dbo.tbl_SALESORDER_item.SOItem_amount, 
                                            dbo.tbl_SALESORDER_item.SOItem_status
                            FROM            dbo.tbl_SALESORDER 
                            INNER JOIN      dbo.tbl_SALESORDER_item 
                            ON              dbo.tbl_SALESORDER.SalesOrder_no = dbo.tbl_SALESORDER_item.SalesOrder_no
                            WHERE           dbo.tbl_SALESORDER.SalesOrder_no = ' " + Convert.ToInt32(txt_salesorderNO.Text) + " ' ;  ";
            cmd.CommandText = select;
            SqlDataAdapter da = new SqlDataAdapter(select, con);
            SqlCommandBuilder cbuilder = new SqlCommandBuilder(da);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);
            bindingSource_SOITEM.DataSource = table;

            dtg_SOITEM.ReadOnly = true;
            dtg_SOITEM.DataSource = bindingSource_SOITEM;
        }
        // DATA GRIDVIEW  SALES ORDER ----------------------------------------------------------------------------------end







        private void txt_searchbox_TextChanged(object sender, EventArgs e)
        {

        }






        private void gunaPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtg_SOLIST_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txt_salesorderNO.Text = dtg_SOLIST.Rows[e.RowIndex].Cells[0].Value.ToString();
                SOITEM();
            }
            catch (Exception)
            {
                MessageBox.Show("Select at least 1 record.");
            }
        }
    }
}
