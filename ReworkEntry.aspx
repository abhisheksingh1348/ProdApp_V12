<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReworkEntry.aspx.cs" Inherits="ReworkEntry"  EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%-- test --%>
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
            $('[id*=txtDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd-mm-yyyy",
                language: "tr"
            });
        });
    </script>
    <%-- test --%>
    <script type="text/javascript">
        function Showalert() {
            alert('Call JavaScript function from codebehind');
        }
    </script>

   <%-- <link href="css/sweetalert.min.css" rel="stylesheet" />
    <script src="assets/javascripts/sweetalert.min.js"></script>--%>
    <%-- commented to make the calendar work --%>
    <%--	<script src="assets/vendor/jquery/jquery.js"></script>--%>
    <%--    <script src="js/abhi.js"></script>--%>
    <%-- commented to make the calendar work --%>
<%--    <script src="js/jquery.searchabledropdown-1.0.8.min.js"></script>--%>
    <%--<script type="text/javascript">
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

    </script>--%>

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
<%--    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    
    <form id="frm1" class="form-inline" runat="server">

                           <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="Enter Date"></asp:TextBox>               
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




                        <div class="row">
                            <div class="col-sm-6">
                              <div class="form-group">
                                
                                                                                                          <label class="control-label">Section</label>
                                    <br />
<asp:DropDownList ID="ddlSection" runat="server" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlSection_SelectedIndexChanged"></asp:DropDownList>
<br />
                                                                    <label class="control-label">Available Quantity</label>
<br />
<asp:TextBox ID="txtAqty" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                  <br />
                                                                    <label class="control-label">Quantity</label>
<br />
<asp:TextBox ID="txtQty" runat="server" CssClass="form-control"></asp:TextBox>
                                  <br />
                              </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group">

                                   


                                   
                                    

                                </div>
                            </div>
                               
                            <div class="col-sm-3">
                                
                            </div>
                        </div>

                    </div>

                    <footer class="panel-footer">
                            <asp:Button ID="btn_rew" Text="Add" class="btn btn-primary" runat="server" OnClick ="btn_rew_Click"  />

                       
                    
                          
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
                            
                                  <asp:GridView ID="Gv_RejEntry" runat="server"
           AutoGenerateColumns="False" DataKeyNames="ID" 
           HeaderStyle-BackColor="#0078d4" HeaderStyle-ForeColor="AliceBlue">
                                      
                          
           <Columns>
              
               <asp:BoundField DataField="ID" HeaderText="Sno." ReadOnly="True" SortExpression="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               

               <asp:BoundField DataField="T_RMREJM_SECTION" HeaderText="Section" ReadOnly="True" SortExpression="T_RMM_SECTION" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMREJM_QTY" HeaderText="Quantity" ReadOnly="True" SortExpression="T_RMM_QUANTITY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />   
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

    </form>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>


