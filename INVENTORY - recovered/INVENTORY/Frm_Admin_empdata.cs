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
    public partial class Frm_Admin_empdata : Form
    {
        public Frm_Admin_empdata()
        {
            InitializeComponent();
            EMPLOYEE_SCROLL.AutoScroll = false;
            positionLIST();
            datetimeTODAY();
            empLIST();
            position();
        }
        //My SQl Connection String
        string connectionString = Properties.Settings.Default.MyConnection;
        SqlConnection con = new SqlConnection();

        //date time --
        public void datetimeTODAY()
        {
            dt_empdob.Value = DateTime.Today;
        }

        // combo box position --
        public void position() {
            string select = "SELECT Position_name FROM tbl_EMPLOYEEDATA_position; ";
            con.Open();
            SqlCommand cmd = new SqlCommand(select, con);

            SqlDataReader DR = cmd.ExecuteReader();
            while (DR.Read()) {
                cmb_empposition.Items.Add(DR[0]);
            }
            con.Close();
        }
        // refresh 
        public void positionrefresh()
        {
            string select = "SELECT TOP 1 Position_name FROM tbl_EMPLOYEEDATA_position ORDER BY Position_no DESC; ";
            con.Open();
            SqlCommand cmd = new SqlCommand(select, con);
            SqlDataReader DR = cmd.ExecuteReader();
            while (DR.Read())
            {
                cmb_empposition.Items.Add(DR[0]);
            }
            con.Close();
        }







        // DATA GRIDVIEW ----------------------------------------------------------------------------------start
        public void empLIST() {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            string select = "SELECT EMP_ID, EMP_lastname, EMP_firstname,EMP_position, EMP_regdate, EMP_regby FROM tbl_EMPLOYEEDATA ORDER BY EMP_ID DESC; ";
            cmd.CommandText = select;
            SqlDataAdapter da = new SqlDataAdapter(select, con);
            SqlCommandBuilder cbuilder = new SqlCommandBuilder(da);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);
            bindingSource_empLIST.DataSource = table;

            dtg_empLIST.ReadOnly = true;
            dtg_empLIST.DataSource = bindingSource_empLIST;
        }
        // DATA GRIDVIEW ----------------------------------------------------------------------------------end

        // NEW MENU BUTTON ///////////////////////////////////////////////////////////////////////////start
        private void btn_NEW_Click(object sender, EventArgs e)
        {
            if (txt_empid.Text.Length > 1)
            {
                if (MessageBox.Show("Previews activity will not be undone. Do you want to continue?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    readyonlyFALSE();

                    btn_empSAVE.Visible = true;
                    btn_empCANCEL.Visible = true;

                    dtg_empLIST.Enabled = false;
                    txt_searchbox.Visible = false;

                    EMPID();
                    btn_updateSAVE.Visible = false;
                    btn_updateCANCEL.Visible = false;
                    label_update.Visible = false;
                    
                    clear();
                    panel_user.Visible = false;
                }
                else { }
            }
            else {
                btn_NEW.Enabled = false;
                readyonlyFALSE();

                btn_empSAVE.Visible = true;
                btn_empCANCEL.Visible = true;

                dtg_empLIST.Enabled = false;
                txt_searchbox.Visible = false;

                EMPID();
            }
        }
        // CLEAR *********************************************************start
        private void clear() {
            txt_empln.Text = "";
            txt_empfn.Text = "";
            txt_empmn.Text = "";

            dt_empdob.Value = DateTime.Today;
            rb_empfemale.Checked = false;
            rb_empmale.Checked = false;

            cmb_empposition.Text = "";
            cmb_empstatus.Text = "";

            txt_empaddress.Text = "";
            txt_empcontact.Text = "";

            txt_empusername.Text = "";
            txt_emppassword.Text = "";

            txt_userregby.Text = "";
            txt_userregdate.Text = "";
        }
        // CLEAR *********************************************************end
        // NEW MENU BUTTON ///////////////////////////////////////////////////////////////////////////end


        // read only == true *********************************************************start
        public void readyonlyTRUE()
        {
            txt_empid.ReadOnly = true;
            txt_empid.ReadOnly = true;
            txt_empln.ReadOnly = true;
            txt_empfn.ReadOnly = true;
            txt_empmn.ReadOnly = true;
            dt_empdob.Enabled = false;
            rb_empmale.Enabled = false;
            rb_empfemale.Enabled = false;

            txt_empaddress.ReadOnly = true;
            txt_empcontact.ReadOnly = true;

            txt_empusername.ReadOnly = true;
            txt_emppassword.ReadOnly = true;
            cmb_empposition.Enabled = false;
            cmb_empstatus.Enabled = false;
        }
        // read only == true *********************************************************end
        // READ ONLY == FALSE --
        public void readyonlyFALSE()
        {
            txt_empid.ReadOnly = false;
            txt_empln.ReadOnly = false;
            txt_empfn.ReadOnly = false;
            txt_empmn.ReadOnly = false;
            dt_empdob.Enabled = true;
            rb_empmale.Enabled = true;
            rb_empfemale.Enabled = true;

            txt_empaddress.ReadOnly = false;
            txt_empcontact.ReadOnly = false;

            txt_empusername.ReadOnly = false;
            txt_emppassword.ReadOnly = false;
            cmb_empposition.Enabled = true;
            cmb_empstatus.Enabled = true;
        }
        // READ ONLY == FALSE --

        // SAVE NEW EMPLOYEE RECORD ????????????????????????????????????????????????????????????????? START
        private void btn_empSAVE_Click(object sender, EventArgs e)
        {
            ERROR();

            if (txt_empln.Text.Length.Equals(0) || txt_empln.Text.Length > 150 ||
                txt_empfn.Text.Length.Equals(0) || txt_empfn.Text.Length > 150 ||
                dt_empdob.Value == DateTime.Today || (rb_empmale.Checked == false && rb_empfemale.Checked == false) ||

                txt_empaddress.Text.Length.Equals(0) || txt_empaddress.Text.Length > 250 ||
                txt_empcontact.Text.Length.Equals(0) || txt_empcontact.Text.Length > 20 ||

                cmb_empposition.SelectedIndex.Equals(-0))
            {
                NULL();
                ERROR();
                MessageBox.Show("Record couldn't be save. Make sure to input valid data.");
            }
            else {
                if (MessageBox.Show("New employee record will be added. Do you want to continue?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    NULL();
                    addempdata();
                    addempdata_contact();
                    addempdata_userdetail();

                    errorHIDE();
                    empLIST();
                    readyonlyTRUE();
                    MessageBox.Show("New Employee record succesfully added.");
                    
                    btn_empSAVE.Visible = false;
                    btn_empCANCEL.Visible = false;
                    btn_generateuser.Visible = false;
                    btn_NEW.Enabled = true;

                    newADDEDEMPLOYEE();
                    dtg_empLIST.Enabled = true;
                }
            }
        }

        // if empty containers --
        public void ERROR() {
            if (txt_empln.Text.Length.Equals(0) || txt_empln.Text.Length > 150) { error_empln.Visible = true; }
            if (txt_empfn.Text.Length.Equals(0) || txt_empfn.Text.Length > 150) { error_empfn.Visible = true; }
            if (txt_empmn.Text.Length > 150) { error_empln.Visible = true; }
            if (dt_empdob.Value == DateTime.Today) { error_empdob.Visible = true; }
            if (rb_empmale.Checked == false && rb_empfemale.Checked == false) { error_empgender.Visible = true; }
            
            if (txt_empaddress.Text.Length.Equals(0) || txt_empaddress.Text.Length > 250) { error_empadd.Visible = true; }
            if (txt_empcontact.Text.Length.Equals(0) || txt_empcontact.Text.Length > 20) { error_empcontact.Visible = true; }
            
            if (cmb_empposition.SelectedIndex.Equals(-0)) { error_empposition.Visible = true; }
        }
        // if empty containers --

        // NULL CONTENT / N/A --
        public void NULL() {
            if (txt_empmn.Text.Length.Equals(0)) { txt_empmn.Text = "N/A"; }

            if (txt_empusername.Text.Length.Equals(0)) { txt_empusername.Text = "N/A"; }
            if (txt_emppassword.Text.Length.Equals(0)) { txt_emppassword.Text = "N/A"; }
        }
        // NULL CONTENT / N/A --

        // ERROR HIDE --
        public void errorHIDE() {
            error_empln.Visible = false; 
            error_empfn.Visible = false; 
            error_empln.Visible = false;
            error_empdob.Visible = false; 
            error_empgender.Visible = false; 

            error_empadd.Visible = false; 
            error_empcontact.Visible = false; 

            error_empposition.Visible = false;
            error_empposition.Visible = false;
        }
        // ERROR HIDE --

        // CLEAR DATA --
        public void CLEAR() {
            txt_empid.Text = "";
            txt_empln.Text = "";
            txt_empfn.Text = "";
            txt_empmn.Text = "";
            dt_empdob.Value = DateTime.Today;
            rb_empmale.Checked = false;
            rb_empfemale.Checked = false;

            txt_empaddress.Text = "";
            txt_empcontact.Text = "";

            txt_empusername.Text = "";
            txt_emppassword.Text = "";
        }
        // CLEAR DATA --



        // Select New Added Employee ----------------------------------------------------------------start
        public void newADDEDEMPLOYEE()
        {
            string select = "SELECT TOP 1 EMP_ID FROM tbl_EMPLOYEEDATA ORDER BY EMP_ID DESC; ";
            con.Open();
            SqlCommand cmd = new SqlCommand(select, con);
            SqlDataReader DR = cmd.ExecuteReader();
            while (DR.Read())
            {
                dtg_empLIST.SelectedRows.Equals(DR[0]);
            }
            con.Close();
        }
        // Select New Added Employee ----------------------------------------------------------------end


        // SQl // ADD TO DATABASE ---------------------------------------------------------start
        public void addempdata()
        {
            //gender
            string gender = "";
            if (rb_empmale.Checked == true) { gender = "MALE"; }
            if (rb_empfemale.Checked == true) { gender = "FEMALE"; }

            Frm_dashboard userID = new Frm_dashboard();
            string regby = userID.txt_employeeID.Text.ToString();

            con.ConnectionString = connectionString;
            SqlCommand empdata = new SqlCommand();
            empdata.Connection = con;
            con.Open();
            string insert1 = @"INSERT INTO      tbl_EMPLOYEEDATA 
                                            (   EMP_ID, 
                                                EMP_lastname, 
                                                EMP_firstname, 
                                                EMP_middlename, 
                                                EMP_dob, 
                                                EMP_gender, 
                                                EMP_position,
                                                EMP_status,
                                                EMP_regdate,
                                                EMP_regby ) 
                                VALUES      (   @EMP_ID, 
                                                @EMP_lastname, 
                                                @EMP_firstname, 
                                                @EMP_middlename, 
                                                @EMP_dob, 
                                                @EMP_gender, 
                                                @EMP_position,
                                                @EMP_status,
                                                @EMP_regdate,
                                                @EMP_regby ); ";

            empdata.Parameters.AddWithValue("@EMP_ID", txt_empid.Text);
            empdata.Parameters.AddWithValue("@EMP_lastname", txt_empln.Text);
            empdata.Parameters.AddWithValue("@EMP_firstname", txt_empfn.Text);
            empdata.Parameters.AddWithValue("@EMP_middlename", txt_empmn.Text);
            empdata.Parameters.AddWithValue("@EMP_dob", SqlDbType.Date).Value = dt_empdob.Value.Date;
            empdata.Parameters.AddWithValue("@EMP_gender", gender);
            empdata.Parameters.AddWithValue("@EMP_position", cmb_empposition.Text);
            empdata.Parameters.AddWithValue("@EMP_status", cmb_empstatus.Text);
            empdata.Parameters.AddWithValue("@EMP_regdate", SqlDbType.Date).Value = DateTime.Today;
            empdata.Parameters.AddWithValue("@EMP_regby", regby);
            empdata.CommandText = insert1;
            empdata.ExecuteNonQuery();
            empdata.Parameters.Clear();
            con.Close();
        }

        public void addempdata_contact()
        {
            con.ConnectionString = connectionString;
            SqlCommand empdata_contact = new SqlCommand();
            empdata_contact.Connection = con;
            con.Open();
            string insert2 = @"INSERT INTO      tbl_EMPLOYEEDATA_contact
                                            (   EMPContact_address, 
                                                EMPContact_number ,
                                                EMP_ID  ) 
                                VALUES      (   @EMPContact_address, 
                                                @EMPContact_number ,
                                                @EMP_ID     ); ";

            empdata_contact.Parameters.AddWithValue("@EMPContact_address", txt_empaddress.Text);
            empdata_contact.Parameters.AddWithValue("@EMPContact_number", txt_empcontact.Text);
            empdata_contact.Parameters.AddWithValue("@EMP_ID", txt_empid.Text);
            empdata_contact.CommandText = insert2;
            empdata_contact.ExecuteNonQuery();
            empdata_contact.Parameters.Clear();
            con.Close();
        }

        public void addempdata_userdetail()
        {
            Frm_dashboard userID = new Frm_dashboard();
            string regby = userID.txt_employeeID.Text.ToString();

            con.ConnectionString = connectionString;
            SqlCommand empdata_userdetail = new SqlCommand();
            empdata_userdetail.Connection = con;
            con.Open();
            string insert3 = @"INSERT INTO      tbl_EMPLOYEEDATA_userdetail
                                            (   EMPUser_name, 
                                                EMPUser_password,
                                                EMP_ID      ) 
                                VALUES      (   @EMPUser_name, 
                                                @EMPUser_password,
                                                @EMP_ID     ); ";

            if (txt_empusername.Text.Length.Equals(0))
            {
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_name", "N/A");
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_password", "N/A");
                empdata_userdetail.Parameters.AddWithValue("@EMP_ID", txt_empid.Text);
                empdata_userdetail.CommandText = insert3;
                empdata_userdetail.ExecuteNonQuery();
                empdata_userdetail.Parameters.Clear();
            }
            if (txt_empusername.Text.Length > 1)
            {
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_name", txt_empusername.Text);
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_password", txt_emppassword.Text);
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_regdate", DateTime.Today);
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_regby", regby);
                empdata_userdetail.Parameters.AddWithValue("@EMP_ID", txt_empid.Text);
                empdata_userdetail.CommandText = insert3;
                empdata_userdetail.ExecuteNonQuery();
                empdata_userdetail.Parameters.Clear();

            }
            con.Close();
        }

        // SQl // ADD TO DATABASE ---------------------------------------------------------end

        //generate EMPLOYEE ID --
        public void EMPID() {
            con.ConnectionString = connectionString;
            SqlCommand empid = new SqlCommand();
            empid.Connection = con;

            con.Open();
            string id = "SELECT COUNT(EMP_ID) +1 FROM tbl_EMPLOYEEDATA;";
            empid.CommandText = id;
            SqlDataReader dr = empid.ExecuteReader();
            while (dr.Read())
            {
                int value = int.Parse(dr[0].ToString());
                if (value.ToString().Length == 1) { txt_empid.Text = "01000000" + String.Format(value.ToString()); }
                if (value.ToString().Length == 2) { txt_empid.Text = "0100000" + String.Format(value.ToString()); }
                if (value.ToString().Length == 3) { txt_empid.Text = "010000" + String.Format(value.ToString()); }
                if (value.ToString().Length == 4) { txt_empid.Text = "01000" + String.Format(value.ToString()); }
                if (value.ToString().Length == 5) { txt_empid.Text = "0100" + String.Format(value.ToString()); }
                if (value.ToString().Length == 6) { txt_empid.Text = "010" + String.Format(value.ToString()); }
            }
            con.Close();
        }

        //generate EMPLOYEE ID --

        // generate AGE --
        private void dt_empdob_ValueChanged(object sender, EventArgs e)
        {
            int age = DateTime.Now.Year - dt_empdob.Value.Year;
            txt_empage.Text = Convert.ToInt32(age).ToString();
        }

        //define new position--
        private void cmb_empposition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_empposition.SelectedIndex.Equals(0))
            {
                panel_employeedata.Enabled = false;
                panel_empgrid.Visible = false;
                panel_newposition.Visible = true;
            }
        }
        // SAVE NEW EMPLOYEE RECORD ????????????????????????????????????????????????????????????????? END


        // CANCEL NEW BUTTON ???????????????????????????????????????????????????????????????????????? START
        private void btn_empCANCEL_Click(object sender, EventArgs e)
        {
            if (txt_empid.Text.Length > 1)
            {
                if (MessageBox.Show("Record will not be save. Are you sure to cancel?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CLEAR();
                    errorHIDE();
                    readyonlyTRUE();
                    dtg_empLIST.Enabled = true;

                    btn_empSAVE.Visible = false;
                    btn_empCANCEL.Visible = false;

                    btn_generateuser.Visible = false;
                    panel_user.Visible = false;
                }
                else { }

            }
            else {
                CLEAR();
                errorHIDE();
                readyonlyTRUE();
                dtg_empLIST.Enabled = true;

                btn_empSAVE.Visible = false;
                btn_empCANCEL.Visible = false;
            }
        }


        // CANCEL NEW BUTTON ????????????????????????????????????????????????????????????????????????? END





        // DATA GRID VIEW CELLCLICK ????????????????????????????????????????????????????????????????? START

        // set data using INNER JOIN
        public void setDATA() {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            string selectemp = @"SELECT         tbl_EMPLOYEEDATA.*, 
                                                tbl_EMPLOYEEDATA_contact.EMPContact_address, 
                                                tbl_EMPLOYEEDATA_contact.EMPContact_number, 
                                                tbl_EMPLOYEEDATA_userdetail.EMPUser_name, 
                                                tbl_EMPLOYEEDATA_userdetail.EMPUser_password, 
                                                tbl_EMPLOYEEDATA_userdetail.EMPUser_regdate, 
                                                tbl_EMPLOYEEDATA_userdetail.EMPUser_regby
                                FROM            tbl_EMPLOYEEDATA 
                                INNER JOIN      tbl_EMPLOYEEDATA_contact 
                                ON              tbl_EMPLOYEEDATA.EMP_ID = tbl_EMPLOYEEDATA_contact.EMP_ID 
                                INNER JOIN      tbl_EMPLOYEEDATA_userdetail 
                                ON              tbl_EMPLOYEEDATA.EMP_ID = tbl_EMPLOYEEDATA_userdetail.EMP_ID
						        WHERE           tbl_EMPLOYEEDATA.EMP_ID ='" + txt_empid.Text + "'";

            con.Open();
            cmd.CommandText = selectemp;
            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                while (myReader.Read())
                {
                    txt_empln.Text = myReader["EMP_lastname"].ToString();
                    txt_empfn.Text = myReader["EMP_firstname"].ToString();
                    txt_empmn.Text = myReader["EMP_middlename"].ToString();
                    dt_empdob.Value = Convert.ToDateTime(myReader["EMP_dob"]).Date;
                    if (myReader["EMP_gender"].ToString().Equals("MALE")) { rb_empmale.Checked = true; }
                    else { rb_empfemale.Checked = true; }
                    cmb_empposition.Text = myReader["EMP_position"].ToString();
                    cmb_empstatus.Text = myReader["EMP_status"].ToString();


                    txt_empaddress.Text = (myReader["EMPContact_address"].ToString());
                    txt_empcontact.Text = (myReader["EMPContact_number"].ToString());


                    txt_empusername.Text = myReader["EMPUser_name"].ToString();
                    txt_emppassword.Text = myReader["EMPUser_password"].ToString();
                    txt_userregdate.Text = myReader["EMPUser_regdate"].ToString();
                    txt_userregby.Text = myReader["EMPUser_regby"].ToString();
                }
            }
            con.Close();
        }

        private void dtg_empLIST_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txt_empid.Text = dtg_empLIST.Rows[e.RowIndex].Cells[0].Value.ToString();

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

                btn_empSAVE.Visible = false;
                btn_empCANCEL.Visible = false;

                btn_updateSAVE.Visible = true;
                btn_updateCANCEL.Visible = true;

                dtg_empLIST.Enabled = false;
                txt_searchbox.Visible = false;

                label_update.Visible = false;

                if (txt_empusername.Text.Equals("N/A"))
                {
                    txt_empusername.ReadOnly = true;
                    txt_emppassword.ReadOnly = true;

                    btn_generateuser.Visible = true;
                }
                else {
                    txt_empusername.ReadOnly = false;
                    txt_emppassword.ReadOnly = false;
                    panel_user.Visible = false;

                    btn_generateuser.Visible = false;
                }
            }
            else { }
        }

        // SQl // UPDATE TO DATABASE ---------------------------------------------------------start
        public void UPDATEempdata() {
            //gender
            string gender = "";
            if (rb_empmale.Checked == true) { gender = "MALE"; }
            if (rb_empfemale.Checked == true) { gender = "FEMALE"; }

            Frm_dashboard userID = new Frm_dashboard();
            string regby = userID.txt_employeeID.Text.ToString();

            con.ConnectionString = connectionString;
            SqlCommand empdata = new SqlCommand();
            empdata.Connection = con;
            con.Open();
            string update1 = @"UPDATE      tbl_EMPLOYEEDATA 
                               SET         EMP_lastname = @EMP_lastname, 
                                           EMP_firstname = @EMP_firstname, 
                                           EMP_middlename = @EMP_middlename, 
                                           EMP_dob = @EMP_dob, 
                                           EMP_gender = @EMP_gender, 
                                           EMP_position = @EMP_position,
                                           EMP_status = @EMP_status,
                                           EMP_regdate = @EMP_regdate,
                                           EMP_regby =  @EMP_regby
                                WHERE      EMP_ID = '" + txt_empid.Text + "'";

            empdata.Parameters.AddWithValue("@EMP_ID", txt_empid.Text);
            empdata.Parameters.AddWithValue("@EMP_lastname", txt_empln.Text);
            empdata.Parameters.AddWithValue("@EMP_firstname", txt_empfn.Text);
            empdata.Parameters.AddWithValue("@EMP_middlename", txt_empmn.Text);
            empdata.Parameters.AddWithValue("@EMP_dob", SqlDbType.Date).Value = dt_empdob.Value.Date;
            empdata.Parameters.AddWithValue("@EMP_gender", gender);
            empdata.Parameters.AddWithValue("@EMP_position", cmb_empposition.Text);
            empdata.Parameters.AddWithValue("@EMP_status", cmb_empstatus.Text);
            empdata.Parameters.AddWithValue("@EMP_regdate", SqlDbType.Date).Value = DateTime.Today;
            empdata.Parameters.AddWithValue("@EMP_regby", regby);
            empdata.CommandText = update1;
            empdata.ExecuteNonQuery();
            empdata.Parameters.Clear();
            con.Close();
        }
        public void UPDATEempdata_contact() {
            con.ConnectionString = connectionString;
            SqlCommand empdata_contact = new SqlCommand();
            empdata_contact.Connection = con;
            con.Open();
            string update2 = @"UPDATE      tbl_EMPLOYEEDATA_contact
                               SET         EMPContact_address = @EMPContact_address, 
                                           EMPContact_number = @EMPContact_number
                               WHERE       EMP_ID = '" + txt_empid.Text + "'";

            empdata_contact.Parameters.AddWithValue("@EMPContact_address", txt_empaddress.Text);
            empdata_contact.Parameters.AddWithValue("@EMPContact_number", txt_empcontact.Text);
            empdata_contact.Parameters.AddWithValue("@EMP_ID", txt_empid.Text);
            empdata_contact.CommandText = update2;
            empdata_contact.ExecuteNonQuery();
            empdata_contact.Parameters.Clear();
            con.Close();
        }
        public void UPDATEempdata_userdetail() {
            Frm_dashboard userID = new Frm_dashboard();
            string regby = userID.txt_employeeID.Text.ToString();

            con.ConnectionString = connectionString;
            SqlCommand empdata_userdetail = new SqlCommand();
            empdata_userdetail.Connection = con;
            con.Open();
            string update3 = @"UPDATE      tbl_EMPLOYEEDATA_userdetail
                               SET         EMPUser_name = @EMPUser_name, 
                                           EMPUser_password = @EMPUser_password
                               WHERE       EMP_ID = '" + txt_empid.Text + "'";
            
            if (txt_empusername.Text.Equals("N/A"))
            {
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_name", "N/A");
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_password", "N/A");
                empdata_userdetail.Parameters.AddWithValue("@EMP_ID", txt_empid.Text);
                empdata_userdetail.CommandText = update3;
                empdata_userdetail.ExecuteNonQuery();
                empdata_userdetail.Parameters.Clear();
            }
            else {
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_name", txt_empusername.Text);
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_password", txt_emppassword.Text);
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_regdate", DateTime.Today);
                empdata_userdetail.Parameters.AddWithValue("@EMPUser_regby", regby);
                empdata_userdetail.Parameters.AddWithValue("@EMP_ID", txt_empid.Text);
                empdata_userdetail.CommandText = update3;
                empdata_userdetail.ExecuteNonQuery();
                empdata_userdetail.Parameters.Clear();

            }
            con.Close();
        }
        // SQl // UPDATE TO DATABASE ---------------------------------------------------------end
        

        private void btn_updateSAVE_Click(object sender, EventArgs e)
        {
            ERROR();

            if (txt_empln.Text.Length.Equals(0) || txt_empln.Text.Length > 150 ||
                txt_empfn.Text.Length.Equals(0) || txt_empfn.Text.Length > 150 ||
                dt_empdob.Value == DateTime.Today || (rb_empmale.Checked == false && rb_empfemale.Checked == false) ||

                txt_empaddress.Text.Length.Equals(0) || txt_empaddress.Text.Length > 250 ||
                txt_empcontact.Text.Length.Equals(0) || txt_empcontact.Text.Length > 20 ||

                cmb_empposition.SelectedIndex.Equals(-0))
            {
                NULL();
                ERROR();
                MessageBox.Show("Record couldn't be updated. Make sure to input valid data.");
            }
            else
            {
                if (MessageBox.Show("Employee record will be updated. Do you want to continue?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    NULL();
                    UPDATEempdata();
                    UPDATEempdata_contact();
                    UPDATEempdata_userdetail();

                    errorHIDE();
                    empLIST();
                    readyonlyTRUE();
                    MessageBox.Show("Employee record succesfully Updated.");

                    CLEAR();
                    btn_updateSAVE.Visible = false;
                    btn_updateCANCEL.Visible = false;
                    btn_generateuser.Visible = false;

                    dtg_empLIST.Enabled = true;
                    btn_generateuser.Visible = false;
                }
            }
        }


        // UPDATE RECORD ???????????????????????????????????????????????????????????????????????????? END



        // CANCEL UPDATE BUTTON ???????????????????????????????????????????????????????????????????????? START
        private void btn_updateCANCEL_Click(object sender, EventArgs e)
        {
            if (txt_empid.Text.Length > 1)
            {
                if (MessageBox.Show("Record will not be updated. Are you sure to cancel?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CLEAR();
                    errorHIDE();
                    readyonlyTRUE();
                    dtg_empLIST.Enabled = true;

                    btn_updateSAVE.Visible = false;
                    btn_updateCANCEL.Visible = false;
                    btn_generateuser.Visible = false;

                    panel_user.Visible = false;
                }
                else { }

            }
            else
            {
                CLEAR();
                errorHIDE();
                readyonlyTRUE();
                dtg_empLIST.Enabled = true;

                btn_updateSAVE.Visible = false;
                btn_updateCANCEL.Visible = false;
            }
        }

        // CANCEL UPDATE BUTTON ????????????????????????????????????????????????????????????????????????? END


        // GENERATE USER NAME AND PASSWORD BUTTON ????????????????????????????????????????????????????????????????????????? START
        private void btn_generateuser_Click(object sender, EventArgs e)
        {
           
        }
        // GENERATE USER NAME AND PASSWORD BUTTON ????????????????????????????????????????????????????????????????????????? END




        // DEFINE NEW POSITION ????????????????????????????????????????????????????????????????????????? START
        private void btn_SAVEposition_Click(object sender, EventArgs e)
        {
            if (txt_position.Text.Length.Equals(0) || txt_position.Text.Length > 20)
            {
                MessageBox.Show("Could not be saved. Make sure to input valid data.");
            }
            else
            {
                if (MessageBox.Show("New employee Position will be added. Do you want to continue?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    addPOSITION();
                    MessageBox.Show("New Employee Position successfully added.");
                    positionLIST();

                    txt_position.Text = " ";
                }
                else { }
            }
        }
        
        // data grid view --
        public void positionLIST()
        {
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            string select = "SELECT * FROM tbl_EMPLOYEEDATA_position; ";
            cmd.CommandText = select;
            SqlDataAdapter da = new SqlDataAdapter(select, con);
            SqlCommandBuilder cbuilder = new SqlCommandBuilder(da);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(table);
            bindingSource_POSITION.DataSource = table;

            dtg_position.ReadOnly = true;
            dtg_position.DataSource = bindingSource_POSITION;
        }
        //SQL COMMAND --
        public void addPOSITION()
        {
            con.ConnectionString = connectionString;
            SqlCommand position = new SqlCommand();
            position.Connection = con;
            con.Open();
            string insert = @"INSERT INTO      tbl_EMPLOYEEDATA_position
                                            (   Position_name   ) 
                                VALUES      (   @Position_name   ); ";

            position.Parameters.AddWithValue("@Position_name", txt_position.Text);
            position.CommandText = insert;
            position.ExecuteNonQuery();
            position.Parameters.Clear();
            con.Close();
        }

        private void btn_EXIT_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Input will not be save. Do you want to Exit?", "My System", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                panel_employeedata.Enabled = true;
                positionrefresh();

                panel_empgrid.Visible = true;
                panel_newposition.Visible = false;
            }
        }
        
        // DEFINE NEW POSITION ????????????????????????????????????????????????????????????????????????? END









    }
}
