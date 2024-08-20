<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RMreport.aspx.cs" Inherits="RMreport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script src="js/jquery-1.12.4.js"></script>
<script src="js/jquery-ui.js"></script>

<!-- Bootstrap -->

<!-- Bootstrap DatePicker -->
<link href="css/bootstrap-datepicker.css" rel="stylesheet" />
<script src="js/bootstrap-datepicker.js" type="text/javascript"></script>
<!-- Bootstrap DatePicker -->
<%--    <script src="asset/js/jquery-3.5.1.slim.min.js"></script>--%>
<script type="text/javascript">
    $(function () {
        $('[id*=FrmDate]').datepicker({
            changeMonth: true,
            changeYear: true,
            format: "dd-mm-yyyy",
            language: "tr"
        });
    });
    $(function () {
        $('[id*=ToDate]').datepicker({
            changeMonth: true,
            changeYear: true,
            format: "dd-mm-yyyy",
            language: "tr"
        });
    });
</script>
</head>
<body>
      <form id="Form1"  runat="server">
        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="Enter Date"></asp:TextBox>
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
<asp:TextBox ID="ToDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="To Date" ></asp:TextBox>  
                    </div>
                                       
                              
                </div>
   </form>
</body>
</html>
