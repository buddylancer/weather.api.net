<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="padding:4pt">
        <tr>
        <td colspan="2" style="text-align: center; font-size:larger">
        This is test application for Web API services.
        <hr />
        </td></tr>
        <tr>
        <td style="text-align: right">Type country code:</td>
        <td>
            <asp:TextBox ID="CountryCode" runat="server"></asp:TextBox> (for ex. RU)
            </td>
        </tr>
        <tr>
        <td style="text-align: right">Type postal (zip) code:</td>
        <td>
            <asp:TextBox ID="ZipCode" runat="server"></asp:TextBox> (for ex. 603000)
            </td>
        </tr>
        <tr>
        <td></td>
        <td>
            <asp:Button ID="Submit" runat="server" Text="Check service" 
                onclick="Button1_Click" />
            </td>
        </tr>
        <tr>
        <td style="text-align: right" valign="top">Result:</td>
        <td>
            <asp:TextBox ID="Result" runat="server" Rows="5" TextMode="MultiLine" 
                Width="262px"></asp:TextBox>
            </td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
