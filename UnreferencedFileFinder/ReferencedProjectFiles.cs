using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnreferencedFileFinder
{
	/// <summary>
	/// Small class used to store the files referenced by a project. To avoid sorting through a long list of
	/// files to find if a file is referenced, the files are grouped together into separate lists per directory.
	/// </summary>
    public class ReferencedProjectFiles : IEnumerable<string>
    {
		public ReferencedProjectFiles()
		{
			ProjectFiles = new Dictionary<string, List<string>>();
		}

		/// <summary>
		/// A collection of all the files referenced by the project.
		/// The files are grouped into lists per directory.
		/// The key is the directory name.
		/// The value is the list of files referenced in that directory.
		/// </summary>
		private Dictionary<string, List<string>> ProjectFiles
		{
			get;
			set;
		}

		/// <summary>
		/// Adds the file into the project files collection.
		/// </summary>
		/// <param name="filePath">The full file path of the file. Can be a relative file path e.g. "..\Test.cs"</param>
		public void AddFile(string filePath)
		{
			string directory = Path.GetDirectoryName(filePath);
			string fileName = Path.GetFileName(filePath);

			if (!ProjectFiles.ContainsKey(directory))
			{
				ProjectFiles.Add(directory, new List<string>());
			}

			ProjectFiles[directory].Add(fileName);
		}

		/// <summary>
		/// Checks if the given file is referenced by the project.
		/// </summary>
		/// <param name="filePath">The full file path of the file. Can be a relative file path e.g. "..\Test.cs"</param>
		/// <returns>True if the given file is referenced by the project.</returns>
		public bool IsFileReferenced(string filePath)
		{
			string directory = Path.GetDirectoryName(filePath);
			string fileName = Path.GetFileName(filePath);
			
			return ProjectFiles.ContainsKey(directory) && ProjectFiles[directory].Contains(fileName);
		}

		/// <summary>
		/// Allows foreach iteration of the files referenced by the project.
		/// </summary>
		/// <returns>An enumerator that will iterate over the referenced files.</returns>
		public IEnumerator<string> GetEnumerator()
		{
			List<string> referencedFiles = new List<string>();
			foreach (string directory in ProjectFiles.Keys)
			{
				var directoryFiles = from file in ProjectFiles[directory]
									 select Path.Combine(directory, file);
				referencedFiles.AddRange(directoryFiles);
			}

			return referencedFiles.GetEnumerator();
		}

		/// <summary>
		/// This method is required to implement System.Collections.Generic.IEnumerable
		/// because that class inherits System.Collections.IEnumerable which requires this.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
    }
}
