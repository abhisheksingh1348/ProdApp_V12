<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="itemMaster_Paging.aspx.cs" Inherits="itemMaster" EnableEventValidation="false" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">--%>
    <%--  --%>
 <style type="text/css">
     body
     {
         font-family: Arial;
         font-size: 10pt;
     }
     table
     {
         border: 1px solid #ccc;
     }
     table th
     {
         background-color: #F7F7F7;
         color: #333;
         font-weight: bold;
     }
     table th, table td
     {
         padding: 5px;
         border-color: #ccc;
     }
     .Pager span
     {
         color: #333;
         background-color: #F7F7F7;
         font-weight: bold;
         text-align: center;
         display: inline-block;
         width: 20px;
         margin-right: 3px;
         line-height: 150%;
         border: 1px solid #ccc;
     }
     .Pager a
     {
         text-align: center;
         display: inline-block;
         width: 20px;
         border: 1px solid #ccc;
         color: #fff;
         color: #333;
         margin-right: 3px;
         line-height: 150%;
         text-decoration: none;
     }
     .highlight
     {
         background-color: #FFFFAF;
     }
 </style>

 <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<%--    <script src="js/aspsnippets_Pager.min.js"></script>--%>
 <script type="text/javascript">
     $(function () {
         GetItems(1);
     });
     $("[id*=txtSearch]").live("keyup", function () {
         GetItems(parseInt(1));
     });
     $(".Pager .page").live("click", function () {
         GetItems(parseInt($(this).attr('page')));
     });
     function SearchTerm() {
         return jQuery.trim($("[id*=txtSearch]").val());
     };
     function GetItems(pageIndex) {
         $.ajax({
             type: "POST",
             url: "ItemMaster_paging.aspx/GetItems",
             data: '{searchTerm: "' + SearchTerm() + '", pageIndex: ' + pageIndex + '}',
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: OnSuccess,
             failure: function (response) {
                 alert(response.d);
             },
             error: function (response) {
                 alert(response.d);
             }
         });
     }
     var row;
     function OnSuccess(response) {
         var xmlDoc = $.parseXML(response.d);
         var xml = $(xmlDoc);
         var items = xml.find("Items");
         if (row == null) {
             row = $("[id*=gvItemMaster] tr:last-child").clone(true);
         }
         $("[id*=gvItemMaster] tr").not($("[id*=gvItemMaster] tr:first-child")).remove();
         if (items.length > 0) {
             $.each(items, function () {
                 var items = $(this);
                 $("td", row).eq(0).html($(this).find("RowNumber").text());
                 $("td", row).eq(1).html($(this).find("T_IM_DESC").text());
                 $("td", row).eq(2).html($(this).find("T_IM_SECTION").text());
                 $("td", row).eq(3).html($(this).find("T_IM_LEAF_NO").text());
                 $("td", row).eq(4).html($(this).find("T_IM_WEIGHT").text());
                 $("td", row).eq(5).html($(this).find("T_IM_LESSPER").text());
                 $("td", row).eq(6).html($(this).find("T_IM_LESSWEIGHT").text());
                 $("td", row).eq(7).html($(this).find("T_IM_THICKNESS").text());
                 $("td", row).eq(8).html($(this).find("T_IM_WIDTH").text());
                 $("td", row).eq(9).html($(this).find("T_IM_SHEARSIZE").text());
                 $("td", row).eq(10).html($(this).find("T_IM_CAT").text());
                 $("td", row).eq(11).html($(this).find("T_IM_TYPE").text());
                 $("td", row).eq(12).html($(this).find("T_IM_CREATED_BY").text());
                 $("td", row).eq(13).html($(this).find("T_IM_CREATED_DT").text());
             

                 $("[id*=gvItemMaster]").append(row);
                 row = $("[id*=gvItemMaster] tr:last-child").clone(true);
             });
            
             var pager = xml.find("Pager");
             $(".Pager").ASPSnippets_Pager({
                 ActiveCssClass: "current",
                 PagerCssClass: "pager",
                 PageIndex: parseInt(pager.find("PageIndex").text()),
                 PageSize: parseInt(pager.find("PageSize").text()),
                 RecordCount: parseInt(pager.find("RecordCount").text())
             });

             $(".T_IM_DESC").each(function () {
                 var searchPattern = new RegExp('(' + SearchTerm() + ')', 'ig');
                 $(this).html($(this).text().replace(searchPattern, "<span class = 'highlight'>" + SearchTerm() + "</span>"));
             });
         } else {
             var empty_row = row.clone(true);
             $("td:first-child", empty_row).attr("colspan", $("td", row).length);
             $("td:first-child", empty_row).attr("align", "center");
             $("td:first-child", empty_row).html("No records found for the search criteria.");
             $("td", empty_row).not($("td:first-child", empty_row)).remove();
             $("[id*=gvItemMaster]").append(empty_row);
         }
     };
 </script>
    <%--  --%>
     <%-- test --%>


 <!-- Bootstrap -->

 <!-- Bootstrap DatePicker -->
 
 <!-- Bootstrap DatePicker -->
 <%--    <script src="asset/js/jquery-3.5.1.slim.min.js"></script>--%>

 <%-- test --%>


 
 <%-- commented to make the calendar work --%>
 <%--	<script src="assets/vendor/jquery/jquery.js"></script>--%>
 <%--    <script src="js/abhi.js"></script>--%>
 <%-- commented to make the calendar work --%>
 <%--<script src="js/jquery.searchabledropdown-1.0.8.min.js"></script>
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
        <form id="frm1" class="form-inline" runat="server">  
<%--             <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" Enabled="false" ></asp:TextBox>--%>
<%--Put all dropdowns in a common panel--%>
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="panel1" runat="server">
<ContentTemplate>
   
      
        <div class="row">
            <div class="col-md-12">
             
                <section class="panel">
                   
                    <div class="panel-body">

                                                <div class="row">
                                                                                <div class="col-sm-3">
                                <div class="form-group">
                                                 <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" Enabled="false" ></asp:TextBox>

                                    <br />
                                     <label class="control-label">Party Name</label>
  <br />
  <asp:DropDownList ID="ddlPartyName" runat="server" Width="70%"></asp:DropDownList>
                                    <br />
                                    
                                    <label class="control-label">Leaf Number</label>
                                    <br />
                                    <asp:DropDownList ID="ddlLeaf" runat="server" Width="90%">
    
</asp:DropDownList>

<br />

                        
                                                                       
<br />


                            


                                   
                                </div>
                            </div>
                                                                                <div class="col-sm-3">
                                <div class="form-group">

                                   


                                    <label class="control-label">Shearing Size</label>
                                    <br />
                                    <asp:TextBox ID="txtSs" runat="server" CssClass="form-control" ></asp:TextBox>

                                    <br />

                                    <label class="control-label">Width</label>
                                    <br />
                                    <asp:TextBox ID="txtWidth" runat="server" CssClass="form-control"></asp:TextBox>
                               
                                    <br />

                                   <label class="control-label">Thickness</label>
<br />
<asp:TextBox ID="txtThickness" runat="server" CssClass="form-control"></asp:TextBox>
                                    <br />
                                    <label class="control-label">Less Percent</label>
<br />
<asp:TextBox ID="lessP" runat="server" CssClass="form-control" Text="3"></asp:TextBox>
                                    <br />
                                                                     
                                </div>
                            </div>
                                                    
                            <div class="col-sm-6">
                                <div class="form-group">
                                                                                                          <label class="conrol-label">Model Description</label>
<br />
                                     <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" Rows="20" Columns="60"></asp:TextBox>
                                    

                                    <br />
                                                                                                            <label class="control-label">Category</label>
                                    <br />
<asp:DropDownList ID="ddlCat" runat="server" Width="90%">
        <asp:ListItem Value="0">Select Category</asp:ListItem>
    <asp:ListItem Value="MARKET">MARKET</asp:ListItem>
    <asp:ListItem Value="GENERAL">GENERAL</asp:ListItem>
   
</asp:DropDownList><br />
                                                                        <label class="control-label">Prab./Conv./Others</label>
<br />
                                    <asp:DropDownList ID="ddlType" runat="server" Width="90%">
         <asp:ListItem Value="0">Select Type</asp:ListItem>
 <asp:ListItem Value="PARAB">PARAB.</asp:ListItem>
 <asp:ListItem Value="CONVENTIONAL">CONV.</asp:ListItem>
                                        <asp:ListItem Value="OTHERS">OTHERS</asp:ListItem>
</asp:DropDownList>                                </div>
                            </div>
                                                                                                        

                        </div>

                    </div>

                    <footer class="panel-footer">
                            <asp:Button ID="btnSave" Text="Save" class="btn btn-primary" runat="server" OnClick="btnSave_Click"  />

                       
                            <asp:Button ID="btnUpdate" Text="Update" class="btn btn-primary" runat="server"  />
                    
                          
                    </footer>
                    
                </section>
            </div>
        </div>




        <div class="row">
            <%--<div class="panel-body" style="overflow: scroll">--%>

                <!-- Flot: Curves -->

                Search:
<asp:TextBox ID="txtSearch" runat="server" />
<hr />
<asp:GridView ID="gvItemMaster" runat="server"  AutoGenerateColumns="false" OnRowDataBound="gvItemMaster_RowDataBound" OnSelectedIndexChanged="gvItemMaster_SelectedIndexChanged" AllowPaging="true" PageSize="5">
    <Columns>
                                      <asp:ButtonField Text = "Select" CommandName = "Select" />

        <asp:BoundField HeaderStyle-Width="150px" DataField="RowNumber" HeaderText="Code"
            ItemStyle-CssClass="ContactName" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_DESC" HeaderText="Desc" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_SECTION" HeaderText="Section" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_LEAF_NO" HeaderText="Leaf" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_WEIGHT" HeaderText="Weight" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_LESSPER" HeaderText="Less %" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_LESSWEIGHT" HeaderText="LessWeight" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_THICKNESS" HeaderText="Thickness" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_WIDTH" HeaderText="Width" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_SHEARSIZE" HeaderText="ShearSize" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CAT" HeaderText="Cat" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_TYPE" HeaderText="Type" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CREATED_BY" HeaderText="Created By" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CREATED_DT" HeaderText="Created Dt" />
  
    </Columns>
</asp:GridView>
                <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
<br />
<div class="Pager">
</div>
                <br />
                <%--<footer class="panel-footer">
                 
                </footer>--%>


           <%-- </div>--%>
        </div>



                                    </ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID ="ddlCat"/>

</Triggers>
</asp:UpdatePanel>
    

     

    </form>
<%--</asp:Content>--%>


