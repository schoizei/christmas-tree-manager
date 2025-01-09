using ChristmasTreeManager.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Fonts = QuestPDF.Helpers.Fonts;

namespace ChristmasTreeManager.Services;

public class ExportService
{
    public Radzen.Query ExtractQuery(IQueryCollection? queryCollection = null, bool keyless = false)
    {
        var query = new Radzen.Query();

        if (queryCollection is null)
            return query;

        var filter = queryCollection.ContainsKey("$filter") ? queryCollection["$filter"].ToString() : null;
        if (!string.IsNullOrEmpty(filter))
        {
            query.Filter = filter;
        }

        if (queryCollection.ContainsKey("$orderBy"))
        {
            query.OrderBy = queryCollection["$orderBy"].ToString();
        }

        if (queryCollection.ContainsKey("$expand"))
        {
            query.Expand = queryCollection["$expand"].ToString();
        }

        if (queryCollection.ContainsKey("$select"))
        {
            query.Select = queryCollection["$select"].ToString();
        }

        if (queryCollection.ContainsKey("$skip"))
        {
            query.Skip = int.Parse(queryCollection["$skip"].ToString());
        }

        if (queryCollection.ContainsKey("$top"))
        {
            query.Top = int.Parse(queryCollection["$top"].ToString());
        }

        return query;
    }

    public FileStreamResult CollectionToursToPDF(CollectionTourExport? data, string? fileName = null)
    {
        // code in your main method
        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.MarginHorizontal(1.5f, Unit.Centimetre);
                page.MarginTop(4.2f, Unit.Centimetre);
                page.MarginBottom(2.0f, Unit.Centimetre);
                page.DefaultTextStyle(TextStyle.Default.FontFamily(Fonts.Arial).FontSize(11));
                page.Background()
                    .AlignTop()
                    .ExtendHorizontal()
                    .Image(Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", "pdf-background.png"));

                if (data is not null)
                {
                    page.Header()
                        .Text($"Sammeltour: {data.Name}")
                        .FontSize(22).FontColor("#231F20");

                    page.Content()
                        .PaddingTop(0.5f, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Item().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(12));

                                text.Span("Fahrzeug: ").ExtraBold();
                                text.Span(data.Vehicle);
                            });
                            column.Item().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(12));

                                text.Span("Fahrer: ").ExtraBold();
                                text.Span(data.Driver);
                            });
                            column.Item().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(12));

                                text.Span(Encoding.UTF8.GetString(Encoding.Default.GetBytes("Schriftführer: "))).ExtraBold();
                                text.Span(data.TeamLeader);
                            });
                            column.Item().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(12));

                                text.Span("Team: ").ExtraBold();
                                text.Span(data.Staff);
                            });
                            column.Item().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(12));

                                text.Span("Bäume: ").Bold();
                                text.Span(data.Registrations.Sum(x => x.TreeCount).ToString());
                            });

                            column.Item().PaddingTop(0.5f, Unit.Centimetre).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(3);
                                    columns.ConstantColumn(0.5f, Unit.Centimetre);
                                    columns.RelativeColumn(3);
                                });

                                uint treeSum = 0;
                                foreach (var item in data.Registrations)
                                {
                                    table.Cell().Border(1).AlignMiddle().PaddingLeft(2).Text($"{item.Address}".Trim()).FontSize(10);
                                    table.Cell().Border(1).AlignMiddle().PaddingLeft(2).Text($"{item.Customer}".Trim());
                                    table.Cell().Border(1).AlignMiddle().AlignCenter().Text($"{item.TreeCount}".Trim());
                                    table.Cell().Border(1).AlignMiddle().PaddingLeft(2).Text($"{item.Comment}".Trim());
                                    treeSum += item.TreeCount;
                                }

                                table.Cell().ColumnSpan(2).AlignMiddle().AlignRight().Padding(2).Text("Summe Bäume:").FontSize(14).FontColor("#C30707");
                                table.Cell().ColumnSpan(2).AlignMiddle().AlignLeft().PaddingLeft(3).Text(treeSum.ToString()).FontSize(14).FontColor("#C30707");
                            });
                        });
                }
            });
        });

        // use the following invocation
        //document.ShowInCompanion();

        var result = new FileStreamResult(new MemoryStream(document.GeneratePdf()), "text/pdf");
        result.FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".pdf";
        return result;
    }

    public FileStreamResult DistributionToursToPDF(DistributionTourExport? data, string? fileName = null)
    {
        // code in your main method
        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.MarginHorizontal(1.5f, Unit.Centimetre);
                page.MarginTop(4.2f, Unit.Centimetre);
                page.MarginBottom(2.0f, Unit.Centimetre);
                page.DefaultTextStyle(TextStyle.Default.FontFamily(Fonts.Arial).FontSize(11));
                page.Background()
                    .AlignTop()
                    .ExtendHorizontal()
                    .Image(Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", "pdf-background.png"));

                if (data is not null)
                {
                    page.Header()
                        .Text($"Zetteltour: {data.Name}")
                        .FontSize(22).FontColor("#231F20");

                    page.Content()
                        .PaddingTop(0.5f, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Item().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(12));

                                text.Span("Team: ").ExtraBold();
                                text.Span(data.Staff);
                            });

                            column.Item().PaddingTop(0.5f, Unit.Centimetre).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(4);
                                    columns.ConstantColumn(1.0f, Unit.Centimetre);

                                    columns.RelativeColumn(1);

                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(4);
                                    columns.ConstantColumn(1.0f, Unit.Centimetre);
                                });

                                uint formCount = 0;
                                foreach (var item in data.Streets)
                                {
                                    table.Cell().Border(1).AlignMiddle().PaddingLeft(2).Text($"{item.City}".Trim()).FontSize(10);
                                    table.Cell().Border(1).AlignMiddle().PaddingLeft(2).Text($"{item.Name}".Trim());
                                    table.Cell().Border(1).AlignMiddle().AlignCenter().Text($"{item.DistributionTourFormCount}".Trim());
                                    table.Cell().Border(0).AlignMiddle().PaddingLeft(2);
                                    table.Cell().Border(1).AlignMiddle().PaddingLeft(2).Text($"{item.City}".Trim()).FontSize(10);
                                    table.Cell().Border(1).AlignMiddle().PaddingLeft(2).Text($"{item.Name}".Trim());
                                    table.Cell().Border(1).AlignMiddle().AlignCenter().Text($"{item.DistributionTourFormCount}".Trim());
                                    formCount += item.DistributionTourFormCount;
                                }

                                table.Cell().ColumnSpan(2).AlignMiddle().AlignRight().Padding(2).Text("Summe Zettel:").FontSize(14).FontColor("#C30707");
                                table.Cell().ColumnSpan(2).AlignMiddle().AlignLeft().PaddingLeft(3).Text(formCount.ToString()).FontSize(14).FontColor("#C30707");

                                table.Cell().ColumnSpan(2).AlignMiddle().AlignRight().Padding(2).Text("Summe Zettel:").FontSize(14).FontColor("#C30707");
                                table.Cell().ColumnSpan(1).AlignMiddle().AlignLeft().PaddingLeft(3).Text(formCount.ToString()).FontSize(14).FontColor("#C30707");
                            });
                        });
                }
            });
        });

        // use the following invocation
        //document.ShowInCompanion();

        var result = new FileStreamResult(new MemoryStream(document.GeneratePdf()), "text/pdf");
        result.FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".pdf";
        return result;
    }

    public FileStreamResult ToCSV(IQueryable query, string? fileName = null)
    {
        var columns = GetProperties(query.ElementType);

        var sb = new StringBuilder();

        foreach (var item in query)
        {
            var row = new List<string>();

            foreach (var column in columns)
            {
                row.Add($"{GetValue(item, column.Key)}".Trim());
            }

            sb.AppendLine(string.Join(",", row.ToArray()));
        }


        var result = new FileStreamResult(new MemoryStream(Encoding.Default.GetBytes($"{string.Join(",", columns.Select(c => c.Key))}{Environment.NewLine}{sb.ToString()}")), "text/csv");
        result.FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".csv";

        return result;
    }

    public FileStreamResult ToExcel(IQueryable query, string? fileName = null)
    {
        var columns = GetProperties(query.ElementType);
        var stream = new MemoryStream();

        using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();

            var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            GenerateWorkbookStylesPartContent(workbookStylesPart);

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
            sheets.Append(sheet);

            workbookPart.Workbook.Save();

            var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

            var headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

            foreach (var column in columns)
            {
                headerRow.Append(new DocumentFormat.OpenXml.Spreadsheet.Cell()
                {
                    CellValue = new CellValue(column.Key),
                    DataType = new EnumValue<CellValues>(CellValues.String)
                });
            }

            sheetData.AppendChild(headerRow);

            foreach (var item in query)
            {
                var row = new DocumentFormat.OpenXml.Spreadsheet.Row();

                foreach (var column in columns)
                {
                    var value = GetValue(item, column.Key);
                    var stringValue = $"{value}".Trim();

                    var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();

                    var underlyingType = column.Value.IsGenericType &&
                        column.Value.GetGenericTypeDefinition() == typeof(Nullable<>) ?
                        Nullable.GetUnderlyingType(column.Value) : column.Value;

                    var typeCode = Type.GetTypeCode(underlyingType);

                    if (typeCode == TypeCode.DateTime)
                    {
                        if (!string.IsNullOrWhiteSpace(stringValue))
                        {
                            cell.CellValue = new CellValue() { Text = ((DateTime)value).ToOADate().ToString(CultureInfo.InvariantCulture) };
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                            cell.StyleIndex = (UInt32Value)1U;
                        }
                    }
                    else if (typeCode == TypeCode.Boolean)
                    {
                        cell.CellValue = new CellValue(stringValue.ToLowerInvariant());
                        cell.DataType = new EnumValue<CellValues>(CellValues.Boolean);
                    }
                    else if (IsNumeric(typeCode))
                    {
                        if (value != null)
                        {
                            stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                        }
                        cell.CellValue = new CellValue(stringValue);
                        cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                    }
                    else
                    {
                        cell.CellValue = new CellValue(stringValue);
                        cell.DataType = new EnumValue<CellValues>(CellValues.String);
                    }

                    row.Append(cell);
                }

                sheetData.AppendChild(row);
            }


            workbookPart.Workbook.Save();
        }

        if (stream?.Length > 0)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        var result = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        result.FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".xlsx";

        return result;
    }

    public static object GetValue(object target, string name)
    {
        return target.GetType().GetProperty(name)!.GetValue(target)!;
    }

    public static IEnumerable<KeyValuePair<string, Type>> GetProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && IsSimpleType(p.PropertyType)).Select(p => new KeyValuePair<string, Type>(p.Name, p.PropertyType));
    }

    public static bool IsSimpleType(Type type)
    {
        var underlyingType = type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(Nullable<>) ?
            Nullable.GetUnderlyingType(type) : type;

        if (underlyingType == typeof(Guid) || underlyingType == typeof(DateTimeOffset))
            return true;

        var typeCode = Type.GetTypeCode(underlyingType);

        switch (typeCode)
        {
            case TypeCode.Boolean:
            case TypeCode.Byte:
            case TypeCode.Char:
            case TypeCode.DateTime:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.SByte:
            case TypeCode.Single:
            case TypeCode.String:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return true;
            default:
                return false;
        }
    }

    private static bool IsNumeric(TypeCode typeCode)
    {
        switch (typeCode)
        {
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return true;
            default:
                return false;
        }
    }

    private static void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1)
    {
        var stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac x16r2 xr" } };
        stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
        stylesheet1.AddNamespaceDeclaration("x16r2", "http://schemas.microsoft.com/office/spreadsheetml/2015/02/main");
        stylesheet1.AddNamespaceDeclaration("xr", "http://schemas.microsoft.com/office/spreadsheetml/2014/revision");

        var fonts1 = new DocumentFormat.OpenXml.Spreadsheet.Fonts() { Count = (UInt32Value)1U, KnownFonts = true };
        var font1 = new DocumentFormat.OpenXml.Spreadsheet.Font();
        var fontSize1 = new FontSize() { Val = 11D };
        var color1 = new DocumentFormat.OpenXml.Office2010.Excel.Color() { Theme = (UInt32Value)1U };
        var fontName1 = new FontName() { Val = "Calibri" };
        var fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
        var fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

        font1.Append(fontSize1);
        font1.Append(color1);
        font1.Append(fontName1);
        font1.Append(fontFamilyNumbering1);
        font1.Append(fontScheme1);

        fonts1.Append(font1);

        var fills1 = new Fills() { Count = (UInt32Value)2U };

        var fill1 = new Fill();
        var patternFill1 = new PatternFill() { PatternType = PatternValues.None };

        fill1.Append(patternFill1);

        var fill2 = new Fill();
        var patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

        fill2.Append(patternFill2);

        fills1.Append(fill1);
        fills1.Append(fill2);

        var borders1 = new Borders() { Count = (UInt32Value)1U };
        var border1 = new Border();
        var leftBorder1 = new LeftBorder();
        var rightBorder1 = new RightBorder();
        var topBorder1 = new TopBorder();
        var bottomBorder1 = new BottomBorder();
        var diagonalBorder1 = new DiagonalBorder();

        border1.Append(leftBorder1);
        border1.Append(rightBorder1);
        border1.Append(topBorder1);
        border1.Append(bottomBorder1);
        border1.Append(diagonalBorder1);

        borders1.Append(border1);

        var cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
        var cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

        cellStyleFormats1.Append(cellFormat1);

        var cellFormats1 = new CellFormats() { Count = (UInt32Value)2U };
        var cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
        var cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)14U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyNumberFormat = true };

        cellFormats1.Append(cellFormat2);
        cellFormats1.Append(cellFormat3);

        var cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
        var cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

        cellStyles1.Append(cellStyle1);
        var differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
        var tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

        var stylesheetExtensionList1 = new StylesheetExtensionList();

        var stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
        stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");

        var stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
        stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");

        var openXmlUnknownElement4 = workbookStylesPart1.CreateUnknownElement("<x15:timelineStyles defaultTimelineStyle=\"TimeSlicerStyleLight1\" xmlns:x15=\"http://schemas.microsoft.com/office/spreadsheetml/2010/11/main\" />");

        stylesheetExtension2.Append(openXmlUnknownElement4);

        stylesheetExtensionList1.Append(stylesheetExtension1);
        stylesheetExtensionList1.Append(stylesheetExtension2);

        stylesheet1.Append(fonts1);
        stylesheet1.Append(fills1);
        stylesheet1.Append(borders1);
        stylesheet1.Append(cellStyleFormats1);
        stylesheet1.Append(cellFormats1);
        stylesheet1.Append(cellStyles1);
        stylesheet1.Append(differentialFormats1);
        stylesheet1.Append(tableStyles1);
        stylesheet1.Append(stylesheetExtensionList1);

        workbookStylesPart1.Stylesheet = stylesheet1;
    }
}
