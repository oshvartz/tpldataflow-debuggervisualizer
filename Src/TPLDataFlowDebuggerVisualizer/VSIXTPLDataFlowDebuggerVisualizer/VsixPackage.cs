using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;

namespace VSIXTPLDataFlowDebuggerVisualizer
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>  
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(VsixPackage.PackageGuidString)]
    [ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.NoSolution_string)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class VsixPackage : Package
    {
        private readonly string[] VisualizerAssmNames = new[]
        {
            "TPLDataFlowDebuggerVisualizer.dll", "GraphSharp.dll", "GraphSharp.Controls.dll" , "QuickGraph.Data.dll",
            "QuickGraph.dll","QuickGraph.Graphviz.dll","QuickGraph.Serialization.dll","WPFExtensions.dll"
        };
        /// <summary>
        /// VsixPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "4d43737a-e8e5-4906-91fd-f08adecfcc7f";

        /// <summary>
        /// Initializes a new instance of the <see cref="VsixPackage"/> class.
        /// </summary>
        public VsixPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {


            try
            {
                base.Initialize();
                VisualizerAssmNames.ToList().ForEach(CopyDll);

            }
            catch (Exception ex)
            {
                // TODO: Handle exception
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void CopyDll(string fileName)
        {
            // The Visualizer dll is in the same folder than the package because its project is added as reference to this project,
            // so it is included inside the .vsix file. We only need to deploy it to the correct destination folder.
            var sourceFolderFullName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Get the destination folder for visualizers
            var shell = base.GetService(typeof(SVsShell)) as IVsShell;
            shell.GetProperty((int)__VSSPROPID2.VSSPROPID_VisualStudioDir, out var documentsFolderFullNameObject);
            var documentsFolderFullName = documentsFolderFullNameObject.ToString();
            var destinationFolderFullName = Path.Combine(documentsFolderFullName, "Visualizers");

            var sourceFileFullName = Path.Combine(sourceFolderFullName, fileName);
            var destinationFileFullName = Path.Combine(destinationFolderFullName, fileName);

            CopyFileIfNewerVersion(sourceFileFullName, destinationFileFullName);
        }

        private void CopyFileIfNewerVersion(string sourceFileFullName, string destinationFileFullName)
        {
            FileVersionInfo destinationFileVersionInfo;
            FileVersionInfo sourceFileVersionInfo;
            bool copy = false;

            if (File.Exists(destinationFileFullName))
            {
                sourceFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(sourceFileFullName);
                destinationFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(destinationFileFullName);
                if (sourceFileVersionInfo.FileMajorPart > destinationFileVersionInfo.FileMajorPart)
                {
                    copy = true;
                }
                else if (sourceFileVersionInfo.FileMajorPart == destinationFileVersionInfo.FileMajorPart
                   && sourceFileVersionInfo.FileMinorPart > destinationFileVersionInfo.FileMinorPart)
                {
                    copy = true;
                }
            }
            else
            {
                // First time
                copy = true;
            }

            if (copy)
            {
                File.Copy(sourceFileFullName, destinationFileFullName, true);
            }
        }

        #endregion
    }
}
