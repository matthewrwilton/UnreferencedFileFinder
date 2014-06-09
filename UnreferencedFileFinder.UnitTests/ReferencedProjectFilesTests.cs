using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnreferencedFileFinder.UnitTests
{
	/// <summary>
	/// Unit tests for the ProjectDirectoryCleaner.ReferencedProjectFiles class.
	/// </summary>
    public class ReferencedProjectFilesTests
	{
		/// <summary>
		/// Assert that a new ReferencedProjectFiles object should not reference any files.
		/// </summary>
		[Fact]
		public void ReferencedProjectFiles_Constructor_DoesNotReferenceAnyFiles()
		{
			string filePath = @"c:\temp\Test.cs";

			ReferencedProjectFiles referencedProjectFiles = new ReferencedProjectFiles();
			Assert.False(referencedProjectFiles.IsFileReferenced(filePath));
		}

		/// <summary>
		/// Assert that after adding a file to a project, the project correctly returns true that it references the file.
		/// </summary>
		[Fact]
		public void ReferencedProjectFiles_AddFile_ReferencesFile()
		{
			string filePath = @"c:\temp\Test.cs";

			ReferencedProjectFiles referencedProjectFiles = new ReferencedProjectFiles();
			referencedProjectFiles.AddFile(filePath);

			Assert.True(referencedProjectFiles.IsFileReferenced(filePath));
		}

		/// <summary>
		/// Assert that a file is correctly not referenced, even though other files in the same directory are referenced.
		/// </summary>
		[Fact]
		public void ReferencedProjectFiles_IsFileReferenced_ReturnsFalseForSameDirectoryDifferentFile()
		{
			string referencedFilePath = @"c:\temp\Test.cs";
			string nonReferencedFilePath = @"c:\temp\Test2.cs";

			ReferencedProjectFiles referencedProjectFiles = new ReferencedProjectFiles();
			referencedProjectFiles.AddFile(referencedFilePath);

			Assert.False(referencedProjectFiles.IsFileReferenced(nonReferencedFilePath));
		}

		/// <summary>
		/// Assert that a file is correctly not referenced, even though another file with the same name in another directory is referenced.
		/// </summary>
		[Fact]
		public void ReferencedProjectFiles_IsFileReferenced_ReturnsFalseForSameFileDifferentDirectory()
		{
			string referencedFilePath = @"c:\temp\Test.cs";
			string nonReferencedFilePath = @"c:\temp2\Test.cs";

			ReferencedProjectFiles referencedProjectFiles = new ReferencedProjectFiles();
			referencedProjectFiles.AddFile(referencedFilePath);

			Assert.False(referencedProjectFiles.IsFileReferenced(nonReferencedFilePath));
		}

		/// <summary>
		/// Assert that the enumerator returned by GetEnumerator() iterates through all the referenced files and includes each file only once.
		/// </summary>
		[Fact]
		public void ReferencedProjectFiles_GetEnumerator_IncludesAllReferencedFilesOnlyOnce()
		{
			List<string> referencedFiles = new List<string>
			{
				@"c:\temp\Test.cs",
				@"c:\temp\Test2.cs",
				@"c:\temp2\Test.cs",

			};

			ReferencedProjectFiles referencedProjectFiles = new ReferencedProjectFiles();
			foreach (string file in referencedFiles)
			{
				referencedProjectFiles.AddFile(file);
			}

			foreach (string referencedFile in referencedProjectFiles)
			{
				referencedFiles.Remove(referencedFile);
			}

			Assert.Equal(0, referencedFiles.Count);
		}
    }
}
