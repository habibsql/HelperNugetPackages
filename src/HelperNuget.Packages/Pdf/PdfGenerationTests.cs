using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HelperNuget.Packages.Pdf
{
    /* Nuget Package Depenency: PdfSharp  */
    public class PdfGenerationTests
    {
        [Fact]
        public void  ShouldGeneratePdfFile()
        {
            string filePath = "c:/data/mypdffile.pdf";
            string data = "Hello Bangladesh! I love you very much.";

            GeneratePdf(filePath, data);
        }

        private void GeneratePdf(string fileName, string data)
        {
            var document = new PdfDocument();
            PdfPage page = document.AddPage();           
            XGraphics gfx = XGraphics.FromPdfPage(page);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            gfx.DrawString(data, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

            document.Save(fileName);
        }

    }
}
