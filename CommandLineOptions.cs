namespace MarkdownToPdf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;

    public class CommandLineOptions
    {
        [Option('i', "input-file", Required = true, HelpText = "The markdown file to process")]
        public string InputFile { get; set; }

        [Option('o', "output-file", Required = true, HelpText = "The file to output the PDF to")]
        public string OutputFile { get; set; }

        [Option('h', "header-html-file", DefaultValue = "", HelpText = "A html file container header content")]
        public string HeaderHtmlFile { get; set; }

        [Option('f', "footer-html-file", DefaultValue = "", HelpText = "A html file container footer content")]
        public string FooterHtmlFile { get; set; }

        [Option('s', "style-css-file", DefaultValue = "", HelpText = "A css file containing style content")]
        public string CssFile { get; set; }

        [Option('t', "document-title", DefaultValue = "", HelpText = "The title of the document. If not specified, the name of the input document will be used")]
        public string DocumentTitle { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
