<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DateWiseReport.aspx.cs" Inherits="DateWiseReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
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
       <form id="form1" runat="server">
       <div id="divExport" runat="server">
    <div id="dvheads" runat="server">
        <b>
          <label id="CompanyName" runat="server" style="font:bold;" >SONI AUTO AND ALLIED INDUSTRIES LTD.</label><br /><br />
          <label id="CompanyAddress" runat="server">M-19, 6th Phase, Adityapur Industrial Area, Gamharia, Jamshedpur</label><br /><br />
                        <div class="row">
                <div class="col-md-12">
                    <div class="col-md-3">
                          <label id="ReportType" runat="server"  style="font:bold;background-color:black;color:white;" >DateWise:-    From</label>
<asp:TextBox ID="FrmDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="From Date"></asp:TextBox>   
                    </div>
                    <div class="col-md-3">
                                                      <label id="To" runat="server"  style="font:bold;background-color:black;color:white;">  To</label>
<asp:TextBox ID="ToDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="To Date"  AutoPostBack="true"></asp:TextBox>  
                    </div>
                                       
                              
                </div>
                             
            </div>
                        <br /><br />
         <label id="Rep" runat="server" > Report Generated On :- </label>  <label id="ReportDate" runat="server" >25-Jun-2024</label> <br /><br />



            </b>
    </div>
                     <table>
             <tr>
                 <td>
                     
                           <asp:GridView ID="Gv_RawMaterial" runat="server"
    AutoGenerateColumns="False" DataKeyNames="ID"
    HeaderStyle-BackColor="#0078d4" HeaderStyle-ForeColor="AliceBlue">
                               
                   
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
        <asp:Button ID="btnexport" runat="server" Text="Button"  />
    </form>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>

