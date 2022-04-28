<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="TailSpyWorksAdvanced.ProductDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:FormView ID="FormView_Product" runat="server" DataKeyNames="ProductID" 
      >
        <ItemTemplate>
        <div class="ContentHead"><%# Eval("ProductName") %></div><br />
        <table  border="0">
        <tr>
            <td style="vertical-align: top;">
             <img src='Catalog/Images/<%# Eval("ProductImage") %>'  border="0" alt='<%# Eval("ProductName") %>' />
            </td>
            <td style="vertical-align: top"><%# Eval("Description") %><br /><br /><br />
         
            </td>
        </tr>
        </table>
        <span class="UnitCost"><b>Your Price:</b>&nbsp;<%# Eval("UnitCost", "{0:c}")%></span><br /><span class="ProductNumber"><b>Product Number:</b>&nbsp;<%# Eval("ModelNumber") %></span><br />
        <a href='AddToCart.aspx?ProductID=<%# Eval("ProductID") %>' style="border: 0 none white"><img src="~/Styles/Images/add_to_cart.gif" runat="server" alt="" style="border-width: 0" /></a><br />
        <br /><div class="SubContentHead">Reviews</div><br />
        <a id="ReviewList_AddReview" href="ReviewAdd.aspx?productID=<%# Eval("ProductID") %>">
           <img runat="server" style="vertical-align: bottom" src="~/Styles/Images/review_this_product.gif" alt="" />
        </a>       
        </ItemTemplate>
    </asp:FormView>
</asp:Content>
