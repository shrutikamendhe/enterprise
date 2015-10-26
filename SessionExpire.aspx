<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="SessionExpire.aspx.cs" Inherits="TTSHWeb.SessionExpire" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
                <tr>
                    <td colspan="3" style="text-align: center;">
                        Your Session has timed out.
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="padding-top: 10px; padding-bottom:10px; text-align: center;">
                        <h3>
                            Click <span><a href="Login.aspx">here</a></span> to login.</h3>
                    </td>
                </tr>
            </table>
</asp:Content>
