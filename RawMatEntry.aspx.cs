using System;
using System.Activities.Expressions;
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
using static System.Net.Mime.MediaTypeNames;

public partial class RawMatEntry : System.Web.UI.Page
{
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
                ((Label)Master.FindControl("label1")).Text = "Raw Material Entry";
                LoadPartyName();
                LoadSection();
                if (ddlPartyName.SelectedIndex != 0)
                {
                    BindRMgrid();

                }
            }
            catch (Exception ex)
            {
            }
          
        }
    }

    protected void LoadPartyName()
    {
        try
        {
            //linq method
            //var bindPartyName = (from d in sonicoPrd.T_PARTY_MASTERs.OrderBy(d => d.T_PARTYM_NAME).Distinct() select new { idval = d.T_PARTYM_NAME, TextFld = d.T_PARTYM_NAME }).Distinct();
            //if (bindPartyName != null)
            //{
            //    ddlPartyName.DataSource = bindPartyName;
            //    ddlPartyName.DataTextField = "TextFld";
            //    ddlPartyName.DataValueField = "idval";
            //    ddlPartyName.DataBind();
            //    ddlPartyName.Items.Insert(0, "Select Party Name");
            //}

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT T_RMPM_PARTY_NAME  FROM T_RM_PARTY_MASTER ORDER BY T_RMPM_PARTY_NAME ASC"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    DataTable dt1= new DataTable();
                    adp1.Fill(dt1);
                    ddlPartyName.DataSource = dt1;
                    ddlPartyName.DataTextField = "T_RMPM_PARTY_NAME";
                    ddlPartyName.DataValueField = "T_RMPM_PARTY_NAME";
                    ddlPartyName.DataBind();
                }
            }
            ddlPartyName.Items.Insert(0, new ListItem("--Select Party Name--", "0"));

        }
        catch (Exception ex)
        {

        }
    }
    
    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        DataTable dt = (DataTable)ViewState["RM"];
        // DataTable dt = new DataTable();
        if (dt.Rows.Count > 0)
        {
            //dt.Rows[e.RowIndex].Delete();
            //Gv_RawMaterial.DataSource = dt;
            //Gv_RawMaterial.DataBind();
            int Sno = Convert.ToInt32(dt.Rows[e.RowIndex]["ID"].ToString());
           int delet =  DeleteFromRMmaster(Sno);
            if (delet > 0)
            {
                string message = "alert('RM Entry Deleted')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }

        }
    }

    protected int DeleteFromRMmaster(int Sno)
    {
        int del = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM [SONIC_PRD].[dbo].[T_RM_MASTER] WHERE ID=@IDM"))
                {
                    cmd.Parameters.AddWithValue("@IDM", Sno.ToString());
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    del = cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        { 
        }
        return del;
    }

    protected void LoadSection()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM T_SECTION_MASTER"))
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

    protected void BindRMgrid()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM T_RM_MASTER WHERE T_RMM_BILL = @BILLNO AND T_RMM_STATUS = 'ENTERED'"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@BILLNO", txtBillNum.Text.Trim().ToString());
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    Gv_RawMaterial.DataSource = dt1;
                    Gv_RawMaterial.DataBind();
                    ViewState["RM"] = dt1;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }

    }

    protected Boolean checkDate ()
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
    protected void btn_add_Click(object sender, EventArgs e)
    {
        CultureInfo provider = CultureInfo.InvariantCulture;
        bool dte = checkDate();
        if (!string.IsNullOrEmpty(txtDate.Text) && !string.IsNullOrEmpty(txtBillNum.Text) && ddlPartyName.SelectedIndex != 0 && !string.IsNullOrEmpty(txtQty.Text) && !String.IsNullOrEmpty(txtRate.Text) && ddlSection.SelectedIndex != 0 && ddlUnit.SelectedIndex !=0  && dte == true)
        {
            
            try
            {
                //
                //CultureInfo provider = CultureInfo.InvariantCulture;
                String dateString = txtDate.Text;
                // Exception: String was not recognized as a valid DateTime because the day of week was incorrect.
                DateTime dateTime13 = DateTime.ParseExact(dateString, "dd-MM-yyyy", provider);
                float num = (float)Convert.ToDouble(txtQty.Text) * (float)Convert.ToDouble(txtRate.Text);

                //

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO T_RM_MASTER (T_RMM_DATAMODE,T_RMM_BILL, T_RMM_PDATE,T_RMM_UNITNUM,T_RMM_PARTY,T_RMM_SECTION,T_RMM_QUANTITY,T_RMM_RATE,T_RMM_TOTALAMNT,T_RMM_STATUS,T_RMM_EDATE,T_RMM_CREATED_BY,T_RMM_REMARKS) VALUES  (@mode,@bill        ,@Pdate         ,@Unitnum          ,@Party          ,@Section          ,@Quantity          ,@Rate         ,@TotalAmnt ,@Status         ,@Edate,@USER,@Rem)"))
                    {
                        cmd.Parameters.AddWithValue("@mode", "PURCHASE");
                        cmd.Parameters.AddWithValue("@bill", txtBillNum.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Pdate", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", provider));
                        //cmd.Parameters.AddWithValue("@Pdate", dateTime13);
                        cmd.Parameters.AddWithValue("@Unitnum", ddlUnit.SelectedValue);
                        cmd.Parameters.AddWithValue("@Party", ddlPartyName.SelectedValue);
                        cmd.Parameters.AddWithValue("@Section", ddlSection.SelectedValue);
                        cmd.Parameters.AddWithValue("@Quantity", txtQty.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Rate", txtRate.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@TotalAmnt", (float)Convert.ToDouble(txtQty.Text) * (float)Convert.ToDouble(txtRate.Text));
                        cmd.Parameters.AddWithValue("@Status", "ENTERED");
                        cmd.Parameters.AddWithValue("@Edate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@USER", Session["userID"].ToString());
                        if (!string.IsNullOrEmpty(txtRemarks.Text))
                        {
                            cmd.Parameters.AddWithValue("@Rem", txtRemarks.Text.ToString().Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Rem", DBNull.Value);
                        }



                        cmd.Connection = con;
                        int Ins_RMM = 0;
                        try
                        {
                            Ins_RMM = cmd.ExecuteNonQuery();
                            if (Ins_RMM > 0)
                            {
                                string message = "alert('RM Added')";
                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            }

                        }
                        catch (Exception ex)
                        {

                        }







                    }
                }
                BindRMgrid();
                resetControls();
            }
            catch (Exception ex)
            {

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
    protected void resetControls()
    {
        try
        {
            LoadSection();
            ddlUnit.ClearSelection();
            txtQty.Text = string.Empty;
            txtRate.Text = string.Empty;
            txtDate.Text = "";    
        }
        catch (Exception ex)
        {
        }
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

  

    protected void Gv_RawMaterial_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

   
}