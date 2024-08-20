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


public partial class ApprovalEntry : System.Web.UI.Page
{
        string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
    DataTable dtRejMmaster = new DataTable();
    DataTable dtRewMmaster = new DataTable();
    DataTable dtRMmaster = new DataTable();
    DataTable dtRMmasterNotAll = new DataTable();
    DataTable dtStock = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {

            if (Session["userID"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }

        }
    }
    protected DataTable GetStockData(String Section) {
        DataTable dt1 = new DataTable();

        try
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM T_RM_STOCK_MASTER WHERE T_RMM_STATUS = @ETYPE AND  T_RMM_DATAMODE = @RTYPE  AND T_RMM_APPROVED_DT = @DATE ORDER BY ID"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@RTYPE", ddlEntryType.SelectedValue);
                    cmd.Parameters.AddWithValue("@ETYPE", ddlRecordType.SelectedValue);
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    adp1.Fill(dt1);
                    Gv_RawMaterial.DataSource = dt1;
                    Gv_RawMaterial.DataBind();
                    ViewState["SD"] = dt1;
                    con.Close();
                }
            }
           
        }
        catch (Exception ex) { }
        return dt1;
    }
    protected void Gv_RawMaterial_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void btn_show_Click(object sender, EventArgs e)
    {
        if (ddlEntryType.SelectedIndex != 0 && ddlRecordType.SelectedIndex != 0) 
        {
            if(ddlEntryType.SelectedIndex == 1)
            {
                BindRMgrid();

            }
            
            else if(ddlEntryType.SelectedIndex == 2)
            { 
                BindRejMgrid(); 
            }

            else if (ddlEntryType.SelectedIndex == 3)
            {
                BindReWMgrid();
            }

            else if (ddlEntryType.SelectedIndex == 4 )
            {
                BindReworkGrid();
            }

        }
        btnApprove.Enabled = true;
        btnReject.Enabled = true;

    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {

        if (ddlEntryType.SelectedIndex == 1 && ddlRecordType.SelectedIndex == 1)
        {
            int AllApprove = 0;
            int Approve = 0;
            String Unit_Num = "";
            try
            {
                int UpdAll = 0;
                int Upd = 0;

                foreach (GridViewRow row in Gv_RawMaterial.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        string BillNum = string.Empty;
                        int id = int.Parse(row.Cells[1].Text);
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            con.Open();
                            //Insert into StockMaster

                            SqlTransaction transApp = con.BeginTransaction();



                            //Select record to enter in stockMaster
                            using (SqlCommand cmd = new SqlCommand("SELECT *  FROM T_RM_MASTER where ID = @id and T_RMM_STATUS = 'ENTERED'",con,transApp))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Connection = con;
                                    cmd.Parameters.AddWithValue("@id", id);
                                    
                                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                                    adp1.Fill(dtRMmasterNotAll);
                                if(dtRMmasterNotAll.Rows.Count > 0)
                                {
                                    Unit_Num = dtRMmasterNotAll.Rows[0]["T_RMM_UNITNUM"].ToString();
                                }
                                else
                                {
                                    transApp.Rollback();
                                    string message = "alert('Error in btnApprove_Click. Please contact IT team!!')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                }
                                    
                                }
                                //
                                using (SqlCommand cmd = new SqlCommand("update T_RM_MASTER set T_RMM_STATUS = 'APPROVED' , T_RMM_APPROVED_BY = @User , T_RMM_APPROVED_DT = @AppDate,T_RMM_REMARKS = @Rem where ID = @id and T_RMM_STATUS = 'ENTERED' ", con,transApp))
                                {
                                    
                                    cmd.Parameters.AddWithValue("@id", id);
                                    cmd.Parameters.AddWithValue("@User", Session["userID"]);
                                    cmd.Parameters.AddWithValue("@AppDate", DateTime.Now);
                                if (!String.IsNullOrEmpty(txtRemarks.Text))
                                {
                                    cmd.Parameters.AddWithValue("@Rem", txtRemarks.Text.Trim());

                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@Rem", DBNull.Value);
                                }
                                // Upd = cmd.ExecuteNonQuery();
                                // con.Close();

                                Approve = InsertToStockMaster(dtRMmasterNotAll,con,transApp);
                                    if (Approve > 0)
                                    {
                                        Upd = cmd.ExecuteNonQuery();
                                    if (Upd > 0)
                                    {
                                        transApp.Commit();
                                        string message = "alert('Approved!!.')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(),    "alert", message, true);
                                    }
                                    else
                                    {
                                        transApp.Rollback();
                                        string message = "alert('Error in btnApprove_Click. Please contact IT team!!')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    }
                                      
                                    }
                                    else
                                    {
                                    transApp.Rollback();
                                        string message = "alert('Error in btnApprove_Click. Please contact IT team!!')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    }
                                }

                            

                           

                        }

                    }



                }
                BindRMgrid();

            }

            catch (Exception ex)
            {
            }
        }

        else if (ddlEntryType.SelectedIndex == 2 && ddlRecordType.SelectedIndex == 1)
        {
            int Approve = 0;
            String Unit_Num = "";
            try
            {
                int UpdAll = 0;
                int Upd = 0;

                try
                {
                    foreach (GridViewRow row in GV_Rej.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect.Checked)
                        {
                            string BillNum = string.Empty;
                            int id = int.Parse(row.Cells[1].Text);
                            using (SqlConnection con = new SqlConnection(constr))
                            {
                                
                                    con.Open();
                              
                                SqlTransaction trans = con.BeginTransaction();
                                //Select record to enter in stockMaster
                                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo].[T_RM_REJ_MASTER] where ID = @id and T_RMREJM_STATUS = 'ENTERED'", con, trans))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.AddWithValue("@id", id);
                                    //con.Open();
                                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                                    adp1.Fill(dtRejMmaster);
                                }
                                //
                                using (SqlCommand cmd = new SqlCommand("update [SONIC_PRD].[dbo].[T_RM_REJ_MASTER] set T_RMREJM_STATUS = 'APPROVED' , T_RMREJM_APPROVED_BY = @User , T_RMREJM_APPROVED_DT = @AppDate where ID = @id and T_RMREJM_STATUS = 'ENTERED' ", con, trans))
                                {
                                    cmd.Parameters.AddWithValue("@id", id);
                                    cmd.Parameters.AddWithValue("@User", Session["userID"]);
                                    cmd.Parameters.AddWithValue("@AppDate", DateTime.Now);
                                    // Upd = cmd.ExecuteNonQuery();
                                    // con.Close();

                                    Approve = InsertToStockMasterRej(dtRejMmaster,con, trans);
                                    if (Approve > 0)
                                    {
                                        Upd = cmd.ExecuteNonQuery();
                                        trans.Commit();
                                        string message = "alert('Approved!!.')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    }
                                    else
                                    {
                                        trans.Rollback();
                                        string message = "alert('Error in btnApprove_Click. Please contact IT team!!')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    }
                                }




                            }

                            //Insert into StockMaster




                            //
                        }



                    }
                }
 
                catch(Exception ex)
                {

                }
                BindRejMgrid();

            }

            catch (Exception ex)
            {
            }
        }
        else if (ddlEntryType.SelectedIndex == 3 && ddlRecordType.SelectedIndex == 1)
        {
            int Approve = 0;
            String Unit_Num = "";
            try
            {
                int UpdAll = 0;
                int Upd = 0;

                try
                {
                    foreach (GridViewRow row in Gv_Rew.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect.Checked)
                        {
                            string BillNum = string.Empty;
                            int id = int.Parse(row.Cells[1].Text);
                            using (SqlConnection con = new SqlConnection(constr))
                            {

                                con.Open();

                                SqlTransaction trans = con.BeginTransaction();
                                //Select record to enter in stockMaster
                                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo].[T_RM_REW_MASTER] where ID = @id and T_RMREWM_STATUS = 'ENTERED'", con, trans))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.AddWithValue("@id", id);
                                    //con.Open();
                                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                                    adp1.Fill(dtRewMmaster);
                                }
                                //
                                using (SqlCommand cmd = new SqlCommand("update [SONIC_PRD].[dbo].[T_RM_REW_MASTER] set T_RMREWM_STATUS = 'APPROVED' , T_RMREWM_APPROVED_BY = @User , T_RMREWM_APPROVED_DT = @AppDate where ID = @id and T_RMREWM_STATUS = 'ENTERED' ", con, trans))
                                {
                                    cmd.Parameters.AddWithValue("@id", id);
                                    cmd.Parameters.AddWithValue("@User", Session["userID"]);
                                    cmd.Parameters.AddWithValue("@AppDate", DateTime.Now);
                                    // Upd = cmd.ExecuteNonQuery();
                                    // con.Close();

                                    Approve = InsertToStockMasterRew(dtRewMmaster, con, trans);
                                    if (Approve > 0)
                                    {
                                        Upd = cmd.ExecuteNonQuery();
                                        trans.Commit();
                                        string message = "alert('Rework Approved!!.')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    }
                                    else
                                    {
                                        trans.Rollback();
                                        string message = "alert('Error in btnApprove_Click. Please contact IT team!!')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                    }
                                }




                            }

                            //Insert into StockMaster




                            //
                        }



                    }
                }

                catch (Exception ex)
                {

                }
               // BindRejMgrid();
                BindReWMgrid();
            }

            catch (Exception ex)
            {
            }
        }

        else if(ddlEntryType.SelectedIndex == 4 && ddlRecordType.SelectedIndex == 1)
        {
          Reje();
            //int rewok = Rewk();
        }

    }
    
    protected void  Reje()
    {
        int Approve = 0;
        String Unit_Num = "";
        int Upda = 0;
        try
        {
            int UpdAll = 0;
            int Upd = 0;

            try
            {
                foreach (GridViewRow row in gv_Rewrk.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        string BillNum = string.Empty;
                        int id = int.Parse(row.Cells[1].Text);
                        using (SqlConnection con = new SqlConnection(constr))
                        {

                            con.Open();

                            SqlTransaction trans = con.BeginTransaction();
                            //Select record to enter in stockMaster
                            using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo].[T_RM_REWORK_MASTER] where ID = @id and T_RMREWRK_STATUS = 'ENTERED'", con, trans))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@id", id);
                                //con.Open();
                                SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                                adp1.Fill(dtRejMmaster);
                            }
                            //
                            //using (SqlCommand cmd = new SqlCommand("update [SONIC_PRD].[dbo].[T_RM_REJ_MASTER] set T_RMREJM_STATUS = 'APPROVED' , T_RMREJM_APPROVED_BY = @User , T_RMREJM_APPROVED_DT = @AppDate where ID = @id and T_RMREJM_STATUS = 'ENTERED' ", con, trans))
                            using (SqlCommand cmd = new SqlCommand("update [SONIC_PRD].[dbo].[T_RM_REWORK_MASTER] set T_RMREWRK_STATUS = 'APPROVED' , T_RMREWRK_APPROVED_BY = @User , T_RMREWRK_APPROVED_DT = @AppDate where ID = @id and T_RMREWRK_STATUS = 'ENTERED' ", con, trans))

                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@User", Session["userID"]);
                                cmd.Parameters.AddWithValue("@AppDate", DateTime.Now);
                                // Upd = cmd.ExecuteNonQuery();
                                // con.Close();
                                //Upda = InsertToRM_REWORK(dtRejMmaster, con, trans);
                                Approve = InsertToStockMasterRej(dtRejMmaster, con, trans);
                                int rewok = InsertToStockMasterRew(dtRejMmaster, con, trans);
                                if (Approve > 0 && rewok > 0 )
                                {
                                    Upd = cmd.ExecuteNonQuery();
                                    trans.Commit();
                                    string message = "alert('Approved!!.')";
                                    ScriptManager.RegisterClientScriptBlock(Page as Page,this.GetType(),"alert",message,true);

                                }
                                else
                                {
                                    trans.Rollback();
                                    string message = "alert('Error in btnApprove_Click. Please contact IT team!!')";
                                    ScriptManager.RegisterClientScriptBlock(Page as Page, this.GetType(), "alert", message, true);

                                }
                            }




                        }

                        //Insert into StockMaster




                        //
                    }



                }
            }

            catch (Exception ex)
            {

            }
            //BindRejMgrid();

        }

        catch (Exception ex)
        {
        }
        //return Approve;
    }

    protected int Rewk()
    {
        int Approve = 0;
        String Unit_Num = "";
        try
        {
            int UpdAll = 0;
            int Upd = 0;

            try
            {
                foreach (GridViewRow row in gv_Rewrk.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        string BillNum = string.Empty;
                        int id = int.Parse(row.Cells[1].Text);
                        using (SqlConnection con = new SqlConnection(constr))
                        {

                            con.Open();

                            SqlTransaction trans = con.BeginTransaction();
                            //Select record to enter in stockMaster
                            using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo].[T_RM_REW_MASTER] where ID = @id and T_RMREWM_STATUS = 'ENTERED'", con, trans))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@id", id);
                                //con.Open();
                                SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                                adp1.Fill(dtRewMmaster);
                            }
                            //
                            using (SqlCommand cmd = new SqlCommand("update [SONIC_PRD].[dbo].[T_RM_REW_MASTER] set T_RMREWM_STATUS = 'APPROVED' , T_RMREWM_APPROVED_BY = @User , T_RMREWM_APPROVED_DT = @AppDate where ID = @id and T_RMREWM_STATUS = 'ENTERED' ", con, trans))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@User", Session["userID"]);
                                cmd.Parameters.AddWithValue("@AppDate", DateTime.Now);
                                // Upd = cmd.ExecuteNonQuery();
                                // con.Close();

                                Approve = InsertToStockMasterRew(dtRewMmaster, con, trans);
                                if (Approve > 0)
                                {
                                    Upd = cmd.ExecuteNonQuery();
                                    trans.Commit();
                                    string message = "alert('Rework Approved!!.')";
                                    ScriptManager.RegisterClientScriptBlock(Page as Page, this.GetType(), "alert", message, true);

                                }
                                else
                                {
                                    trans.Rollback();
                                    string message = "alert('Error in btnApprove_Click. Please contact IT team!!')";
                                    ScriptManager.RegisterClientScriptBlock(Page as Page, this.GetType(), "alert", message, true);

                                }
                            }




                        }

                        //Insert into StockMaster




                        //
                    }



                }
            }

            catch (Exception ex)
            {

            }
            // BindRejMgrid();
            BindReWMgrid();
        }

        catch (Exception ex)
        {
        }
        return Approve;
    }
    
    protected void BindRMgrid()
    {
        try
        {
           
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM T_RM_MASTER WHERE T_RMM_STATUS = @ETYPE AND  T_RMM_DATAMODE = @RTYPE ORDER BY ID"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@RTYPE", ddlEntryType.SelectedValue);
                    cmd.Parameters.AddWithValue("@ETYPE", ddlRecordType.SelectedValue);
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    Gv_RawMaterial.DataSource = dt1;
                    Gv_RawMaterial.DataBind();
                    ViewState["AP"] = dt1;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }

    }
    protected void BindRejMgrid()
    {
        try
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM  [SONIC_PRD].[dbo].[T_RM_REJ_MASTER] WHERE T_RMREJM_STATUS = @Rtyp ORDER BY T_RMREJM_CREATED_DT DESC"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Rtyp", ddlRecordType.SelectedValue);
                    cmd.Connection = con;
                    
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    DataTable dt2 = new DataTable();
                    adp1.Fill(dt2);
                    GV_Rej.DataSource = dt2;
                    GV_Rej.DataBind();
                    ViewState["Rej"] = dt2;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }

    }
    
            protected void BindReWMgrid()
    {
        try
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM  [SONIC_PRD].[dbo].[T_RM_REW_MASTER] WHERE T_RMREWM_STATUS = @Rtyp ORDER BY T_RMREWM_CREATED_DT DESC"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Rtyp", ddlRecordType.SelectedValue);
                    cmd.Connection = con;

                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    DataTable dt2 = new DataTable();
                    adp1.Fill(dt2);
                    Gv_Rew.DataSource = dt2;
                    Gv_Rew.DataBind();
                    ViewState["Rew"] = dt2;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }

    }
    
          protected void BindReworkGrid()
    {
        try
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM  [SONIC_PRD].[dbo].[T_RM_REWORK_MASTER] WHERE T_RMREWRK_STATUS = @Rtyp ORDER BY T_RMREWRK_CREATED_DT DESC"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Rtyp", ddlRecordType.SelectedValue);
                    cmd.Connection = con;

                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    DataTable dt2 = new DataTable();
                    adp1.Fill(dt2);
                    gv_Rewrk.DataSource = dt2;
                    gv_Rewrk.DataBind();
                    ViewState["Rew"] = dt2;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }

    }
    protected void Gv_RawMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(Gv_RawMaterial, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }

    protected void Gv_RawMaterial_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in Gv_RawMaterial.Rows)
        {
            if (row.RowIndex == Gv_RawMaterial.SelectedIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            }
        }

        foreach (GridViewRow row in Gv_RawMaterial.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                if (chkRow.Checked)
                {
                    string name = row.Cells[1].Text;
                    string country = (row.Cells[2].FindControl("lblCountry") as Label).Text;
                    //dt.Rows.Add(name, country);
                }
            }
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (row.RowIndex == GridView1.SelectedIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                row.ToolTip = string.Empty;
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                row.ToolTip = "Click to select this row.";
            }
        }
    }

    protected void LinkButton_Del_Click(object sender, EventArgs e)
    {

    }

    protected DataTable CheckCurrentRecordStatus( DataTable RMMasterNotAllDt  )
    {
        DataTable dtCCR = new DataTable();
        try
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from T_RM_STOCK_MASTER where  T_RMSM_SECTION = @SECTION  AND  MONTH(T_RMSM_CREATED_DT) = @MONTH"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@MONTH",DateTime.Now.Month);
                    cmd.Parameters.AddWithValue("@SECTION", RMMasterNotAllDt.Rows[0]["T_RMM_SECTION"]);
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    adp1.Fill(dtCCR);
                  
                    ViewState["CCR"] = dtCCR;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }
        return dtCCR;
    }

    protected DataTable CheckCurrentRecordStatus(DataTable RMMasterNotAllDt,SqlConnection con, SqlTransaction trans)
    {
        DataTable dtCCR = new DataTable();
        try
        {

            //using (SqlConnection con = new SqlConnection(constr))
            //{
                using (SqlCommand cmd = new SqlCommand("select * from T_RM_STOCK_MASTER where  T_RMSM_SECTION = @SECTION  AND  MONTH(T_RMSM_CREATED_DT) = @MONTH", con, trans))
                {
              //  con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@MONTH", DateTime.Now.Month);
                    cmd.Parameters.AddWithValue("@SECTION", RMMasterNotAllDt.Rows[0]["T_RMM_SECTION"]);
                    //con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    adp1.Fill(dtCCR);

                    ViewState["CCR"] = dtCCR;
                   // con.Close();
                }
            //}
        }
        catch (Exception ex) { }
        return dtCCR;
    }
    protected DataTable CheckCurrentRecordStatusRej(DataTable RMMasterNotAllDt, SqlConnection con, SqlTransaction trans)
    {
        DataTable dtCCR = new DataTable();
        try
        {

            //using (SqlConnection con = new SqlConnection(constr))
            //{
            using (SqlCommand cmd = new SqlCommand("select * from T_RM_STOCK_MASTER where  T_RMSM_SECTION = @SECTION  AND  MONTH(T_RMSM_CREATED_DT) = @MONTH", con, trans))
            {
                //  con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@MONTH", DateTime.Now.Month);
                cmd.Parameters.AddWithValue("@SECTION", RMMasterNotAllDt.Rows[0]["T_RMREWRK_FRMSECTION"]);
                //con.Open();
                SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                adp1.Fill(dtCCR);

                ViewState["CCR"] = dtCCR;
                // con.Close();
            }
            //}
        }
        catch (Exception ex) { }
        return dtCCR;
    }

    protected DataTable CheckCurrentRecordStatusRew(DataTable RMMasterNotAllDt, SqlConnection con, SqlTransaction trans)
    {
        DataTable dtCCR = new DataTable();
        try
        {

            //using (SqlConnection con = new SqlConnection(constr))
            //{
            using (SqlCommand cmd = new SqlCommand("select * from T_RM_STOCK_MASTER where  T_RMSM_SECTION = @SECTION  AND  MONTH(T_RMSM_CREATED_DT) = @MONTH", con, trans))
            {
                //  con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@MONTH", DateTime.Now.Month);
                cmd.Parameters.AddWithValue("@SECTION", RMMasterNotAllDt.Rows[0]["T_RMREWRK_TOSECTION"]);
                //con.Open();
                SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                adp1.Fill(dtCCR);

                ViewState["CCR"] = dtCCR;
                // con.Close();
            }
            //}
        }
        catch (Exception ex) { }
        return dtCCR;
    }
   //protected int InsertToRM_REWORK(DataTable dt, SqlConnection con, SqlTransaction trans)
   // {
   //     int clnQty = 0;
   //     int RejQty = 0;
   //     //con.Open();
   //     int Ins = 0;
   //     try
   //     {
   //         using (SqlCommand cmd = new SqlCommand("update [SONIC_PRD].[dbo].[T_RM_REWORK_MASTER] set T_RMREWRK_STATUS = 'APPROVED' , T_RMREWRK_APPROVED_BY = @User , T_RMREWRK_APPROVED_DT = @AppDate where ID = @id and T_RMREWRK_STATUS = 'ENTERED' ", con, trans))
   //         {
   //             cmd.Parameters.Clear();
   //             cmd.Parameters.AddWithValue("@id", CheckRecord.Rows[0]["ID"].ToString());
   //             cmd.Parameters.AddWithValue("@Rej", RejQty);
   //             cmd.Parameters.AddWithValue("@clQty", clnQty);
   //             cmd.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
   //             cmd.Parameters.AddWithValue("@Udt", DateTime.Now);


   //             cmd.Connection = con;
   //             try
   //             {
   //                 InsRej = cmd.ExecuteNonQuery();
   //             }
   //             catch (Exception ex)
   //             {

   //             }



   //         }
   //     }
   //     catch(Exception ex) 
   //     { 
   //     } 
   //     return Ins;
   // }
    protected int InsertToStockMasterRej(DataTable dt,SqlConnection con, SqlTransaction trans)
    {
        int clnQty = 0;
        int RejQty = 0;
        //con.Open();
        int InsRej = 0;
        DataTable CheckRecord = CheckCurrentRecordStatusRej(dt,con,trans); //check whether record present for predent month or not ?
        


        if (CheckRecord.Rows.Count > 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()) && !string.IsNullOrEmpty(dt.Rows[0]["T_RMREWRK_QTY"].ToString()))
                {
                    if ((Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString())) > 0 && (Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString()) > 0))
                    {
                        clnQty = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()) - (Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString()));

                    }
                    //else
                    //{
                    //    clnQty =  0 - Convert.ToInt32(dt.Rows[0]["T_RMREJM_QTY"].ToString());
                    //}
                }
                if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_REJECTION"].ToString()) && !string.IsNullOrEmpty(dt.Rows[0]["T_RMREWRK_QTY"].ToString()))
                {

                    if ((Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_REJECTION"].ToString())) > 0 && (Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString()) > 0))
                    {
                        RejQty = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_REJECTION"].ToString()) + (Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString()));

                    }
                    else
                    {
                        RejQty = Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString());
                    }
                }
                else
                {
                    RejQty = Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString());
                }
                //using (SqlConnection con = new SqlConnection(constr))
                //{

                // con.Open();

                using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET T_RMSM_REJECTION = @Rej, T_RMSM_UPDATED_BY= @Uby , T_RMSM_UPDATED_DT = @Udt ,T_RMSM_CL_QTY = @clQty where ID = @id  ", con, trans))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", CheckRecord.Rows[0]["ID"].ToString());
                        cmd.Parameters.AddWithValue("@Rej", RejQty);
                        cmd.Parameters.AddWithValue("@clQty", clnQty);
                        cmd.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                        cmd.Parameters.AddWithValue("@Udt", DateTime.Now);


                        cmd.Connection = con;
                        try
                        {
                            InsRej = cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                        }



                    }
                    //Update affecting columns of table
                    //using (SqlCommand cmd1 = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET T_RMSM_TOTAL_PUR = @TotalPurch"))
                    //{

                    //}
                    

                    
                //}
            }
            catch (Exception ex) { }
          
        }
        return InsRej;  
    }
    protected int InsertToStockMasterRew(DataTable dt, SqlConnection con, SqlTransaction trans)
    {
        int clnQty = 0;
        int RewQty = 0;
        //con.Open();
        int InsRej = 0;
        int InsRew = 0;

        DataTable CheckRecord = CheckCurrentRecordStatusRew(dt, con, trans); //check whether record present for present month or not ?
        if (CheckRecord.Rows.Count > 0)
        {
            try
            {


                if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()) && !string.IsNullOrEmpty(dt.Rows[0]["T_RMREWRK_QTY"].ToString()))
                {
                    if ((Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString())) > 0 && (Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString()) > 0))
                    {
                        clnQty = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()) + (Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString()));

                    }
                    //else
                    //{
                    //    clnQty =  0 - Convert.ToInt32(dt.Rows[0]["T_RMREJM_QTY"].ToString());
                    //}
                }
                if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_REWORK"].ToString()) && !string.IsNullOrEmpty(dt.Rows[0]["T_RMREWRK_QTY"].ToString()))
                {

                    if ((Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_REWORK"].ToString())) > 0 && (Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString()) > 0))
                    {
                        RewQty = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_REWORK"].ToString()) + (Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString()));

                    }
                    else
                    {
                        RewQty = Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString());
                    }
                }
                else
                {
                    RewQty = Convert.ToInt32(dt.Rows[0]["T_RMREWRK_QTY"].ToString());
                }


                if (CheckRecord.Rows.Count > 0)
                {
                    try
                    {
                        //using (SqlConnection con = new SqlConnection(constr))
                        //{

                        // con.Open();

                        using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET T_RMSM_REWORK = @Rew, T_RMSM_UPDATED_BY= @Uby , T_RMSM_UPDATED_DT = @Udt ,T_RMSM_CL_QTY = @clQty where ID = @id  ", con, trans))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", CheckRecord.Rows[0]["ID"].ToString());
                            cmd.Parameters.AddWithValue("@Rew", RewQty);
                            cmd.Parameters.AddWithValue("@clQty", clnQty);
                            cmd.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@Udt", DateTime.Now);


                            cmd.Connection = con;
                            try
                            {
                                InsRej = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }



                        }
                        //Update affecting columns of table
                        //using (SqlCommand cmd1 = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET T_RMSM_TOTAL_PUR = @TotalPurch"))
                        //{

                        //}



                        //}
                    }
                    catch (Exception ex) { }
                }
            }

            catch (Exception ex)
            { }
        }
        else 
        {
            try
            {



                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }


                using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[T_RM_STOCK_MASTER]          ([T_RMSM_SECTION],T_RMSM_REWORK ,T_RMSM_CL_QTY ,[T_RMSM_CREATED_BY]           ,[T_RMSM_CREATED_DT])     VALUES (@Section, @Rew,@ClQty,@Cby,@Cdt) ", con, trans))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Section", dt.Rows[0]["T_RMREWRK_TOSECTION"].ToString());
                    cmd.Parameters.AddWithValue("@Rew", dt.Rows[0]["T_RMREWRK_QTY"].ToString());
                    cmd.Parameters.AddWithValue("@ClQty", dt.Rows[0]["T_RMREWRK_QTY"].ToString());
                    cmd.Parameters.AddWithValue("@Cby", Session["userID"].ToString());
                    cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);


                    cmd.Connection = con;
                    try
                    {
                        InsRej = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }







                }

                BindRMgrid();
            }
            catch(Exception ex) { } 
          
        }
        return InsRej;
    }
    protected int InsertToStockMaster (DataTable dt,SqlConnection con,SqlTransaction transApp) 
    {
       String  UnitNum = dt.Rows[0]["T_RMM_UNITNUM"].ToString();
        int Ins_RMM = 0;
        int DataInserted = 0;
        int ClosingQty = 0;
       int U1Purch = 0;
        int U2Purch = 0;
        int U3Purch = 0;

        int ExistU1 = 0;
        int ExistU2 = 0;
        int ExistU3 = 0;
        int TotalPurch = 0;
        int opQty = 0;
        string BillNum = "";
        DataTable CheckRecord = CheckCurrentRecordStatus(dt,con,transApp);
        if (CheckRecord.Rows.Count > 0 ) 
        {
            try
            {
                int clnQty = 0;
                //If Unit 1
                if (UnitNum == "Unit1")

                {
                    if(!string.IsNullOrEmpty(dt.Rows[0]["T_RMM_QUANTITY"].ToString()))
        
                    {
                        U1Purch = Convert.ToInt32(dt.Rows[0]["T_RMM_QUANTITY"].ToString());
                    }
                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_U1_PUR"].ToString()))

                    {
                        ExistU1 = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_U1_PUR"].ToString());
                    }

                    //Add U1Purch
                    
                   
                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_TOTAL_PUR"].ToString()))

                    {
                        TotalPurch = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_TOTAL_PUR"].ToString()) + U1Purch;
                    }
                    else
                    {
                        TotalPurch = U1Purch;
                    }
                    //if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_OPENING_QTY"].ToString()))
                    //{
                    //    opQty = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_OPENING_QTY"].ToString());
                    //}
                    //else
                    //{
                    //    opQty = U1Purch;
                    //}
                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()))
                    {
                        clnQty = U1Purch + Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()) ;
                    }
                    else
                    {

                    }
                  

                       

                        using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET  T_RMSM_U1_PUR = @TotalU1Purch,T_RMSM_TOTAL_PUR = @TotalPur, T_RMSM_CL_QTY = @clQty,T_RMSM_UPDATED_BY = @Uby,T_RMSM_UPDATED_DT = @Udt where ID = @id ", con, transApp))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", CheckRecord.Rows[0]["ID"].ToString());
                            //cmd.Parameters.AddWithValue("@Open", opQty);
                            cmd.Parameters.AddWithValue("@TotalPur", TotalPurch);
                            cmd.Parameters.AddWithValue("@TotalU1Purch", ExistU1 + U1Purch);
                            cmd.Parameters.AddWithValue("@clQty", clnQty);
                            cmd.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@Udt", DateTime.Now);


                            cmd.Connection = con;
                            try
                            {
                                Ins_RMM = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }



                        }
                        //Update affecting columns of table
                        //using (SqlCommand cmd1 = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET T_RMSM_TOTAL_PUR = @TotalPurch"))
                        //{

                        //}
                        
                   
                    BindRMgrid();
                    
                }
                //If Unit 2
                if (UnitNum == "Unit2")

                {
                    if (!string.IsNullOrEmpty(dt.Rows[0]["T_RMM_QUANTITY"].ToString()))

                    {
                        U2Purch = Convert.ToInt32(dt.Rows[0]["T_RMM_QUANTITY"].ToString());
                    }
                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_U2_PUR"].ToString()))

                    {
                        ExistU2 = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_U2_PUR"].ToString());
                    }

                    //Add U1Purch


                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_TOTAL_PUR"].ToString()))

                    {
                        TotalPurch = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_TOTAL_PUR"].ToString()) + U2Purch;
                    }
                    else
                    {
                        TotalPurch = U2Purch;
                    }
                    //if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_OPENING_QTY"].ToString()))
                    //{
                    //    opQty = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_OPENING_QTY"].ToString());
                    //}
                    //else
                    //{
                    //    opQty = U2Purch;
                    //}
                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()))
                    {
                        clnQty = U2Purch + Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString());
                    }
                    else
                    {

                    }
                   

                       

                        using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET  T_RMSM_U2_PUR = @TotalU2Purch,T_RMSM_TOTAL_PUR = @TotalPur, T_RMSM_CL_QTY = @clQty,T_RMSM_UPDATED_BY = @Uby,T_RMSM_UPDATED_DT = @Udt where ID = @id ", con, transApp))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", CheckRecord.Rows[0]["ID"].ToString());
                            //cmd.Parameters.AddWithValue("@Open", opQty);
                            cmd.Parameters.AddWithValue("@TotalPur", TotalPurch);
                            cmd.Parameters.AddWithValue("@TotalU2Purch", ExistU2 + U2Purch);
                            cmd.Parameters.AddWithValue("@clQty", clnQty);
                            cmd.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@Udt", DateTime.Now);


                            cmd.Connection = con;
                            try
                            {
                                Ins_RMM = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }



                        }
                        //Update affecting columns of table
                        //using (SqlCommand cmd1 = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET T_RMSM_TOTAL_PUR = @TotalPurch"))
                        //{

                        //}
                       
                   
                    BindRMgrid();

                }
                if (UnitNum == "Unit3")

                {
                    if (!string.IsNullOrEmpty(dt.Rows[0]["T_RMM_QUANTITY"].ToString()))

                    {
                        U3Purch = Convert.ToInt32(dt.Rows[0]["T_RMM_QUANTITY"].ToString());
                    }
                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_U3_PUR"].ToString()))

                    {
                        ExistU3 = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_U3_PUR"].ToString());
                    }

                    //Add U1Purch


                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_TOTAL_PUR"].ToString()))

                    {
                        TotalPurch = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_TOTAL_PUR"].ToString()) + U3Purch;
                    }
                    else
                    {
                        TotalPurch = U3Purch;
                    }
                    //if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_OPENING_QTY"].ToString()))
                    //{
                    //    opQty = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_OPENING_QTY"].ToString());
                    //}
                    //else
                    //{
                    //    opQty = U3Purch;
                    //}
                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()))
                    {
                        clnQty = U3Purch + Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString());
                    }
                    else
                    {

                    }
                   

                     

                        using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET T_RMSM_U3_PUR = @TotalU3Purch,T_RMSM_TOTAL_PUR = @TotalPur, T_RMSM_CL_QTY = @clQty,T_RMSM_UPDATED_BY = @Uby,T_RMSM_UPDATED_DT = @Udt where ID = @id ", con, transApp))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", CheckRecord.Rows[0]["ID"].ToString());
                            //cmd.Parameters.AddWithValue("@Open", opQty);
                            cmd.Parameters.AddWithValue("@TotalPur", TotalPurch);
                            cmd.Parameters.AddWithValue("@TotalU3Purch", ExistU3 + U3Purch);
                            cmd.Parameters.AddWithValue("@clQty", clnQty);
                            cmd.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@Udt", DateTime.Now);


                            cmd.Connection = con;
                            try
                            {
                                Ins_RMM = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }



                        }
                        //Update affecting columns of table
                        //using (SqlCommand cmd1 = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET T_RMSM_TOTAL_PUR = @TotalPurch"))
                        //{

                        //}
                      
               
                    BindRMgrid();

                }
            }
            catch (Exception ex)
            {


            }       

        }

        else
        {
            try
            {
                //If Unit 1
                if (UnitNum == "Unit1")

                {
                     U1Purch = Convert.ToInt32(dt.Rows[0]["T_RMM_QUANTITY"].ToString());
                   
                    //int TotalPurchase = U1Purch;

                    //Convert.ToInt32(dt.Rows[0]["T_RMM_QUANTITY"].ToString()) + Convert.ToInt32(dt.Rows[0]  

                    //                ["T_RMM_QUANTITY"].ToString()) + Convert.ToInt32(dt.Rows[0]["T_RMM_QUANTITY"].ToString());

                        if (con.State == ConnectionState.Closed)
                          {
                        con.Open();
                          }
                       

                        using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[T_RM_STOCK_MASTER]          ([T_RMSM_SECTION],[T_RMSM_U1_PUR]         ,[T_RMSM_TOTAL_PUR] ,T_RMSM_CL_QTY ,[T_RMSM_CREATED_BY]           ,[T_RMSM_CREATED_DT])     VALUES (@Section, @U1P,@TotPur,@ClQty,@Cby,@Cdt) ",con,transApp))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@Section", dt.Rows[0]["T_RMM_SECTION"]);
                            //cmd.Parameters.AddWithValue("@OpQty", U1Purch);
                            cmd.Parameters.AddWithValue("@U1P", U1Purch);
                            cmd.Parameters.AddWithValue("@TotPur", U1Purch);
                            cmd.Parameters.AddWithValue("@ClQty", U1Purch);
                            cmd.Parameters.AddWithValue("@Cby", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);


                            cmd.Connection = con;
                            try
                            {
                                Ins_RMM = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }







                        }
                   
                    BindRMgrid();
                }
                //If Unit 2
                else if (UnitNum == "Unit2")
                {
                     U2Purch = Convert.ToInt32(dt.Rows[0]["T_RMM_QUANTITY"].ToString());
                   

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }


                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[T_RM_STOCK_MASTER]          ([T_RMSM_SECTION],[T_RMSM_U2_PUR]         ,[T_RMSM_TOTAL_PUR]  ,[T_RMSM_CREATED_BY]           ,[T_RMSM_CREATED_DT])     VALUES (@Section, @U2P,@TotPur,@Cby,@Cdt) ", con, transApp))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@Section", dt.Rows[0]["T_RMM_SECTION"]);
                            cmd.Parameters.AddWithValue("@U2P", U2Purch);
                            cmd.Parameters.AddWithValue("@TotPur", U2Purch);
                            // cmd.Parameters.AddWithValue("@ClQty", ClosingQty);
                            cmd.Parameters.AddWithValue("@Cby", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);


                            cmd.Connection = con;
                            try
                            {
                                Ins_RMM = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }







                        }
                   
                    BindRMgrid();
                }
                //If Unit 3
                else if (UnitNum == "Unit3")
                {
                     U3Purch = Convert.ToInt32(dt.Rows[0]["T_RMM_QUANTITY"].ToString());
                    //int ClosingQty = 
                    //int TotalPurchase = U1Purch;

                    //Convert.ToInt32(dt.Rows[0]["T_RMM_QUANTITY"].ToString()) + Convert.ToInt32(dt.Rows[0]  

                    //                ["T_RMM_QUANTITY"].ToString()) + Convert.ToInt32(dt.Rows[0]["T_RMM_QUANTITY"].ToString());

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[T_RM_STOCK_MASTER]          ([T_RMSM_SECTION],[T_RMSM_U3_PUR]         ,[T_RMSM_TOTAL_PUR]  ,[T_RMSM_CREATED_BY]           ,[T_RMSM_CREATED_DT])     VALUES (@Section, @U3P,@TotPur,@Cby,@Cdt) ", con, transApp))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@Section", dt.Rows[0]["T_RMM_SECTION"]);
                            cmd.Parameters.AddWithValue("@U3P", U3Purch);
                            cmd.Parameters.AddWithValue("@TotPur", U3Purch);
                            // cmd.Parameters.AddWithValue("@ClQty", ClosingQty);
                            cmd.Parameters.AddWithValue("@Cby", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);


                            cmd.Connection = con;
                            try
                            {
                                Ins_RMM = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }







                        }
                   
                    BindRMgrid();
                }
            }
            catch (Exception ex)
            {

            }


        }



        DataInserted = Ins_RMM;
        return DataInserted;
    }


    protected void ddlEntryType_SelectedIndexChanged(object sender, EventArgs e)
    {

        Gv_RawMaterial.DataSource = "";
        Gv_RawMaterial.DataBind();
        GV_Rej.DataSource = "";
        GV_Rej.DataBind();
        Gv_Rew.DataSource = "";
        Gv_Rew.DataBind();
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(txtRemarks.Text))
        {
            try
            {
                int Upd = 0;

                foreach (GridViewRow row in Gv_RawMaterial.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        string BillNum = string.Empty;
                        int id = int.Parse(row.Cells[1].Text);
                        using (SqlConnection con = new SqlConnection(constr))
                        {

                            //Insert into StockMaster





                            using (SqlCommand cmd = new SqlCommand("update T_RM_MASTER set T_RMM_STATUS = 'REJECTED' , T_RMM_APPROVED_BY = @User , T_RMM_APPROVED_DT = @AppDate,T_RMM_REMARKS = @Rem where ID = @id and T_RMM_STATUS = 'ENTERED' ", con))
                            {
                                con.Open();
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@User", Session["userID"]);
                                cmd.Parameters.AddWithValue("@AppDate", DateTime.Now);
                                if (!String.IsNullOrEmpty(txtRemarks.Text))
                                {
                                    cmd.Parameters.AddWithValue("@Rem", txtRemarks.Text.Trim());

                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@Rem", DBNull.Value);
                                }
                                Upd = cmd.ExecuteNonQuery();
                                con.Close();


                                if (Upd > 0)
                                {

                                    string message = "alert('Item Rejected')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                }
                                else
                                {
                                    string message = "alert('Error in btnReject_Click. Please contact IT team!!')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                }
                            }





                        }

                    }



                }
                BindRMgrid();

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