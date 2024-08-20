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

public partial class ReductionEntry : System.Web.UI.Page
{
    DataClassesDataContext sonicoPrd = new DataClassesDataContext("SONIC_PRDConnectionString");
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["userID"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
            try
            {
                ((Label)Master.FindControl("label1")).Text = "Reduction Entry";

               
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
                    con.Close();
                }
            }
            ddlSection.Items.Insert(0, new ListItem("--Select Section--", "0"));
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

    protected void btn_rej_Click(object sender, EventArgs e)
    {
        int UpdSM = 0;
        
        if (txtDate.Text != " " && txtQty.Text != " " && ddlSection.SelectedIndex != 0)
        {
            if(Convert.ToInt32(txtAQty.Text) > Convert.ToInt32(txtQty.Text)) 
            {
                try
                {
                    //
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    String dateString = txtDate.Text;
                    // Exception: String was not recognized as a valid DateTime because the day of week was incorrect.
                    DateTime Edate = DateTime.ParseExact(dateString, "dd-MM-yyyy", provider);

                    //

                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        DataTable dtRej = new DataTable();
                        // dtRej = GetRecord();
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand("INSERT INTO [SONIC_PRD].[dbo].[T_RM_REJ_MASTER] ([T_RMREJM_SECTION] ,[T_RMREJM_QTY],[T_RMREJM_STATUS],[T_RMREJM_ENTRY_DT],[T_RMREJM_CREATED_BY] ,[T_RMREJM_CREATED_DT]) VALUES  (@Section          ,@Quantity    ,@Status      ,@Edate,@USER,@Cdate)"))
                        {

                            cmd.Parameters.AddWithValue("@Edate", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", provider)); cmd.Parameters.AddWithValue("@Section", ddlSection.SelectedValue);
                            cmd.Parameters.AddWithValue("@Quantity", txtQty.Text.ToString().Trim());
                            cmd.Parameters.AddWithValue("@Status", "ENTERED");
                            cmd.Parameters.AddWithValue("@Cdate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@USER", Session["userID"].ToString());

                            cmd.Connection = con;
                            int Ins_RMM = 0;
                            try
                            {
                                Ins_RMM = cmd.ExecuteNonQuery();
                                if (Ins_RMM > 0)
                                {
                                    string message = "alert('Rejection Entered!!')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Only alert Message');", true);
                                    //  ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);



                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            //    using (SqlCommand cmd1 = new SqlCommand("UPDATE[dbo].[T_RM_STOCK_MASTER] SET T_RMSM_REJECTION = @Rej, T_RMSM_CL_QTY = @clQty,T_RMSM_UPDATED_BY = @Uby , T_RMSM_UPDATED_DT = @Udt where ID = @id "))
                            //{
                            //    cmd1.Parameters.Clear();
                            //    cmd1.Parameters.AddWithValue("@id", dtRej.Rows[0]["ID"].ToString());
                            //    cmd1.Parameters.AddWithValue("@Rej", txtQty.Text);
                            //    if(!string.IsNullOrEmpty(dtRej.Rows[0]["T_RMSM_CL_QTY"].ToString()))
                            //    {
                            //        cmd1.Parameters.AddWithValue("@clQty", Convert.ToInt32(dtRej.Rows[0]["T_RMSM_CL_QTY"].ToString()) - (Convert.ToInt32(txtQty.Text)));

                            //    }
                            //    else
                            //    {
                            //        cmd1.Parameters.AddWithValue("@clQty", 0 - (Convert.ToInt32(txtQty.Text)));
                            //    }

                            //    cmd1.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                            //    cmd1.Parameters.AddWithValue("@Udt", DateTime.Now);


                            //    cmd1.Connection = con;

                            //    try
                            //    {
                            //        Ins_RMM = cmd.ExecuteNonQuery();

                            //        if (Ins_RMM > 0)
                            //        {
                            //            UpdSM = cmd1.ExecuteNonQuery();
                            //        }
                            //        if (Ins_RMM > 0 && UpdSM > 0)
                            //        {
                            //            trans.Commit();
                            //        }
                            //        else
                            //        {
                            //            trans.Rollback();
                            //            string message = "alert('Error  Please contact IT team!!')";
                            //            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            //        }


                            //    }

                            //    catch
                            //    {
                            //        string message = "alert('Error in  btn_rej_Click. Please contact IT team!!')";
                            //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            //    }







                            //    BindRejgrid();
                            //}

                            resetControls();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = "alert('Error in  btn_rej_Click. Please contact IT team!!')";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
           
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
}