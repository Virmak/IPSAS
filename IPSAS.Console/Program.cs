using IPSAS.Application.UseCases;
using System;

namespace IPSAS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            PayslipPdfGenerator payslipPdf = new PayslipPdfGenerator();
            payslipPdf.Generate();
        }
    }
}
