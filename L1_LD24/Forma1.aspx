<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forma1.aspx.cs" Inherits="L1_LD24.Forma1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>L1_24</title>
    <style type="text/css">
        #form1 {
            margin-left: 0px;
        }
    </style>
</head>
<body style="margin-top: 26px">
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Pasirinkite tekstinio failo variantą:"></asp:Label>
            <br />
        </div>
        <p>
            <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" CausesValidation="True" DataSourceID="XmlDataSource1" DataTextField="title" DataValueField="title">
            </asp:DropDownList>
            <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/FailuPavadinimai.xml"></asp:XmlDataSource>
            
            
        </p>
        <p>
            <asp:Label ID="Label2" runat="server" Text="Pasirinkto failo duomenys:" Visible="False"></asp:Label>
        </p>
        <p>
            <asp:Table ID="Table1" runat="server" Visible="False">
            </asp:Table>
            <asp:Button ID="Button1" runat="server" Text="Apskaičiuoti rezultatą" UseSubmitBehavior="False" OnClick="Button1_Click" />
        </p>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
        <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="Custom error label" Visible="False"></asp:Label>
        <br />
        <asp:Label ID="Label3" runat="server" Text="Rezultatas:" Visible="False"></asp:Label>
        <asp:Table ID="Table2" runat="server" Visible="False">
        </asp:Table>
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Išvalyti duomenis" UseSubmitBehavior="False" CausesValidation="False" />
    </form>
</body>
</html>
