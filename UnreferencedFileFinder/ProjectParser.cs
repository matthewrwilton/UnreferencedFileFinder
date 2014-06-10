using System;
using System.Linq;
using System.Xml.Linq;

namespace UnreferencedFileFinder
{
	/// <summary>
	/// Parses a MSBuild project file (e.g. .csproj) to build a list of the files referenced by the project.
	/// For more information on the structure of the XML see http://msdn.microsoft.com/en-us/library/bcxfsh87.aspx
	/// </summary>
	public class ProjectParser
	{
		// The MSBuild project XML namespace.
		static XNamespace MSBUILD_NAMESPACE = "http://schemas.microsoft.com/developer/msbuild/2003";

		// Every item that is used in a MSBuild project must be specified as a child of an ItemGroup element.
		const string ELEMENT_ITEM_GROUP = "ItemGroup";

		// Ignore other assemblies referenced by the project. They are stored in Reference elements in an ItemGroup.
		const string ELEMENT_REFERENCE = "Reference";

		// The name of the attribute that the referenced file or wildcard is stored in.
		const string ATTRIBUTE_INCLUDE = "Include";

		/// <summary>
		/// Parses the given project file to build a ReferencedProjectFiles object containing all the files referenced by the project.
		/// </summary>
		/// <param name="projectElement">The project element from a MSBuild project.</param>
		/// <returns>A ReferencedProjectFiles containing all the files referenced by the project.</returns>
		public ReferencedProjectFiles GetReferencedFilesForProject(XElement projectElement)
		{
			ReferencedProjectFiles referencedProjectFiles = new ReferencedProjectFiles();

			AddProjectFiles(projectElement, referencedProjectFiles);

			return referencedProjectFiles;
		}

		/// <summary>
		/// This method assumes that all referenced files are children of an ItemGroup element.
		/// It finds all the project's item groups and then adds the files from each item group to the ReferencedProjectFiles object.
		/// </summary>
		/// <param name="projectElement">The project element from a MSBuild project.</param>
		/// <param name="referencedProjectFiles">The object to add all referenced files into.</param>
		private void AddProjectFiles(XElement projectElement, ReferencedProjectFiles referencedProjectFiles)
		{
			var itemGroupElements = from element in projectElement.Descendants(MSBUILD_NAMESPACE + ELEMENT_ITEM_GROUP)
									select element;

			foreach (XElement itemGroupElement in itemGroupElements)
			{
				AddProjectFilesFromItemGroup(itemGroupElement, referencedProjectFiles);
			}
		}

		/// <summary>
		/// Adds all of the files in the item group into the ReferencedProjectFiles object.
		/// </summary>
		/// <param name="itemGroupElement">An ItemGroup element from a MSBuild project.</param>
		/// <param name="referencedProjectFiles">The object to add all referenced files into.</param>
		private void AddProjectFilesFromItemGroup(XElement itemGroupElement, ReferencedProjectFiles referencedProjectFiles)
		{
			var itemElements = from element in itemGroupElement.Elements()
							   select element;

			foreach (XElement itemElement in itemElements)
			{
				if (itemElement.Name == MSBUILD_NAMESPACE + ELEMENT_REFERENCE)
				{
					// Ignore Reference elements.
					continue;
				}

				XAttribute includeAttribute = itemElement.Attribute(ATTRIBUTE_INCLUDE);
				if (includeAttribute == null)
				{
					// Include is a required attribute on an item element. See http://msdn.microsoft.com/en-us/library/ms164283.aspx
					string msg = String.Format("The '{0}' attribute was missing from the '{1}' element.", ATTRIBUTE_INCLUDE, itemElement.Name);
					throw new Exception();
				}
				string filePath = includeAttribute.Value;

				referencedProjectFiles.AddFile(filePath);
			}
		}
	}
}
