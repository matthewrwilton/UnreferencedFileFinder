using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnreferencedFileFinder
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 1)
			{
				Console.WriteLine("Usage: UnreferencedFileFinder.exe projectFile");
				Console.WriteLine();
				Console.WriteLine(@"  e.g. UnreferencedFileFinder.exe C:\repos\UnreferencedFileFinder\UnreferencedFileFinder\UnreferencedFileFinder.csproj");
				Console.WriteLine();
				return;
			}

			string projectPath = args[0];

			try
			{
				UnreferencedFilesFinder unreferencedFilesFinder = new UnreferencedFilesFinder(projectPath);
				List<string> unreferencedFiles = unreferencedFilesFinder.FindUnreferencedProjectFiles();

				foreach (string file in unreferencedFiles)
				{
					Console.WriteLine(file);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Error: " + e);
			}
		}
	}
}
