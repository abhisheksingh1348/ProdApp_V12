<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RM_stock_Report.aspx.cs" Inherits="DateWise_Report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Bootstrap -->
    <script src="js/jquery-1.8.3.min.js"></script>
<%--<script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>--%>
<%--<script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>--%>
    <script src="ReportJsandCss/bootstrap.min.js"></script>
    <link href="ReportJsandCss/bootstrap.min.css" rel="stylesheet" />
<%--<link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />--%>
<!-- Bootstrap -->
    <link href="ReportJsandCss/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="ReportJsandCss/bootstrap-datepicker.js"></script>
<!-- Bootstrap DatePicker -->
<%--<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css"/>--%>
<%--<script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>--%>
    <link href="css/bootstrap-datepicker.css" rel="stylesheet" />
<script src="js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.min.js"></script>
<!-- Bootstrap DatePicker -->
<%--    <script src="asset/js/jquery-3.5.1.slim.min.js"></script>--%>
    <%--<script type="text/javascript">
        $(function () {
            $('[id*=txtFromDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd-mm-yyyy",
                language: "tr",
                orientation: 'bottom'

            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=txtToDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd-mm-yyyy",
                language: "tr",
                orientation: 'bottom'
            });
        });
    </script>--%>
<!-- Bootstrap DatePicker -->
<script type="text/javascript">
    $(function () {
        $('[id*=FrmDate]').datepicker({
            changeMonth: true,
            changeYear: true,
            format: "dd-mm-yyyy",
            language: "tr"
        });
    });
    
</script>
    <script type="text/javascript">
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
    <form id="form1" runat="server">
        <div id="container" runat="server" >
                   <div id="divExport" runat="server"  >
    <div id="dvheads" runat="server">
        
          <label id="CompanyName" runat="server" style="font:bold;background-color:black;color:white;" >SONI AUTO AND ALLIED INDUSTRIES LTD.</label><br /><br />

          <label id="CompanyAddress" runat="server"  style="font:bold;background-color:black;color:white;">M-19, 6th Phase, Adityapur Industrial Area, Gamharia, Jamshedpur</label><br /><br />
            <div class="row">
                
                             
            </div>
              
<%--                        <br />--%>
         <label id="Rep" runat="server" style="font:bold;background-color:black;color:white;" > Report Generated On :- </label>  <label id="ReportDate" runat="server" style="font:bold;background-color:black;color:white;" ></label> <br />



            
    </div>
                     <table >
             <tr>
                 <td>
                     
                     <asp:GridView ID="gv_SM_report" runat="server"></asp:GridView>
                 </td>
             </tr>
         </table>
        </div>
        </div>
       
       

                <asp:Button ID="btnexport" runat="server" Text="ExportToexcel" OnClick="btnexport_Click" />

    </form>
</body>

</html>
