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

public partial class ShearingRejectionEntry : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                ((Label)Master.FindControl("label1")).Text = "Shearing Rejection Entry";

               
                    try
                    {
                            loadItemMaster();


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

    protected void loadItemMaster()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                //using (SqlCommand cmd = new SqlCommand("select  T_IM_DESC +  ' '+'||' + ' '+ T_IM_SECTION +  ' '+'||' + ' '+ CONVERT(VARCHAR,T_IM_SHEARSIZE) +  ' '+'||' + ' '+ T_IM_LEAF_NO AS TEST from sonic_prd.dbo.T_ITEMMASTER order by T_IM_CODE  asc"))
                using (SqlCommand cmd = new SqlCommand("select * from sonic_prd.dbo.T_SHEARING_MASTER order by T_SM_CODE  asc"))

                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                  

                    ddlDesc.DataValueField = "T_SM_CODE";
                    ddlDesc.DataTextField = "T_SM_DESCRIPTION";
                    ddlDesc.DataSource = dt;
                    ddlDesc.DataBind();


                }
            }
            ddlDesc.Items.Insert(0, new ListItem("--Select Item--", "0"));
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
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo].[T_SHREJ_MASTER] WHERE  MONTH(SHREJ_CREATED_DT) = MONTH(GETDATE())"))
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
        int Ins_RejM = 0;
        int UpdShM = 0;
        if (txtDate.Text != " " && txtQty.Text != " " && ddlDesc.SelectedIndex != 0)
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
                    con.Open();
                    SqlTransaction trans = con.BeginTransaction();

                    DataTable dtRej = new DataTable();
                    dtRej = GetRecord();
                   

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[T_SHREJ_MASTER] ( [T_SREJ_CODE], [T_SREJ_UNIT],[T_SHREJ_ENTRY_DT], [T_SHREJ_CREATED_BY], [T_SHREJ_CREATED_DT], [T_SHREJ_UPDATED_BY], [T_SHREJ_UPDATED_DT]) VALUES (  @ShRejCode, @Unit,@Edate,@Cby,@Cdt,@Uby,@Udt",con,trans))
                    {

                        cmd.Parameters.AddWithValue("@ShRejCode", ddlDesc.SelectedValue);
                        cmd.Parameters.AddWithValue("@Unit", ddlUnit.SelectedValue); 
                        cmd.Parameters.AddWithValue("@Edate", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", provider));
                        cmd.Parameters.AddWithValue("@Cby", Session["userID"].ToString()); 
                        cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                        cmd.Parameters.AddWithValue("@Udt", DateTime.Now);

                        cmd.Connection = con;
                        
                       
                        using (SqlCommand cmd1 = new SqlCommand("UPDATE[dbo].[T_SHEARING_MASTER] SET T_SM_WEIGHT = @SmWt ,T_SM_UPDATED_BY = @Uby,T_SM_UPDATED_DT = @Udt where T_SM_CODE = @Code ",con))
                        {
                            cmd1.Parameters.Clear();
                            cmd1.Parameters.AddWithValue("@SmWt", Convert.ToDouble(dtRej.Rows[0]["T_SM_WEIGHT"].ToString()) - (Convert.ToDouble(txtQty.Text.Trim())* Convert.ToDouble(dtRej.Rows[0]["T_SM_UNIT_WEIGHT"].ToString())));
                            cmd1.Parameters.AddWithValue("@Rej", txtQty.Text);
                            cmd1.Parameters.AddWithValue("Code", dtRej.Rows[0]["T_SM_CODE"].ToString());

                            cmd1.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                            cmd1.Parameters.AddWithValue("@Udt", DateTime.Now);


                            cmd1.Connection = con;

                            try
                            {
                                Ins_RejM = cmd.ExecuteNonQuery();

                                if (Ins_RejM > 0)
                                {
                                    UpdShM = cmd1.ExecuteNonQuery();
                                }
                                if (Ins_RejM > 0 && UpdShM > 0)
                                {
                                    trans.Commit();
                                    string message = "alert('Shearing rejection Done')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                }
                                else
                                {
                                    trans.Rollback();
                                    string message = "alert('Error  Please contact IT team!!')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                }


                            }

                            catch
                            {
                                string message = "alert('Error in  btn_rej_Click. Please contact IT team!!')";
                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            }







                            BindRejgrid();
                        }

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


    protected DataTable GetRecord()
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo].[T_SHEARING_MASTER] WHERE  T_SM_CODE = @Code"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Code", ddlDesc.SelectedItem.Value.ToString());
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
            loadItemMaster();
            txtDate.Text = string.Empty;
            txtQty.Text = string.Empty;

        }
        catch (Exception ex)
        {
        }
    }

}