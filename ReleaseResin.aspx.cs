using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using jnj.visus.cim.OracleConnection;
using jnj.visus.cim.RMT.NextGenWeb.Helper;
using System.Configuration;
using log4net;
using System.Web.Services;
using jnj.visus.cim.RMT.NextGenWeb.Data;
using System.Text.RegularExpressions;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.IO;
using System.Xml;
using DeviceWISE;
using DeviceWise;
using jnj.visus.cim.InTouch;
using System.Globalization;
using Microsoft.Reporting.WebForms;

namespace jnj.visus.cim.RMT.NextGenWeb
{
    public partial class ReleaseResin : System.Web.UI.Page
    {
        private ILog _Log;
        protected Connection gridOc;
        GetDbConnectionHelper GetDbConnection;

        protected System.Web.UI.WebControls.Button updateResinBtn;

        private string RMTCOnnectionstring = ConfigurationManager.AppSettings["RMTConfig"];

        #region Custom Paging

        LinkButton lbFirstPage = null;

        LinkButton lbPreviousPage = null;

        LinkButton lbNextPage = null;


        LinkButton lbLastPage = null;
        Button Buttonreferesh = null;


        protected void dgCustomer_Init(object sender, EventArgs e)
        {
            lbFirstPage = new LinkButton();
            lbFirstPage.ID = "lbFirstPage";
            lbFirstPage.Text = "First";
            lbFirstPage.ForeColor = System.Drawing.Color.White;
            lbFirstPage.Click += new EventHandler(lbFirstPage_Click);

            lbPreviousPage = new LinkButton();
            lbPreviousPage.ID = "lbPreviousPage";
            lbPreviousPage.Text = "Previous";
            lbPreviousPage.ForeColor = System.Drawing.Color.White;
            lbPreviousPage.Click += new EventHandler(lbPreviousPage_Click);

            lbNextPage = new LinkButton();
            lbNextPage.ID = "lbNextPage";
            lbNextPage.Text = "Next";
            lbNextPage.ForeColor = System.Drawing.Color.White;
            lbNextPage.Click += new EventHandler(lbNextPage_Click);

            lbLastPage = new LinkButton();
            lbLastPage.ID = "lbLastPage";
            lbLastPage.Text = "Last";
            lbLastPage.ForeColor = System.Drawing.Color.White;
            lbLastPage.Click += new EventHandler(lbLastPage_Click);
        }


        void lbLastPage_Click(object sender, EventArgs e)
        {
            datagrid.CurrentPageIndex = datagrid.PageCount - 1;
            PopulateReleaseGrid();
        }

        void lbNextPage_Click(object sender, EventArgs e)
        {
            if (datagrid.CurrentPageIndex < (datagrid.PageCount - 1))
            {
                datagrid.CurrentPageIndex++;
                PopulateReleaseGrid();
            }
        }

        void lbPreviousPage_Click(object sender, EventArgs e)
        {
            if (datagrid.CurrentPageIndex > 0)
            {
                datagrid.CurrentPageIndex--;
                PopulateReleaseGrid();
            }
        }

        void lbFirstPage_Click(object sender, EventArgs e)
        {
            datagrid.CurrentPageIndex = 0;
            PopulateReleaseGrid();
        }

        protected void dgCustomer_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Pager)
            {
                if (datagrid.CurrentPageIndex > 0)
                {
                    if (e.Item.Cells[0].FindControl("lbPreviousPage") == null)
                    {
                        e.Item.Cells[0].Controls.AddAt(0, new LiteralControl("&nbsp;&nbsp;"));
                        e.Item.Cells[0].Controls.AddAt(0, lbPreviousPage);
                    }
                }

                if (datagrid.PageCount > 0)
                {
                    if (e.Item.Cells[0].FindControl("lbFirstPage") == null)
                    {
                        e.Item.Cells[0].Controls.AddAt(0, new LiteralControl("&nbsp;&nbsp;"));
                        e.Item.Cells[0].Controls.AddAt(0, lbFirstPage);
                    }
                }

                if (datagrid.CurrentPageIndex < (datagrid.PageCount - 1))
                {
                    if (e.Item.Cells[0].FindControl("lbNextPage") == null)
                    {
                        e.Item.Cells[0].Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                        e.Item.Cells[0].Controls.Add(lbNextPage);
                    }
                }


                if (datagrid.PageCount > 0)
                {
                    if (e.Item.Cells[0].FindControl("lbLastPage") == null)
                    {
                        e.Item.Cells[0].Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                        e.Item.Cells[0].Controls.Add(lbLastPage);
                    }
                }
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(dd.SelectedValue) > 1)
            {
                datagrid.PageSize = int.Parse(dd.SelectedValue);
                PopulateReleaseGrid();
            }
        }

        protected void GridReferesh(object sender, EventArgs e)
        {
            datagrid.PageSize = 10;

            if (int.Parse(dd.SelectedValue) > 1)
            {
                datagrid.PageSize = int.Parse(dd.SelectedValue);
            }

            PopulateReleaseGrid();
        }

        #endregion

        public void DataGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            
                if ((e.Item.ItemType == ListItemType.AlternatingItem) ||
                             (e.Item.ItemType == ListItemType.Item))
                {
                    string cellValue = e.Item.Cells[8].Text.Trim();//is active

                    string silo1value = e.Item.Cells[9].Text.Trim();
                    string silo2value = e.Item.Cells[10].Text.Trim();
                    string silo3value = e.Item.Cells[11].Text.Trim();
                    string silo4value = e.Item.Cells[12].Text.Trim();
                    string silo5value = e.Item.Cells[13].Text.Trim();
                    string silo6value = e.Item.Cells[14].Text.Trim();
                    //string silo7value = e.Item.Cells[15].Text.Trim();
                    //string silo8value = e.Item.Cells[16].Text.Trim();

                    CheckBox myCheckbox = (CheckBox)e.Item.Cells[0].FindControl("chkSelection");
                    myCheckbox.Checked = false;

                    CheckBox cbSilo1 = (CheckBox)e.Item.Cells[0].FindControl("chkSilo1");
                    CheckBox cbSilo2 = (CheckBox)e.Item.Cells[0].FindControl("chkSilo2");
                    CheckBox cbSilo3 = (CheckBox)e.Item.Cells[0].FindControl("chkSilo3");
                    CheckBox cbSilo4 = (CheckBox)e.Item.Cells[0].FindControl("chkSilo4");
                    CheckBox cbSilo5 = (CheckBox)e.Item.Cells[0].FindControl("chkSilo5");
                    CheckBox cbSilo6 = (CheckBox)e.Item.Cells[0].FindControl("chkSilo6");
                    //CheckBox cbSilo7 = (CheckBox)e.Item.Cells[0].FindControl("chkSilo7");
                    //CheckBox cbSilo8 = (CheckBox)e.Item.Cells[0].FindControl("chkSilo8");
                    cbSilo1.Checked = false;
                    cbSilo2.Checked = false;
                    cbSilo3.Checked = false;
                    cbSilo4.Checked = false;
                    cbSilo5.Checked = false;
                    cbSilo6.Checked = false;
                    //cbSilo7.Checked = false;
                    //cbSilo8.Checked = false;
                    if (silo1value == "True")
                    {
                        cbSilo1.Checked = true;
                    }
                    if (silo2value == "True")
                    {
                        cbSilo2.Checked = true;
                    }
                    if (silo3value == "True")
                    {
                        cbSilo3.Checked = true;
                    }
                    if (silo4value == "True")
                    {
                        cbSilo4.Checked = true;
                    }
                    if (silo5value == "True")
                    {
                        cbSilo5.Checked = true;
                    }
                    if (silo6value == "True")
                    {
                        cbSilo6.Checked = true;
                    }
                    //if (silo7value == "True")
                    //{
                    //    cbSilo7.Checked = true;
                    //}
                    //if (silo8value == "True")
                    //{
                    //    cbSilo8.Checked = true;
                    //}
                    if (cellValue == "True")
                    {
                        myCheckbox.Checked = true;
                    }
                }
             
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _Log = LogManager.GetLogger("web app");

            string ctrlname = Page.Request.Params.Get("__EVENTTARGET");

            if (Page.IsPostBack)
            {
                if (HiddenField1.Value == "MainContent_updateResinBtn" && ctrlname == "")
                {
                    updateResinBtn_Click();
                }
            }

            if (!Page.IsPostBack)
            {
                MasterExpDate.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                PopulateDDList();
                PopulateReleaseGrid();
            }
        }

        protected void Grid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            datagrid.CurrentPageIndex = e.NewPageIndex;
            PopulateReleaseGrid();
        }

        public void PopulateDDList()
        {
            GetDbConnection = new GetDbConnectionHelper(Server.MapPath(ConfigurationManager.AppSettings["RMTConfig"]));

            DataSet ds = GetDbConnection.PopulateList(ConfigurationManager.AppSettings["ResinTypeQuery"].ToString());
            try
            {
                foreach (DataRow CurrRow in ds.Tables[0].Rows)
                {
                    ResinType.Items.Add(new ListItem(Convert.ToString(CurrRow[0]), Convert.ToString(CurrRow[1])));
                }
            }
            catch (System.Exception ex)
            {
                _Log.Error(ex.Message, ex);
            }
        }
        
        private void PopulateReleaseGrid()
        {
            DataSet ds = new DataSet();
            GetDbConnection = new GetDbConnectionHelper(Server.MapPath(ConfigurationManager.AppSettings["RMTConfig"]));
            string SQL = "select IS_ACTIVE, RAW_MAT_NUM, BATCH,CREATE_DATE, LAST_UPDT_DATE,ESIG_USERID1,SILO_1,SILO_2,SILO_3,SILO_4,SILO_5,SILO_6 from resin_release where LAST_UPDT_DATE > (SYSDATE-90) or IS_ACTIVE = 'True' order by  raw_mat_num,CREATE_DATE DESC";

            try
            {
                gridOc = GetDbConnection.GetDbConnection();
                ds = gridOc.ProcessDataset(SQL);

                if (int.Parse(dd.SelectedValue) > 1)
                {
                    datagrid.PageSize = int.Parse(dd.SelectedValue);
                }

                datagrid.DataSource = ds;
                datagrid.DataBind();
            }
            catch (System.Exception ex)
            {
                _Log.Error(ex.Message, ex);
            }
            finally
            {
                if ((gridOc != null))
                {
                    gridOc.Dispose();
                }
            }
        }

        protected void buttonExport_Click(object sender, EventArgs e)
        {
            //datagrid.Visible = false;
            //ResinReportViewer.Visible = true;
            ResinReportViewer.LocalReport.ReportPath = "RDLC/ResinReport.rdlc";
            DataSet ds = new DataSet();
            GetDbConnection = new GetDbConnectionHelper(Server.MapPath(ConfigurationManager.AppSettings["RMTConfig"]));
            string SQL = "select RAW_MAT_NUM,BATCH,Esig_WWID1,ESIG_USERID1, Action, CREATE_DATE Create_Date, LAST_UPDT_DATE ,SILO_1,SILO_2,SILO_3,SILO_4,SILO_5,SILO_6,PHASE_NUM_1,PHASE_NUM_7 from resin_release_hist where LAST_UPDT_DATE >= to_date('" + txtStartDate.Text + "','yyyy-mm-dd') and LAST_UPDT_DATE <= to_date('" + txtEndDate.Text + "','yyyy-mm-dd') order by CREATE_DATE DESC,raw_mat_num";
            try
            {
                gridOc = GetDbConnection.GetDbConnection();
                ds = gridOc.ProcessDataset(SQL);  

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    ResinReportViewer.SizeToReportContent = true;
                    ResinReportViewer.ProcessingMode = ProcessingMode.Local;
                    ResinReportViewer.LocalReport.ReportPath = Server.MapPath("~/RDLC/ResinReport.rdlc");

                    ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);
                    ResinReportViewer.LocalReport.DataSources.Clear();
                    ResinReportViewer.LocalReport.DataSources.Add(datasource);
                }
                else
                {
                    string errmsg2 = "NO RECORDS FOUND";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), string.Format("{0}_js", this.ClientID), string.Format("<script type='text/javascript'>alert('{0}');</script>", errmsg2), false);
                    ResinReportViewer.Visible = false;
                    return;
                }
            }
            catch (System.Exception ex)
            {
                _Log.Error(ex.Message, ex);
            }
            finally
            {
                ;
            }
        }

        public static void TagIntWrite(string StatusCode, string tagName)
        {
            DeviceWise.IntegerTagHarness setRMTSTATUSCODE = new IntegerTagHarness();
            setRMTSTATUSCODE.IntTagdataType = DwDataType.INT2;
            setRMTSTATUSCODE.write(StatusCode, DeviceWise.Global.GlobalDevice, tagName);
        }

        [WebMethod]
        public static string[] CreateBatch(string JJVCPart, string BatchNumber, string R_ESIG_WWID1, string R_ESIG_USERID1, string R_EXPECTED_DATE, string SILO_1,string SILO_2,string SILO_3,string SILO_4, string SILO_5,string SILO_6, string PHASE_7)
        {
            ReleaseResinInDbEntity entity = new ReleaseResinInDbEntity();
            
            string MTA3_A_PartNo = null;
            string MTA3_B_PartNo = null;
            string Phase1 = "False";
            string Phase7="False";
            Phase7 = PHASE_7;

            string[] vReturn = new string[2];
            var rx = new System.Text.RegularExpressions.Regex(@"(?<=\w)\w");

            ReleaseResin pl = new ReleaseResin();

            GetDbConnectionHelper GetDbConnection = new GetDbConnectionHelper(HttpContext.Current.Server.MapPath(pl.RMTCOnnectionstring));
            try
            {
                if (SILO_1.ToUpper() == "TRUE" || SILO_2.ToUpper() == "TRUE" || SILO_3.ToUpper() == "TRUE" || SILO_4.ToUpper() == "TRUE" || SILO_5.ToUpper() == "TRUE" || SILO_6.ToUpper() == "TRUE")
                {
                    Phase1 = "True";
                }
                //if (SILO_7.ToUpper() == "TRUE" || SILO_8.ToUpper() == "TRUE")
                //{
                //    Phase7 = "True";
                //}
                entity.R_IS_ACTIVE = "True";
                entity.R_RAW_MAT_NUM = JJVCPart;
                entity.R_BATCH = BatchNumber;
                entity.R_PHASE_NUM_1 = rx.Replace(Phase1.ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_PHASE_NUM_7 = rx.Replace(Phase7.ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_1 = rx.Replace(SILO_1.ToString().ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_2 = rx.Replace(SILO_2.ToString().ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_3 = rx.Replace(SILO_3.ToString().ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_4 = rx.Replace(SILO_4.ToString().ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_5 = rx.Replace(SILO_5.ToString().ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_6 = rx.Replace(SILO_6.ToString().ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                //entity.R_SILO_7 = rx.Replace(SILO_7.ToString().ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                //entity.R_SILO_8 = rx.Replace(SILO_8.ToString().ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant())); 
                entity.R_ESIG_USERID1 = R_ESIG_USERID1;
                entity.R_ESIG_WWID1 = R_ESIG_WWID1;
                entity.R_EXPECTED_DATE = Convert.ToDateTime(DateTime.Now);

                DataSet ds = GetDbConnection.PopulateList(string.Format("select {0},{1} from RMT.HELIOS_CONF ", "MTA3_A", "MTA3_B"));
                foreach (DataRow CurrRow in ds.Tables[0].Rows)
                {
                    MTA3_A_PartNo = Convert.ToString(CurrRow[0]);

                    MTA3_B_PartNo = Convert.ToString(CurrRow[1]);
                }
               
                vReturn[0] = GetDbConnection.CreateBatch(entity);
            }
            catch (System.Exception e)
            {
                vReturn[0] = "false";
                if (vReturn[0] == "false")
                {
                    TagIntWrite("3", TagNameEnum.P6RF_MTA3A_TRG.ToString());
                    TagIntWrite("3", TagNameEnum.P6RF_MTA3B_TRG.ToString());
                }
                vReturn[1] = e.Message;
            }
            return vReturn.ToArray();
        }
      
        [WebMethod]
        public static string[] CreateBatchLoop(string JJVCPart, string BatchNumber, string R_ESIG_WWID1, string R_ESIG_USERID1, string myCheckbox, string cbsilo1, string cbsilo2, string cbsilo3, string cbsilo4, string cbsilo5, string cbsilo6)
        {
            string[] vReturn = new string[2];
            string Phase1 = "False";
            string Phase7 = "False";
            if (cbsilo1.ToUpper() == "TRUE" || cbsilo2.ToUpper() == "TRUE" || cbsilo3.ToUpper() == "TRUE" || cbsilo4.ToUpper() == "TRUE" || cbsilo5.ToUpper() == "TRUE" || cbsilo6.ToUpper() == "TRUE")
            {
                Phase1 = "True";
            }
            //if (cbsilo7.ToUpper() == "TRUE" || cbsilo8.ToUpper() == "TRUE")
            //{
            //    Phase7 = "True";
            //}

            var rx = new System.Text.RegularExpressions.Regex(@"(?<=\w)\w");
            ReleaseResin pl = new ReleaseResin();
            GetDbConnectionHelper GetDbConnection = new GetDbConnectionHelper(HttpContext.Current.Server.MapPath(pl.RMTCOnnectionstring));
            try
            {
                ReleaseResinInDbEntity entity = new ReleaseResinInDbEntity();
                entity.R_IS_ACTIVE = rx.Replace(myCheckbox.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_PHASE_NUM_1 = rx.Replace(Phase1.ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_PHASE_NUM_7 = rx.Replace(Phase7.ToUpperInvariant(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_1 = rx.Replace(cbsilo1.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_2 = rx.Replace(cbsilo2.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_3 = rx.Replace(cbsilo3.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_4 = rx.Replace(cbsilo4.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_5 = rx.Replace(cbsilo5.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                entity.R_SILO_6 = rx.Replace(cbsilo6.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                //entity.R_SILO_7 = rx.Replace(cbsilo7.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                //entity.R_SILO_8 = rx.Replace(cbsilo8.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                entity.R_RAW_MAT_NUM = JJVCPart;
                entity.R_BATCH = BatchNumber;
                entity.R_ESIG_USERID1 = R_ESIG_USERID1;
                entity.R_ESIG_WWID1 = R_ESIG_WWID1;
                entity.R_EXPECTED_DATE = Convert.ToDateTime(DateTime.Now);

                vReturn[0] = GetDbConnection.CreateBatch(entity);
            }
            catch (System.Exception e)
            {
                vReturn[0] = "false";
                vReturn[1] = e.Message;
            }
            
            return vReturn.ToArray();
        }
      
        public void updateResinBtn_Click()
        {
            string[] vReturn = new string[2];
            ReleaseResin pl = new ReleaseResin();
            GetDbConnectionHelper GetDbConnection = new GetDbConnectionHelper(HttpContext.Current.Server.MapPath(pl.RMTCOnnectionstring));
            try
            {
                var rx = new System.Text.RegularExpressions.Regex(@"(?<=\w)\w");
                foreach (DataGridItem DemoGridItem in datagrid.Items)
                {                     
                    CheckBox myCheckbox = (CheckBox)DemoGridItem.FindControl("chkSelection");
                    string cellValue = DemoGridItem.Cells[10].Text.ToString().ToLower();//isActive
                    CheckBox Silo1 = (CheckBox)DemoGridItem.FindControl("chkSilo1");
                    string siloValue1 = DemoGridItem.Cells[9].Text.ToString().ToLower();
                    CheckBox Silo2 = (CheckBox)DemoGridItem.FindControl("chkSilo2");
                    string siloValue2 = DemoGridItem.Cells[10].Text.ToString().ToLower();
                    CheckBox Silo3 = (CheckBox)DemoGridItem.FindControl("chkSilo3");
                    string siloValue3 = DemoGridItem.Cells[11].Text.ToString().ToLower();
                    CheckBox Silo4 = (CheckBox)DemoGridItem.FindControl("chkSilo4");
                    string siloValue4 = DemoGridItem.Cells[12].Text.ToString().ToLower();
                    CheckBox Silo5 = (CheckBox)DemoGridItem.FindControl("chkSilo5");
                    string siloValue5 = DemoGridItem.Cells[13].Text.ToString().ToLower();
                    CheckBox Silo6 = (CheckBox)DemoGridItem.FindControl("chkSilo6");
                    string siloValue6 = DemoGridItem.Cells[14].Text.ToString().ToLower();
                    //CheckBox Silo7 = (CheckBox)DemoGridItem.FindControl("chkSilo7");
                    //string siloValue7 = DemoGridItem.Cells[15].Text.ToString().ToLower();
                    //CheckBox Silo8 = (CheckBox)DemoGridItem.FindControl("chkSilo8");
                    //string siloValue8 = DemoGridItem.Cells[16].Text.ToString().ToLower(); 
                    if (myCheckbox.Checked.ToString().ToLower() != cellValue ||Silo1.Checked.ToString().ToLower() != siloValue1 || Silo2.Checked.ToString().ToLower() != siloValue2 || Silo3.Checked.ToString().ToLower() != siloValue3 || Silo4.Checked.ToString().ToLower() != siloValue4 || Silo5.Checked.ToString().ToLower() != siloValue5 || Silo6.Checked.ToString().ToLower() != siloValue6)
                    {                       
                        ReleaseResinInDbEntity entity = new ReleaseResinInDbEntity();
                        entity.R_IS_ACTIVE = rx.Replace(myCheckbox.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        entity.R_PHASE_NUM_1 = "";// rx.Replace(Phase1.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        entity.R_PHASE_NUM_7 = "";// rx.Replace(Phase7.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        entity.R_SILO_1 = rx.Replace(Silo1.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        entity.R_SILO_2 = rx.Replace(Silo2.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        entity.R_SILO_3 = rx.Replace(Silo3.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        entity.R_SILO_4 = rx.Replace(Silo4.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        entity.R_SILO_5 = rx.Replace(Silo5.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        entity.R_SILO_6 = rx.Replace(Silo6.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        //entity.R_SILO_7 = rx.Replace(Silo7.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        //entity.R_SILO_8 = rx.Replace(Silo8.Checked.ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));
                        entity.R_RAW_MAT_NUM = DemoGridItem.Cells[3].Text.ToString();
                        entity.R_BATCH = DemoGridItem.Cells[4].Text.ToString();
                        entity.R_ESIG_USERID1 = R_ESIG_USERID1.Value;
                        entity.R_ESIG_WWID1 = R_ESIG_WWID1.Value;
                        entity.R_EXPECTED_DATE = Convert.ToDateTime(MasterExpDate.Text);

                        vReturn[0] = GetDbConnection.CreateBatch(entity);

                        _Log.Info("Item will be updated");
                    }
                }
            }
            catch (System.Exception ex)
            {
                _Log.Error(ex.Message, ex);
                vReturn[1] = Regex.Replace(ex.Message, "[^0-9a-zA-Z]+", " ");
                string error = "OKDialogTest('Communication Error','" + vReturn[1].ToString() + "','500px','');";
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", error, true);
            }
            finally
            {
                if (vReturn[0] == "true")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "OKDialogTest('Resin Information Updated','<b>Status available immediately.</b><br><br>Thank you.','500px','');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "OKDialogTest('Not ready','<b>Unknown Condition encountered.</b><br><br>Please try again. If this continues, please contact support.','500px','');", true);
                }
            }
        }

        
    }
}