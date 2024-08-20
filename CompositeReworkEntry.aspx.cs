using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;


public partial class CompositeReworkEntry : System.Web.UI.Page
{
    DataClassesDataContext sonicoPrd = new DataClassesDataContext("SONIC_PRDConnectionString");
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userID"] == null)
        {
            Response.Redirect("LoginPage.aspx");
        }
        if (!IsPostBack)
        {

           
            try
            {
                ((Label)Master.FindControl("label1")).Text = "Rework Entry";

               
                    try
                    {
                        LoadSection();
                       
                      //  BindRejgrid();

                    }
                    catch (Exception ex)
                    {
                    }

                

            }
            catch (Exception ex)
            {
            }
          
        }
    }

    
    protected void LoadSection()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo]. T_SECTION_MASTER"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp2 = new SqlDataAdapter(cmd);
                    DataTable dt2 = new DataTable();
                    adp2.Fill(dt2);
                    ddlSection.DataSource = dt2;
                    ddlSection.DataTextField = "T_SECTIONM_DIMN";
                    ddlSection.DataValueField = "T_SECTIONM_DIMN";
                    ddlSection.DataBind();

                    ddlToSection.DataSource = dt2;
                    ddlToSection.DataTextField = "T_SECTIONM_DIMN";
                    ddlToSection.DataValueField = "T_SECTIONM_DIMN";
                    ddlToSection.DataBind();
                    con.Close();
                }
            }
            ddlSection.Items.Insert(0, new ListItem("--Select Section--", "0"));
            ddlToSection.Items.Insert(0, new ListItem("--Select Section--", "0"));
        }
        catch (Exception ex)
        {
        }

    }

    protected void BindRejgrid()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo].[T_RM_REJ_MASTER] WHERE  MONTH(T_RMREJM_CREATED_DT) = MONTH(GETDATE())"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    //cmd.Parameters.AddWithValue("@BILLNO", txtBillNum.Text.Trim().ToString());
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    Gv_RejEntry.DataSource = dt1;
                    Gv_RejEntry.DataBind();
                    ViewState["REJ"] = dt1;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }

    }


  


    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Request.RawUrl);
        }
        
        catch (Exception ex)
        {

        }
    }

 


    protected DataTable GetRecord()
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo].[T_RM_STOCK_MASTER] WHERE  MONTH(T_RMSM_CREATED_DT) = MONTH(GETDATE()) and T_RMSM_SECTION = @Sec"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Sec", ddlSection.SelectedItem.Value.ToString());
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    adp1.Fill(dt);

                    con.Close();
                }
            }
        }
        catch (Exception ex) { }
        return dt;  
    }
    protected void resetControls()
    {
        try
        {
            LoadSection();
            txtDate.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtAQty.Text = string.Empty;    

        }
        catch (Exception ex)
        {
        }
    }


    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtAvailable = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT T_RMSM_CL_QTY  FROM [SONIC_PRD].[dbo].[T_RM_STOCK_MASTER] WHERE  MONTH(T_RMSM_CREATED_DT) = MONTH(GETDATE()) and T_RMSM_SECTION = @Sec"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Sec", ddlSection.SelectedItem.Value.ToString());
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    adp1.Fill(dtAvailable);

                    con.Close();
                }
            }
            if (dtAvailable.Rows.Count > 0) {
                txtAQty.Text = dtAvailable.Rows[0]["T_RMSM_CL_QTY"].ToString();
            }
            else
            {
                txtAQty.Text = "0";
            }

        }
        catch (Exception ex) { }
    }

   
    protected void btn_rew_Click(object sender, EventArgs e)
    {
        int Ins_RMM = 0;
        int Ins_RMW = 0;
        int Ins_RMWRK = 0;

        CultureInfo provider = CultureInfo.InvariantCulture;
        try
        {
            bool dte = checkDate();
            if (dte == true && !string.IsNullOrEmpty(txtDate.Text) && !string.IsNullOrEmpty(txtAQty.Text) && ddlSection.SelectedIndex != 0 &&
                !string.IsNullOrEmpty(txtQty.Text)  && ddlToSection.SelectedIndex != 0) {
                if (Convert.ToInt32(txtAQty.Text) > Convert.ToInt32(txtQty.Text))
                {
                    try
                    {
                        //
                        String dateString = txtDate.Text;
                        // Exception: String was not recognized as a valid DateTime because the day of week was incorrect.
                        DateTime Edate = DateTime.ParseExact(dateString, "dd-MM-yyyy", provider);

                        //

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            con.Open();
                            SqlTransaction trans = con.BeginTransaction();
                            //DataTable dtRej = new DataTable();
                            // dtRej = GetRecord();
                            
                           
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO [SONIC_PRD].[dbo].[T_RM_REJ_MASTER] ([T_RMREJM_SECTION] ,[T_RMREJM_QTY],[T_RMREJM_STATUS],[T_RMREJM_ENTRY_DT],[T_RMREJM_CREATED_BY] ,[T_RMREJM_CREATED_DT]) VALUES  (@Section          ,@Quantity    ,@Status      ,@Edate,@USER,@Cdate)",con,trans))
                            {

                                cmd.Parameters.AddWithValue("@Edate", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", provider)); cmd.Parameters.AddWithValue("@Section", ddlSection.SelectedValue);
                                cmd.Parameters.AddWithValue("@Quantity", txtQty.Text.ToString().Trim());
                                cmd.Parameters.AddWithValue("@Status", "ENTERED");
                                cmd.Parameters.AddWithValue("@Cdate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@USER", Session["userID"].ToString());

                                cmd.Connection = con;
                                try
                                {
                                    Ins_RMM = cmd.ExecuteNonQuery();
                                    //if (Ins_RMM > 0)
                                    //{
                                    //    string message = "alert('Rejection Entered!!')";
                                    //    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);




                                    //}
                                }
                                catch (Exception ex)
                                {
                                }

                            }
                            //////////
                            using (SqlCommand cmd1 = new SqlCommand("INSERT INTO [SONIC_PRD].[dbo].[T_RM_REWORK_MASTER] ([T_RMREWRK_FRMSECTION] ,[T_RMREWRK_TOSECTION],[T_RMREWRK_QTY],[T_RMREWRK_STATUS],[T_RMREWRK_ENTRY_DT],[T_RMREWRK_CREATED_BY] ,[T_RMREWRK_CREATED_DT]) VALUES  (@Section    ,@Sec       ,@Quantity    ,@Status      ,@Edate,@USER,@Cdate)", con,trans))
                            {

                                cmd1.Parameters.AddWithValue("@Edate", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", provider)); cmd1.Parameters.AddWithValue("@Section", ddlSection.SelectedValue);
                                cmd1.Parameters.AddWithValue("@Sec", ddlToSection.SelectedValue);
                                cmd1.Parameters.AddWithValue("@Quantity", txtQty.Text.ToString().Trim());
                                cmd1.Parameters.AddWithValue("@Status", "ENTERED");
                                cmd1.Parameters.AddWithValue("@Cdate", DateTime.Now);
                                cmd1.Parameters.AddWithValue("@USER", Session["userID"].ToString());

                                cmd1.Connection = con;
                                try
                                {
                                    Ins_RMWRK = cmd1.ExecuteNonQuery();
                                    //if (Ins_RMM > 0)
                                    //{
                                    //    string message = "alert('Rejection Entered!!')";
                                    //    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);




                                    //}
                                }
                                catch (Exception ex)
                                {
                                }

                            }

                            /////////

                            using (SqlCommand cmd2 = new SqlCommand("INSERT INTO [SONIC_PRD].[dbo].[T_RM_REW_MASTER]" +
                    " ([T_RMREWM_SECTION] ,[T_RMREWM_QTY],[T_RMREWM_STATUS],[T_RMREWM_ENTRY_DT],[T_RMREWM_CREATED_BY] ,[T_RMREWM_CREATED_DT]) VALUES  (@Section         ,@Quantity    ,@Status      ,@Edate,@USER,@Cdate)",con,trans))
                            {

                                //    using (SqlCommand cmd = new SqlCommand("INSERT INTO [SONIC_PRD].[dbo].[T_RM_REJ_MASTER] ([T_RMREJM_SECTION] ,[T_RMREJM_QTY],[T_RMREJM_STATUS],[T_RMREJM_ENTRY_DT],[T_RMREJM_CREATED_BY] ,[T_RMREJM_CREATED_DT]) VALUES  (@Section          ,@Quantity    ,@Status      ,@Edate,@USER,@Cdate)"))
                                //{ 
                                cmd2.Parameters.AddWithValue("@Edate", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", provider)); cmd2.Parameters.AddWithValue("@Section", ddlToSection.SelectedValue);
                               
                                cmd2.Parameters.AddWithValue("@Quantity", txtQty.Text.ToString().Trim());
                                cmd2.Parameters.AddWithValue("@Status", "ENTERED");
                                cmd2.Parameters.AddWithValue("@Cdate", DateTime.Now);
                                cmd2.Parameters.AddWithValue("@USER", Session["userID"].ToString());

                                
                                try
                                {
                                    Ins_RMW = cmd2.ExecuteNonQuery();
                                    if (Ins_RMW > 0 && Ins_RMM >0 && Ins_RMWRK > 0)
                                    {
                                        trans.Commit();
                                        string message = "alert('Rework Entered!!')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                        //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Only alert Message');", true);
                                        //  ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);



                                    }
                                    else
                                    {
                                        trans.Rollback();
                                        string message = "alert('Error in btn_rew_Click!!  Please contact IT team!')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                               
                            }

                            /////////
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = "alert('Error in  btn_rew_Click. Please contact IT team!!')";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                }

            }
            else
            {
                if (dte == false)
                {
                    string message = "alert('Please enter correct Date!!')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
        }
       catch (Exception ex)
        {

        }
    }

    protected Boolean checkDate()
    {
        CultureInfo provider = CultureInfo.InvariantCulture;

        bool dat = false;
        try
        {
            if (DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", provider) > DateTime.Now.Date)
            {
                dat = false;
            }
            else
            {
                dat = true;
            }


        }
        catch (Exception ex) { }
        return dat;
    }
}