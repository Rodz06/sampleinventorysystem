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
    public partial class Frm_login : Form
    {
        public Frm_login()
        {
            InitializeComponent();
        }
        //My SQl Connection String
        string connectionString = Properties.Settings.Default.MyConnection;
        SqlConnection con = new SqlConnection();


        public static string SetValueForEmployeeID = "";
        public static string SetValueForEmployeePosition = "";
        public static string SetValueForEmployeeName = "";



        private void btn_login_Click(object sender, EventArgs e)
        {

        }

        //LOGIN verify ****************start
        private void verify()
        {
            con.ConnectionString = connectionString;
            con.Open();
            string select = @"SELECT        tbl_EMPLOYEEDATA.EMP_ID, 
                                            tbl_EMPLOYEEDATA.EMP_position,
                                            tbl_EMPLOYEEDATA.EMP_lastname,
                                            tbl_EMPLOYEEDATA.EMP_firstname,
                                            tbl_EMPLOYEEDATA_userdetail.EMPUser_name, 
                                            tbl_EMPLOYEEDATA_userdetail.EMPUser_password
                            FROM            tbl_EMPLOYEEDATA_userdetail 
                            INNER JOIN      tbl_EMPLOYEEDATA 
                            ON              tbl_EMPLOYEEDATA_userdetail.EMP_ID = tbl_EMPLOYEEDATA.EMP_ID";
            SqlCommand cmd = new SqlCommand(select, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                if (txt_userid.Text.Equals(dr["EMPUser_name"].ToString()) &&
                     txt_password.Text.Equals(dr["EMPUser_password"].ToString()) &&
                      (dr["EMP_position"].ToString().Equals("Admin") || dr["EMP_position"].ToString().Equals("ADMIN") || dr["EMP_position"].ToString().Equals("admin")))
                {
                    SetValueForEmployeeID = dr["EMP_ID"].ToString();
                    SetValueForEmployeePosition = dr["EMP_position"].ToString();
                    SetValueForEmployeeName = dr["EMP_firstname"].ToString() + " " + dr["EMP_lastname"].ToString();

                    Frm_dashboard dash = new Frm_dashboard();
                    dash.Show();
                    dash.setADMIN();
                    this.Close();
                }

                if (txt_userid.Text.Equals(dr["EMPUser_name"].ToString()) &&
                     txt_password.Text.Equals(dr["EMPUser_password"].ToString()) &&
                      (!dr["EMP_position"].ToString().Equals("Admin") || !dr["EMP_position"].ToString().Equals("ADMIN") || !dr["EMP_position"].ToString().Equals("admin")))
                {
                    SetValueForEmployeeID = dr["EMP_ID"].ToString();
                    SetValueForEmployeePosition = dr["EMP_position"].ToString();
                    SetValueForEmployeeName = dr["EMP_firstname"].ToString() + " " + dr["EMP_lastname"].ToString();

                    Frm_dashboard dash = new Frm_dashboard();
                    dash.Show();
                    dash.setNOTADMIN();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Username or password!");
                }
            }
            con.Close();
        }
        //LOGIN verify ****************end
    }
}
