<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="TTSHWeb.Reports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="css/jquery-ui.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.10.2.js"></script>
    <script src="Scripts/jquery-1.7.1.js"></script>
    <script src="Scripts/jquery-ui-1.8.20.min.js"></script>
    <script src="Scripts/jquery-ui-1.8.20.js"></script>--%>
    <script src="../Scripts/vendor/jquery-1.11.2.min.js"></script>
    <script src="../Scripts/jquery-ui.js"></script>
    <script src="../Scripts/jquery-migrate-1.2.1.min.js"></script>
    <script src="Scripts/Webform/jquery.blockUI.js"></script>
    <style type="text/css">
        .SSRScontainer {
            min-height: 600px;
            overflow: auto;
        }

       
        /*[id*=MainContent_ReportViewerSSRS]*/
        #ctl00_MainContent_ReportViewerSSRS_ctl09 {
            height: 450px !important;
        }

        #ParameterTable_ctl00_MainContent_ReportViewerSSRS_ctl04 {
            background-color: white !important;
        }

        #ctl00_MainContent_ReportViewerSSRS_ToggleParam {
            background-color: white !important;
        }

        #ctl00_MainContent_ReportViewerSSRS_ctl05_ctl03_ctl00 {
            height: 22px;
        }

        #ctl00_MainContent_ReportViewerSSRS_ctl05_ctl00_CurrentPage {
            height: 22px;
        }

 


          
    </style>

    <script type="text/javascript">
        var TxtStartDate = ''; var TxtEndDate = '';
        $(document).ready(function () {

            $('body input[type=submit]').addClass('action');



            $('body input[type=submit]').on('click', function () {
                if (!isValidate()) {
                    return false;
                }

                var span = $('.ReportViewerControl div[id$=_AsyncWait_Wait] span');
                span.text('Please Wait..');
                var img = $('.ReportViewerControl div[id$=_AsyncWait_Wait] img');
                img.attr('src', 'Images/loader-32.GIF')
            });
            $('.SSRScontainer').removeAttr('class');
            $('.SSRScontainer').css('overflow', 'auto');
            $('.SSRScontainer').css('max-height', '400px');
            $('body label span').removeAttr("style").css({
                'font-size': '12px',
                'width': '100px',
            });
            initDatePickers(); myfunction();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(applicationInitHandler);
            function applicationInitHandler() {

                initDatePickers();
                $('[id*=ParametersGrid] tbody tr td label').each(function (index, item) {

                    if ($(item).text().toLowerCase().indexOf("date") != -1) {
                        $('[id*=ParametersGrid] tbody tr td div>div').find('.ui-datepicker-trigger:eq(1)').remove()
                        $('[id*=ParametersGrid] tbody tr td div>div').find('.ui-datepicker-trigger:eq(1)').remove()


                        if ($('[id*=ParametersGrid] tbody tr td').find('img:eq(1)').length > 0) {
                            $('[id*=ParametersGrid] tbody tr td').find('img:eq(1)').hide();
                        }

                        if ($('[id*=ParametersGrid] tbody tr td').find('img:eq(1)').attr('disabled') != undefined) {
                            if ($('[id*=ParametersGrid] tbody tr td').find('img:eq(1)').attr('disabled').toLowerCase() == 'disabled') {
                                $('[id*=ParametersGrid] tbody tr td').find('img:eq(1)').hide();
                            }
                        }

                        if ($(item).text().toLowerCase().indexOf("from date") >= 0) {
                            $(item).parent().next().find('input[type=text]').css('width', '190px');

                        }
                        else if ($(item).text().toLowerCase().indexOf("to date") >= 0) {
                            $(item).parent().next().find('input[type=text]').css('width', '190px');

                        }
                        else if ($(item).text().toLowerCase().indexOf("column name") >= 0 || ($(item).text().toLowerCase().indexOf("select column to display") >= 0)
                        || ($(item).text().toLowerCase().indexOf("select status") >= 0) || ($(item).text().toLowerCase().indexOf("feasibility status") >= 0)
                        || ($(item).text().toLowerCase().indexOf("category") >= 0) || ($(item).text().toLowerCase().indexOf("department") >= 0)
                        || ($(item).text().toLowerCase().indexOf("type") >= 0) || ($(item).text().toLowerCase().indexOf("pi name") >= 0)) {
                            $(item).parent().next().find('input[type=text]').css('width', '220px');

                        }

                    }
                });

                $('body input[type=submit]').on('click', function () {
                    if (!isValidate()) {
                        return false;
                    }

                    var span = $('.ReportViewerControl div[id$=_AsyncWait_Wait] span');
                    span.text('Please Wait..');
                    var img = $('.ReportViewerControl div[id$=_AsyncWait_Wait] img');
                    img.attr('src', 'Images/loader-32.GIF')
                });
                $('body input[type=submit]').addClass('action');
                $('.SSRScontainer').removeAttr('class');
                $('.SSRScontainer').css('overflow', 'auto');
                $('.SSRScontainer').css('max-height', '400px');

                $('body label span').removeAttr("style").css({
                    'font-size': '12px',
                    'width': '100px',
                });

                var span = $('.ReportViewerControl div[id$=_AsyncWait_Wait] span');
                span.text('Please Wait..');
                var img = $('.ReportViewerControl div[id$=_AsyncWait_Wait] img');
                img.attr('src', 'Images/loader-32.GIF')


                $('body select').css('width', '220px');
               
            }
            function initDatePickers() {
                $('[id*=ParametersGrid] tbody tr td input[type=text]').each(function (index, item) {
                    $(item).removeClass('datepicker');

                });

                $('[id*=ParametersGrid] tbody tr td label').each(function (index, item) {

                    if ($(item).text().toLowerCase().indexOf("date") != -1) {


                        $(item).parent().next().find('input[type=text]').addClass('ctlinput ctlinput-sm datepicker')

                        $(item).parent().next().find('[id$=ddDropDownButton]').hide()
                        $('[id*=Parameters] tbody tr td input[type=submit]').addClass('action')




                        $(".datepicker").datepicker({
                            buttonText: datePickerTitle,
                            showOn: "both",
                            buttonImage: "../images/icon-cal.png",
                            buttonImageOnly: true,
                            changeMonth: true,
                            changeYear: true,
                            prevText: "",
                            nextText: "",
                            disabled: false,
                            dateFormat: 'dd-M-yy'


                        });


                    }
                    else {
                        $(item).parent().parent().parent().find('[id$=ddDropDownButton]').hide()
                        $(item).parent().parent().next().find('input[type=text]').addClass('ctlinput')
                        $(item).parent().parent().parent().find('select').addClass('ctlselect')


                    }
                    if ($(item).text().toLowerCase().indexOf("from date") >= 0) {
                        $(item).parent().next().find('input[type=text]').css('width', '190px');

                    }
                    else if ($(item).text().toLowerCase().indexOf("to date") >= 0) {
                        $(item).parent().next().find('input[type=text]').css('width', '190px');

                    }
                    else if ($(item).text().toLowerCase().indexOf("column name") >= 0 || ($(item).text().toLowerCase().indexOf("select column to display") >= 0)
                        || ($(item).text().toLowerCase().indexOf("select status") >= 0) || ($(item).text().toLowerCase().indexOf("feasibility status") >= 0)
                        || ($(item).text().toLowerCase().indexOf("category") >= 0) || ($(item).text().toLowerCase().indexOf("department") >= 0)
                        || ($(item).text().toLowerCase().indexOf("type") >= 0) || ($(item).text().toLowerCase().indexOf("pi name") >= 0)) {
                        $(item).parent().next().find('input[type=text]').css('width', '220px');

                    }
                });
                $('body select').css('width', '220px');
              
                myfunction();

            }


            var span = $('.ReportViewerControl div[id$=_AsyncWait_Wait] span');
            span.text('Please Wait..');
            var img = $('.ReportViewerControl div[id$=_AsyncWait_Wait] img');
            img.attr('src', 'Images/loader-32.GIF')
          

        });





        function myfunction() {
            $(".datepicker").keydown(function () {

                return false;
            });
            $(".datepicker").on("paste", function () { return false; });
            $(".datepicker").on("cut", function () { return false; });
            $(".datepicker").on("delete", function () { return false; });
        }

        function isValidate() {

            $('[id*=ParametersGrid] tbody tr td label').each(function (index, item) {

                if ($(item).text().toLowerCase().indexOf("date") != -1) {

                    if ($(item).text().toLowerCase().indexOf("from date") >= 0) {
                        TxtStartDate = $(item).parent().next().find('input[type=text]').val();

                    }
                    else if ($(item).text().toLowerCase().indexOf("to date") >= 0) {
                        TxtEndDate = $(item).parent().next().find('input[type=text]').val();

                    }
                }
            });



            if (TxtStartDate.trim() != "" && TxtEndDate.trim() != "") {

                TxtStartDate = ConvertDatetoMDYforRpt(TxtStartDate);
                TxtEndDate = ConvertDatetoMDYforRpt(TxtEndDate);
                var reslt = CompareDate(TxtStartDate, TxtEndDate);
                if (reslt == false) {
                    MessageBox("To Date should be Greater than or Equal to From Date ");

                    return false;
                }
            }

            return true;
        }

        function CompareDate(dat1, dat2) {
            var cfd = Date.parse(dat1);
            var ctd = Date.parse(dat2);

            var date1 = new Date(cfd);
            var date2 = new Date(ctd);

            if (Date.parse(date1) > Date.parse(date2)) {
                return false;
            }

            return true;
        }

        function ConvertDatetoMDYforRpt(inputdt) {
            var ndate = "";
            if (inputdt != null) {
                var d = inputdt.split('-')[0];
                var m = (inputdt.split('-')[1]);
                var y = inputdt.split('-')[2];

                if (/^[a-zA-Z]/.test(m)) {
                    m = RetMonthNo(m);
                }

                ndate = m + '/' + d + '/' + y;
            }
            return ndate;
        }
        document.onreadystatechange = function () {
            if (document.readyState == "interactive") {
                hideLoading();
                var span = $('.ReportViewerControl div[id$=_AsyncWait_Wait] span');
                span.text('Please Wait..');
                var img = $('.ReportViewerControl div[id$=_AsyncWait_Wait] img');
                img.attr('src', 'Images/loader-32.GIF')
            }
        }
            window.onload=function () {
                hideLoading();
                var span = $('.ReportViewerControl div[id$=_AsyncWait_Wait] span');
                span.text('Please Wait..');
                var img = $('.ReportViewerControl div[id$=_AsyncWait_Wait] img');
                img.attr('src', 'Images/loader-32.GIF')
            }
        $(document).ready(function () {
            hideLoading();
            var span = $('.ReportViewerControl div[id$=_AsyncWait_Wait] span');
            span.text('Please Wait..');
            var img = $('.ReportViewerControl div[id$=_AsyncWait_Wait] img');
            img.attr('src', 'Images/loader-32.GIF')
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SSRScontainer">
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
            <ContentTemplate>

                <rsweb:reportviewer id="ReportViewerSSRS" runat="server" class="ReportViewerControl" height="100%" width="100%" font-names="Cambria"
                    font-size="10pt" keepsessionalive="False" showcredentialprompts="False" showdocumentmapbutton="true"
                    showfindcontrols="false" showparameterprompts="true" showpromptareabutton="true" showrefreshbutton="true"
                    showtoolbar="true" showzoomcontrol="False" sizetoreportcontent="false" waitcontroldisplayafter="10">
                    <localreport>
                       <maptileserverconfiguration timeout="100000" />
                    </localreport>
                </rsweb:reportviewer>

            </ContentTemplate>


        </asp:UpdatePanel>
    </div>
</asp:Content>
