<%@ Page EnableEventValidation="false" EnableViewStateMac="false" Title="Release Resin"
    Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="ReleaseResin.aspx.cs" Inherits="jnj.visus.cim.RMT.NextGenWeb.ReleaseResin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dialog" title="Dialog Title">
    </div>
    <div id="dialogNew" title="Dialog Title">
    </div>
    <div>
        <table width="100%">
            <tr>
                <td>
                    <fieldset>
                        <legend class="legendcss">Release Resin Batch</legend>
                        <table>
                            <tr>
                                <td class="labecss">
                                    Resin type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ResinType" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="labecss">
                                    Batch number
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="MasterBatchNumber" class="inputboxcss"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="MasterBatchNumberReg" runat="server" ErrorMessage="<b>Batch number contains invalid characters.</b><br><br>Please edit the batch number appropriately.<br>Batch may not contain:<pre>spaces !* ~`@#$^&{}[]|\\:\;'<>?,_<pre>"
                                        ControlToValidate="MasterBatchNumber" Display="None" ValidationExpression="^[0-9a-zA-Z\^-]+$"></asp:RegularExpressionValidator>
                                    <asp:ValidatorCalloutExtender ID="MasterBatchNumberRegExt" runat="server" TargetControlID="MasterBatchNumberReg"
                                        Width="320px">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="labecss">
                                    Silo
                                </td>
                                <td>
                                    <b>Phase1:</b>
                                    <asp:CheckBox ID="cbSilo1" Text="Silo1" runat="server" />
                                    <asp:CheckBox ID="cbSilo2" Text="Silo2" runat="server" />
                                    <asp:CheckBox ID="cbSilo3" Text="Silo3" runat="server" />
                                    <asp:CheckBox ID="cbSilo4" Text="Silo4" runat="server" />
                                    <asp:CheckBox ID="cbSilo5" Text="Silo5" runat="server" />
                                    <asp:CheckBox ID="cbSilo6" Text="Silo6" runat="server" />
                                    <b>Phase7:</b>
                                    <asp:CheckBox ID="cbPhase7" Text="Phase7" runat="server" />
                                    <%--<asp:CheckBox ID="cbSilo7" Text="Silo7" runat="server" />
                                    <asp:CheckBox ID="cbSilo8" Text="Silo8" runat="server" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="labecss">
                                    JJVC Part
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="JJVCPart" CssClass="inputboxcss" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="labecss">
                                    Expected Date
                                </td>
                                <td>
                                    <asp:TextBox runat="server" CssClass="inputboxcss" ID="MasterExpDate"> </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                               
                                    <input type="button" value="Submit" class="inputButtoncss" id="buttonSubmit" onclick="buttonSubmit_Click();" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td>
                    <fieldset>
                        <legend class="legendcss">Release Resin History Report</legend>
                        <table align="right">
                            <tr>
                                <td class="labecss">
                                    Start Date
                                </td>
                                <td>
                                    <asp:TextBox runat="server" CssClass="inputboxcss" ID="txtStartDate" onclick="WdatePicker({el:'MainContent_txtStartDate'})"> </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                        ValidationGroup="e1" ControlToValidate="txtStartDate" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="labecss">
                                    End Date
                                </td>
                                <td>
                                    <asp:TextBox runat="server" CssClass="inputboxcss" ID="txtEndDate" onclick="WdatePicker({el:'MainContent_txtEndDate'})"> </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                        ValidationGroup="e1" ControlToValidate="txtEndDate" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="Button1" Text="Export Data" CssClass="inputButtoncss" Name="idExpToExcel"
                                        runat="server" OnClick="buttonExport_Click" CausesValidation="true" ValidationGroup="e1" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td valign="baseline">
                    <img src="Images/alert1.png" style="height: 25px; width: 25px; cursor: hand;" onclick="retriveerror();" />
                    <input id="PermID" type="hidden" value="" name="PermID">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id="Progress" class="updateProgress">
                                <img src="Images/ajax-loader.gif" style="vertical-align: middle" />
                                Updating Records Please Wait...
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left">
                              <asp:Button ID="Buttonreferesh" runat="server" Text="Refresh" class="inputButtonRefereshcss"
                                    OnClick="GridReferesh" />
                            </td>
                            <td align="right">
                                <font class="labecss">Select Records per Page: </font>
                                <asp:DropDownList ID="dd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                                    CssClass="inputboxcss">
                                    <asp:ListItem Value="1" Text="Select"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5 Records"></asp:ListItem>
                                    <asp:ListItem Value="15" Text="15 Records"></asp:ListItem>
                                    <asp:ListItem Value="20" Text="20 Records"></asp:ListItem>
                                    <asp:ListItem Value="25" Text="25 Records"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    
                            <asp:DataGrid ID="datagrid" runat="server" AutoGenerateColumns=false AllowPaging ="true"  

                          Width="100%" AlternatingItemStyle-BackColor="Silver"
                          ItemStyle-CssClass ="gridlabecss" HeaderStyle-CssClass="gridheader" 
                          OnPageIndexChanged="Grid_PageIndexChanged"
                          onitemcreated="dgCustomer_ItemCreated"
                          oninit="dgCustomer_Init"
                          OnItemDataBound ="DataGrid_ItemDataBound"  HeaderStyle-Height="25px">

                      <PagerStyle Mode =NumericPages HorizontalAlign="Center"   Position =Bottom VerticalAlign =Middle  Height="25px" Font-Bold="true" CssClass="inputButtoncss" ForeColor="#FFFFFF" />
                           
                              
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Active">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelection" runat="server" />
                                            <asp:HiddenField ID="hiddenvalye" Value='<%# Eval("IS_ACTIVE")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Phase1">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSilo1" Text="Silo1" runat="server" />
                                            <asp:HiddenField ID="hdn_Silo1" Value='<%# Eval("SILO_1")%>' runat="server" />
                                            <asp:CheckBox ID="chkSilo2" Text="Silo2" runat="server" />
                                            <asp:HiddenField ID="hdn_Silo2" Value='<%# Eval("SILO_2")%>' runat="server" />
                                            <asp:CheckBox ID="chkSilo3" Text="Silo3" runat="server" />
                                            <asp:HiddenField ID="hdn_Silo3" Value='<%# Eval("SILO_3")%>' runat="server" />
                                            <asp:CheckBox ID="chkSilo4" Text="Silo4" runat="server" />
                                            <asp:HiddenField ID="hdn_Silo4" Value='<%# Eval("SILO_4")%>' runat="server" />
                                            <asp:CheckBox ID="chkSilo5" Text="Silo5" runat="server" />
                                            <asp:HiddenField ID="hdn_Silo5" Value='<%# Eval("SILO_5")%>' runat="server" />
                                            <asp:CheckBox ID="chkSilo6" Text="Silo6" runat="server" />
                                            <asp:HiddenField ID="hdn_Silo6" Value='<%# Eval("SILO_6")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Phase7">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkPhase7" Text="Phase7" runat="server" />
                                            <asp:HiddenField ID="hdn_Phase7" Value='<%# Eval("PHASE_NUM_7")%>' runat="server" />
                                            <%--<asp:CheckBox ID="chkSilo7" Text="Silo7" runat="server" />
                                            <asp:HiddenField ID="hdn_Silo7" Value='<%# Eval("SILO_7")%>' runat="server" />
                                            <asp:CheckBox ID="chkSilo8" Text="Silo8" runat="server" />
                                            <asp:HiddenField ID="hdn_Silo8" Value='<%# Eval("SILO_8")%>' runat="server" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="RAW_MAT_NUM" HeaderText="Material Number"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="BATCH" HeaderText="Batch Number"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="CREATE_DATE" HeaderText="Create Date"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="LAST_UPDT_DATE" HeaderText="Last Update"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="ESIG_USERID1" HeaderText="Released By"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="IS_ACTIVE" HeaderText="IS_ACTIVE" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="SILO_1" HeaderText="SILO_1" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="SILO_2" HeaderText="SILO_2" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="SILO_3" HeaderText="SILO_3" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="SILO_4" HeaderText="SILO_4" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="SILO_5" HeaderText="SILO_5" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="SILO_6" HeaderText="SILO_6" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="PHASE_NUM_7" HeaderText="PHASE_NUM_7" Visible="false"></asp:BoundColumn>
                                    <%--<asp:BoundColumn DataField="SILO_7" HeaderText="SILO_7" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="SILO_8" HeaderText="SILO_8" Visible="false"></asp:BoundColumn>--%>
                                </Columns>
                            </asp:DataGrid>
                          
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="padding-top: 30px; padding-bottom: 30px;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="updateResinBtn" runat="server" Text="Update Resin Table" class="inputButtoncss"
                                            OnClientClick="updateResin_Click()"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:HiddenField ID="HiddenField1" runat="server" EnableViewState="false" />
                                <asp:HiddenField ID="R_ESIG_USERID1" runat="server" />
                                <asp:HiddenField ID="R_ESIG_WWID1" runat="server" />
                            </td>
                            <td>  <%--<input type="button" value="xx" class="inputButtoncss" id="button2" onclick="resinVerifyUser()" />--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
       <div style="height:250px; width:1000px; overflow-x:scroll ; overflow-y: scroll; padding-bottom:10px;">      
            <rsweb:ReportViewer ID="ResinReportViewer" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                                Width="50%" BackColor="#FFFFCC">
              </rsweb:ReportViewer>
      </div>    
</asp:Content>
