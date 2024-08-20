using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Globalization;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public partial class ShearingApproval : System.Web.UI.Page
{
    string ShNo = "";
    string SNo = "";
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
        if (!IsPostBack)
        {
            try
            {
                ((Label)Master.FindControl("label1")).Text = "Shearing Entry";
                
            }
            catch (Exception ex)
            {
            }

        }

    }

    protected void BindData()
    {
        try
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT T_SM_SHR_NO as Shear_No,T_SM_UNIT as Unit,T_SM_DATE as ShearDate,T_SM_STATUS as Status,T_SM_CREATED_BY as Entered_By,T_SM_CREATED_DT as Entered_On,T_SM_APPROVED_BY as Approved_By,T_SM_APPROVED_DT as Approved_On   FROM [SONIC_PRD].[dbo].[T_SHEARING_MASTER] where T_SM_STATUS = @Stat")) 
                    //"WHERE  MONTH(T_RMSM_CREATED_DT) = MONTH(GETDATE())"))
                {
                    cmd.Parameters.AddWithValue("@Stat", ddlType.SelectedValue);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    adp1.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        gv_Data.DataSource = dt;
                        gv_Data.DataBind();
                    }


                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void BindApprovalGrid(string ShNo)
    {
        try
        {
            
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT T_SM_SHR_NO as Shear_No,T_SM_CODE as Code,T_SM_DESC as Descrp , T_SM_SECTION as Section,T_SM_LEAF_NO as LeafNo,[T_SM_QTY] as Qty,[T_SM_UNIT_WEIGHT] as UnitWeight      ,[T_SM_WEIGHT] as Weight" +
                    "  FROM [SONIC_PRD].[dbo].[T_SHEARING_MASTER] where T_SM_SHR_NO = @Sno"))
                //"WHERE  MONTH(T_RMSM_CREATED_DT) = MONTH(GETDATE())"))
                {
                    cmd.Parameters.AddWithValue("@Sno", ShNo);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    adp1.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }


                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }


    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedIndex != 0)
        {
            GridView1.DataSource=null;
            GridView1.DataBind();   
            BindData();
        }

     
    }

    protected void gv_Data_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();

        GridViewRow row = gv_Data.SelectedRow;

        row.Font.Bold = true;

        row.BackColor = System.Drawing.Color.Yellow;
        row.ForeColor = System.Drawing.Color.Black;
        row.Font.Bold = true;
        ViewState["SNo"] = row.Cells[1].Text;
       
        BindApprovalGrid(ViewState["SNo"].ToString());
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count>0)
        {
            try
            {
               

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("UPDATE T_SHEARING_MASTER SET T_SM_STATUS = @Status, T_SM_APPROVED_BY = @Aby,T_SM_APPROVED_DT = @Adt where [T_SM_SHR_NO]=@pShNo "))
                    {
                        cmd.Parameters.AddWithValue("@pShNo", ViewState["SNo"].ToString());
                        cmd.Parameters.AddWithValue("@Status", "APPROVED");
                        cmd.Parameters.AddWithValue("@Aby", Session["userID"].ToString());
                        cmd.Parameters.AddWithValue("@Adt", DateTime.Now);
                        cmd.Connection = con;
                        int Ins_RMM = 0;
                        try
                        {
                            Ins_RMM = cmd.ExecuteNonQuery();
                            if (Ins_RMM > 0)
                            {
                                string message = "alert('Shearing Entry Approved !!!')";
                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            }

                        }
                        catch
                        {

                        }







                    }
                    // BindRMgrid();
                   // resetControls();
                    //Response.Redirect(Request.RawUrl);

                }
            }


            catch (Exception ex)
            {
            }
        }

        else
        {

        }
    }
}