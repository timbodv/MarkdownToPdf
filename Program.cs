namespace MarkdownToPdf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using CommandLine;
    using MarkdownDeep;
    using Pechkin;

    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();

            if (Parser.Default.ParseArguments(args, options))
            {
                string html = ConvertMarkdownToHtml(options.InputFile);

                if (string.IsNullOrEmpty(options.DocumentTitle))
                {
                    options.DocumentTitle = new System.IO.FileInfo(options.InputFile).Name;
                }

                WritePdfFromHtml(html, options.OutputFile, options.DocumentTitle, options.CssFile, options.HeaderHtmlFile, options.FooterHtmlFile);
            }
        }

        private static string ConvertMarkdownToHtml(string markdown)
        {
                Markdown markdownDocument = new Markdown();
                markdownDocument.ExtraMode = true;
                return markdownDocument.Transform(System.IO.File.ReadAllText(markdown));
        }

        private static void WritePdfFromHtml(string html, string outputFile, string documentTitle, string cssFile, string headerHtmlFile, string footerHtmlFile)
        {
            GlobalConfig gc = new GlobalConfig();
            gc.SetPaperSize(System.Drawing.Printing.PaperKind.A4);
            gc.SetPaperOrientation(false);
            gc.SetDocumentTitle(documentTitle);

            SimplePechkin pechkin = new SimplePechkin(gc);
            ObjectConfig oc = new ObjectConfig();
            oc.SetAllowLocalContent(true);
            oc.SetPrintBackground(true);

            if (!string.IsNullOrEmpty(cssFile))
            {
                /// for some reason oc.SetUserStylesheetUri(new Uri(cssFile).AbsoluteUri) is not working, so although
                /// this looks hacky, it gets the job done
                html = @"<link href=""" + new Uri(cssFile).AbsoluteUri + @""" rel=""stylesheet"" type=""text/css"" />" + System.Environment.NewLine + html;
            }

            if (!string.IsNullOrEmpty(headerHtmlFile))
            {
                oc.Header.SetHtmlContent(new Uri(headerHtmlFile).AbsoluteUri);
            }

            if (!string.IsNullOrEmpty(footerHtmlFile))
            {
                oc.Footer.SetHtmlContent(new Uri(footerHtmlFile).AbsoluteUri);
            }

            try
            {
                System.IO.File.WriteAllBytes(outputFile, pechkin.Convert(oc, html));
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("The PDF file you are writing to is likely already open/in use");
            }
        }
    }
}
