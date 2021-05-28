using BracketPipe;
using RtfPipe;
using System;
using System.IO;
using System.Text;

namespace ConvertRTF
{
        /// <summary>
        /// Command-line application to convert a rich-text file (.rtf) to HTML or plain text
        /// </summary>
        public static class Program
        {
        /// <summary>
        /// Converts a rich-text file (.rtf) to HTML or plain text
        /// </summary>
        /// <param name="input">The path to the .rtf file to be converted.</param>
        /// <param name="text">If plain text is the desired output</param>
        /// <param name="html">If HTML is the desired output (the default)</param>
        /// <param name="output">The path to the output file (defaults to input file with different extension)</param>
        public static void Main(FileInfo input, bool text, bool html = true, FileInfo output = null)
        {
            if (input == null)
            {
                Console.WriteLine("Error: No input file specified!");
                return;
            }
            if (!input.Name.EndsWith(".rtf", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Error: Input must be a .rtf file!");
                return;
            }

            Console.WriteLine($"Converting {input}");

            // Using RTFPipe/BracketPipe for conversion
            // This line is required for .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Convert RTF->HTML
            using StreamReader reader = File.OpenText(input.FullName);
            RtfSource rtf = new RtfSource(reader);
            var htmlOutput = RtfPipe.Rtf.ToHtml(rtf);
            var outPath = output?.FullName ?? Path.Combine(input.DirectoryName, Path.GetFileNameWithoutExtension(input.Name) + ".html");
            if (html)
            {
                Console.WriteLine($"Writing to {outPath}");
                File.WriteAllText(outPath, htmlOutput);
            }

            // Convert the HTML to plain text
            var outPath2 = output?.FullName ?? Path.Combine(input.DirectoryName, Path.GetFileNameWithoutExtension(input.Name) + ".txt");
            var textOutput = Html.ToPlainText(htmlOutput);
            if (text)
            {
                Console.WriteLine($"Writing to {outPath2}");
                File.WriteAllText(outPath2, textOutput);
            }
        }
    }
}
