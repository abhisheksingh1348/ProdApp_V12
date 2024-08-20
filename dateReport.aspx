<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dateReport.aspx.cs" Inherits="dateReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <script type="text/javascript" src="js/jquery-3.7.1.min.js"></script>

    <!-- Bootstrap -->
<%--        <script src="assets/vendor/jquery-browser-mobile/jquery.browser.mobile.js"></script>--%>
<%--    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>--%>

<%--    <script src="js/jquery-1.8.3.min.js"></script>--%>
    <link href="ReportJsandCss/bootstrap.min.css" rel="stylesheet" />
    <script src="ReportJsandCss/bootstrap.min.js"></script>
<!-- Bootstrap -->
<!-- Bootstrap DatePicker -->
    <link href="ReportJsandCss/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="ReportJsandCss/bootstrap-datepicker.js"></script>
<!-- Bootstrap DatePicker -->
<script type="text/javascript">
    $(function () {
        $('[id*=FrmDate]').datepicker({
            format: "dd-mm-yyyy",
            language: "tr"
        });
    });
       $(function () {
       $('[id*=ToDate]').datepicker({
           format: "dd-mm-yyyy",
           language: "tr"
       });
   });
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="container" runat="server" >
                   <div id="divExport" runat="server" style="background-color:black" >
    <div id="dvheads" runat="server">
        
          <label id="CompanyName" runat="server" style="font:bold;background-color:black;color:white;" >SONI AUTO AND ALLIED INDUSTRIES LTD.</label><br /><br />

          <label id="CompanyAddress" runat="server"  style="font:bold;background-color:black;color:white;">M-19, 6th Phase, Adityapur Industrial Area, Gamharia, Jamshedpur</label><br /><br />
            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-3">
                          <label id="ReportType" runat="server"  style="font:bold;background-color:black;color:white;" >DateWise:-    From</label>
<asp:TextBox ID="FrmDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="From Date"></asp:TextBox>   
                    </div>
                    <div class="col-md-3">
                                                      <label id="To" runat="server"  style="font:bold;background-color:black;color:white;">  To</label>
<asp:TextBox ID="ToDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="To Date" AutoPostBack="true"></asp:TextBox>  
                    </div>
                                       
                              
                </div>
                             
            </div>
              
<%--                        <br />--%>
         <label id="Rep" runat="server" style="font:bold;background-color:black;color:white;" > Report Generated On :- </label>  <label id="ReportDate" runat="server" style="font:bold;background-color:black;color:white;" >25-Jun-2024</label> <br />



            
    </div>
                     <table >
             <tr>
                 <td>
                     
                           <asp:GridView ID="Gv_RawMaterial" runat="server"
    AutoGenerateColumns="False" DataKeyNames="ID" 
    HeaderStyle-BackColor="Yellow" HeaderStyle-ForeColor="Black">
                               
                   
    <Columns>
       <%-- <asp:TemplateField HeaderText="Delete">
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton_Del" runat="server" CommandName="delete" CausesValidation="false">Delete</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:BoundField DataField="ID" HeaderText="Sno." ReadOnly="True" SortExpression="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="T_RMM_BILL" HeaderText="Bill No." ReadOnly="True" SortExpression="T_RMM_BILL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="T_RMM_PDATE" HeaderText="Purchase Date" ReadOnly="True" SortExpression="T_RMM_PDATE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="T_RMM_UNITNUM" HeaderText="Unit No." ReadOnly="True" SortExpression="T_RMM_UNITNUM" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="T_RMM_PARTY" HeaderText="Party Name" ReadOnly="True" SortExpression="T_RMM_PARTY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />

        <asp:BoundField DataField="T_RMM_SECTION" HeaderText="Section" ReadOnly="True" SortExpression="T_RMM_SECTION" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="T_RMM_QUANTITY" HeaderText="Quantity" ReadOnly="True" SortExpression="T_RMM_QUANTITY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />

        <asp:BoundField DataField="T_RMM_RATE" HeaderText="Rate" ReadOnly="True" SortExpression="T_RMM_RATE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="T_RMM_TOTALAMNT" HeaderText="Total" ReadOnly="True" SortExpression="T_RMM_TOTALAMNT" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />

        <asp:BoundField DataField="T_RMM_EDATE" HeaderText="Entry Date" ReadOnly="True" SortExpression="T_RMM_EDATE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        
    
    </Columns>

</asp:GridView>
                 </td>
             </tr>
         </table>
        </div>
        </div>
       
        <br />
        <asp:Button ID="btnexport" runat="server" Text="ExportToexcel" />
    </form>
</body>
</html>
