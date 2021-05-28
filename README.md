## Description

A simple console application written in .NET Core that can convert Rich-Text (.rtf) files to HTML or plain text.

## Usage

    ConvertRTF
      Converts a rich-text file (.rtf) to HTML or plain text

    Usage:
      ConvertRTF [options]

    Options:
      --input <input>    The path to the .rtf file to be converted.
      --text             If plain text is the desired output
      --html             If HTML is the desired output (the default) [default: True]
      --output <output>  The path to the output file (defaults to input file with different extension) [default: ]
      --version          Show version information
      -?, -h, --help     Show help and usage information`

  ## Information

  Written in C# (.NET 5.0). Visual Studio 16.8+ is required, or Visual Studio Code. 
  Uses the RTFPipe and BracketPipe NuGet packages. 

  ## Publishing Linux executable

  A Linux executable can be published with the following command:

  `dotnet publish -c release -r linux-x64 --self-contained`

  See [this link](https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli) for more details.
