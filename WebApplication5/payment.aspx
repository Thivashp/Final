<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payment.aspx.cs" Inherits="WebApplication5.payment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/bootstrap.js"></script>

    <script src="Scripts/jquery-1.10.2.js"></script>
    <title>MOTION INC</title>
    <style>
        main {
            background-image: url(https://i.embed.ly/1/display/resize?key=1e6a1a1efdb011df84894040444cdc60&url=http%3A%2F%2Fwww.osisa.org%2Fsites%2Fdefault%2Ffiles%2Fimagecache%2Farticle_full%2Fprog_prof%2Fimages%2Fhiv_ribbon_web2.jpg);
        }
        .auto-style1 {
            width: 420px;
            height: 120px;
        }
    </style>
</head>
<body>
    
    <div class ="col-sm-4">
 Powered By:&nbsp;&nbsp;&nbsp;&nbsp;
        <img alt="" class="auto-style1" src="Images/downloadPayfastlogo.png" /></div>
<div class ="col-sm-4">

    &nbsp;<br />
</div>
   <form id="form1" runat="server">
       <div id="main">
       <h3>
           Enter credentials then click log in
       </h3>

    <div>
        Amount :&nbsp R<asp:TextBox ID="TextBox3" runat="server" ReadOnly="true "></asp:TextBox>
<br />
        <br />

      User Name :&nbsp;&nbsp;&nbsp;  <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
       <br />
        <br />
        Password :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="TextBox2" runat="server" ></asp:TextBox>
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:button runat="server" ID="btnDonate" text="Login" OnClick="btnOrder_Click"/>  <p class="button"><a href="http://itstudents.dut.ac.za/201626/Orders"> Cancel </a></p>
    </div>
           </div>
    </form>
</body>
</html>

