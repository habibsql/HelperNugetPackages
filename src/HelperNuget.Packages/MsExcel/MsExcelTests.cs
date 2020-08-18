using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Xunit;

namespace Helper.Nuget.Packages.MsExcel
{
    /* Nuget Package Dependency NPOI */

    /// <summary>
    /// Generate Excel File. It is compitable with xls or xlsx format.
    /// Note: You can also use OpenXml for creating/manipulation any MS Office files.
    /// </summary>
    public class MsExcelTests
    {
        [Fact]
        public void ShouldGenerateExcelFileFromGivenData()
        {
            var table = new DataTable("data");
            table.Columns.Add("Id");
            table.Columns.Add("Name");

            DataRow row = table.NewRow();
            row["Id"] = 1;
            row["Name"] = "Name-1";
            table.Rows.Add(row);

            row = table.NewRow();
            row["Id"] = 2;
            row["Name"] = "Name-2";
            table.Rows.Add(row);

            // If no exception is generated the test will be passed.
            GridToExcelByNPOI(table, @"d:\excels\data.xlsx");
        }

        private void GridToExcelByNPOI(DataTable dt, string strExcelFileName)
        {
            IWorkbook workbook = Path.GetExtension(strExcelFileName).EndsWith(".xls")
                ? (IWorkbook)new HSSFWorkbook()
                : (IWorkbook)new XSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("Sheet1");
            ICellStyle HeadercellStyle = workbook.CreateCellStyle();
            HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            //NPOI.SS.UserModel.IFont headerfont = workbook.CreateFont();
            //headerfont.Boldweight = (short)FontBoldWeight.Bold;
            //HeadercellStyle.SetFont(headerfont);

            //As the column name with [Name,number, value, date]
            int icolIndex = 0;
            IRow headerRow = sheet.CreateRow(0);
            foreach (DataColumn item in dt.Columns)
            {
                ICell cell = headerRow.CreateCell(icolIndex);
                cell.SetCellValue(dt.Rows[0][icolIndex].ToString());
                cell.CellStyle = HeadercellStyle;
                icolIndex++;
            }

            ICellStyle cellStyle = workbook.CreateCellStyle();

            //Excel date format in order to avoid being automatically replaced, so the format is set to "@" represents a rate of view as text
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            int iRowIndex = 1;
            int iCellIndex = 0;
            int count = 0;

            foreach (DataRow Rowitem in dt.Rows)
            {
                if (count != 0)
                {
                    IRow DataRow = sheet.CreateRow(iRowIndex);
                    foreach (DataColumn Colitem in dt.Columns)
                    {
                        ICell cell = DataRow.CreateCell(iCellIndex);
                        cell.SetCellValue(Rowitem[Colitem].ToString());
                        cell.CellStyle = cellStyle;
                        iCellIndex++;
                    }
                    iCellIndex = 0;
                    iRowIndex++;
                }
                count++;
            }

            for (int i = 0; i < icolIndex; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using var file = new FileStream(strExcelFileName, FileMode.OpenOrCreate);
            workbook.Write(file);
            workbook = null;
        }
    }
}
