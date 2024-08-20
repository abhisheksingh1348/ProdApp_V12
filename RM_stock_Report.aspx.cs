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

public partial class DateWise_Report : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;

    DataClassesDataContext sonicPrd = new DataClassesDataContext("constr");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["userID"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
            BindSMgrid();
          ReportDate.InnerText = DateTime.Now.ToShortDateString();

        }
        else
        {
        }

    }


    protected void BindSMgrid()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM T_RM_STOCK_MASTER where t_rmsm_status IS  NULL and month(t_rmsm_created_dt) =month(getdate())  ;"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    gv_SM_report.DataSource = dt1;
                    gv_SM_report.DataBind();
                    ViewState["SM"] = dt1;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }

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
                gv_SM_report.AllowPaging = false;
               // this.BindRMgrid(); commented for testing
                gv_SM_report.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in gv_SM_report.HeaderRow.Cells)
                {
                    cell.BackColor = gv_SM_report.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gv_SM_report.Rows)
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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

  
}