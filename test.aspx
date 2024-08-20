<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    <script src="js/aspsnippets_Pager.min.js"></script>
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
             url: "ItemMaster.aspx/GetItems",
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
                 $("td", row).eq(0).html($(this).find("T_IM_CODE").text());
                 $("td", row).eq(1).html($(this).find("T_IM_DESC").text());
                 $("td", row).eq(2).html($(this).find("T_IM_SECTION").text());
                 $("td", row).eq(3).html($(this).find("T_IM_LEAF_NO").text());
                 $("td", row).eq(4).html($(this).find("T_IM_LESSPER").text());
                 $("td", row).eq(5).html($(this).find("T_IM_LESSWEIGHT").text());
                 $("td", row).eq(6).html($(this).find("T_IM_THICKNESS").text());
                 $("td", row).eq(7).html($(this).find("T_IM_WIDTH").text());
                 $("td", row).eq(8).html($(this).find("T_IM_SHEARSIZE").text());
                 $("td", row).eq(9).html($(this).find("T_IM_CAT").text());
                 $("td", row).eq(10).html($(this).find("T_IM_TYPE").text());
                 $("td", row).eq(11).html($(this).find("T_IM_CREATED_BY").text());
                 $("td", row).eq(12).html($(this).find("T_IM_CREATED_DT").text());


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
</head>
<body>
    <form id="form1" runat="server">
        <div>
                            Search:
<asp:TextBox ID="txtSearch" runat="server" />
<hr />
<asp:GridView ID="gvItemMaster" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CODE" HeaderText="T_IM_CODE"
            ItemStyle-CssClass="ContactName" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_DESC" HeaderText="Desc" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_SECTION" HeaderText="Section" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_LEAF_NO" HeaderText="LEAF" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_WEIGHT" HeaderText="T_IM_WEIGHT" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_LESSPER" HeaderText="Less %" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_LESSWEIGHT" HeaderText="T_IM_LESSWEIGHT" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_THICKNESS" HeaderText="T_IM_THICKNESS" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_WIDTH" HeaderText="T_IM_WIDTH" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_SHEARSIZE" HeaderText="T_IM_SHEARSIZE" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CAT" HeaderText="T_IM_CAT" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_TYPE" HeaderText="T_IM_TYPE" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CREATED_BY" HeaderText="T_IM_CREATED_BY" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CREATED_DT" HeaderText="T_IM_CREATED_DT" />
  
    </Columns>
</asp:GridView>
<br />
<div class="Pager">
</div>
        </div>
    </form>
</body>
</html>
