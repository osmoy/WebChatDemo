<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebChatDemo.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模拟请求</title>
    <script src="/js/jquery-1.8.2.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">

        <input type="button" id="btn" value="测试接入" style="width:100px; font-size:16px;"  />
        <br />
        <br />
        <input type="button" id="btn2" value="post请求" style="width:100px; font-size:16px;"  />

    </form>


    <script type="text/javascript">
        $('#btn').click(function () {
            $.get('/ashx/wxapi.ashx', { echoStr: 'ok', signature: 'ql0afwmix8nrztkeof9cxsvbvu8=', timestamp: null, nonce: null }, function (data) {
                alert(data);
            })
        })

        $('#btn2').click(function () {
            $.post('/ashx/wxapi.ashx', null, function (data) {
                alert(data);
            })
        })

    </script>
</body>
</html>
