<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMessage.aspx.cs" Inherits="WebChatDemo.SendMessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtMsg" runat="server"></asp:TextBox>

        <asp:Button ID="btngo" runat="server" OnClick="btngo_Click" />
    </div>
    </form>
</body>
</html>
