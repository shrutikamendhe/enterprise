<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="frmContractMgmt_ProjectUser.aspx.cs" Inherits="TTSHWeb.frmContractMgmt_ProjectUser" %>

<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

	<script src="../Scripts/WebForm/jsContract_Master.js"></script>
	<script type="text/javascript">
	    $(function () {
	        var mode = document.getElementById('<%=HdnMode.ClientID%>').value;
	        if (mode.toLowerCase() == 'new' || mode.toLowerCase() == 'insert') {
	            var c_Date = GetDate('-');
	            document.getElementById('<%=TxtContractReqDate.ClientID%>').value = c_Date;
	            document.getElementById('<%=TxtContAssignDate.ClientID%>').value = c_Date;
	        }
	        CallAutocomplete();
	        SearchText('<%=TxtMCollaborator.ClientID%>', '<%=HdnMCollaboratorId.ClientID%>', 10, "COLLABORATOR~spAutoComplete", getValue);
	        DisableMoreCollaboratorControls('<%=TxtMInstitution.ClientID%>', '<%=TxtMPhNo.ClientID%>', '<%=TxtMEmailAdd1.ClientID%>', '<%=TxtMEmailAdd2.ClientID%>', '<%=TxtMCountry.ClientID%>')
	        DisableProjectControls('<%=TxtProjTitle.ClientID%>', '<%=TxtAlias1.ClientID%>', '<%=TxtShortTitle.ClientID%>', '<%=TxtprojCategory.ClientID%>', '<%=TxtAlias2.ClientID%>', '<%=TxtIrbNo.ClientID%>')
	        ClearCollaborator();

	        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(CallAutocomplete)
	    });

	    function getValue(result) {
	        var ID = (result != null) ? result.split('|')[1] : result;
	        FillCollaboratorDetail(ID, '<%=TxtMEmailAdd1.ClientID %>', '<%=TxtMEmailAdd2.ClientID %>', '<%=TxtMInstitution.ClientID %>', '<%=TxtMPhNo.ClientID %>', '<%=TxtMCountry.ClientID %>')
        }

        function CallAutocomplete() {
            SearchText('<%=TxtNCountry.ClientID%>', '<%=HdnNCountryId.ClientID%>', 10, "Country~spAutoComplete");

        }
        function ValidateNewCollaborator() {
            var Ncollabrator = $get('<%=TxtNCollabator.ClientID%>');
            var NInstitution = $get('<%=TxtNInstitution.ClientID%>');
            var NEmail = $get('<%=TxtNEmail1.ClientID%>');
            if (Ncollabrator.value.trim() == "") {
                MessageBox("Please Enter Collaborator");
                Ncollabrator.focus();
                return false;
            }

            if (NEmail.value.trim() == "") {
                MessageBox("Please Enter Email");
                NEmail.focus();
                return false;
            }
            if (NInstitution.value.trim() == "") {
                MessageBox("Please Enter Institution");
                NInstitution.focus();
                return false;
            }
            return true;
        }
        function PerformCancel() {
            $('#<%=btnCancel.ClientID %>').click();
		}

		function CallNCollaboratorDetails() {
		    CallAutocomplete();
		    ClearCloseNewCollaboratorControls()

		}

		function ClearCollaborator() {
		    $('#<%=TxtMCollaborator.ClientID%>').bind("mouseup", function (e) {
		        var $input = $(this),
					oldValue = $input.val();

		        if (oldValue == "") return;

		        // When this event is fired after clicking on the clear button
		        // the value is not cleared yet. We have to wait for it.
		        setTimeout(function () {
		            var newValue = $input.val();

		            if (newValue == "") {
		                items();
		                $input.trigger("cleared");
		            }
		        }, 1);
		    });

		}
		function items() {

		    $("[id*=TxtMInstitution]").val('');
		    $("[id*=HdnMCollaboratorId]").val('');
		    $("[id*=TxtMPhNo]").val('');
		    $("[id*=TxtMEmailAdd1]").val('');
		    $("[id*=TxtMEmailAdd2]").val('');
		    $("[id*=TxtMCountry]").val('');
		}


	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<asp:HiddenField ID="HdnMode" Value="New" runat="server" />
	<asp:HiddenField ID="HdnId" Value="0" runat="server" />
	<asp:HiddenField ID="HdnProject_Id" Value="0" runat="server" />
	<asp:HiddenField ID="HdnContract_Collaborator_Details" runat="server" />
	<div class="projectGrid container" runat="server" id="DivMain">
		<div class="row">
			<div class="col-md-6 col-sm-6 paging">
				<h1>Contract Master View <span>Search, Filter and Edit Contract Master Details</span></h1>

			</div>
			<div class="col-md-6 col-sm-6 paging">
				<div class="grid-search">
					<uc1:SearchBox runat="server" ID="SearchBox" />
				</div>
			</div>
		</div>

		<div class="row">
			<div class="col-md-12">
				<div class="tblResposiveWrapper">
					<table id="tblResposive">
						<thead>
							<tr>
								<th style="width: 100px">Project ID</th>
								<th>Project Title</th>
								<th>Project Category</th>
															<th>DSRB/IRB No.</th>
								<th>PI Name</th>
								<th>Contract Application Status</th>
								<th style="width: 95px">Action</th>
							</tr>
						</thead>

						<tbody>

							<asp:Repeater ID="rptrContractDetail" OnItemCommand="rptrContractDetail_ItemCommand" runat="server">
								<ItemTemplate>
									<tr>
										<td data-th="Project ID">
											<p><%#Eval("s_Display_Project_ID") %></p>
										</td>
										<td data-th="Project Title">
											<p><%#Eval("s_Project_Title") %></p>
										</td>
										<td data-th="Project Category">
											<p><%#Eval("Project_Category_Name") %></p>
										</td>
									
										<td data-th="DSRB/IRB No.">

											<p><%#Eval("s_IRB_No") %></p>
										</td>
										<td data-th="PI Name">
											<p><%#Eval("PI_Name") %></p>
										</td>
										<td data-th="Contract Application Status">
											<p><%#Eval("ContAppStatus")%></p>
										</td>
										<td data-th="Action">
											<p class="grid-action">
												<asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# Eval("Status").ToString() == "New" %>'>
													<asp:LinkButton ID="LinkButton1" runat="server" CommandName="cmdAdd" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_Project_ID")%>'>
												
													<img title="Add Collaborator Details" alt="" style="width:20px;" src="Images/Add-New.png"></asp:LinkButton>
												</asp:PlaceHolder>

												<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# Eval("Status").ToString() == "Edit"  %>'>

													<asp:LinkButton ID="ImgEdit" runat="server" CommandName="cmdEdit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>'>
												
													<img title="Edit Collaborator Details" alt="" src="../images/icon-edit.png"></asp:LinkButton></asp:PlaceHolder>

												<asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# Eval("Status").ToString() == "Edit"  %>'>

													<asp:LinkButton ID="ImgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>'
                                                           OnClientClick='<%# String.Format("return ConfirmDelete(\"{0}\");",  Eval("i_ID")) %>'
                                                         CommandName="cmdDelete" runat="server">
                                                        <img title="Delete Collaborator Details" alt="" src="../images/icon-delete.png">
													</asp:LinkButton></asp:PlaceHolder>

												<asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# "Edit,View".Contains(Eval("Status").ToString()) %>'>
													<asp:LinkButton ID="ImgView" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>' CommandName="cmdView" runat="server">
                                                    
												<img title="View Collaborator Details" alt="" src="../images/icon-view.png">
													</asp:LinkButton></asp:PlaceHolder>
											</p>
										</td>
									</tr>
								</ItemTemplate>

							</asp:Repeater>

						</tbody>
					</table>

					<!-- Grid View -->



					<!-- Grid View -->
				</div>
			</div>
		</div>


		<div class="row" id="Paging">
			<div class="col-md-6 paging">
				<div class="page-info">
					<h3>18  Results Found</h3>
					<p>Showing Page 2 of Total 4 Pages | <a href="#">First Page</a> | <a href="#">Last Page</a></p>
				</div>
			</div>
			<div class="col-md-6 paging">
				<div class="pages">
				</div>
			</div>
		</div>
		<div class="row margin-top frmAction">
			<div class="col-md-12">
				<p style="text-align: left">



					<asp:Button CssClass="action" ID="btnNew" runat="server" Text="Add New Project" OnClientClick="window.open( 'frmProject_Master.aspx?NewPage=true','_blank' );return false;" />

				</p>
			</div>
		</div>
	</div>
	<div class="container ContractContainer" id="DivContractDetailContainer" runat="server">
		<span style="float: right; margin-top: 65px">
			<asp:LinkButton ID="lnkback" Text="Back to View" runat="server" OnClick="lnkback_Click"></asp:LinkButton></span>
		<div class="row">
			<div class="col-md-6 col-sm-6">
				<h1>Contract Details View<span>Contract Entry Form <b>( Project ID:<b id="DispProjectId" runat="server"> </b>)</b></span></h1>
			</div>

		</div>
		<div class="row">
			<div class="col-md-12">
				<h3 class="frmHead" data-frm="frmDetails">Project Details <span>( - )</span></h3>
			</div>
		</div>
		<div class="frmProject">

			<div class="frm frmDetails" style="display: block;">
				<div class="row">
					<div class="col-md-6 col-sm-6">

						<p>
							<label>Project Title <b>*</b></label>
							<asp:TextBox ID="TxtProjTitle" CssClass="ctltext"  TextMode="MultiLine" runat="server"></asp:TextBox>
						</p>

						<p>
							<label>Alias 1</label>
							<asp:TextBox ID="TxtAlias1" CssClass="ctlinput" runat="server"></asp:TextBox>
						</p>

						<p>
							<label>Short Title</label>
							<asp:TextBox ID="TxtShortTitle" CssClass="ctlinput" runat="server"></asp:TextBox>
						</p>

					</div>
					<div class="col-md-6 col-sm-6">
						<p>
							<label>Project Category <b>*</b></label>


							<asp:TextBox ID="TxtprojCategory" CssClass="ctlinput" runat="server"></asp:TextBox>
						</p>
						<p>
							<label>Alias 2</label>
							<asp:TextBox ID="TxtAlias2" CssClass="ctlinput" runat="server"></asp:TextBox>
						</p>
						<p>
							<label>DSRB/IRB No</label>
							<asp:TextBox ID="TxtIrbNo" CssClass="ctlinput" runat="server"></asp:TextBox>

						</p>
					</div>
				</div>


			</div>
		</div>
		<div class="row">
			<div class="col-md-12 col-sm-12">
				<h3 class="frmHead" data-frm="frmPIDetails">Principal Investigator (PI) Details<span>( - )</span>
				</h3>


			</div>
		</div>
		<div class="frm frmPIDetails" style="display: block;">
			<div class="row">
				<div class="col-md-12 col-sm-12">
					<div class="tblResposiveWrapper" style="overflow-x: hidden; overflow-y: auto; max-height: 250px; min-height: 20px;">
						<table id="tblPiDetail" class="tblResposive">
							<thead>
								<tr>
									<th style="width: 450px; text-align: left">Department</th>
									<th style="text-align: left">PI Name</th>
									<th style="text-align: left">Email</th>
									<th style="text-align: left">Phone</th>
									<th style="text-align: left">PI MCR No.</th>
								</tr>
							</thead>

							<tbody>
								<asp:Repeater ID="rptrPIDetails" runat="server">
									<ItemTemplate>
										<tr>
											<td data-th="Department">
												<p><%# Eval("s_DeptName")==DBNull.Value?Eval("DeptName"):Eval("s_DeptName") %></p>
											</td>
											<td data-th="PI Name">
												<p><%# Eval("s_PIName")==DBNull.Value?Eval("s_PI_Name"):Eval("s_PIName") %></p>
											</td>
											<td data-th="Email">
												<p><%# Eval("s_Email") %></p>
											</td>
											<td data-th="Phone">
												<p><%# Eval("s_Phone_no") %></p>
											</td>
											<td data-th="PI MCR No.">
												<p><%# Eval("s_MCR_No") %></p>
											</td>

										</tr>
									</ItemTemplate>
								</asp:Repeater>




							</tbody>
						</table>
					</div>

				</div>
			</div>
		</div>


		<div class="row">
			<div class="col-md-12 col-sm-12">
				<h3 class="frmHead" data-frm="frmPI">Collaborator Detail <span>( - )</span>
				</h3>
				<p  runat="server" id="PNewCollaborator"><span>+</span> <a class="newPI link" data-frm="frmNewPIDetails">Record New Collaborator Details</a></p>

			</div>
		</div>

		<div class="frm frmPI" style="display: block;">
			<div class="row">
				<div class="col-md-12 col-sm-12">
                    <div style="overflow: hidden;" id="DivHeaderRow">
                     </div>
					
                        <div class="tblResposiveWrapper"  style="overflow-x: hidden; overflow-y: auto; max-height: 250px; min-height: 20px;">
						<table id="tblCollaboratorDetail" class="tblResposive">
							<thead>
								<tr>

									<th style="width: 200px">Collaborator</th>
									<th>Email Address1</th>
									<th >Phone No</th>
																
									<th style="width: 150px">Institution</th>
									<th>Country</th>
									<th >Contract Request Date</th>
									<th style="width: 45px; text-align: right">Action</th>
								</tr>
							</thead>

							<tbody>
								<asp:Repeater ID="RptrCollaborator" runat="server">
									<ItemTemplate>
										<tr ColaborId=<%# Eval("i_ID") %> ^ inicontractid=<%# Eval("s_initialId") %> ^ contractreqdate=<%# Eval("s_date") %>>
											
											<td data-th="Collaborator">
												<p><%# Eval("s_Name") %></p>
											</td>
											<td data-th="Email Address1">
												<p><%# Eval("s_Email1") %></p>
											</td>
											<td data-th="Phone No">
												<p><%# Eval("s_PhoNo") %></p>
											</td>
											<td data-th="Institution">
												<p><%# Eval("s_Institution") %></p>
											</td>
											
											<td data-th="Country">
												<p><%# Eval("Country_Name") %></p>
											</td>
											
										
											<td data-th="Contract Request Date">
												<p><%# Eval("s_date") %></p>

											</td>
											<td data-th="Action">
												<p id='<%# Eval("I_Collaborator_Id") %>'  class="grid-action">
												
													  <a>
                                                        <img title="Delete Collaborator Details" alt="Delete" onclick="return delCollaboratorRows(this);" src="../images/icon-delete.png"></a>
                                                    
                                                
												</p>
											</td>
										</tr>
									</ItemTemplate>
								</asp:Repeater>




							</tbody>
						</table>
                            </div>
					
                   
					<p runat="server" id="PMoreCollaborator" class="align-right"><a class="link" onclick="AddMorePI();"><span>+</span>Add More Collaborator</a></p>
				</div>
			</div>
		</div>

		<div class="frmNewPIDetails" style="display: none;">
			<div class="row">
				<div class="col-md-12 col-sm-12">
					<h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">Record New Collaborator Details					                  
                </h3>
				</div>
			</div>
			<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpPi">
				<ContentTemplate>
					<div class="row">
						<div class="col-md-6 col-sm-6">
                            	<p>
								<label>Institution <b>*</b></label>
								<asp:TextBox ID="TxtNInstitution" CssClass="ctlinput" runat="server"></asp:TextBox>
							</p>

							<p>
								<label>Email Address1 <b>*</b></label>
								<asp:TextBox ID="TxtNEmail1" CssClass="ctlinput" placeHolder="Enter Email Address1" onblur="checkValidEmail(this);" runat="server"></asp:TextBox>
							</p>
                              <p>
								<label>Phone No </label>
								<asp:TextBox ID="TxtNPhNo" CssClass="ctlinput" onKeypress="return SingaporePhformat();" onpaste="return false"  runat="server"></asp:TextBox>
							</p>
						
						</div>
						<div class="col-md-6 col-sm-6">
                            
							<p>
								<label>Collaborator Name<b>*</b></label>

								<asp:TextBox ID="TxtNCollabator" CssClass="ctlinput" runat="server"></asp:TextBox>
							</p>
                            <p>
								<label>Email Address2 </label>
								<asp:TextBox ID="TxtNEmail2" CssClass="ctlinput" placeHolder="Enter Email Address2" onblur="checkValidEmail(this);" runat="server"></asp:TextBox>
							</p>
                          
							<p>
								<label>Country </label>
								<asp:HiddenField ID="HdnNCountryId" runat="server" />
								<asp:TextBox ID="TxtNCountry" CssClass="ctlinput" onkeypress="return CheckChar(event);" runat="server"></asp:TextBox>
							</p>
							
							
						</div>

					</div>
					<div class="row margin-top frmAction">
						<div class="col-md-12">
							<p style="text-align: right">


								<asp:Button CssClass="action" ID="btnNewCollaboratorSave" OnClick="btnNewCollaboratorSave_Click" OnClientClick="return ValidateNewCollaborator();" runat="server" Text="Save" />
								<asp:Button CssClass="action" ID="btnNewCollaboratorCancel" runat="server" Text="Reset" />

							</p>
						</div>
					</div>
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>

		<div class="frmAddMorePIDetails" style="display: none;">
			<div class="row">
				<div class="col-md-12 col-sm-12">
					<h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">Add More Collaborator 
                </h3>
				</div>
			</div>

			<div class="row">
				<div class="col-md-6 col-sm-6">
					<p>
						<label>Collaborator Name<b>*</b></label>
						<asp:HiddenField ID="HdnMCollaboratorId" runat="server" />
						<asp:TextBox ID="TxtMCollaborator" CssClass="ctlinput" placeholder="Type Keyword to search Collaborator" onkeyup="if(this.value==''){items();}" onKeydown="items();" runat="server"></asp:TextBox>
					</p>
					
                    <p>
						<label>Email Address1</label>

						<asp:TextBox ID="TxtMEmailAdd1" CssClass="ctlinput" runat="server"></asp:TextBox>

					</p>
					<p>
						<label>Phone No</label>
						<asp:TextBox ID="TxtMPhNo" onKeypress="return SingaporePhformat();" onpaste="return false" CssClass="ctlinput" runat="server"></asp:TextBox>
					</p>
					<p>
						<label>Collaborator Initial Contract Id</label>
						<asp:TextBox ID="TxtContractId" CssClass="ctlinput" runat="server"></asp:TextBox>
					</p>
					

				</div>
				<div class="col-md-6 col-sm-6">
                    <p>
						<label>Institution </label>
						<asp:TextBox ID="TxtMInstitution" CssClass="ctlinput" runat="server"></asp:TextBox>
					</p>
                    <p>
						<label>Email Address2</label>

						<asp:TextBox ID="TxtMEmailAdd2" CssClass="ctlinput" runat="server"></asp:TextBox>

					</p>
					
                    <p>
						<label>Country</label>

						<asp:TextBox ID="TxtMCountry" placeholder="Type Keyword to search Country" CssClass="ctlinput" runat="server"></asp:TextBox>

					</p>
					
					<p>
						<label>Contract Request Date<b>*</b></label>

						<asp:TextBox ID="TxtContractReqDate" CssClass="ctlinput ctlinput-sm datepicker Req" onpaste="return false;" onkeypress="return false;" runat="server"></asp:TextBox>

					</p>
				</div>
			</div>
			<div class="row margin-top frmAction">
				<div class="col-md-12">
					<p style="text-align: right">


						<asp:Button CssClass="action" ID="btnMCollaboratrSave" runat="server" Text="Save" />
						<asp:Button CssClass="action" ID="btnMCollaboratrCancel" runat="server" Text="Reset" />

					</p>
				</div>
			</div>
		</div>


		<div class="row">
			<div class="col-md-12">
				<h3 class="frmHead" data-frm="frmother">Other Detail <span>( - )</span></h3>
			</div>
		</div>
		<div class="frm frmother" style="display: block;">
			<div class="row">
				<div class="col-md-6 col-sm-6">
					<p>
						<label>Contract Assign Date<b>*</b></label>
						<asp:TextBox ID="TxtContAssignDate" CssClass="ctlinput ctlinput-sm datepicker" onpaste="return false;" onkeypress="return false;" runat="server"></asp:TextBox>
					</p>


				</div>
				<div class="col-md-6 col-sm-6">
					<p>
						<label>Reviewed By <b>*</b></label>


						<asp:DropDownList ID="DdlReviewedBy" CssClass="ctlselect" runat="server"></asp:DropDownList>
					</p>


				</div>
			</div>
		</div>




		<div class="row margin-top frmAction">
			<div class="col-md-12">
				<p style="text-align: right">


					<asp:Button CssClass="action" ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save Details" />
					<asp:Button CssClass="action" ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
                    <asp:Button CssClass="action" ID="delete" runat="server" style="display:none" OnClick="delete_Click"  />
                    
				</p>
			</div>
		</div>
	</div>
	<asp:HiddenField ID="HdnContractReqDate" runat="server" />
</asp:Content>
