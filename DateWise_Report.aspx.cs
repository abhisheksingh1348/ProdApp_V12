using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Globalization;
using System.Activities.Expressions;

public partial class DateWise_Report : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;

    DataClassesDataContext sonicPrd = new DataClassesDataContext("constr");
    DataTable dt1 = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userID"] == null)
        {
            Response.Redirect("LoginPage.aspx");
        }

        if (!IsPostBack)
        
        {

           
                ReportDate.InnerText = DateTime.Now.ToShortDateString();
                LoadSection();
        
           
            //  BindRMgrid();
           
        }
        else
        {
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
    protected void BindRMgrid()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM T_RM_MASTER ;"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
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
    protected void BindRMgrid(String ToDate, string FromDate)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM T_RM_MASTER where T_RMM_PDATE between &;"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@From", FromDate);
                    cmd.Parameters.AddWithValue("@To", ToDate);
                    cmd.Connection = con;
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


    protected void Gv_RawMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                if (row["Id"].ToString() == "1" || row["Id"].ToString() == "3")
                {
                    e.Row.DataItem = "test";
                    e.Row.BackColor = System.Drawing.Color.Green;
                    e.Row.ForeColor = System.Drawing.Color.White;
                }

                else
                {
                    e.Row.BackColor = System.Drawing.Color.Blue;
                    e.Row.ForeColor = System.Drawing.Color.White;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnexport_Click(object sender, EventArgs e)
    {
        try
        {
            btnexport.Visible=false;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                // To Export all pages
                //AbhiCode

                //
                Gv_RawMaterial.AllowPaging = false;
               // this.BindRMgrid(); commented for testing
                Gv_RawMaterial.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in Gv_RawMaterial.HeaderRow.Cells)
                {
                    cell.BackColor = Gv_RawMaterial.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in Gv_RawMaterial.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        //Abhi Code
                        cell.BackColor = System.Drawing.Color.White;
                        cell.ForeColor = System.Drawing.Color.Black;
                        //if (row.RowIndex == 3)
                        //{
                        //    cell.BackColor = System.Drawing.Color.Black;
                        //    cell.ForeColor = System.Drawing.Color.White;
                        //}
                        // Abhi Code
                        //if (row.RowIndex == 5)
                        //{
                        //    cell.BackColor = System.Drawing.Color.Green;
                        //    cell.ForeColor = System.Drawing.Color.White;
                        //}
                        //
                        //if (row.RowIndex % 2 == 0)
                        //{
                        //    cell.BackColor = System.Drawing.Color.Green;
                        //    cell.ForeColor = System.Drawing.Color.White;
                        //}
                        //else
                        //{
                        //    cell.BackColor = System.Drawing.Color.Blue;
                        //    cell.ForeColor = System.Drawing.Color.White;
                        //} Commented to remove color in alternate rows
                        cell.CssClass = "textmode";
                    }
                }
                //Gv_RawMaterial.RenderControl(hw);  //commented to append text above sheet data
                divExport.RenderControl(hw);
                // style to format numbers to string
                string style = "<style> .textmode { } </style>";
                Response.Write(style);
                //test code
                //StringBuilder buildHtml = new StringBuilder();
                //buildHtml.Append("<HTML>");
                //buildHtml.AppendLine("dfsfdsfdsfd");
                //buildHtml.Append("</HTML>");
                //string sHtml = buildHtml.ToString();
                //

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        catch (Exception ex)
        {

        }
    }
    
            protected DataTable GetFilterData()
    {
        DataTable dt1 = new DataTable();
        if (FrmDate.Text != string.Empty && ToDate.Text != string.Empty)
        {
            DateTime fromdt = DateTime.ParseExact(FrmDate.Text, "dd-MM-yyyy", CultureInfo.InstalledUICulture);
            DateTime Todt = DateTime.ParseExact(ToDate.Text, "dd-MM-yyyy", CultureInfo.InstalledUICulture);

            //var v11 = sonicPrd.SP_CMP_GET_MASTER_REQUEST_DETAILS_HOD(Session["userID"].ToString()).ToList().Where(x => x.CREATED_DATE >= fromdt && x.CREATED_DATE <= Todt).ToList();
            ////
            //var V12 = (from d in sonicPrd.T_RM_MASTERs.OrderBy(d => d.ID) select new { x => x.T_RMM_PDATE >= fromdt && x.T_RMM_PDATE <= Todt }).Distinct()
            //           .Where(x => x.T_RMM_PDATE >= fromdt && x.T_RMM_PDATE <= Todt)).ToList();
            //
            //
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM T_RM_MASTER where T_RMM_PDATE between @From and  @ToDate and T_RMM_SECTION = @Sec and T_RMM_STATUS = @Status;"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@From", fromdt);
                        cmd.Parameters.AddWithValue("@ToDate", Todt);
                        cmd.Parameters.AddWithValue("@Sec", ddlSection.SelectedValue);
                        cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                        adp1.Fill(dt1);

                        ViewState["RM"] = dt1;
                        con.Close();
                    }
                }
            }
            catch (Exception ex) { }
            //
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "alert('Error: please enter fromDate and Todate');", true);
        }
        return dt1;
    }

    protected DataTable GetData()
    {
        DataTable dt1 = new DataTable();
        if (FrmDate.Text != string.Empty && ToDate.Text != string.Empty)
        {
            DateTime fromdt = DateTime.ParseExact(FrmDate.Text, "dd-MM-yyyy", CultureInfo.InstalledUICulture);
            DateTime Todt = DateTime.ParseExact(ToDate.Text, "dd-MM-yyyy", CultureInfo.InstalledUICulture);

            //var v11 = sonicPrd.SP_CMP_GET_MASTER_REQUEST_DETAILS_HOD(Session["userID"].ToString()).ToList().Where(x => x.CREATED_DATE >= fromdt && x.CREATED_DATE <= Todt).ToList();
            ////
            //var V12 = (from d in sonicPrd.T_RM_MASTERs.OrderBy(d => d.ID) select new { x => x.T_RMM_PDATE >= fromdt && x.T_RMM_PDATE <= Todt }).Distinct()
            //           .Where(x => x.T_RMM_PDATE >= fromdt && x.T_RMM_PDATE <= Todt)).ToList();
            //
            //
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM T_RM_MASTER where T_RMM_PDATE between @From and  @ToDate;"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@From", fromdt);
                        cmd.Parameters.AddWithValue("@ToDate", Todt);
                        cmd.Connection = con;
                        con.Open();
                        SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                        adp1.Fill(dt1);
                      
                        ViewState["RM"] = dt1;
                        con.Close();
                    }
                }
            }
            catch (Exception ex) { }
            //
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "alert('Error: please enter fromDate and Todate');", true);
        }
        return dt1;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void ToDate_TextChanged(object sender, EventArgs e)
    {
        ddlSection.SelectedIndex = 0;
        ddlSection.Enabled = true;
        ddlStatus.SelectedIndex = 0;


        //BindRMgrid(ToDate.Text.Trim().ToString(), FrmDate.Text.Trim().ToString());
        if (FrmDate.Text != string.Empty && ToDate.Text != string.Empty)
        {
            DateTime fromdt = DateTime.ParseExact(FrmDate.Text, "dd-MM-yyyy", CultureInfo.InstalledUICulture);
            DateTime Todt = DateTime.ParseExact(ToDate.Text, "dd-MM-yyyy", CultureInfo.InstalledUICulture);

            //var v11 = sonicPrd.SP_CMP_GET_MASTER_REQUEST_DETAILS_HOD(Session["userID"].ToString()).ToList().Where(x => x.CREATED_DATE >= fromdt && x.CREATED_DATE <= Todt).ToList();
            ////
            //var V12 = (from d in sonicPrd.T_RM_MASTERs.OrderBy(d => d.ID) select new { x => x.T_RMM_PDATE >= fromdt && x.T_RMM_PDATE <= Todt }).Distinct()
            //           .Where(x => x.T_RMM_PDATE >= fromdt && x.T_RMM_PDATE <= Todt)).ToList();
            //
            //
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM T_RM_MASTER where T_RMM_PDATE between @From and  @ToDate;"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@From", fromdt);
                        cmd.Parameters.AddWithValue("@ToDate", Todt);
                        cmd.Connection = con;
                        con.Open();
                        SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                        adp1.Fill(dt1);
                        Gv_RawMaterial.DataSource = dt1;
                        Gv_RawMaterial.DataBind();
                        ViewState["RM"] = dt1;
                        con.Close();
                    }
                }
            }
            catch (Exception ex) { }
            //
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "alert('Error: please enter fromDate and Todate');", true);
        }
    }

    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {

        //if (FrmDate.Text != string.Empty && ToDate.Text != string.Empty && ddlSection.SelectedIndex != 0)
        //{
        //    DateTime fromdt = DateTime.ParseExact(FrmDate.Text, "dd-MM-yyyy", CultureInfo.InstalledUICulture);
        //    DateTime Todt = DateTime.ParseExact(ToDate.Text, "dd-MM-yyyy", CultureInfo.InstalledUICulture);

        //    //var v11 = sonicPrd.SP_CMP_GET_MASTER_REQUEST_DETAILS_HOD(Session["userID"].ToString()).ToList().Where(x => x.CREATED_DATE >= fromdt && x.CREATED_DATE <= Todt).ToList();
        //    ////
        //    //var V12 = (from d in sonicPrd.T_RM_MASTERs.OrderBy(d => d.ID) select new { x => x.T_RMM_PDATE >= fromdt && x.T_RMM_PDATE <= Todt }).Distinct()
        //    //           .Where(x => x.T_RMM_PDATE >= fromdt && x.T_RMM_PDATE <= Todt)).ToList();
        //    //
        //    //
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(constr))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("SELECT * FROM T_RM_MASTER where T_RMM_PDATE between @From and  @ToDate and T_RMM_SECTION=@Sec;"))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.Parameters.AddWithValue("@From", fromdt);
        //                cmd.Parameters.AddWithValue("@ToDate", Todt);
        //            cmd.Parameters.AddWithValue("@Sec", ddlSection.SelectedValue);
        //                cmd.Connection = con;
        //                con.Open();
        //                SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
        //                DataTable dt2 = new DataTable();
        //                adp1.Fill(dt2);
        //                Gv_RawMaterial.DataSource = dt2;
        //                Gv_RawMaterial.DataBind();
        //                ViewState["RM"] = dt2;
        //                con.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex) { }
        //    //
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "alert('Error: please enter fromDate and Todate');", true);
        //}
        ddlStatus.SelectedIndex = 0;
        ddlStatus.Enabled=true;
        string Sec = ddlSection.SelectedItem.Value;
        DataTable dt = this.GetData();
        DataView dataView = dt.DefaultView;

        if (!string.IsNullOrEmpty(Sec))
        {
            dataView.RowFilter = "T_RMM_SECTION = '" + Sec + "'";
        }
        
        Gv_RawMaterial.DataSource = dataView;
        Gv_RawMaterial.DataBind();
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Sta = ddlStatus.SelectedItem.Value;
        DataTable dt = this.GetFilterData();
        DataView dataView = dt.DefaultView;

        if (!string.IsNullOrEmpty(Sta))
        {
            dataView.RowFilter = "T_RMM_STATUS = '" + Sta + "'";
        }
        Gv_RawMaterial.DataSource = dataView;
        Gv_RawMaterial.DataBind();
    }
}