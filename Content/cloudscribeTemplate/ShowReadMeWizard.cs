using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;
using System.IO;

namespace cloudscribeTemplate
{
    public class ShowReadMeWizard : IWizard
    {
        private DTE _dte;
        private string _projectDirectory = "";

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            
        }

        public void ProjectFinishedGenerating(Project project)
        {
            
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            
        }

        public void RunFinished()
        {
            var readmePath = _projectDirectory + "\\readme.html";
            
            if (File.Exists(readmePath))
            {
                // _dte.ItemOperations.Navigate(readmePath, vsNavigateOptions.vsNavigateOptionsNewWindow); // this works but browser window inside vs
                //_dte.ItemOperations.OpenFile(readmePath, Constants.vsWindowKindWebBrowser); //this works but in html raw view
                //_dte.ExecuteCommand("File.OpenFile", readmePath); // this works but in html raw view
                
                System.Diagnostics.Process.Start(readmePath); //this works and opens external web browser!



            }
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _dte = (DTE)automationObject;
            _projectDirectory = replacementsDictionary["$destinationdirectory$"];

        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
