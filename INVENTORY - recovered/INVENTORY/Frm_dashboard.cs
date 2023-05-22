using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INVENTORY
{
    public partial class Frm_dashboard : Form
    {
        public Frm_dashboard()
        {
            InitializeComponent();

            datetimeTODAY();
            
        }

        public void datetimeTODAY()
        {
            txt_date.Text = (DateTime.Today).ToString();
        }

        //SET EMPLOYEE DASHBOARD // NOT ADMIN ------------------------------------------------------------------------start
        public void setNOTADMIN()
        {
            txt_employeeID.Text = Frm_login.SetValueForEmployeeID;
            txt_position.Text = Frm_login.SetValueForEmployeePosition;
            txt_employeename.Text = Frm_login.SetValueForEmployeeName;

            panel_adminbtn.Visible = false;
            submenu_admin.Visible = false;
        }
        public void setADMIN()
        {
            txt_employeeID.Text = Frm_login.SetValueForEmployeeID;
            txt_position.Text = Frm_login.SetValueForEmployeePosition;
            txt_employeename.Text = Frm_login.SetValueForEmployeeName;

            panel_adminbtn.Visible = true;
            submenu_admin.Visible = true;
        }
        //SET EMPLOYEE DASHBOARD ------------------------------------------------------------------------end

        //INVENTORY MENU -----------------------------------------------------------------------start
        private void btn_inventory_Click(object sender, EventArgs e)
        {
            hl_inventory.BackColor = Color.Khaki;

            if ( submenu_inventory.Visible == false ) {
                submenu_inventory.Visible = true;
            }
        }

        private void btn_itemdata_Click(object sender, EventArgs e)
        {
            btn_itemdata.BaseColor = Color.FromArgb(178, 175, 144);
            btn_itemdata.ForeColor = Color.White;

            Frm_Inventory_itemdata itemdata = new Frm_Inventory_itemdata();
            itemdata.TopLevel = false;
            itemdata.BringToFront();
            panel_view.Controls.Add(itemdata);
            itemdata.Show();
        }

        private void btn_managestocks_Click(object sender, EventArgs e)
        {
            btn_managestocks.BaseColor = Color.FromArgb(178, 175, 144);
            btn_managestocks.ForeColor = Color.White;

            Frm_Inventory_managestocks stock = new Frm_Inventory_managestocks();
            stock.TopLevel = false;
            stock.BringToFront();
            panel_view.Controls.Add(stock);
            stock.Show();
        }
        private void btn_qualityinspect_Click(object sender, EventArgs e)
        {
            btn_qualityinspect.BaseColor = Color.FromArgb(178, 175, 144);
            btn_qualityinspect.ForeColor = Color.White;

            Frm_Inventory_qualityinspection quality = new Frm_Inventory_qualityinspection();
            quality.TopLevel = false;
            quality.BringToFront();
            panel_view.Controls.Add(quality);
            quality.Show();
        }
        private void btn_inventoryrecord_Click(object sender, EventArgs e)
        {
            btn_inventoryrecord.BaseColor = Color.FromArgb(178, 175, 144);
            btn_inventoryrecord.ForeColor = Color.White;

            Frm_Inventory_records record = new Frm_Inventory_records();
            record.TopLevel = false;
            record.BringToFront();
            panel_view.Controls.Add(record);
            record.Show();
        }
        //INVENTORY MENU -----------------------------------------------------------------------end

        //SALES MENU ---------------------------------------------------------------------------start
        private void btn_Sales_Click(object sender, EventArgs e)
        {
            hl_Sales.BackColor = Color.Khaki;

            if (submenu_sale.Visible == false ) {
                submenu_sale.Visible = true;
            }
        }

        private void btn_salesorder_Click(object sender, EventArgs e)
        {
            btn_salesorder.BaseColor = Color.FromArgb(178, 175, 144);
            btn_salesorder.ForeColor = Color.White;

            Frm_Sales_salesorder salesorder = new Frm_Sales_salesorder();
            salesorder.TopLevel = false;
            salesorder.BringToFront();
            panel_view.Controls.Add(salesorder);
            salesorder.Show();
        }
        //SALES MENU ---------------------------------------------------------------------------end 

        //ADMINISTRATION MENU ---------------------------------------------------------------------------start
        private void btn_ADMIN_Click(object sender, EventArgs e)
        {
            hl_admin.BackColor = Color.Khaki;

            if (submenu_admin.Visible == false)
            {
                submenu_admin.Visible = true;
            }
        }

        private void btn_employeedata_Click(object sender, EventArgs e)
        {
            btn_employeedata.BaseColor = Color.FromArgb(178, 175, 144);
            btn_employeedata.ForeColor = Color.White;

            Frm_Admin_empdata empdata = new Frm_Admin_empdata();
            empdata.TopLevel = false;
            empdata.BringToFront();
            panel_view.Controls.Add(empdata);
            empdata.Show();
        }

        private void btn_employeerecords_Click(object sender, EventArgs e)
        {
            btn_employeerecords.BaseColor = Color.FromArgb(178, 175, 144);
            btn_employeerecords.ForeColor = Color.White;

            Frm_Admin_records records = new Frm_Admin_records();
            records.TopLevel = false;
            records.BringToFront();
            panel_view.Controls.Add(records);
            records.Show();
        }
        //ADMINISTRATION MENU ---------------------------------------------------------------------------end


        // EXIT FORM ///////////////////////////////////////////////////////////////////////////////////////////// 
        private void btn_dashEXIT_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Changes will not be saved. Do you want to exit?", "My Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Frm_dashboard dash = new Frm_dashboard();
                dash.Close();

                Frm_login login = new Frm_login();
                login.Show();
            }
            else
            {
            }
        }

        private void copyright_Click(object sender, EventArgs e)
        {

        }
        // EXIT FORM /////////////////////////////////////////////////////////////////////////////////////////////



    }
}
