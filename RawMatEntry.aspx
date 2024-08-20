<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RawMatEntry.aspx.cs" Inherits="RawMatEntry" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--  --%>
       <script src="js/jquery-1.12.4.js"></script>
   <script src="js/jquery-ui.js"></script>
    <script type="text/javascript">
        //On Page Load
        $(function () {
            $("#dvAccordian").accordion();
            $("#tabs").tabs();
        });

        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $("#dvAccordian").accordion();
                    $("#tabs").tabs();
                }
            });
        };
    </script>
    <%--  --%>
    <%-- test --%>
 

    <!-- Bootstrap -->

    <!-- Bootstrap DatePicker -->
    <link href="css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="js/bootstrap-datepicker.js" type="text/javascript"></script>
    <!-- Bootstrap DatePicker -->
    <%--    <script src="asset/js/jquery-3.5.1.slim.min.js"></script>--%>
    <script type="text/javascript">
        $(function () {
            $('[id*=txtDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd-mm-yyyy",
                language: "tr",

            });
        });
    </script>
  
    <%-- test --%>


    <link href="css/sweetalert.min.css" rel="stylesheet" />
    <script src="assets/javascripts/sweetalert.min.js"></script>
    <%-- commented to make the calendar work --%>
    <%--	<script src="assets/vendor/jquery/jquery.js"></script>--%>
    <%--    <script src="js/abhi.js"></script>--%>
    <%-- commented to make the calendar work --%>
    <script src="js/jquery.searchabledropdown-1.0.8.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("select").searchable({
                maxListSize: 200, // if list size are less than maxListSize, show them all
                maxMultiMatch: 300, // how many matching entries should be displayed
                exactMatch: false, // Exact matching on search
                wildcards: true, // Support for wildcard characters (*, ?)
                ignoreCase: true, // Ignore case sensitivity
                latency: 200, // how many millis to wait until starting search
                warnMultiMatch: 'top {0} matches ...',
                warnNoMatch: 'no matches ...',
                zIndex: 'auto'
            });
        });

    </script>
    <script language="javascript" type="text/javascript">
        function validate() {
            var summary = "";
            summary += isvalidDate();
            summary += isvalidBillNo();
            summary += isvalidParty();
            summary += isvalidRate();
            summary += isvalidQty();
            summary += isvalidUnit();
            summary += isvalidSection();

            if (summary != "") {
                alert(summary);
                return false;
            }
            else {
                return true;
            }
        }
        function isvalidDate() {
            var uid;
            var temp = document.getElementById("<%=txtDate.ClientID %>");
            uid = temp.value;
            if (uid == "") {
                return ("Please Enter Date" + "\n");
            }
            else {
                return "";
            }
           
          
        }
        function isvalidBillNo() {
            var uid;
            var temp = document.getElementById("<%=txtBillNum.ClientID %>");
uid = temp.value;
if (uid == "") {
return ("Please Enter BillNum" + "\n");
}
else {
return "";
}
}
        function isvalidParty() {
var uid;
var temp = document.getElementById("<%=ddlPartyName.ClientID %>");
            uid = temp.value;
            if (uid == "0") {
                return ("Please select Party" + "\n");
            }
            else {
                return "";
            }
        }
        function isvalidRate() {
            var uid;
            var temp = document.getElementById("<%=txtRate.ClientID %>");
            uid = temp.value;
            if (uid == "") {
                return ("Please Enter Rate" + "\n");
            }
            else {
                return "";
            }
        }
        function isvalidQty() {
            var uid;
            var temp = document.getElementById("<%=txtQty.ClientID %>");
uid = temp.value;
if (uid == "") {
return ("Please Enter Qty" + "\n");
}
else {
return "";
}
}
        function isvalidUnit() {
var uid;
            var temp = document.getElementById("<%=ddlUnit.ClientID %>");
            uid = temp.value;
            if (uid == "0") {
                return ("Please select Unit" + "\n");
            }
            else {
                return "";
            }
        }
        function isvalidSection() {
            var uid;
            var temp = document.getElementById("<%=ddlSection.ClientID %>");
                    uid = temp.value;
                    if (uid == "0") {
                        return ("Please select Section" + "\n");
                    }
                    else {
                        return "";
                    }
                }
    </script>
    <script language="javascript" type="text/javascript">
        function validateDate() {
            var today = new Date();
            var dd = today.getDate();

            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }

            if (mm < 10) {
                mm = '0' + mm;
            }
            today = dd + '-' + mm + '-' + yyyy;
            if (uid > today) {
                return ("Please Enter correct Date" + "\n");
            }
        }
    </script>

    <style type="text/css">
        .clsTxt {
            width: 100%;
            width: 100%;
            min-height: 25px;
            max-height: 200px;
            resize: none;
        }
    </style>
    <style type="text/css">
        .clsTxt1 {
            width: 100%;
            width: 100%;
            min-height: 100px;
            max-height: 400px;
            resize: none;
        }
    </style>
    <form id="frm1" class="form-inline" runat="server">
                           <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="Receiving Date"  ></asp:TextBox>               
<%--Put all dropdowns in a common panel--%>
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="panel1" runat="server">
<ContentTemplate>

        <%--<asp:DropDownList ID="DropDownList1" runat="server" Width="90%" OnSelectedIndexChanged="ddlMaterialCode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
        <div class="row">
            <div class="col-md-12">
                <%--<section class="panel">
                    <header class="panel-heading" style="background-color:orangered">
                                      <label style="color:aliceblue; font:bolder;">Request No.</label>
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="small"></asp:TextBox>
                                    
                                    
                                    <label style="color:black;margin-left:10px;">Request No. Date:</label>
                                   <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>

                                     <label style="color:black; margin-left:10px;">Request No. Status</label>
                                  <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>

									<div class="panel-actions">
										<a href="#" class="fa fa-caret-down"></a>
										
									</div>
                                    

								</header>
                </section>--%>
                <section class="panel">
                    <%--<header class="panel-heading">
                       

                        <h2 class="panel-title">Create New RM ENTRY</h2>


                        <p class="panel-subtitle">
                        </p>
                    </header>--%>
                                        <div class="panel-body">
<%--                    <div class="panel-body" style="color:black;background-color:grey">--%>




                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
<%--                                       <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="Enter Date"></asp:TextBox>   --%>
                                    <br />
                                    <label class="control-label">Bill Number</label>
                                           
                                    <asp:TextBox ID="txtBillNum" runat="server"  Width="90%"  AutoCompleteType="Disabled"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator ID="ReqtxtBillNum" runat="server" ControlToValidate="txtBillNum" Display="None" ErrorMessage="Enter the Bill Number" ForeColor="Red" ValidationGroup="RM"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator Display="Dynamic" ID="RegtxtBillNum" ErrorMessage="Please Enter Bill Number" runat="server" ControlToValidate="txtBillNum" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                         
                                    <br />
                                    <label class="control-label">Party Name</label>
                                    <br />
                                    <asp:DropDownList ID="ddlPartyName" runat="server" Width="70%"  AutoPostBack="true"></asp:DropDownList>
                                 
                                    <br />

                                    
                                    


                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group">

                                   


                                    <label class="control-label">Rate</label>
                                    <br />
                                    <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" ></asp:TextBox>
                               
<%--<asp:RequiredFieldValidator ID="ReqtxtRate" runat="server" ControlToValidate="txtRate" Display="None" ErrorMessage="Enter Rate" ForeColor="Red" ValidationGroup="RM"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator1" ErrorMessage="Please Enter Bill Number"  runat="server" ControlToValidate="txtRate" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                    <br />

                                    <label class="control-label">Quantity</label>
                                    <br />
                                    <asp:TextBox ID="txtQty" runat="server" CssClass="form-control"></asp:TextBox>
                                 <%--   <asp:RequiredFieldValidator ID="ReqtxtQty" runat="server" ControlToValidate="txtQty" Display="None" ErrorMessage="Enter Quantity" ForeColor="Red" ValidationGroup="RM"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator2" ErrorMessage="Please Enter Quantity" runat="server" ControlToValidate="txtRate" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                    <br />

                                   
                                     <label class="control-label">Remarks</label>
 <br />
 <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                                    <br />

                                </div>
                            </div>
                               
                            <div class="col-sm-3">
                                <div class="form-group">
                                   

                                    
                                    <label class="control-label">Unit Number</label>
                                    <br />
                                    <asp:DropDownList ID="ddlUnit" runat="server" Width="90%">
    <asp:ListItem Value="0">Select Unit</asp:ListItem>
    <asp:ListItem Value="Unit1">Unit1</asp:ListItem>
    <asp:ListItem Value="Unit2">Unit2</asp:ListItem>
    <asp:ListItem Value="Unit3">Unit3</asp:ListItem>
</asp:DropDownList>

<br />
                                                                        <label class="control-label">Section</label>
                                    <br />
<asp:DropDownList ID="ddlSection" runat="server" Width="90%"></asp:DropDownList>
<br />


                                   <%-- <label class="control-label"> Date</label>
                                    <br />
                                       <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                    <br />--%>


                                   
                                </div>
                            </div>
                        </div>

                    </div>

                    <footer class="panel-footer">
                            <asp:Button ID="btn_add" Text="Add" class="btn btn-primary" runat="server" OnClientClick="validate()" OnClick="btn_add_Click" />

                       
                            <asp:Button ID="btnAddNew" Text="New Entry" class="btn btn-primary" runat="server" OnClick="btnAddNew_Click" />
                    
                          
                    </footer>
                    
                </section>
            </div>
        </div>




        <div class="row">
            <div class="panel-body" style="overflow: scroll">

                <!-- Flot: Curves -->

                <table>
                    <tr>
                        <td>
                            
                                  <asp:GridView ID="Gv_RawMaterial" runat="server"
           AutoGenerateColumns="False" DataKeyNames="ID" OnRowDeleting="OnRowDeleting"
           HeaderStyle-BackColor="#0078d4" HeaderStyle-ForeColor="AliceBlue" OnRowEditing="Gv_RawMaterial_RowEditing" >
                                      
                          
           <Columns>
               <%--<asp:TemplateField HeaderText="Delete">
                   <ItemTemplate>
                      
                                   <asp:Button ID="btnDelete" runat="server" class="btn btn-small primary" Text="Delete"  CommandName="Delete" OnRowDataBound="OnRowDataBound" />
                                 
                   </ItemTemplate>
               </asp:TemplateField>--%>
                                                <%-- <asp:CommandField ButtonType="Link" ShowDeleteButton="true" ItemStyle-CssClass="btn"
ItemStyle-Width="150" />--%>
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
                <%--<asp:CommandField ButtonType="Link" ShowEditButton="true" ItemStyle-CssClass="btn"
     ItemStyle-Width="150" />--%>
          
           </Columns>

       </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
                <footer class="panel-footer">
                   <%-- <asp:Button ID="btn_submit_req" Text="Submit" class="btn btn-primary" runat="server" OnClick="btn_submit_req_Click" />--%>

<%--                    <asp:Button ID="btn_draft" Text="Save as Draft" class="btn btn-primary" runat="server" />--%>
                </footer>


            </div>
        </div>



                                    </ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID ="ddlSection"/>

</Triggers>
</asp:UpdatePanel>
    

        <script type="text/javascript">
            $(function () {
                SetDatePicker();
            });

            //On UpdatePanel Refresh.
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        SetDatePicker();
                    }
                });
            };

            function SetDatePicker() {
                $('#txtDate').datepicker({ minDate: 0 });
            }
        </script>

         <script type="text/javascript">
             //On Page Load.
             $(function () {
                 SetDatePicker();
             });

             //On UpdatePanel Refresh.
             var prm = Sys.WebForms.PageRequestManager.getInstance();
             if (prm != null) {
                 prm.add_endRequest(function (sender, e) {
                     if (sender._postBackSettings.panelsToUpdate != null) {
                         SetDatePicker();
                         $(".datepicker-orient-bottom").hide();
                         SetDD();
                     }
                 });
             };

             function SetDatePicker() {
                 $("#datepicker").datepicker();
                 if ($("#txtBookDate").val() == "") {
                     var dateNow = new Date();
                     $('#datepicker').datepicker("setDate", dateNow);
                 }
             }
         </script>
        <script type="text/javascript">
          
            function SetDD() { 
            $(document).ready(function () {
                $("select").searchable({
                    maxListSize: 200, // if list size are less than maxListSize, show them all
                    maxMultiMatch: 300, // how many matching entries should be displayed
                    exactMatch: false, // Exact matching on search
                    wildcards: true, // Support for wildcard characters (*, ?)
                    ignoreCase: true, // Ignore case sensitivity
                    latency: 200, // how many millis to wait until starting search
                    warnMultiMatch: 'top {0} matches ...',
                    warnNoMatch: 'no matches ...',
                    zIndex: 'auto'
                });
            });
            }
        </script>

    </form>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>


