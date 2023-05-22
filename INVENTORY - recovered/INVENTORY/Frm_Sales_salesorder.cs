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
    public partial class Frm_Sales_salesorder : Form
    {
        public Frm_Sales_salesorder()
        {
            InitializeComponent();
            
            item();
            initializedDATAGRIDfields();
        }
        //My SQl Connection String
        string connectionString = Properties.Settings.Default.MyConnection;
        SqlConnection con = new SqlConnection();

        // initialize datagrid field ---------------START
        DataTable dt1;
        public void initializedDATAGRIDfields()
        {
            dt1 = new DataTable();
            dt1.Columns.Add("Item ID");
            dt1.Columns.Add("Description");
            dt1.Columns.Add("Price");
            dt1.Columns.Add("Qty");
            dt1.Columns.Add("Amount");
            this.dtg_toaddsalesorder.DataSource = dt1;
        }
        // initialize datagrid field --------------- END







        // NEW SALES ORDER BUTTON ///////////////////////////////////////////////////////////////////////////start
        private void btn_NEWSALESORDER_Click(object sender, EventArgs e)
        {
            salesorderNO();
            CLEAR();
            readonlyFALSE();
        }
        //generate SALESORDER NO --
        public void salesorderNO()
        {
            con.ConnectionString = connectionString;
            SqlCommand salesorder = new SqlCommand();
            salesorder.Connection = con;

            con.Open();
            string number = "SELECT COUNT(SalesOrder_no) +1 FROM tbl_SALESORDER;";
            salesorder.CommandText = number;
            SqlDataReader dr = salesorder.ExecuteReader();
            while (dr.Read())
            {
                int value = int.Parse(dr[0].ToString());
                txt_salesorderNO.Text = String.Format(value.ToString());
            }
            con.Close();
        }
        //generate SALESORDER NO --
        // NEW SALES ORDER BUTTON ///////////////////////////////////////////////////////////////////////////end


        // other activities -----------------------------------------------------------------start
        //clear content ---
        private void CLEAR() {
            txt_customername.Text = "";

            cmb_itemID.SelectedIndex = -1;
            txt_itemname.Text = "";
            txt_itemprice.Text = "";
            txt_itemQTY.Text = "";

            if (dt1.Rows.Count.Equals(0))
            {
                dt1.Rows.Clear();
            }

            txt_subtotal.Text = "";
            txt_totalvat.Text = "";
            txt_percentVAT.Text = "";
            txt_totaldiscount.Text = "";
            txt_percentDISCOUNT.Text = "";
            txt_TOTALPAYABLE.Text = "";

            txt_totalCASH.Text = "";
            txt_totalCHANGE.Text = "";
        }
        // ready only = true ---
        private void readonlyTRUE() {
            txt_customername.ReadOnly = true;

            cmb_itemID.Enabled = false;
            txt_itemname.ReadOnly = true;
            txt_itemprice.ReadOnly = true;
            txt_itemQTY.ReadOnly = true;

            dtg_toaddsalesorder.Enabled = false;
        }
        // ready only = false ---
        private void readonlyFALSE()
        {
            txt_customername.ReadOnly = false;

            cmb_itemID.Enabled = true;
            txt_itemname.ReadOnly = false;
            txt_itemprice.ReadOnly = false;
            txt_itemQTY.ReadOnly = false;

            dtg_toaddsalesorder.Enabled = true;
        }
        // other activities -----------------------------------------------------------------end

        // COMBO BOX ------------------------------------------------------------------------start
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
                    txt_itemprice.Text = myReader["ITEM_price"].ToString();
                }
            }
            con.Close();
        }
        // set data --- end
        private void cmb_itemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            setDATA();
        }

        // COMBO BOX ------------------------------------------------------------------------end


        // INPUT ITEM QUANTITY ------------------------------------------------------------------start
        private void txt_itemQTY_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_itemQTY.Text, "[^0-9]"))
            {
                MessageBox.Show("Invalid input. Please enter numbers only.");
                txt_itemQTY.Text = txt_itemQTY.Text.Remove(txt_itemQTY.Text.Length - 1);
                btn_ADDITEM.Enabled = false;
            }
            if (txt_itemQTY.Text.Length > 7)
            {
                MessageBox.Show("Please input valid data.");
                txt_itemQTY.Text = txt_itemQTY.Text.Remove(txt_itemQTY.Text.Length - 1);
                btn_ADDITEM.Enabled = false;
            }
            if (txt_itemQTY.Text.Length > 1)
            {
                btn_ADDITEM.Enabled = true;
            }
        }
        // INPUT ITEM QUANTITY ------------------------------------------------------------------end

        // ADD ITEM ------------------------------------------------------------------start
        private void btn_ADDITEM_Click(object sender, EventArgs e)
        {
            if (txt_itemQTY.Text.Length.Equals(0))
            {
                MessageBox.Show("Empty stck will not be added. Make sure to enter valid data.");
                txt_itemQTY.Focus();
            }
            else {
                toadditem();
                cmb_itemID.SelectedIndex = -1;
                txt_itemname.Text = "";
                txt_itemprice.Text = "";
                txt_itemQTY.Text = "";
            }
        }
        // dublication ---
        public void duplicate() {
            for (int i = 0; i < dtg_toaddsalesorder.Rows.Count; i++) {
                string data = dtg_toaddsalesorder.Rows[i].Cells[0].Value.ToString();
                if (data == cmb_itemID.Text)
                {
                    MessageBox.Show("Item is already exist.");
                    cmb_itemID.SelectedIndex = -1;
                }
                else {
                    toadditem();
                    cmb_itemID.SelectedIndex = -1;
                    txt_itemname.Text = "";
                    txt_itemprice.Text = "";
                    txt_itemQTY.Text = "";
                }
            }
        }


        // to add in sales order -- start
        public void toadditem()
        {
            double pr = Convert.ToDouble(txt_itemprice.Text);
            int qt = Convert.ToInt32(txt_itemQTY.Text);

            double amt = pr * qt;

            string itemid = cmb_itemID.Text;
            string description = txt_itemname.Text;
            string price = Convert.ToInt32(txt_itemprice.Text).ToString();
            string qty = Convert.ToInt16(txt_itemQTY.Text).ToString();
            string amount = Convert.ToInt32(amt).ToString();
            string[] row = { itemid, description, price, qty, amount};
            dt1.Rows.Add(row);

        }
        // to add in sales order -- end

        // duplicate entry ------ start
        public void restrictduplication() {
            
        }
        // duplicate entry ------ end
        // ADD ITEM ------------------------------------------------------------------end




        // CALCULATE -------------------------------------------------------------------start
        // subtotal -----
        public void subtotal() {
            int subtotal = 0;
            for (int i = 0; i < dtg_toaddsalesorder.Rows.Count; ++i)
            {
                subtotal += Convert.ToInt32(dtg_toaddsalesorder.Rows[i].Cells["amount"].Value);
            }

            txt_subtotal.Text = subtotal.ToString();
        }
        private void btn_forpayment_Click(object sender, EventArgs e)
        {
            if (dtg_toaddsalesorder.Rows.Count.Equals(0))
            {
                MessageBox.Show("Please input Valid data.");
            }
            else {
                subtotal();
                calculateTOTAL();
            }
        }
        // subtotal ----- end

        // vat --------start
        private void txt_percentVAT_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_percentVAT.Text, "[^0-9]"))
            {
                MessageBox.Show("Invalid input. Please enter numbers only.");
                txt_percentVAT.Text = txt_percentVAT.Text.Remove(txt_percentVAT.Text.Length - 1);
            }
            if (txt_percentVAT.Text.Length > 0)
            {
                double subtotal = Convert.ToDouble(txt_subtotal.Text);
                double percent = Convert.ToDouble(txt_percentVAT.Text);
                double totalvat = subtotal * percent / 100;

                txt_totalvat.Text = totalvat.ToString();
                calculateTOTAL();
            }
            if (txt_percentVAT.Text.Length <= 0)
            {
                txt_totalvat.Text = "0";
                calculateTOTAL();
            }
        }
        //vat ------ end

        // discount ----- start
        private void txt_percentDISCOUNT_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_percentDISCOUNT.Text, "[^0-9]"))
            {
                MessageBox.Show("Invalid input. Please enter numbers only.");
                txt_percentVAT.Text = txt_percentDISCOUNT.Text.Remove(txt_percentDISCOUNT.Text.Length - 1);
            }
            if (txt_percentDISCOUNT.Text.Length > 0)
            {
                double subtotal = Convert.ToDouble(txt_subtotal.Text);
                double percent = Convert.ToDouble(txt_percentDISCOUNT.Text);
                double totaldisc = subtotal * percent / 100;

                txt_totaldiscount.Text = totaldisc.ToString();

                calculateTOTAL();
            }
            if (txt_percentDISCOUNT.Text.Length <= 0)
            {
                txt_totaldiscount.Text = "0";
                calculateTOTAL();
            }
        }
        // discount ----- end

        // TOTAL AMOUNT PAYABLE ----------------start
        public void calculateTOTAL()
        {
            if (txt_percentVAT.Text.Length.Equals(0)) { txt_totalvat.Text = "0"; }
            if (txt_percentDISCOUNT.Text.Length.Equals(0)) { txt_totaldiscount.Text = "0"; }

            decimal vat = Convert.ToDecimal(txt_totalvat.Text);
            decimal disc = Convert.ToDecimal(txt_totaldiscount.Text);
            decimal subtotal = Convert.ToDecimal(txt_subtotal.Text);
            decimal total = subtotal - (vat + disc);
            txt_TOTALPAYABLE.Text = total.ToString();
        }
        // TOTAL AMOUNT PAYABLE --------------------end

        // CASH ------- start
        private void txt_totalCASH_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_totalCASH.Text, "[^0-9]"))
            {
                MessageBox.Show("Invalid input. Please enter numbers only.");
                txt_totalCASH.Text = "";
            }
            if (txt_totalCASH.Text.Length > 0)
            {
                double cash = Convert.ToDouble(txt_totalCASH.Text);
                double totalamount = Convert.ToDouble(txt_TOTALPAYABLE.Text);
                double change = cash - totalamount;

                txt_totalCHANGE.Text = change.ToString();
            }
            if (txt_totalCASH.Text.Length <= 0)
            {
                txt_totalCASH.Text = "";
            }
        }
        //CASH ----- end
        // CALCULATE -------------------------------------------------------------------end




        // PAID ------------------------------------------------------------------- start
        private void btn_PAID_Click(object sender, EventArgs e)
        {
            if (txt_totalCASH.Text.Length > 0)
            {
                if (MessageBox.Show("Are you sure to save this sales order?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    saveSALESORDER();
                    saveSOITEM();
                    savePAYMENT();
                    MessageBox.Show("Sales Order successfully PAID.");
                    CLEAR();
                }
            }
            else {
                MessageBox.Show("Please make sure to input payment first.");
            }
        }

        // PAID ------------------------------------------------------------------- start


        // SQL // save SALES ORDER // PAYMENT -----------start
        public void saveSALESORDER()
        {
            Frm_dashboard userID = new Frm_dashboard();
            string regby = userID.txt_employeeID.Text.ToString();

            con.ConnectionString = connectionString;
            SqlCommand salesorder = new SqlCommand();
            salesorder.Connection = con;
            con.Open();
            string insert = @"INSERT INTO      tbl_SALESORDER
                                            (   SalesOrder_customer, 
                                                SalesOrder_date, 
                                                SalesOrder_status,
                                                EMP_ID          ) 
                                VALUES      (   @SalesOrder_customer, 
                                                @SalesOrder_date, 
                                                @SalesOrder_status,
                                                @EMP_ID          ); ";

            salesorder.Parameters.AddWithValue("@SalesOrder_customer", txt_customername.Text );
            salesorder.Parameters.AddWithValue("@SalesOrder_date", SqlDbType.Date).Value = DateTime.Today;
            salesorder.Parameters.AddWithValue("@SalesOrder_status", "PAID"); 
            salesorder.Parameters.AddWithValue("@EMP_ID", regby);
            salesorder.CommandText = insert;
            salesorder.ExecuteNonQuery();
            salesorder.Parameters.Clear();
            con.Close();
        }
        public void saveSOITEM() {
            Frm_dashboard userID = new Frm_dashboard();
            string regby = userID.txt_employeeID.Text.ToString();


            for (int i = 0; i < dtg_toaddsalesorder.Rows.Count; i++)
            {
                con.ConnectionString = connectionString;
                SqlCommand soitem = new SqlCommand();
                soitem.Connection = con;
                con.Open();
                string insert = @"INSERT INTO      tbl_SALESORDER_item
                                                (   SalesOrder_no, 
                                                    ITEM_ID, 
                                                    SOItem_qty,
                                                    SOItem_amount,
                                                    SOItem_status   ) 
                                    VALUES      (   @SalesOrder_no, 
                                                    @ITEM_ID, 
                                                    @SOItem_qty,
                                                    @SOItem_amount   ,
                                                    @SOItem_status        ); ";

                soitem.Parameters.AddWithValue("@SalesOrder_no", Convert.ToInt32(txt_salesorderNO.Text));
                soitem.Parameters.AddWithValue("@ITEM_ID", dtg_toaddsalesorder.Rows[i].Cells[0].Value);
                soitem.Parameters.AddWithValue("@SOItem_qty", Convert.ToInt32(dtg_toaddsalesorder.Rows[i].Cells[3].Value));
                soitem.Parameters.AddWithValue("@SOItem_amount", Convert.ToDouble(dtg_toaddsalesorder.Rows[i].Cells[4].Value));
                soitem.Parameters.AddWithValue("@SOItem_status", "SOLD");
                soitem.CommandText = insert;
                soitem.ExecuteNonQuery();
                soitem.Parameters.Clear();
                con.Close();
            }
        }
        public void savePAYMENT()
        {
            Frm_dashboard userID = new Frm_dashboard();
            string regby = userID.txt_employeeID.Text.ToString();

            con.ConnectionString = connectionString;
            SqlCommand payment = new SqlCommand();
            payment.Connection = con;
            con.Open();
            string insert = @"INSERT INTO      tbl_SALESORDER_payment
                                            (   SalesOrder_no,
                                                SOPayment_subtotal, 
                                                SOPayment_vat,
                                                SOPayment_discount,
                                                SOPayment_totalpayable,
                                                SOPayment_totalcash,
                                                SOPayment_totalchange,
                                                SOPayment_date,
                                                EMP_ID    ) 
                                VALUES      (   @SalesOrder_no,
                                                @SOPayment_subtotal, 
                                                @SOPayment_vat,
                                                @SOPayment_discount,
                                                @SOPayment_totalpayable,
                                                @SOPayment_totalcash,
                                                @SOPayment_totalchange,
                                                @SOPayment_date,
                                                @EMP_ID           ); ";

            payment.Parameters.AddWithValue("@SalesOrder_no", txt_salesorderNO.Text);
            payment.Parameters.AddWithValue("@SOPayment_subtotal", Convert.ToDecimal(txt_subtotal.Text));
            payment.Parameters.AddWithValue("@SOPayment_vat", Convert.ToDecimal(txt_totalvat.Text));
            payment.Parameters.AddWithValue("@SOPayment_discount", Convert.ToDecimal(txt_totaldiscount.Text));
            payment.Parameters.AddWithValue("@SOPayment_totalpayable", Convert.ToDecimal(txt_TOTALPAYABLE.Text));
            payment.Parameters.AddWithValue("@SOPayment_totalcash", Convert.ToDecimal(txt_totalCASH.Text));
            payment.Parameters.AddWithValue("@SOPayment_totalchange", Convert.ToDecimal(txt_totalCHANGE.Text));
            payment.Parameters.AddWithValue("@SOPayment_date", SqlDbType.Date).Value = DateTime.Today;
            payment.Parameters.AddWithValue("@EMP_ID", regby);
            payment.CommandText = insert;
            payment.ExecuteNonQuery();
            payment.Parameters.Clear();
            con.Close();
        }
        // SQL // save SALES ORDER // PAYMENT -----------end

        // SAVE PAID SALES ORDER ORDER to DATABASE ?????????????????????????????????????????????????????????????????????????END

    }
}
