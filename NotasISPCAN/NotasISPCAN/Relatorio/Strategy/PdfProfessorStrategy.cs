using NotasISPCAN.Relatorio.Interfaces;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotasISPCAN.Relatorio.Strategy
{
    public class PdfProfessorStrategy : Igenerate
    {
        public async Task CriarPdf(DataTable fonteDados)
        {
            PdfDocument documento = new PdfDocument();
            PdfPage pagina = documento.Pages.Add();
            PdfGraphics graphics = pagina.Graphics;
            PdfGrid tabela = new PdfGrid();
            Stream imageStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("NotasISPCAN.Relatorio.Img.ispcan.jpg");

            RectangleF bounds = new RectangleF(0, 0, 90, 70);
            PdfImage image = PdfImage.FromStream(imageStream);
            pagina.Graphics.DrawImage(image, bounds);

            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9, PdfFontStyle.Bold);
            PdfTextElement element = new PdfTextElement("INSTITUTO SUPERIOR POLITÉCNICO DOM ALEXANDRE CARDEAL DO NASCIMENTO", subHeadingFont)
            {
                Brush = PdfBrushes.Black
            };
            element.Draw(pagina, new PointF(115, 10));
            PdfTextElement element2 = new PdfTextElement("DEPARTAMENTO DA ENGENHARIA E TELECOMUNICAÇÃO", subHeadingFont)
            {
                Brush = PdfBrushes.Black
            };
            element2.Draw(pagina, new PointF(180, 22));
            PdfTextElement element3 = new PdfTextElement("PAUTA DAS PARCELARES", subHeadingFont)
            {
                Brush = PdfBrushes.Black
            };
            element3.Draw(pagina, new PointF(240, 50));
            PdfTextElement element4 = new PdfTextElement("CURSO DE ENGª INFORMÁTICA", subHeadingFont)
            {
                Brush = PdfBrushes.Black
            };
            element4.Draw(pagina, new PointF(0, 70));
            PdfTextElement element5 = new PdfTextElement("DISCIPLINA: programação web", subHeadingFont)
            {
                Brush = PdfBrushes.Black
            };
            element5.Draw(pagina, new PointF(0, 85));
            PdfTextElement element6 = new PdfTextElement("4º ano - Manhã - I Simestre - 2021/2022", subHeadingFont)
            {
                Brush = PdfBrushes.Black
            };
            element6.Draw(pagina, new PointF(0, 100));

            DataTable invoiceDetails = fonteDados;
            tabela.DataSource = invoiceDetails;

            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = tabela.Headers[0];

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12f);
            tabela.Style.Font = font;
            for (int i = 0; i < header.Cells.Count; i++)
            {
                header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                if (i > 1)
                {
                    header.Height = CalculateHeightInRotation(header, font);
                }
            }
            tabela.BeginCellLayout += Tabela_BeginCellLayout;
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat
            {
                Layout = PdfLayoutType.Paginate
            };
            tabela.Draw(pagina, new RectangleF(new PointF(0, 140), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

            MemoryStream stream = new MemoryStream();
            documento.Save(stream);
            documento.Close(true);
            string name = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()+".pdf";
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView(name, "application/pdf", stream);
            MessagingCenter.Send<object>(name, "pdfgerado");
        }

        private float CalculateHeightInRotation(PdfGridRow row, PdfFont font)
        {
            float height = 20;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                string asss = row.Cells[i].Value.ToString();
                if (asss != null && asss is String)
                {
                    Syncfusion.Drawing.SizeF size = font.MeasureString(asss);
                    if (size.Width > height)
                        height = size.Width;
                }
            }
            return height;
        }

        private void Tabela_BeginCellLayout(object sender, PdfGridBeginCellLayoutEventArgs args)
        {
            if (args.IsHeaderRow && args.CellIndex > 1)
            {
                PdfGrid grid = sender as PdfGrid;
                PdfGridCell cell = grid.Headers[args.RowIndex].Cells[args.CellIndex];
                var values = cell.Value;
                if (values != null && values is String)
                {
                    PdfGraphicsState state = args.Graphics.Save();
                    if (args.CellIndex == 0)
                    {
                        args.Graphics.TranslateTransform(args.Bounds.Right, args.Bounds.Top);
                        args.Graphics.RotateTransform(90);
                        args.Graphics.DrawString(values.ToString(), new PdfStandardFont(PdfFontFamily.Helvetica, 10), brush: PdfBrushes.Black, point: new PointF(2, 2));
                    }
                    else
                    {
                        args.Graphics.TranslateTransform(args.Bounds.Left, args.Bounds.Bottom);
                        args.Graphics.RotateTransform(270);
                        args.Graphics.DrawString(
                            values.ToString(), new PdfStandardFont(PdfFontFamily.Helvetica, 10),
                            brush: PdfBrushes.Black,
                            point: new PointF(2, 2));
                    }
                    args.Graphics.Restore(state);
                    grid.Headers[args.RowIndex].Cells[args.CellIndex].Value = "";
                }
            }
        }
    }
}
