using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace UnreferencedFileFinder.UnitTests
{
	public class UnreferencedFilesFinderTests
	{
		[Fact]
		public void UnreferencedFilesFinder_FindUnreferencedProjectFiles_IgnoresProjectFile()
		{
			string projectDirectory = @"C:\temp\UnreferencedFilesFinderTests";
			string projectFile = Path.Combine(projectDirectory, "test.csproj");

			string projectXml = @"<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003""></Project>";

			try
			{
				// Create directories and files for testing.
				Directory.CreateDirectory(projectDirectory);
				File.WriteAllText(projectFile, projectXml);

				UnreferencedFilesFinder unreferencedFilesFinder = new UnreferencedFilesFinder(projectFile);
				List<string> unreferencedFiles = unreferencedFilesFinder.FindUnreferencedProjectFiles();

				Assert.DoesNotContain<string>(projectFile, unreferencedFiles);
			}
			finally
			{
				if (Directory.Exists(projectDirectory))
				{
					Directory.Delete(projectDirectory, true);
				}
			}
		}

		[Fact]
		public void UnreferencedFilesFinder_FindUnreferencedProjectFiles_IgnoresFilesInTheBinDirectory()
		{
			string projectDirectory = @"C:\temp\UnreferencedFilesFinderTests";
			string projectFile = Path.Combine(projectDirectory, "test.csproj");
			string binDirectory = Path.Combine(projectDirectory, "bin");
			string ignoredFile = Path.Combine(binDirectory, "test.txt");

			string projectXml = @"<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003""></Project>";

			try
			{
				// Create directories and files for testing.
				Directory.CreateDirectory(projectDirectory);
				Directory.CreateDirectory(binDirectory);
				File.WriteAllText(ignoredFile, "Test");
				File.WriteAllText(projectFile, projectXml);
				
				UnreferencedFilesFinder unreferencedFilesFinder = new UnreferencedFilesFinder(projectFile);
				List<string> unreferencedFiles = unreferencedFilesFinder.FindUnreferencedProjectFiles();

				Assert.DoesNotContain<string>(ignoredFile, unreferencedFiles);
			}
			finally
			{
				if (Directory.Exists(projectDirectory))
				{
					Directory.Delete(projectDirectory, true);
				}
			}
		}

		[Fact]
		public void UnreferencedFilesFinder_FindUnreferencedProjectFiles_IgnoresFilesInTheObjDirectory()
		{
			string projectDirectory = @"C:\temp\UnreferencedFilesFinderTests";
			string projectFile = Path.Combine(projectDirectory, "test.csproj");
			string objDirectory = Path.Combine(projectDirectory, "obj");
			string ignoredFile = Path.Combine(objDirectory, "test.txt");

			string projectXml = @"<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003""></Project>";

			try
			{
				// Create directories and files for testing.
				Directory.CreateDirectory(projectDirectory);
				Directory.CreateDirectory(objDirectory);
				File.WriteAllText(ignoredFile, "Test");
				File.WriteAllText(projectFile, projectXml);

				UnreferencedFilesFinder unreferencedFilesFinder = new UnreferencedFilesFinder(projectFile);
				List<string> unreferencedFiles = unreferencedFilesFinder.FindUnreferencedProjectFiles();

				Assert.DoesNotContain<string>(ignoredFile, unreferencedFiles);
			}
			finally
			{
				if (Directory.Exists(projectDirectory))
				{
					Directory.Delete(projectDirectory, true);
				}
			}
		}

		[Fact]
		public void UnreferencedFilesFinder_FindUnreferencedProjectFiles_IgnoresFilesInThePropertiesDirectory()
		{
			string projectDirectory = @"C:\temp\UnreferencedFilesFinderTests";
			string projectFile = Path.Combine(projectDirectory, "test.csproj");
			string propertiesDirectory = Path.Combine(projectDirectory, "Properties");
			string ignoredFile = Path.Combine(propertiesDirectory, "test.txt");

			string projectXml = @"<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003""></Project>";

			try
			{
				// Create directories and files for testing.
				Directory.CreateDirectory(projectDirectory);
				Directory.CreateDirectory(propertiesDirectory);
				File.WriteAllText(ignoredFile, "Test");
				File.WriteAllText(projectFile, projectXml);

				UnreferencedFilesFinder unreferencedFilesFinder = new UnreferencedFilesFinder(projectFile);
				List<string> unreferencedFiles = unreferencedFilesFinder.FindUnreferencedProjectFiles();

				Assert.DoesNotContain<string>(ignoredFile, unreferencedFiles);
			}
			finally
			{
				if (Directory.Exists(projectDirectory))
				{
					Directory.Delete(projectDirectory, true);
				}
			}
		}
	}
}
