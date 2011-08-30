﻿/*
Copyright ©  Olav Christian Botterli.

Dual licensed under the MIT or GPL Version 2 licenses.

Date: 30.08.2011, Norway.

http://www.webgrid.com
*/


#region Header

/*
Copyright ©  Olav Christian Botterli. 

Dual licensed under the MIT or GPL Version 2 licenses.

Date: 30.08.2011, Norway.

http://www.webgrid.com
*/

#endregion Header

namespace WebGridUnitTest
{
    using System;
    using System.Data;
    using DateTime = System.DateTime;
    using System.IO;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using NUnit.Framework;

    using WebGrid;
    using WebGrid.Enums;

    [TestFixture]
    public class DataSourceControl : UnitTestCommon
    {
        #region Fields

        private int employees;

        #endregion Fields

        #region Methods

        [Test]
        public void CreateAndDeleteRecord()
        {
            CreateRecordSelect();

            int CategoryId = GetCategoryId();

            Grid grid = new Grid();
            Testpage.Controls.Add(grid);
            SetDataSource(grid);
            grid.ID = "test";
            grid.DefaultVisibility = Visibility.Both;
            grid.RecordsPerRow = 2;
            grid.PagerSettings.PagerType = PagerType.Standard;
            grid.Width = Unit.Pixel(1000);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
            grid.RenderControl(gridwriter);
            Assert.AreEqual(grid.MasterTable.DataSourceType, DataSourceControlType.AccessDataSource);
            int records = grid.MasterTable.Rows.Count;

            grid = new Grid();
            Testpage.Controls.Add(grid);
            SetDataSource(grid);
            grid.ID = "test";
            grid.DefaultVisibility = Visibility.Both;
            grid.RecordsPerRow = 2;
            grid.PagerSettings.PagerType = PagerType.Standard;
            grid.Width = Unit.Pixel(1000);
            grid.RaisePostBackEvent(string.Format("RecordDeleteClick!{0}", CategoryId));

            sb = new StringBuilder();
            sw = new StringWriter(sb);
            gridwriter = new HtmlTextWriter(sw);
            ////grid.ReLoadData = true;
            grid.RenderControl(gridwriter);

         //   Assert.AreEqual(grid.MasterTable.Rows.Count,  GetCategoryCount());
            //   Assert.AreEqual(records-1, grid.MasterTable.Rows.Count);
            Assert.AreEqual(grid.DisplayView, DisplayView.Grid);
            Assert.LessOrEqual(grid.SystemMessage.Count, 1);
        }

        [Test]
        public void CreateRecordSelect()
        {
            Grid grid = new Grid();
            Testpage.Controls.Add(grid);
            grid = CreateNewCategorySelect(grid);
            grid["CategoryID"].Visibility = Visibility.None;
            int records = grid.MasterTable.Rows.Count;

            Assert.AreEqual(grid.DisplayView, DisplayView.Detail);
            grid.RaisePostBackEvent("RecordUpdateClick!!False");
            Assert.AreEqual(grid.SystemMessage.Count, 0);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
            ////grid.ReLoadData = true;
            grid.RenderControl(gridwriter);
            Assert.Greater(grid.MasterTable.Rows.Count, records);

            Assert.AreEqual(grid.DisplayView, DisplayView.Grid);
        }

        //[Test]
        public void NewRecordWithDataSourceControl()
        {
            Grid grid = new Grid();
            Testpage.Controls.Add(grid);
            SetDataSource(grid);
            grid.ID = "test";
            grid.DefaultVisibility = Visibility.Both;
            grid.RecordsPerRow = 2;
            grid.PagerSettings.PagerType = PagerType.None;
            grid.Width = Unit.Pixel(1000);

            Assert.Greater(grid.MasterTable.Rows.Count, 1);
            Assert.AreEqual(grid.DisplayView, DisplayView.Grid);
            grid.RaisePostBackEvent("NewRecordClick!");
            Assert.AreEqual(grid.MasterTable.Rows.Count, 1);
            Assert.AreEqual(grid.DisplayView, DisplayView.Detail);
        }

        [Test]
        public void SelectFromAccessDataSourceControl()
        {
            Grid grid = new Grid();

            SetupGrid(grid);

            AccessDataSource ds = new AccessDataSource();
            ds.SelectCommand = "select * from Employees";
            ds.ID = "test1";
            ds.DataFile = Path + "\\App_Data\\Nwind.mdb";
            Testpage.Controls.Add(ds);
            grid.DataSourceId = ds.ID;

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
            grid.RenderControl(gridwriter);
            Assert.Greater(grid.MasterTable.Rows.Count, 1);
            employees = grid.MasterTable.Rows.Count;
        }

        [Test]
        public void SelectFromAccessDataSourceControlSearch()
        {
            SelectFromAccessDataSourceControl();
            Grid grid = new Grid();

            SetupGrid(grid);

            AccessDataSource ds = new AccessDataSource();
            ds.SelectCommand = "select * from Employees";
            ds.ID = "test1";
            ds.DataFile = Path + "\\App_Data\\Nwind.mdb";
            Testpage.Controls.Add(ds);
            grid.DataSourceId = ds.ID;
            grid.Search = "ol -olav";

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
            grid.RenderControl(gridwriter);

            Assert.Less(grid.MasterTable.Rows.Count, employees);
        }

        [Test]
        public void SelectFromSqlDataSourceControl()
        {
            Testpage.Controls.Clear();
            Grid grid = new Grid();

            SetupGrid(grid);

            SqlDataSource ds = new SqlDataSource();
            ds.SelectCommand = "select * from test";
            ds.ID = "test1";
            ds.ConnectionString = ConnectionSqlConnection;
            Testpage.Controls.Add(ds);
            grid.DataSourceId = ds.ID;
            grid.ConnectionString = null;
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
            grid.RenderControl(gridwriter);
            Assert.AreEqual(grid.MasterTable.DataSourceType, DataSourceControlType.SqlDataSource);
            Assert.Greater(grid.MasterTable.Rows.Count, 1);
        }

        [Test]
        public void SelectFromXmlDataSourceControl()
        {
            Testpage.Controls.Clear();
            Grid grid = new Grid();

            SetupGrid(grid);

            XmlDataSource ds = new XmlDataSource();
            ds.XPath = "//Language//*";
            ds.ID = "test1";
            ds.DataFile = Path + "\\WebGridMessages.xml";
               Testpage.Controls.Add(ds);
            grid.DataSourceId = ds.ID;
            grid["LanguageID"].Primarykey = true;
            grid["LanguageID"].HyperLinkColumn = true;
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
            grid.RenderControl(gridwriter);
            //It is converted to from XmlDataSource to EnumerableDataSource
            Assert.AreEqual(grid.MasterTable.DataSourceType, DataSourceControlType.EnumerableDataSource);
            Assert.Greater(grid.MasterTable.Rows.Count, 1);

            string[] triggerStrings = new[]
                                          {
                                              "Fehler beim Speichern mehrerer",
                                              "Forrige side",
                                              "RecordDeleteClick!Danish",
                                              "ExcelExportClick",
                                              "RecordClick!LanguageID!Norwegian",
                                          };
            string content = sb.ToString();
            foreach (string part in triggerStrings)
            {
                int res = content.IndexOf(part);
                Assert.Greater(res, -1, part + Environment.NewLine + content);
            }
        }

        [Test]
        public void SelectFromXmlDataSourceControlSearch()
        {
            Grid grid = new Grid();

            SetupGrid(grid);

            XmlDataSource ds = new XmlDataSource();
            ds.XPath = "//Language//*";
            ds.ID = "test1";
            ds.DataFile = Path + "\\WebGridMessages.xml";
            Testpage.Controls.Add(ds);
            grid.DataSourceId = ds.ID;
            grid["LanguageID"].Primarykey = true;
            grid.Search = "\"a\" -t";
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
            grid.RenderControl(gridwriter);
            Assert.AreEqual(grid.MasterTable.Rows.Count, 6);
        }

        [Test]
        public void SelectFromXmlDataSourceEdit()
        {
            Grid grid = new Grid();

            SetupGrid(grid);

            XmlDataSource ds = new XmlDataSource();
            ds.XPath = "//Language//*";
            ds.ID = "test1";
            ds.DataFile = Path + "\\WebGridMessages.xml";
            Testpage.Controls.Add(ds);
            grid.DataSourceId = ds.ID;
            grid["LanguageID"].Primarykey = true;
            grid.RaisePostBackEvent("RecordClick!!Norwegian");

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
            grid.RenderControl(gridwriter);
            Assert.AreEqual(grid.MasterTable.Rows.Count, 1);
            Assert.LessOrEqual(grid.SystemMessage.Count, 1);

            string[] triggerStrings = new[]
                                          {
                                              "Forrige side",
                                              "RecordCancelClick!Norwegian!",
                                              "RecordUpdateClick!Norwegian!False"
                                          };
            string content = sb.ToString();
            foreach (string part in triggerStrings)
            {
                int res = content.IndexOf(part);
                Assert.Greater(res, -1, part + Environment.NewLine + content);
            }
        }

        [Test]
        public void SelectFromXmlDataSourceEditUpdate()
        {
            Grid grid = new Grid();

            SetupGrid(grid);

            XmlDataSource ds = new XmlDataSource();
            ds.XPath = "//Language//*";
            ds.ID = "test1";
            ds.DataFile = Path + "\\WebGridMessages.xml";
            Testpage.Controls.Add(ds);
            grid.DataSourceId = ds.ID;
            grid["LanguageID"].Primarykey = true;
            grid.EditIndex = "Norwegian";
            grid.DisplayView = DisplayView.Detail;
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
            string text = "T:" + DateTime.Now.Ticks;
            grid.Rows[0]["PagerPrefix"].Value = text;
            grid.BeforeUpdateInsert += grid_BeforeUpdateInsert;
            grid.RaisePostBackEvent("RecordUpdateClick!Norwegian!False");
            grid.RenderControl(gridwriter);
            Assert.Greater(grid.Rows.Count, 1, sb.ToString());
            Assert.LessOrEqual(grid.SystemMessage.Count, 1);

            string filecontent = System.IO.File.ReadAllText(Path + "\\WebGridMessages.xml");
            Assert.Greater(filecontent.IndexOf(text), 0);

               /* string[] triggerStrings = new[]
                                          {
                                              "Forrige side",
                                              "RecordCancelClick!Norwegian!",
                                              "RecordUpdateClick!Norwegian!False"
                                          };
            string content = sb.ToString();
            foreach (string part in triggerStrings)
            {
                int res = content.IndexOf(part);
                Assert.Greater(res, -1, part + Environment.NewLine + content);
            }
            */
        }

        public void SetObjectDataSource(Grid grid)
        {
            ObjectDataSource ds = new ObjectDataSource();
            ds.ID = "EmployeesObjectDataSource";

            ds.TypeName = "WebGridUnitTest.ObjectDataSource.NorthwindEmployeeData";
            ds.EnablePaging = true;
            ds.StartRowIndexParameterName = "StartRecord";
            ds.MaximumRowsParameterName = "MaxRecords";
            ds.SelectMethod = "GetAllEmployees";
            ds.DataObjectTypeName = "WebGridUnitTest.ObjectDataSource.NorthwindEmployee";
            ds.DeleteMethod = "DeleteEmployee";
            ds.InsertMethod = "InsertEmployee";
            ds.UpdateMethod = "UpdateEmployee";
            Testpage.Controls.Add(ds);
            grid.DataSourceId = ds.ID;
            grid["EmployeeID"].Primarykey = true;
            grid["EmployeeID"].Identity = true;
        }

        [Test]
        public void UpdateRecord()
        {
            Grid grid = new Grid();
            Testpage.Controls.Add(grid);
            grid = SelectAccessCategories(grid);
            grid.EditIndex = "6";
            int records = grid.MasterTable.Rows.Count;

            Assert.AreEqual(grid.DisplayView, DisplayView.Detail);
            grid.RaisePostBackEvent("RecordUpdateClick!!False");
            Assert.AreEqual(grid.SystemMessage.Count, 0);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
             grid.RenderControl(gridwriter);

            Assert.AreEqual(grid.DisplayView, DisplayView.Grid);
            Assert.Greater(grid.MasterTable.Rows.Count, records);
        }

        [Test]
        public void UpdateRecordStayInDetail()
        {
            Grid grid = new Grid();
            Testpage.Controls.Clear();
            Testpage.Controls.Add(grid);
            grid = SelectAccessCategories(grid);
            grid.EditIndex = "6";
            grid.StayInDetail = true;
            int records = grid.MasterTable.Rows.Count;

            Assert.AreEqual(grid.DisplayView, DisplayView.Detail);
            grid.RaisePostBackEvent("RecordUpdateClick!!False");
            Assert.AreEqual(grid.SystemMessage.Count, 0);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter gridwriter = new HtmlTextWriter(sw);
            ////grid.ReLoadData = true;
            grid.RenderControl(gridwriter);

            Assert.AreEqual(grid.DisplayView, DisplayView.Detail);
            Assert.AreEqual(grid.MasterTable.Rows.Count, records);
            Assert.AreEqual(grid.SystemMessage.Count, 0);
        }

        private static int GetCategoryCount()
        {
            AccessDataSource ds = new AccessDataSource();
            ds.SelectCommand = "SELECT count(*) as CategoryID FROM Categories";
            ds.ID = "test1";
            ds.DataFile = Path + "\\App_Data\\Nwind.mdb";
            return (int)((DataView)ds.Select(DataSourceSelectArguments.Empty))[0]["CategoryID"];
        }

        private static int GetCategoryId()
        {
            AccessDataSource ds = new AccessDataSource();
            ds.SelectCommand = "SELECT TOP 1 CategoryID FROM Categories ORDER BY CategoryID DESC";
            ds.ID = "test1";
            ds.DataFile = Path + "\\App_Data\\Nwind.mdb";
            return (int) ((DataView) ds.Select(DataSourceSelectArguments.Empty))[0]["CategoryID"];
        }

        static void grid_BeforeUpdateInsert(object sender, WebGrid.Events.BeforeUpdateInsertEventArgs e)
        {
            //   e.Row["PagerPrefix"].Value = "Olav was here..";
        }

        /*
        private Grid SelectObjectDataSourceNorthWind(Grid grid)
        {
            SetObjectDataSource(grid);

            grid.ID = "test";

            grid.DefaultVisibility = Visibility.Both;
            grid.RecordsPerRow = 2;
            grid.PagerSettings.PagerType = PagerType.None;
            grid.Width = Unit.Pixel(1000);
            return grid;
        }
        */
        private Grid CreateNewCategorySelect(Grid grid)
        {
            SetDataSource(grid);
            grid.ID = "test";
            grid.DefaultVisibility = Visibility.Both;
            grid.RecordsPerRow = 2;
            grid.PagerSettings.PagerType = PagerType.None;
            grid.Width = Unit.Pixel(1000);

            grid.RaisePostBackEvent("NewRecordClick!");
            grid.MasterTable.Rows[0]["CategoryName"].Value = "Category name";
            grid.MasterTable.Rows[0]["Description"].Value = "test test";
            return grid;
        }

        private Grid SelectAccessCategories(Grid grid)
        {
            SetDataSource(grid);

            grid.ID = "test";
            Testpage.Controls.Add(grid);
            grid.ConnectionString = ConnectionAccessOleDb;
            grid.DefaultVisibility = Visibility.Both;
            grid.RecordsPerRow = 2;
            grid.PagerSettings.PagerType = PagerType.None;
            grid.Width = Unit.Pixel(1000);
            grid["CategoryID"].Primarykey = true;
            grid["CategoryID"].Identity = true;
            grid["CategoryID"].IsInDataSource = true;
            grid.RaisePostBackEvent("NewRecordClick!");

            grid.MasterTable.Rows[0]["CategoryName"].Value = "Category name";
            grid.MasterTable.Rows[0]["Description"].Value = "Unique ID:" + DateTime.Now.Ticks;
            return grid;
        }

        private void SetDataSource(Grid grid)
        {
            AccessDataSource ds = new AccessDataSource();
            ds.SelectCommand = "SELECT [CategoryID], [CategoryName], [Description] FROM [Categories]";
            ds.DeleteCommand = "DELETE FROM [Categories] WHERE [CategoryID] = ?";
            ds.InsertCommand = "INSERT INTO [Categories] ([CategoryName], [Description]) VALUES ( ?, ?)";
            ds.UpdateCommand = "UPDATE [Categories] SET [CategoryName] = ?, [Description] = ? WHERE [CategoryID] = ?";
            ds.InsertParameters.Add("CategoryName", null);
            ds.InsertParameters.Add("Description", null);
            ds.UpdateParameters.Add("CategoryName", null);
            ds.UpdateParameters.Add("Description", null);
            ds.DeleteParameters.Add("CategoryID", null);
            ds.UpdateParameters.Add("CategoryID", null);
            ds.ID = "test1";
            ds.DataFile = Path + "\\App_Data\\Nwind.mdb";
            Testpage.Controls.Add(ds);
            grid.DataSourceId = ds.ID;
            grid["CategoryID"].Primarykey = true;
            grid["CategoryID"].Identity = true;
        }

        private void SetupGrid(Grid grid)
        {
            grid.ID = "wggrid";
            grid.DefaultVisibility = Visibility.Both;
            grid.RecordsPerRow = 2;
            grid.PagerSettings.PagerType = PagerType.None;
            grid.Width = Unit.Pixel(1000);

            grid.DataSourceId = "Categories";
            grid.ConnectionString = ConnectionAccessOleDb;
            Testpage.Controls.Add(grid);
        }

        #endregion Methods
    }
}