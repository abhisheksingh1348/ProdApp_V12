using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class LoginPage : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            Session.Clear();
            Session.Abandon();
        }
    }

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        loading.Visible = true;
        string msg = "";
        DataTable dt1 = new DataTable();

        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo].[T_USER_MASTER] WHERE T_UM_EMP_ID = @ID and T_UM_PASSWORD = @PWD "))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@ID", username.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@PWD", password.Text.Trim().ToString());
                    try
                    {
                        con.Open();
                        SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                        adp1.Fill(dt1);
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message.ToString();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Error3232: " + msg.Replace("'", "").Replace("\"", "") + "');", true);
                    }



                    ViewState["LoginData"] = dt1;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }


        if (dt1.Rows.Count > 0)
        {
            try
            {
                Session["userID"] = dt1.Rows[0]["T_UM_EMP_ID"].ToString();
                Session["username"] = dt1.Rows[0]["T_UM_NAME"].ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "alert('success');", true);
                Response.Redirect("Dashboard.aspx");
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Either UserID or Password is not valid: " + msg.Replace("'", "").Replace("\"", "") + "');", true);

            }



            // Response.Redirect("Dashboard.aspx");
        }

        else
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "alert('Either UserID or Password is not valid');", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg","alert(msg)", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Either UserID or Password is not valid: " + msg.Replace("'", "").Replace("\"", "") + "');", true);

        }
        loading.Visible=false;
    }
}