using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using IPSAS.Domain.Entities;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace IPSAS.Application.UseCases
{
    public class PayslipPdfGenerator
    {
        public PayslipPdfGenerator()
        {

        }

        public void Generate(Teacher teacher, Payslip payslip, PayrollRecord payrollRecord, string filename)
        {
            var months = new string[] { "Janvier", "Février", "Mars", "Avril", "Mai", "Juin",
                "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre" };

            var template = PdfReader.Open("fiche_paie.pdf");

            var page = template.Pages[0];

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont titleFont = new XFont("Calibri", 20, XFontStyle.Bold);
            XFont normalFont = new XFont("Calibri", 12);

            // Draw the text
            //gfx.DrawString("FICHE DE PAIE", titleFont, XBrushes.Black,
            //  new XRect(0, 0, page.Width, page.Height),
            //  XStringFormats.TopCenter);

            // Draw teacher info box
            //var teacherInfoRect = new XRect(50, 50, 150, 100);
            //XPen pen = new XPen(XColors.Black, 1);
            //gfx.DrawRectangle(pen, teacherInfoRect);



            // Draw Teacher info
            gfx.DrawString(teacher.Id.ToString(), normalFont, XBrushes.Black,
                new XPoint(130, 114), XStringFormats.TopLeft);

            gfx.DrawString(teacher.CIN, normalFont, XBrushes.Black,
               new XPoint(130, 129), XStringFormats.TopLeft);

            gfx.DrawString(teacher.FullName, normalFont, XBrushes.Black,
               new XPoint(130, 144), XStringFormats.TopLeft);

            gfx.DrawString(teacher.Grade.ToString(), normalFont, XBrushes.Black,
               new XPoint(130, 158), XStringFormats.TopLeft);

            gfx.DrawString(teacher.Status.ToString(), normalFont, XBrushes.Black,
              new XPoint(130, 172), XStringFormats.TopLeft);

            gfx.DrawString(teacher.Speciality, normalFont, XBrushes.Black,
              new XPoint(130, 188), XStringFormats.TopLeft);

            // Draw Payment info

            gfx.DrawString(months[payslip.Payment.PaymentDate.Month - 1] + " " + payslip.Payment.PaymentDate.Year, normalFont, XBrushes.Black,
                new XPoint(382, 114), XStringFormats.TopLeft);

            gfx.DrawString(payslip.Payment.PaymentDate.ToString(), normalFont, XBrushes.Black,
               new XPoint(382, 129), XStringFormats.TopLeft);

            gfx.DrawString(payslip.Payment.PaymentType.ToString(), normalFont, XBrushes.Black,
               new XPoint(382, 144), XStringFormats.TopLeft);

            gfx.DrawString(payslip.Payment.Bank ?? "", normalFont, XBrushes.Black,
               new XPoint(382, 158), XStringFormats.TopLeft);

            if (payslip.Payment.PaymentType == PaymentType.BankTransfer)
            {
                gfx.DrawString(payslip.Payment.Reference, normalFont, XBrushes.Black,
                    new XPoint(382, 172), XStringFormats.TopLeft);
            }
            else if (payslip.Payment.PaymentType == PaymentType.Check)
            {
                gfx.DrawString(payslip.Payment.Reference, normalFont, XBrushes.Black,
                    new XPoint(382, 188), XStringFormats.TopLeft);
            }

            // Draw payroll info
            gfx.DrawString(payrollRecord.HoursCount.ToString(), normalFont, XBrushes.Black,
              new XPoint(36, 238), XStringFormats.TopLeft);

            gfx.DrawString(formatMoney(payrollRecord.Rate), normalFont, XBrushes.Black,
              new XPoint(135, 238), XStringFormats.TopLeft);;

            gfx.DrawString(formatMoney(payrollRecord.GrossPay), normalFont, XBrushes.Black,
              new XPoint(245, 238), XStringFormats.TopLeft);

            gfx.DrawString("15%", normalFont, XBrushes.Black,
              new XPoint(350, 238), XStringFormats.TopLeft);

            gfx.DrawString(formatMoney(payrollRecord.Net), normalFont, XBrushes.Black,
              new XPoint(447, 238), XStringFormats.TopLeft);

            // Draw total

            gfx.DrawString(formatMoney(payrollRecord.Net), normalFont, XBrushes.Black,
              new XPoint(447, 636), XStringFormats.TopLeft);


            // Save the document...
            // const string filename = "HelloWorld.pdf";


                template.Save(filename);
                Process.Start(new ProcessStartInfo(/*Directory.GetCurrentDirectory() + "\\" +*/ filename) { UseShellExecute = true });

            // ...and start a viewer.
            //Process.Start(filename);



        }

        private string formatMoney(double n)
        {
            return string.Format("{0:N2}", n);
        }

    }
}
