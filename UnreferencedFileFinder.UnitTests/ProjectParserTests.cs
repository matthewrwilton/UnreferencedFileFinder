using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace UnreferencedFileFinder.UnitTests
{
	/// <summary>
	/// Unit tests for the ProjectDirectoryCleaner.FileParsing.ProjectParser class.
	/// </summary>
	public class ProjectParserTests
	{
		// The MSBuild project XML namespace.
		static XNamespace MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";

		/// <summary>
		/// Assert that the GetReferencedFilesForProject method returns a ReferencedProjectFiles object containing
		/// all the referenced files when parsing a project with a single item group.
		/// </summary>
		[Fact]
		public void ProjectParser_GetReferencedFilesForProject_ParsesAllFilesFromASingleItemGroup()
		{
			string referencedFile1 = @"Controllers\Controller.cs";
			string referencedFile2 = @"Models\Model.cs";
			string referencedFile3 = @"Views\View.cshtml";
			string referencedFile4 = @"Web.config";

			string projectXml = String.Format(
				@"<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
					<ItemGroup>
						<Item Include=""{0}"" />
						<Item Include=""{1}"" />
						<Item Include=""{2}"" />
						<Item Include=""{3}"" />
					</ItemGroup>
				</Project>", referencedFile1, referencedFile2, referencedFile3, referencedFile4);

			XElement projectElement = XElement.Parse(projectXml);
			
			ProjectParser projectParser = new ProjectParser();
			ReferencedProjectFiles referencedProjectFiles = projectParser.GetReferencedFilesForProject(projectElement);

			Assert.True(referencedProjectFiles.IsFileReferenced(referencedFile1));
			Assert.True(referencedProjectFiles.IsFileReferenced(referencedFile2));
			Assert.True(referencedProjectFiles.IsFileReferenced(referencedFile3));
			Assert.True(referencedProjectFiles.IsFileReferenced(referencedFile4));
		}

		/// <summary>
		/// Assert that the GetReferencedFilesForProject method returns a ReferencedProjectFiles object containing
		/// all the referenced files when parsing a project with a multiple item groups.
		/// </summary>
		[Fact]
		public void ProjectParser_GetReferencedFilesForProject_ParsesAllFilesFromMultipleItemGroups()
		{
			string referencedFile1 = @"Controllers\Controller.cs";
			string referencedFile2 = @"Models\Model.cs";
			string referencedFile3 = @"Views\View.cshtml";
			string referencedFile4 = @"Web.config";

			string projectXml = String.Format(
				@"<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
					<ItemGroup>
						<Item Include=""{0}"" />
						<Item Include=""{1}"" />
					</ItemGroup>
					<ItemGroup>
						<Item Include=""{2}"" />
						<Item Include=""{3}"" />
					</ItemGroup>
				</Project>", referencedFile1, referencedFile2, referencedFile3, referencedFile4);

			XElement projectElement = XElement.Parse(projectXml);

			ProjectParser projectParser = new ProjectParser();
			ReferencedProjectFiles referencedProjectFiles = projectParser.GetReferencedFilesForProject(projectElement);

			Assert.True(referencedProjectFiles.IsFileReferenced(referencedFile1));
			Assert.True(referencedProjectFiles.IsFileReferenced(referencedFile2));
			Assert.True(referencedProjectFiles.IsFileReferenced(referencedFile3));
			Assert.True(referencedProjectFiles.IsFileReferenced(referencedFile4));
		}

		/// <summary>
		/// Assert that the GetReferencedFilesForProject method does not return any assembly references in the ReferencedProjectFiles object.
		/// </summary>
		[Fact]
		public void ProjectParser_GetReferencedFilesForProject_IgnoresReferencedAssemblies()
		{
			string nonReferencedFile1 = @"Microsoft.CSharp";
			string nonReferencedFile2 = @"System.Data";
			string referencedFile1 = @"Controllers\Controller.cs";
			string referencedFile2 = @"Web.config";

			string projectXml = String.Format(
				@"<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
					<ItemGroup>
						<Reference Include=""{0}"" />
						<Reference Include=""{1}"" />
					</ItemGroup>
					<ItemGroup>
						<Item Include=""{2}"" />
						<Item Include=""{3}"" />
					</ItemGroup>
				</Project>", nonReferencedFile1, nonReferencedFile2, referencedFile1, referencedFile2);

			XElement projectElement = XElement.Parse(projectXml);

			ProjectParser projectParser = new ProjectParser();
			ReferencedProjectFiles referencedProjectFiles = projectParser.GetReferencedFilesForProject(projectElement);

			Assert.False(referencedProjectFiles.IsFileReferenced(nonReferencedFile1));
			Assert.False(referencedProjectFiles.IsFileReferenced(nonReferencedFile2));

			Assert.True(referencedProjectFiles.IsFileReferenced(referencedFile1));
			Assert.True(referencedProjectFiles.IsFileReferenced(referencedFile2));
		}
	}
}
