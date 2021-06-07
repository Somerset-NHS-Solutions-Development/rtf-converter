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
        private static readonly string STDIN = "stdin";
        private static readonly string STDOUT = "stdout";

        /// <summary>
        /// Converts a rich-text file (.rtf) to HTML or plain text
        /// </summary>
        /// <param name="input">The path to the .rtf file to be converted.</param>
        /// <param name="text">If plain text is the desired output</param>
        /// <param name="html">If HTML is the desired output (the default)</param>
        /// <param name="output">The path to the output file (defaults to input file with different extension)</param>
        public static void Main(FileInfo input = null, bool text = false, bool html = false, FileInfo output = null)
        {
            if (input != null && !input.Name.EndsWith(".rtf", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Error: Input must be a .rtf file!");
                return;
            }

            // Must write to either text or html, so ensure at least on of these is true
            if (text == false)
                html = true;

            // Get the name of the input file, or use stdin if none supplied
            var inputName = input == null ? STDIN : input.ToString();
            Console.WriteLine($"Converting {inputName}");

            // Using RTFPipe/BracketPipe for conversion
            // This line is required for .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Convert RTF->HTML
            // Read from input file or stdin if none specified
            using StreamReader reader = (input == null) ? new StreamReader(Console.OpenStandardInput()) : File.OpenText(input.FullName);
            RtfSource rtf = new RtfSource(reader);
            var htmlOutput = RtfPipe.Rtf.ToHtml(rtf);

            // Output can go to:
            // - An output file if one was specified
            // - An output file which has the same name as the input file with different extension, if input file was specified
            // - Stdout if neither input or output file specified
            string outPath, outPath2;

            if (output != null)
            {
                outPath = output.FullName;
                outPath2 = output.FullName;
            }
            else if (input != null)
            {
                outPath = Path.Combine(input.DirectoryName, Path.GetFileNameWithoutExtension(input.Name) + ".html");
                outPath2 = Path.Combine(input.DirectoryName, Path.GetFileNameWithoutExtension(input.Name) + ".txt");
            }
            else
            {
                outPath = STDOUT;
                outPath2 = STDOUT;
            }

            // Write out the HTML if required
            if (html)
            {
                Console.WriteLine($"Writing to {outPath}");
                if (outPath == STDOUT)
                {
                    Console.Write(htmlOutput);
                }
                else
                {
                    File.WriteAllText(outPath, htmlOutput);
                }
            }

            // Convert the HTML to plain text, if required
            if (text)
            {
                var textOutput = Html.ToPlainText(htmlOutput);
                Console.WriteLine($"Writing to {outPath2}");
                if (outPath2 == STDOUT)
                {
                    Console.Write(textOutput);
                }
                else
                {
                    File.WriteAllText(outPath2, textOutput);
                }
            }
        }
    }
}
