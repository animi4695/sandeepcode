//
//	Busines rules
//		User enters a polypro and polystyrene Resin Type (up to 15 characters allowed)
//		User clicks the submit button:
//			An authorizing user must give their biometric approval
//				Combination is logged to ORACLE
//
var BatchNumber, JJVCPart, ResinType, phase_num;
var buttonSubmit, updateResinBtn, PermID, ExpectedDate, varHidden;

//
//	Called when page is first loaded.
//	Page initialization and support object created.
//

function window_onload() {

    //	Get convenient handles to all interface elements including:
    buttonSubmit = document.getElementById("buttonSubmit");
    updateResinBtn = document.getElementById("MainContent_updateResinBtn");
    BatchNumber = document.getElementById("MainContent_MasterBatchNumber");
    JJVCPart = document.getElementById("MainContent_JJVCPart");
    ResinType = document.getElementById("MainContent_ResinType");
    PermID = document.getElementById("PermID");
    ExpectedDate = document.getElementById("MainContent_MasterExpDate");
    //ExpectedDate.disabled = true;

    ResinType.onchange = ResinType_OnChange;
    ResinType_OnChange();

    // Give user some instructions
    //OKDialog("Ready to begin", "<b>Select a Resin Name,<br>Enter the new Batch number</b><br>And click '<b>Submit</b>'.", '600px', 'MainContent_MasterBatchNumber');
}

//	Called when the user hits the Print button or when the user hits the Enter key in an edit field.
//	Validate the Resin Type looking for some value that does not contain (bar code) unprintable characters.
//	If the Resin Type / jjvc part number are the same as the last run, immediately print.
//

function buttonSubmit_Click() {
    //debugger

    //	If the button is disabled, immediately exit.
    if (buttonSubmit.disabled) return;

    // is Page valid

    var pageValidated = Page_IsValid;

    //	Validate that there is a Resin Type to look up.
    //	Batch Number is a required field.
    if (BatchNumber.value.length === 0) {
        //	No Batch Number entered. Inform user and exit.
        OKDialog("Not Ready", "<b>Batch Number missing.</b><br><br>You must enter a Batch Number to continue.", '500px', 'MainContent_MasterBatchNumber');
        return false;
    }
    //    var phase_num_1 = $('#MainContent_cbPhase1').is(':checked');
    //    var phase_num_7 = $('#MainContent_cbPhase7').is(':checked');
    //    if (phase_num_1 === false && phase_num_7 === false) {
    //        //	No Phase Number entered. Inform user and exit.
    //        OKDialog("Not Ready", "<b><font color='red'>Phase Number missing.</b><br><br></font>You must enter a Phase Number to continue.", '500px', 'MainContent_MasterBatchNumber');

    //        return false;
    //    }

    var silo_1 = $('#MainContent_cbSilo1').is(':checked');
    var silo_2 = $('#MainContent_cbSilo2').is(':checked');
    var silo_3 = $('#MainContent_cbSilo3').is(':checked');
    var silo_4 = $('#MainContent_cbSilo4').is(':checked');
    var silo_5 = $('#MainContent_cbSilo5').is(':checked');
    var silo_6 = $('#MainContent_cbSilo6').is(':checked');
    var phase_7 = $('#MainContent_cbPhase7').is(':checked');

    var phase_num_7 = $('#MainContent_cbPhase7').is(':checked');

    //var silo_7 = $('#MainContent_cbSilo7').is(':checked');
    //var silo_8 = $('#MainContent_cbSilo8').is(':checked');
    if ((silo_1 === true || silo_2 === true || silo_3 === true || silo_4 === true || silo_5 === true || silo_6 === true) && (phase_7 === true)) {
        //	No Phase Number entered. Inform user and exit.
        OKDialog("Not Ready", "<b><font color='red'>One Phase Only.</b><br><br></font>You should not do two phases at one time.", '500px', 'MainContent_MasterBatchNumber');

        return false;
    }
    if (silo_1 === false && silo_2 === false && silo_3 === false && silo_4 === false && silo_5 === false && silo_6 === false) {
        //	No Phase Number entered. Inform user and exit.
        OKDialog("Not Ready", "<b><font color='red'>Silo Missing.</b><br><br></font>You must enter at least one silo.", '500px', 'MainContent_MasterBatchNumber');

        return false;
    }
    if (!pageValidated) {

        BatchNumber.focus();
        BatchNumber.select();
        return false;
    }

    // Batchs are not case sensitive. Insure all letters are upper case.
    BatchNumber.value = BatchNumber.value.toUpperCase();


    //	Disable the print button during validation and printing.
    buttonSubmit.disabled = true;
    buttonSubmit.className = 'inputboxcss';

    //	Call VerifyUser() and retreive the authorizing user's ID.
    if (VerifyUser("Release Resin", "You are authorizing the release of this batch of Resin.", PermID.value)) {

        var answer = confirm("PLEASE CHECK RESIN INFORMATION.\n Click Ok to Release the Resin.\n Otherwise click cancel and correct the information.")
        if (answer == false) {
            buttonSubmit.disabled = false;1
            buttonSubmit.className = 'inputButtoncss';
            return false;
        }
        $.ajax({
            type: "POST",
            url: "ReleaseResin.aspx/CreateBatch",
            //data: '{JJVCPart: "' + JJVCPart.value + '",BatchNumber: "' + BatchNumber.value + '" ,phase_num_1: "' + phase_num_1 + '",phase_num_7: "' + phase_num_7 + '", R_ESIG_WWID1: "' + esigWWID + '",R_ESIG_USERID1: "' + esigUSERID + '",R_EXPECTED_DATE: "' + ExpectedDate.value + '"    }',
            //data: '{JJVCPart: "' + JJVCPart.value + '",BatchNumber: "' + BatchNumber.value + '" ,SILO_1: "' + silo_1 + '",SILO_2: "' + silo_2 + '", SILO_3: "' + silo_3 + '",SILO_4: "' + silo_4 + '",SILO_5: "' + silo_5 + '",SILO_6: "' + silo_6 + '",SILO_7: "' + silo_7 + '",SILO_8: "' + silo_8 + '",R_ESIG_WWID1: "' + esigWWID + '",R_ESIG_USERID1: "' + esigUSERID + '",R_EXPECTED_DATE: "' + ExpectedDate.value + '"    }',
            data: '{JJVCPart: "' + JJVCPart.value + '",BatchNumber: "' + BatchNumber.value + '" ,SILO_1: "' + silo_1 + '",SILO_2: "' + silo_2 + '", SILO_3: "' + silo_3 + '",SILO_4: "' + silo_4 + '",SILO_5: "' + silo_5 + '",SILO_6: "' + silo_6 + '",PHASE_7: "' + phase_7 + '",R_ESIG_WWID1: "' + esigWWID + '",R_ESIG_USERID1: "' + esigUSERID + '",R_EXPECTED_DATE: "' + ExpectedDate.value + '"    }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessCreateBatch,
            failure: function (response) {
                alert(response.d);
            }
        });
        //$("#MainContent_Buttonreferesh").trigger('click');
        buttonSubmit.disabled = false;
        buttonSubmit.className = 'inputButtoncss';
    }
    else {
        // User cancelled the biometric authorization. Inform user and enable the Print button.
        OKDialog("Authorize", "<b>Authoization cancelled.</b><br><br>The batch has not been created.", '500px', '');
        buttonSubmit.disabled = false;
        buttonSubmit.className = 'inputButtoncss';
    }
}

function OnSuccessCreateBatch(response) {
    if (response.d[1] != null) {
        //	Communication error. Inform user.
        OKDialog("Communication Error", "Error communicating with server. Please try again.<br><br><b><font color='red'>" + response.d[1] + "</font></b>", '500px', '');
    }
    else {
        if (response.d[0] == "true") {
            OKDialog("Resin Released", "<b>Resin Release has been logged.</b><br><br>Thank you.", '500px', '');
        }
        else {
            //	Server did not log the batch combination, but also didn't throw an exception.
            //		However, code logic generally prevents this possibility. 
            //		This possibility is included for completeness and to handle future change.
            OKDialog("Not ready", "<b>Unknown Condition encountered.</b><br><br>Please try again. If this continues, please contact support.", '500px', '');
        }
    }
}

function ResinType_OnChange() {
    JJVCPart.value = ResinType.value;
}

//	Called when the user hits the Print button or when the user hits the Enter key in an edit field.
//	Validate the Resin Type looking for some value that does not contain (bar code) unprintable characters.
//	If the Resin Type / jjvc part number are the same as the last run, immediately print.
//	
function updateResin_Click() {
    //debugger
    var rowposi = document.getElementById('MainContent_datagrid').rows;    // set position of row

    var message = '';
    var mymessagetBoolean = new Boolean();
    mymessagetBoolean = false;
    var chkBoxvalue;
    var hiddenvalue;
    var chkBoxvalue1;
    var hdnsilo1;
    var chkBoxvalue2;
    var hdnsilo2;
    var chkBoxvalue3;
    var hdnsilo3;
    var chkBoxvalue4;
    var hdnsilo4;
    var chkBoxvalue5;
    var hdnsilo5;
    var chkBoxvalue6;
    var hdnsilo6;

    var chkBoxvalue7;
    var hdnphase7;

    //var chkBoxvalue7;
    //var hdnsilo7;
    //var chkBoxvalue8;
    //var hdnsilo8;


    for (var i = 1; i < document.getElementById('MainContent_datagrid').rows.length - 1; i++) {

        var chkindx = i - 1;

        chkBoxvalue = document.getElementById('MainContent_datagrid_chkSelection_' + chkindx).checked;
        hiddenvalue = document.getElementById('MainContent_datagrid_hiddenvalye_' + chkindx).value.toLowerCase();

        //chkBoxvalue1 = document.getElementById('MainContent_datagrid_chkPhase1_' + chkindx).checked;
        //hdnphase1 = document.getElementById('MainContent_datagrid_hdn_Phase1_' + chkindx).value.toLowerCase();

        //chkBoxvalue7 = document.getElementById('MainContent_datagrid_chkPhase7_' + chkindx).checked;
        //hdnphase7 = document.getElementById('MainContent_datagrid_hdn_Phase7_' + chkindx).value.toLowerCase();

        //        if ((chkBoxvalue === true) && ((chkBoxvalue1 === false) && (chkBoxvalue7 === false))) {
        //            //	No Phase Number entered. Inform user and exit.
        //            OKDialog("Resin Not Released", "<b><font color='red'>Phase Number missing.</b><br><br></font>You must enter a Phase Number to continue.", '500px', 'MainContent_MasterBatchNumber');

        //            return false;
        //        }

        chkBoxvalue7 = document.getElementById('MainContent_datagrid_chkPhase7_' + chkindx).checked;
        hdnphase7 = document.getElementById('MainContent_datagrid_hdn_Phase7_' + chkindx).value.toLowerCase();

        chkBoxvalue1 = document.getElementById('MainContent_datagrid_chkSilo1_' + chkindx).checked;
        hdnsilo1 = document.getElementById('MainContent_datagrid_hdn_Silo1_' + chkindx).value.toLowerCase();

        if (chkBoxvalue1.toString() != hdnsilo1.toString()) {
            mymessagetBoolean = true;
            message = message + "You have selected Silo1: <font color='red'>" + chkBoxvalue1 + "</font> for  Material Number:" + rowposi[i].childNodes[3].innerText + ", BatchNumber:  " + rowposi[i].childNodes[4].innerText + "</br>";

        }

        chkBoxvalue2 = document.getElementById('MainContent_datagrid_chkSilo2_' + chkindx).checked;
        hdnsilo2 = document.getElementById('MainContent_datagrid_hdn_Silo2_' + chkindx).value.toLowerCase();
        if (chkBoxvalue2.toString() != hdnsilo2.toString()) {
            mymessagetBoolean = true;
            message = message + "You have selected Silo2: <font color='red'>" + chkBoxvalue2 + "</font> for  Material Number:" + rowposi[i].childNodes[3].innerText + ", BatchNumber:  " + rowposi[i].childNodes[4].innerText + "</br>";

        }

        chkBoxvalue3 = document.getElementById('MainContent_datagrid_chkSilo3_' + chkindx).checked;
        hdnsilo3 = document.getElementById('MainContent_datagrid_hdn_Silo3_' + chkindx).value.toLowerCase();
        if (chkBoxvalue3.toString() != hdnsilo3.toString()) {
            mymessagetBoolean = true;
            message = message + "You have selected Silo3: <font color='red'>" + chkBoxvalue3 + "</font> for  Material Number:" + rowposi[i].childNodes[3].innerText + ", BatchNumber:  " + rowposi[i].childNodes[4].innerText + "</br>";

        }

        chkBoxvalue4 = document.getElementById('MainContent_datagrid_chkSilo4_' + chkindx).checked;
        hdnsilo4 = document.getElementById('MainContent_datagrid_hdn_Silo4_' + chkindx).value.toLowerCase();
        if (chkBoxvalue4.toString() != hdnsilo4.toString()) {
            mymessagetBoolean = true;
            message = message + "You have selected Silo4: <font color='red'>" + chkBoxvalue4 + "</font> for  Material Number:" + rowposi[i].childNodes[3].innerText + ", BatchNumber:  " + rowposi[i].childNodes[4].innerText + "</br>";

        }

        chkBoxvalue5 = document.getElementById('MainContent_datagrid_chkSilo5_' + chkindx).checked;
        hdnsilo5 = document.getElementById('MainContent_datagrid_hdn_Silo5_' + chkindx).value.toLowerCase();
        if (chkBoxvalue5.toString() != hdnsilo5.toString()) {
            mymessagetBoolean = true;
            message = message + "You have selected Silo5: <font color='red'>" + chkBoxvalue5 + "</font> for  Material Number:" + rowposi[i].childNodes[3].innerText + ", BatchNumber:  " + rowposi[i].childNodes[4].innerText + "</br>";

        }

        chkBoxvalue6 = document.getElementById('MainContent_datagrid_chkSilo6_' + chkindx).checked;
        hdnsilo6 = document.getElementById('MainContent_datagrid_hdn_Silo6_' + chkindx).value.toLowerCase();
        if (chkBoxvalue6.toString() != hdnsilo6.toString()) {
            mymessagetBoolean = true;
            message = message + "You have selected Silo6: <font color='red'>" + chkBoxvalue6 + "</font> for  Material Number:" + rowposi[i].childNodes[3].innerText + ", BatchNumber:  " + rowposi[i].childNodes[4].innerText + "</br>";

        }

        //chkBoxvalue7 = document.getElementById('MainContent_datagrid_chkSilo7_' + chkindx).checked;
        //hdnsilo7 = document.getElementById('MainContent_datagrid_hdn_Silo7_' + chkindx).value.toLowerCase();
        //if (chkBoxvalue7.toString() != hdnsilo7.toString()) {
        //    mymessagetBoolean = true;
        //    message = message + "You have selected Silo7: <font color='red'>" + chkBoxvalue7 + "</font> for  Material Number:" + rowposi[i].childNodes[3].innerText + ", BatchNumber:  " + rowposi[i].childNodes[4].innerText + "</br>";

        //}

        //chkBoxvalue8 = document.getElementById('MainContent_datagrid_chkSilo8_' + chkindx).checked;
        //hdnsilo8 = document.getElementById('MainContent_datagrid_hdn_Silo8_' + chkindx).value.toLowerCase();
        //if (chkBoxvalue8.toString() != hdnsilo8.toString()) {
        //    mymessagetBoolean = true;
        //    message = message + "You have selected Silo8: <font color='red'>" + chkBoxvalue8 + "</font> for  Material Number:" + rowposi[i].childNodes[3].innerText + ", BatchNumber:  " + rowposi[i].childNodes[4].innerText + "</br>";

        //}




        if (chkBoxvalue.toString() != hiddenvalue.toString()) {
            mymessagetBoolean = true;
            message = message + "You have selected Active: <font color='red'>" + chkBoxvalue + "</font> for  Material Number:" + rowposi[i].childNodes[3].innerText + ", BatchNumber:  " + rowposi[i].childNodes[4].innerText + "</br>";
        }

    }



    // end for loop
    // Confirmation Dilog

    // End Confirmation

    if (mymessagetBoolean) { message = message + " </br> " + "Please click <font color=red> Ok</font>  to continue"; }
    OKCancelDialog("Release Resin Confirmation", message, '800px', '');
}


function resinVerifyUser() {
    //debugger

    if (VerifyUser("Update Resin Status", "You are updating the status of the released Resins.", PermID.value)) {

        var chkBoxvalue;
        var hiddenvalue;
        var chkBoxvalue1;
        var hdnsilo1;
        var chkBoxvalue2;
        var hdnsilo2;
        var chkBoxvalue3;
        var hdnsilo3;
        var chkBoxvalue4;
        var hdnsilo4;
        var chkBoxvalue5;
        var hdnsilo5;
        var chkBoxvalue6;
        var hdnsilo6;

        var chkBoxvalue7;
        var hdnphase7;

        //var chkBoxvalue7;
        //var hdnsilo7;
        //var chkBoxvalue8;
        //var hdnsilo8;

        var rowposi = document.getElementById('MainContent_datagrid').rows;    // set position of row


        for (var i = 1; i < document.getElementById('MainContent_datagrid').rows.length - 1; i++) {

            var chkindx = i - 1;

            chkBoxvalue = document.getElementById('MainContent_datagrid_chkSelection_' + chkindx).checked;
            hiddenvalue = document.getElementById('MainContent_datagrid_hiddenvalye_' + chkindx).value.toLowerCase();


            chkBoxvalue1 = document.getElementById('MainContent_datagrid_chkSilo1_' + chkindx).checked;
            hdnsilo1 = document.getElementById('MainContent_datagrid_hdn_Silo1_' + chkindx).value.toLowerCase();

            chkBoxvalue2 = document.getElementById('MainContent_datagrid_chkSilo2_' + chkindx).checked;
            hdnsilo2 = document.getElementById('MainContent_datagrid_hdn_Silo2_' + chkindx).value.toLowerCase();

            chkBoxvalue3 = document.getElementById('MainContent_datagrid_chkSilo3_' + chkindx).checked;
            hdnsilo3 = document.getElementById('MainContent_datagrid_hdn_Silo3_' + chkindx).value.toLowerCase();

            chkBoxvalue4 = document.getElementById('MainContent_datagrid_chkSilo4_' + chkindx).checked;
            hdnsilo4 = document.getElementById('MainContent_datagrid_hdn_Silo4_' + chkindx).value.toLowerCase();

            chkBoxvalue5 = document.getElementById('MainContent_datagrid_chkSilo5_' + chkindx).checked;
            hdnsilo5 = document.getElementById('MainContent_datagrid_hdn_Silo5_' + chkindx).value.toLowerCase();

            chkBoxvalue6 = document.getElementById('MainContent_datagrid_chkSilo6_' + chkindx).checked;
            hdnsilo6 = document.getElementById('MainContent_datagrid_hdn_Silo6_' + chkindx).value.toLowerCase();

            chkBoxvalue7 = document.getElementById('MainContent_datagrid_chkPhase7_' + chkindx).checked;
            hdnphase7 = document.getElementById('MainContent_datagrid_hdn_Phase7_' + chkindx).value.toLowerCase();

            //chkBoxvalue7 = document.getElementById('MainContent_datagrid_chkSilo7_' + chkindx).checked;
            //hdnsilo7 = document.getElementById('MainContent_datagrid_hdn_Silo7_' + chkindx).value.toLowerCase();

            //chkBoxvalue8 = document.getElementById('MainContent_datagrid_chkSilo8_' + chkindx).checked;
            //hdnsilo8 = document.getElementById('MainContent_datagrid_hdn_Silo8_' + chkindx).value.toLowerCase();

            if ((chkBoxvalue.toString() != hiddenvalue.toString()) || (chkBoxvalue1.toString() != hdnsilo1.toString()) || (chkBoxvalue2.toString() != hdnsilo2.toString()) || (chkBoxvalue3.toString() != hdnsilo3.toString()) || (chkBoxvalue4.toString() != hdnsilo4.toString()) || (chkBoxvalue5.toString() != hdnsilo5.toString()) || (chkBoxvalue6.toString() != hdnsilo6.toString()) || (chkBoxvalue7.toString() != hdnsilo7.toString()) || (chkBoxvalue8.toString() != hdnsilo8.toString())) {
                $.ajax({
                    type: "POST",
                    url: "ReleaseResin.aspx/CreateBatchLoop",
                    data: '{JJVCPart: "' + rowposi[i].childNodes[3].innerText + '",BatchNumber: "' + rowposi[i].childNodes[4].innerText + '" ,R_ESIG_WWID1: "' + esigWWID + '",R_ESIG_USERID1: "' + esigUSERID + '",myCheckbox: "' + chkBoxvalue.toString().toUpperCase() + '", cbsilo1: "' + chkBoxvalue1.toString().toUpperCase() + '",cbsilo2: "' + chkBoxvalue2.toString().toUpperCase() + '",cbsilo3: "' + chkBoxvalue3.toString().toUpperCase() + '",cbsilo4: "' + chkBoxvalue4.toString().toUpperCase() + '",cbsilo5: "' + chkBoxvalue5.toString().toUpperCase() + '",cbsilo6: "' + chkBoxvalue6.toString().toUpperCase() + '",cbphase7: "' + chkBoxvalue7.toString().toUpperCase() + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccessCreateBatchloop,
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }
        }     // end for loop



        OKDialog('Resin Information Updated', '<b>Status available immediately.</b><br><br>Thank you.', '500px', '');

    }
    else {

        // User cancelled the biometric authorization. Inform user and enable the Print button.
        $("#MainContent_Buttonreferesh").trigger('click');
        OKDialogTest("Authorize", "<b>Authoization cancelled.</b><br><br>The batch has not been created.", '500px', '');
        updateResinBtn.disabled = false;
        updateResinBtn.className = 'inputButtoncss';

    }


}



function OnSuccessCreateBatchloop(response) {
    //debugger


    if (response.d[1] != null) {


        //	Communication error. Inform user.
        OKDialog("Communication Error", "Error communicating with server. Please try again.<br><br><b><font color='red'>" + response.d[1] + "</font></b>", '500px', '');

    }


    else {

        if (response.d[0] == "false") {


            //	Server did not log the batch combination, but also didn't throw an exception.
            //		However, code logic generally prevents this possibility. 
            //		This possibility is included for completeness and to handle future change.
            OKDialog("Not ready", "<b>Unknown Condition encountered.</b><br><br>Please try again. If this continues, please contact support.", '500px', '');

        }

    }

    return true;
}