﻿"use strict;"

$(function () {
    ApplyScript();
    SetNoRecord();
    PagingGrantSeniorCSCS();
    CallAutocomplete();
    DisableAllFormControls();
});

//**********************  Toggle and Load Fucntion***********************
function ApplyScript() {

    $('.frmHead').click(function () {
        var frmName = $(this).attr('data-frm');
        var h3Obj = $(this);
        $('.' + frmName).slideToggle("slow", function () {
            if ($(this).is(':visible')) {
                h3Obj.find('span').text('( - )');
            } else {
                h3Obj.find('span').text('( + )');
            }
        });



        //------------------For Top head click hide add or more region
        if (frmName == "frmPI") {

            if ($('.frmAddMorePIDetails').is(':visible')) {
                //  $('.newPI').text('Record New PI Details');
                $('[id*=PMorePi]').hide();
                $('.newPI').attr('type', '');
                $('.frmAddMorePIDetails').slideToggle("slow");
                $('.newPI').prev("span").text("+");

            }

            if ($('.frmNewPIDetails').is(':visible')) {
                $('.newPI').prev("span").text("+");

                ClearCloseMorePiSection();
                $('.frmNewPIDetails').slideToggle("slow");
                //  $('.newPI').text('Record New PI Details');
            }
        }
    });

    $('.newPI').click(function () {
        var frmName = $(this).attr('data-frm');
        $(".frmNewPIDetails input[type=text]").val("");
        if ($(this).attr('type') == "addmore") {
            $('.frmPI').show();
            $('[id*=PMorePi]').hide();
            //  $('.newPI').text('Record New PI Details');
            $('.newPI').prev("span").text("+");
            $('.frmAddMorePIDetails').slideToggle("slow").hide();
            $(this).attr('type', '');
            return;
        }

        if ($('.frmPI').is(':visible')) {
            $('.frmPI').slideToggle("slow");
        }

        $('.frmNewPIDetails').slideToggle("slow", "swing", function () {
            if ($('.frmNewPIDetails').is(':visible')) {

                $('.newPI').text('Cancel Recording New PI Details');
                $('.newPI').prev("span").text("-");
            } else {
                // $('.newPI').text('Record New PI Details');
                $('.newPI').prev("span").text("+");
                $('.frmPI').show();
            }
        });

    });

    ApplyEvents();

}

function ApplyEvents() {

    /*Grant Extended*/
    $("[id*=ddlGrantExtended]").change(function () {

        if (this.value == "1") {

            EnableExtendedFieds();
        }
        else {

            DisableExtendedFieds();
        }
    });

    if ($("[id*=ddlGrantExtended]")[0].value != 1) {

        DisableExtendedFieds();
    }
    else {
        EnableExtendedFieds();
    }
    /*End of Grant Extended*/

    /*PI Drop down*/



    /*End of PI Drop down*/

    /*To keep all the years hidden on page load*/
    $(".tblSingleBudget .tabFin").not(".tabFin:eq(0)").hide();
    $(".tblSingleBudget p").not("p:eq(0)").hide();

    /*Grant duration change event*/

    $("[id*=ddlDurationofGrant]").change(function () {

        $(".tblSingleBudget .tabFin").hide();
        $(".tblSingleBudget p").hide();

        var duration = $("[id*=ddlDurationofGrant] option:selected").text();
        var sixMonth = duration.indexOf(".") != -1 ? true: false;
        var No_tr_To_Show = Math.ceil(duration);

        for (var i = 1; i <= No_tr_To_Show; i++) {
            $(".tblSingleBudget .tabFin:eq("+i+")").show();
            $(".tblSingleBudget p:eq(" + i + ")").show();

            if (i == No_tr_To_Show && sixMonth) {
                $(".tblSingleBudget .tabFin:eq(" + i + ") .tblFinYearHead").text("6 Month");
            }
            else {
                $(".tblSingleBudget .tabFin:eq(" + i + ") .tblFinYearHead").text("Year "+i);
            }
        }

        //if (duration.indexOf(".") != -1) {
        //    $(".tblSingleBudget .tabFin[rid=trMonths]").show();
        //    $(".tblSingleBudget p.ptrMonths").show();
        //}
    });
    /*Grant duration change event*/

    GenearteBudgetTable();

}

function GenearteBudgetTable() {

    if ($('[id*=hdnYearlyBudget]').val() != "")
    {
        var yearArr = $('[id*=hdnYearlyBudget]').val().split("~");
        for (var i = 0; i < yearArr.length; i++) {

            var year = yearArr[i].split("|")[0];
            var allocated = yearArr[i].split("|")[1];
            var actual = yearArr[i].split("|")[2];


        }

    }
}

//Sub Functions - Functions used within the functions
function EnableExtendedFieds() {
    $(".extendedField").removeAttr("disabled");
    $(".extendedField").next("img").removeAttr("disabled").css("opacity", 1);
}

function DisableExtendedFieds() {
    $(".extendedField").attr("disabled", true);
    $(".extendedField").next("img").attr("disabled", true).css("opacity", 0.5);
}
//End Sub Functions



function AddMorePI() {
    $(".frmAddMorePIDetails input[type=text]").val("");
    $('.newPI').attr("type", 'addmore');
    var frmName = $(this).attr('data-frm');
    $('[id*=PMorePi]').show();
    if ($('.frmPI').is(':visible')) {
        $('.frmPI').slideToggle("slow");
    }
    $('.frmAddMorePIDetails').slideToggle("slow", "swing", function () {
        if ($('.frmAddMorePIDetails').is(':visible')) {

            $('.newPI').text('Cancel Adding  PI');
            $('.newPI').prev("span").text("-");
        } else {
            // $('.newPI').text('Record New PI Details');
            $('.newPI').prev("span").text("+");
        }
    });

}
//************************* END *******************************************

//******************* Main Grid Event*************************************
function SetNoRecord() {

    if ($("#tblResposive tbody tr").length == 0) {
        $('[id*=MainContent_SearchBox_txtSearch]').val('');
        $("#tblResposive tbody").html("<tr><td colspan='6' > No Records Available <td></tr>");
        $("#projectPaging").hide();
        $("#tblResposive thead th").css("background-image", "none");
        $("#tblResposive thead th").unbind("click");
    }

    if ($("#tblPiDetail tbody tr").length == 0) {

        $("#tblPiDetail tbody").html("<tr class='NoPI'><td colspan='5' > No Records Available <td></tr>");
        $("#tblPiDetail thead th").css("background-image", "none");
        $("#tblPiDetail thead th").unbind("click");
    }


}

function PagingGrantSeniorCSCS() {
    ApplyPaging('tblResposive', 'Paging', 10);
    $('#tblResposive').tablesorter();
    var pagingInterval = setInterval(function () {

        if ($(".header").length > 0) {
            ReApplyPaging('tblResposive');
            clearInterval(pagingInterval);
        }
    }, 100);

}

function ClearCloseMorePiSection() {
    $('[id*=PMorePi]').hide();
    $("[id*=TxtDepartment]").val(''); $("[id*=TxtDepartment]").focus();
    $("[id*=TxtPIName]").val(''); $('[id*=HdnDeptId]').val('');
    $("[id*=txtPIEmail]").val('');
    $("[id*=txtPiPhoneNo]").val('');
    $("[id*=txtPiMCRNo]").val('');
    return false;
}
//********************** END *********************************************

//****************** Auto Complete *******************************************
function CallAutocomplete() {

    var TxtDepartment = $('[id*=TxtDepartment]').attr('id');
    var HdnDeptId = $('[id*=HdnDeptId]').attr('id');
    var HdnDeptTxt = $('[id*=HdnDeptTxt]').attr('id');
    SearchText(TxtDepartment, HdnDeptId, 10, "Department~spAutoComplete", FillPi, HdnDeptTxt);
}
//********************** END **************************************************


//****************** Other Fucntiion*****************
function FillPi(result) {
    var TxtPIName = $('[id*=TxtPIName]').attr('id');
    var HdnpiId = $('[id*=HdnpiId]').attr('id');
    var HdnPITxt = $('[id*=HdnPITxt]').attr('id');
    var DeptId = result != null ? result.split('|')[1] : result;
    SearchText(TxtPIName, HdnpiId, 10, "FillPi~spAutoComplete~" + DeptId, fillPiDetails, HdnPITxt);
}

function fillPiDetails(result) {
    var ID = (result != null) ? result.split('|')[1] : result;
    var txtPIEmail = $('[id$=txtPIEmail]').attr('id');
    var txtPiPhoneNo = $('[id*=txtPiPhoneNo]').attr('id');
    var txtPiMCRNo = $('[id*=txtPiMCRNo]').attr('id');
    GetPI_MasterDetailsByID(ID, txtPIEmail, txtPiPhoneNo, txtPiMCRNo)
}

function GetPI_MasterDetailsByID(ID, txtPIEmail, txtPiPhoneNo, txtPiMCRNo) {

    txtPIEmail = document.getElementById(txtPIEmail);
    txtPiPhoneNo = document.getElementById(txtPiPhoneNo);
    txtPiMCRNo = document.getElementById(txtPiMCRNo);
    var ID = (ID != null) ? ID : 0;
    if (parseInt(ID) > 0) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            dataType: "json",
            timeout: 1000,
            url: "../PageMethods.aspx/GetPI_MasterDetailsByID",
            data: '{ "ID": "' + ID + '" }',
            contentType: "application/json;charset=utf-8;",
            success: function (r) {
                var customers = eval(r.d);
                if (customers.length > 0) {
                    txtPIEmail.value = customers[0].s_Email;
                    txtPiPhoneNo.value = customers[0].s_Phone_no;
                    txtPiMCRNo.value = customers[0].s_MCR_No;

                }

            },
            error: function (e) { MessageBox(e.statusText); }
        });
    }
    else {
        txtPIEmail.value = "";
        txtPiPhoneNo.value = "";
        txtPiMCRNo.value = "";
    }
}


function ClearOnblur(obj) {
    var HdnDeptTxt = $('[id*=HdnDeptTxt]').val();
    if (obj.value.trim() != "") {
        if (HdnDeptTxt != "") {
            if (obj.value.toLowerCase() != HdnDeptTxt.toLowerCase()) {
                MessageBox("Please select Proper Department");
                items('');
                obj.value = "";
                $("[id*=HdnDeptId]").val('');
                return false;

            }
        }
    }


}

function CheckPiOnBlur(obj) {
    if (obj.value == "") {
        items();
    }
}

function ClearPiDetails() {
    $('[id*=TxtDepartment]').bind("mouseup", function (e) {
        var $input = $(this),
            oldValue = $input.val();

        if (oldValue == "") return;
        setTimeout(function () {
            var newValue = $input.val();

            if (newValue == "") {
                items('');
                $input.trigger("cleared");
            }
        }, 1);
    });

    $('[id*=TxtPIName]').bind("mouseup", function (e) {
        var $input = $(this),
            oldValue = $input.val();
        if (oldValue == "") return;
        setTimeout(function () {
            var newValue = $input.val();

            if (newValue == "") {
                items('');
                $input.trigger("cleared");
            }
        }, 1);
    });
}

function items(pi) {

    var TxtPIName = $('[id*=TxtPIName]').val().trim();
    var HdnPITxt = $('[id*=HdnPITxt]').val().trim();
    if (TxtPIName != HdnPITxt) {
        $("[id*=txtPIEmail]").val('');
        $("[id*=txtPiPhoneNo]").val('');
        if (pi == "") {
            $("[id*=TxtPIName]").val('');
        }
        $("[id*=HdnpiId]").val('');
        $("[id*=txtPiMCRNo]").val('');
    }
    else if (TxtPIName == '') {
        $("[id*=txtPIEmail]").val('');
        $("[id*=txtPiPhoneNo]").val('');
        if (pi == "") {
            $("[id*=TxtPIName]").val('');
        }
        $("[id*=HdnpiId]").val('');
        $("[id*=txtPiMCRNo]").val('');
    }
}

function SaveMorePi() {
    var TxtDepartment = $('[id*=TxtDepartment]').attr('id');
    var TxtPIName = $('[id*=TxtPIName]').attr('id');
    var hdnPiID = $('[id*=HdnpiId]').attr('id');
    var txtPIEmail = $('[id$=txtPIEmail]').attr('id');
    var txtPiPhoneNo = $('[id*=txtPiPhoneNo]').attr('id');
    var txtPiMCRNo = $('[id*=txtPiMCRNo]').attr('id');


    TxtDepartment = document.getElementById(TxtDepartment);
    TxtPIName = document.getElementById(TxtPIName);
    hdnPiID = document.getElementById(hdnPiID);
    txtPIEmail = document.getElementById(txtPIEmail);
    txtPiPhoneNo = document.getElementById(txtPiPhoneNo);
    txtPiMCRNo = document.getElementById(txtPiMCRNo);
    var DeptId = $("[id*=HdnDeptId]");

    if (TxtDepartment.value.trim() == "") {
        MessageBox("Please select Department");
        TxtDepartment.focus();
        return false;
    }
    if (DeptId.val().trim() == "") {
        MessageBox("Please select Department Search Result");
        return false;
    }

    if (TxtPIName.value.trim() == "") {
        MessageBox("Please select PI");
        TxtPIName.focus();
        return false;
    }
    if (hdnPiID.value.trim() == "") {
        MessageBox("Please select PI Search Result");
        return false;
    }
    $("#tblPiDetail tbody tr td:contains('No Records Available')").each(function () {
        $("#tblPiDetail tbody tr").remove();
    });


    var flag = true;
    $("#tblPiDetail tbody tr").each(function () {

        if ($(this).attr("piid") == hdnPiID.value) {
            MessageBox("PI already added");
            flag = false;
        }
    });
    if (!flag) {
        return false;
    }


    var table = '<tr piId=' + hdnPiID.value + '><td><p>' + TxtDepartment.value + '</p></td><td><p>' + TxtPIName.value + '</p></td><td><p>' + txtPIEmail.value + '</p></td><td><p>' + txtPiPhoneNo.value + '</p></td><td><p>' + txtPiMCRNo.value + '</p></td>'
    table += '<td style="width: 45px; text-align: right"><p  class="grid-action"><a><img title="Delete Pi Detail" alt="" onclick=delPiRows(this) return false; src="../images/icon-delete.png"></a></p></td></tr>'
    $('#tblPiDetail  tbody').append(table);

    //To add the PI in Drop down
    if ($("[id*=ddlPIName]").attr("choosed") == "No") {
        $("[id*=ddlPIName]").append("<option value='" + hdnPiID.value + "'>" + TxtPIName.value + "</option>");
        $("[id*=ddlPIName]").removeAttr("disabled");
    }
    //End of To add the PI in Drop down


    MessageBox('Pi added Successfully..!!');
    $('.frmAddMorePIDetails').slideToggle("slow", "swing", function () {
        $('[id*=PMorePi]').hide();
        if ($('.frmAddMorePIDetails').is(':visible')) {
            $('.newPI').text('Cancel Addind  PI');

        } else {
            // $('.newPI').text('Record New PI Details');
            $('.newPI').prev("span").text("+");
            TxtDepartment.value = "";
            TxtPIName.value = "";
            txtPIEmail.value = "";
            txtPiPhoneNo.value = "";
            txtPiMCRNo.value = "";
            $('.frmPI').show();
        }
    });
    $('.newPI').attr('type', '');
    return false;
}
function delPiRows(Obj) {
    var mode = $("[id*=HdnMode]");
    if (mode.val().trim().toLowerCase() == "update") {
        if ($(Obj).parents("tbody").find("tr").length == 1) {
            MessageBox("There should be at least one PI  Required.");
            return false;
        }
    }
    if ($(Obj).parents("tbody").find("tr").length >= 1) {

        //var id = $(Obj).parent().parent().parent().parent().attr('piId');
        if ($(Obj).parent().parent().parent().parent().attr('piId') == undefined) {
            id = $(Obj).parent().parent().parent().attr('piId')
        }
        else {
            id = $(Obj).parent().parent().parent().parent().attr('piId')
        }

        return ConfirmBox('Are you sure to Delete this Record..??', "$('#tblPiDetail  tbody tr[piId =" + id + "]').remove();CallNoRecord();");

    }





    return false;
}

function CallNoRecord() {
    if ($("#tblPiDetail tbody tr").length == 0) {

        $("#tblPiDetail tbody").html("<tr><td colspan='6' > No Records Available <td></tr>");
        $("#tblPiDetail thead th").css("background-image", "none");
        $("#tblPiDetail thead th").unbind("click");
    }

}


function IsValidate() {



    var HdnMode = $('[id*=HdnMode]');
    var Mode = HdnMode.val().toLowerCase();
    if (Mode != 'delete' || Mode != 'view') {
        var ddlAwardOrg = $('[id*=ddlAwardOrg] option:selected');
        var TxtGrantNo = $('[id*=TxtGrantNo]');
        var ddlGrantName = $('[id*=ddlGrantName]');
        var TxtReaserchIO = $('[id*=TxtReaserchIO]');
        var ddlDurationofGrant = $('[id*=ddlDurationofGrant]');
        var TxtgrantExpDate = $('[id*=TxtgrantExpDate]');
        var TxtAwrdLetterDate = $('[id*=TxtAwrdLetterDate]');
        var TxtStartDate = $('[id*=TxtStartDate]');
        var HdnPi_ID = $('[id*=HdnPi_ID]');

        if (ddlAwardOrg.text().toLowerCase() == '--select--') {
            MessageBox('Please Select Awarding Organization'); ddlAwardOrg.focus(); return false;
        }
        if (TxtGrantNo.val().trim() == '') {
            MessageBox('Please Enter Grant Number'); TxtGrantNo.focus(); return false;
        }
        if (ddlGrantName.text().toLowerCase() == '--select--') {
            MessageBox('Please Enter Grant Name'); TxtGrantName.focus(); return false;
        }
        if (TxtReaserchIO.val().trim() == '') {
            MessageBox('Please Enter  Research IO'); TxtReaserchIO.focus(); return false;
        }
        if (ddlDurationofGrant.text().toLowerCase() == '--select--') {
            MessageBox('Please Enter  Duration of Grant'); TxtDurationofGrant.focus(); return false;
        }
        if (TxtgrantExpDate.val().trim() == '') {
            MessageBox('Please Select  Grant Expiry Date'); TxtgrantExpDate.focus(); return false;
        }
        if (TxtAwrdLetterDate.val().trim() == '') {
            MessageBox('Please Select Date of Award Letters'); TxtAwrdLetterDate.focus(); return false;
        }
        if (TxtStartDate.val().trim() == '') {
            MessageBox('Please Select Start Date'); TxtStartDate.focus(); return false;
        }
        if ($("#tblPiDetail tbody tr td").html().toLowerCase().trim().replace(/ +/g, "") == "norecordsavailable") {
            MessageBox("Enter Atleast One PI Detail"); return false;
        };
        
        CollectPIIDs();
        if (HdnPi_ID.val().trim() == "") {
            MessageBox("Please Enter Atleast One PI Detail ");
            return false;
        }
        
        CollectBudgetDetails();
    }

    $("[disabled=disabled]").removeAttr("disabled");

    
    return true;
}

function CollectBudgetDetails() {

    $("[id*=hdnSelectedPI]").val($("[id*=ddlPIName] option:selected").val());

    var budgetDetails = "";


    if ($('[id*=ddlAwardOrg] option:selected').text() == "NMRC" || $('[id*=ddlAwardOrg] option:selected').text() == "NKF") {

       // $('[id*=QuarterlyBudget]').val(budgetDetails);
    }
    else {

        var count = 0;
        var year = "";
        var allocated = "";
        var actual = "";

        //================
        $(".tabFin:visible").each(function (ind, itm) {

            year = $(itm).find(".tblFinYearHead").text();

            $(itm).find("table .tblFinCtrl").each(function () {


                var tempAllocated = $(this).find("td:eq(1)").find("input").val() == "" ? -1 : $(this).find("td:eq(1)").find("input").val();
                var tempActual = $(this).find("td:eq(2)").find("input").val() == "" ? -1 : $(this).find("td:eq(2)").find("input").val();

                allocated += allocated == "" ? tempAllocated : "," + tempAllocated;

                actual += actual == "" ? tempActual : "," + tempActual;

            });

            budgetDetails += budgetDetails == "" ? year + "|" + allocated + "|" + actual : "~" + year + "|" + allocated + "|" + actual;


        });

        //=============

        //$(".tabFin:visible").each(function (ind, itm) {

        //    count = ind;

        //    if ($(itm).hasClass("total"))
        //        return;

        //    if (count % 5 == 0 && count >= 5) {

        //        if (budgetDetails == "") {
        //            budgetDetails = year + "|" + allocated + "|" + actual;

        //            year = ""; allocated = ""; actual = "";

        //        } else {

        //            budgetDetails += "~" + year + "|" + allocated + "|" + actual;

        //            year = ""; allocated = ""; actual = "";

        //        }
        //    }

        //    if ($(itm).find("td[data-th='Year']").length > 0) {

        //        year = $(itm).find("td[data-th='Year'] span").text();
        //    }

        //    var allocatedValue = $(itm).find("[data-th='Estimated Budget'] input").val() == "" ? -1 : $(itm).find("[data-th='Estimated Budget'] input").val();

        //    var actualValue = $(itm).find("[data-th='Actual Spending'] input").val() == "" ? -1 : $(itm).find("[data-th='Actual Spending'] input").val();

        //    allocated += allocated == "" ? allocatedValue : "," + allocatedValue;

        //    actual += actual == "" ? actualValue : "," + actualValue;

        //});

        //if (budgetDetails == "") {
        //    budgetDetails = year + "|" + allocated + "|" + actual;

        //    year = ""; allocated = ""; actual = "";

        //} else {

        //    budgetDetails += "~" + year + "|" + allocated + "|" + actual;

        //    year = ""; allocated = ""; actual = "";

        //}

        $('[id*=hdnYearlyBudget]').val(budgetDetails);

    }
}

function ResetAll() {
    $('.frmGrantDetails input[type=text]').val("");
    $('.frmGrantDetails  select').val(-1);
    $('.frmAwardLetter input[type=text]').val("");
    SetNoRecord();
}

function DoPostBack() {
    return true;
}

function DisableAllFormControls() {
    var mode = $('[id*=HdnMode]').val().toLowerCase();
    if (mode == 'view') {
        $('body input,textarea,select,file,img,a').not('[id*=lnkback],[id*=btnCancel]').attr('disabled', true);
        $('.ui-datepicker-trigger').css("opacity", "0.5");
        $('[id*=DivTab] input').removeAttr('disabled').removeAttr('sel')
    }
    else if (mode == 'delete') {
        $('body input[type=text],textarea,select,file,img,a').not('[id*=lnkback]').attr('disabled', true);
        $('.ui-datepicker-trigger').css("opacity", "0.5");
        $('[id*=DivTab] input').removeAttr('disabled').removeAttr('sel')
    }

}

function CollectPIIDs() {
    var HdnPi_ID = document.getElementById($('[id*=HdnPi_ID]').attr('id'));
    HdnPi_ID.value = '';
    if ($("#tblPiDetail tbody tr td").html().toLowerCase().trim().replace(/ +/g, "") != "norecordsavailable") {
        $('#tblPiDetail  tbody tr').each(function (index, item) {
            HdnPi_ID.value += HdnPi_ID.value == "" ? $(item).attr('piId') : ',' + $(item).attr('piId');
        });
    }

}

function DownloadAwardFile(obj) {
    var file = $(obj).attr("filepath");
    var url = "/DownloadHandler.ashx";
    Callhandler(url, file);
}

function Callhandler(url, path) {
    var downloadpath = url + '?fileName=' + path;
    window.location = downloadpath;
    return false;
}
//******************* END******************************