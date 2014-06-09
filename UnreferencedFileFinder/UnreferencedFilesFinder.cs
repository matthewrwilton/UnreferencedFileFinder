using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UnreferencedFileFinder
{
	/// <summary>
	/// Finds any files in the project's directory or any of its subdirectories that are not referenced by the project.
	/// </summary>
	public class UnreferencedFilesFinder
	{
		// The following directories do not need to be checked for unreferenced files.
		private const string DIRECTORY_BIN = "bin";
		private const string DIRECTORY_OBJ = "obj";
		private const string DIRECTORY_PROPERTIES = "Properties";

		private const string FILE_EXTENSION_USER = ".user";

		/// <summary>
		/// Creates a new UnreferencedFilesFinder instance.
		/// </summary>
		/// <param name="projectFile">The MSBuild project file to find unreferenced files for.</param>
		public UnreferencedFilesFinder(string projectFile)
		{
			ProjectFile = projectFile;

			ProjectDirectory = Path.GetDirectoryName(projectFile);
			BinDirectory = Path.Combine(ProjectDirectory, DIRECTORY_BIN);
			ObjDirectory = Path.Combine(ProjectDirectory, DIRECTORY_OBJ);
			PropertiesDirectory = Path.Combine(ProjectDirectory, DIRECTORY_PROPERTIES);
		}

		/// <summary>
		/// The MSBuild project file to find unreferenced files for.
		/// </summary>
		private string ProjectFile
		{
			get;
			set;
		}

		private string ProjectDirectory
		{
			get;
			set;
		}

		private string BinDirectory
		{
			get;
			set;
		}

		private string ObjDirectory
		{
			get;
			set;
		}

		private string PropertiesDirectory
		{
			get;
			set;
		}

		public List<string> FindUnreferencedProjectFiles()
		{
			try
			{
				DirectoryInfo projectDirectory = new DirectoryInfo(Path.GetDirectoryName(ProjectFile));
				ReferencedProjectFiles referencedProjectFiles = GetReferencedProjectFiles();
				List<string> unreferencedFiles = new List<string>();

				FindUnreferencedFilesInDirectory(projectDirectory, referencedProjectFiles, unreferencedFiles);

				return unreferencedFiles;
			}
			catch (Exception e)
			{
				string msg = String.Format("Error while finding unreferenced files for the '{0}' project.", ProjectFile);
				throw new Exception(msg, e);
			}
		}

		private ReferencedProjectFiles GetReferencedProjectFiles()
		{
			XElement projectElement = XElement.Load(ProjectFile);

			ProjectParser projectParser = new ProjectParser();
			ReferencedProjectFiles referencedProjectFiles = projectParser.GetReferencedFilesForProject(projectElement);

			return referencedProjectFiles;
		}

		private void FindUnreferencedFilesInDirectory(DirectoryInfo directory, ReferencedProjectFiles referencedProjectFiles, List<string> unreferencedFiles)
		{
			try
			{
				foreach (FileInfo file in directory.GetFiles())
				{
					if (FileRequiresChecking(file.FullName))
					{
						string relativeFileName = GetFileNameRelativeToProject(file.FullName);
						if (!referencedProjectFiles.IsFileReferenced(relativeFileName))
						{
							unreferencedFiles.Add(file.FullName);
						}
					}
				}

				foreach (DirectoryInfo subDirectory in directory.GetDirectories())
				{
					if (DirectoryRequiresChecking(subDirectory.FullName))
					{
						FindUnreferencedFilesInDirectory(subDirectory, referencedProjectFiles, unreferencedFiles);
					}
				}
			}
			catch (Exception e)
			{
				string msg = String.Format("Error while finding unreferenced files in the '{0}' directory.", directory.FullName);
				throw new Exception(msg, e);
			}
		}

		private bool FileRequiresChecking(string fileName)
		{
			return !fileName.Equals(ProjectFile) &&
				   !Path.GetExtension(fileName).Equals(FILE_EXTENSION_USER);
		}

		/// <summary>
		/// Do not check the bin, obj or Properties directories for unreferenced files.
		/// </summary>
		private bool DirectoryRequiresChecking(string directoryName)
		{
			return !directoryName.Equals(BinDirectory) &&
				   !directoryName.Equals(ObjDirectory) &&
				   !directoryName.Equals(PropertiesDirectory);
		}

		private string GetFileNameRelativeToProject(string absoluteFilePath)
		{
			return absoluteFilePath.Replace(ProjectDirectory, String.Empty).TrimStart('\\');
		}
	}
}
