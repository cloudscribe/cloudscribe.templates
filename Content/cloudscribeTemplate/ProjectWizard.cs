using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using System.Windows.Forms;
using System.IO;

namespace cloudscribeTemplate
{
    public class ProjectWizard : IWizard
    {
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
            // project hasn't actually been created here because the main
            // template wizard runs after this wizard replaces values in the dictionary
            // therefore added ShowReadMeWizard after that wizard

            //if (!_exceptionOccurred)
            //{
                //var readmePath = _projectDirectory + "\\readme.html";
                //var url = "https://www.cloudscribe.com/docs";
                //var url = "file://" + readmePath.Replace("\\", "/");
                //try
                //{
                //_dte.ItemOperations.OpenFile(readmePath, Constants.vsWindowKindWebBrowser);
                // _dte.ItemOperations.Navigate(url);
                //if(File.Exists(readmePath))
                //{
                //    _dte.ItemOperations.Navigate(readmePath);
                //}
                
                //_dte.ExecuteCommand("cmd /c start " + readmePath);
                //_dte.ExecuteCommand("File.OpenFile", readmePath);
                //}
                //catch(InvalidOperationException)
                //{

                //}
            //}
        }

        private DTE _dte;
        private string _projectDirectory = "";
        private UserInputForm _inputForm;
        private string _dataStorage = "MSSQL";
        private bool _useLogging = true;
        private bool _useSimpleContent = true;
        private bool _useContactForm = false;
        private bool _useKvpProfile = false;
        private bool _useIdentityServer = false;
        private bool _exceptionOccurred = false;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _dte = (DTE)automationObject;
           

            _projectDirectory = replacementsDictionary["$destinationdirectory$"];
            //var solutionDirectory = replacementsDictionary["$solutiondirectory$"];

            try
            {
                
                _inputForm = new UserInputForm();
                _inputForm.ShowDialog();

                _dataStorage = UserInputForm.DataStorage;
                _useLogging = UserInputForm.UseLogging;
                _useSimpleContent = UserInputForm.UseSimpleContent;
                _useContactForm = UserInputForm.UseContactForm;
                _useKvpProfile = UserInputForm.UseKvpProfile;
                _useIdentityServer = UserInputForm.UseIdentityServer;

                replacementsDictionary.Add("passthrough:DataStorage", _dataStorage);
                replacementsDictionary.Add("passthrough:Logging", _useLogging.ToString().ToLowerInvariant());
                replacementsDictionary.Add("passthrough:SimpleContent", _useSimpleContent.ToString().ToLowerInvariant());
                replacementsDictionary.Add("passthrough:ContactForm", _useContactForm.ToString().ToLowerInvariant());
                replacementsDictionary.Add("passthrough:KvpCustomRegistration", _useKvpProfile.ToString().ToLowerInvariant());
                replacementsDictionary.Add("passthrough:IdentityServer", _useIdentityServer.ToString().ToLowerInvariant());
                
            }
            catch (Exception ex)
            {
                _exceptionOccurred = true;
                // Clean up the template that was written to disk
                if (Directory.Exists(_projectDirectory))
                {
                    Directory.Delete(_projectDirectory, true);
                }

                MessageBox.Show(ex.ToString());
            }

            
            



        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

    }

    public partial class UserInputForm : Form
    {
        private static string dataStorage = "MSSQL";
        public static string DataStorage
        {
            get { return dataStorage;}
            set { dataStorage = value; }
        }

        private static bool useLogging = true;
        public static bool UseLogging
        {
            get { return useLogging; }
            set { useLogging = value; }
        }

        private static bool useSimpleContent = true;
        public static bool UseSimpleContent
        {
            get { return useSimpleContent; }
            set { useSimpleContent = value; }
        }

        private static bool useContactForm = false;
        public static bool UseContactForm
        {
            get { return useContactForm; }
            set { useContactForm = value; }
        }

        private static bool useKvpProfile = false;
        public static bool UseKvpProfile
        {
            get { return useKvpProfile; }
            set { useKvpProfile = value; }
        }

        private static bool useIdentityServer = false;
        public static bool UseIdentityServer
        {
            get { return useIdentityServer; }
            set { useIdentityServer = value; }
        }


        private Button button1;

        private ComboBox cbDataStorage;
        private CheckBox chkLogging;
        private CheckBox chkSimpleContent;
        private CheckBox chkKvpProfile;
        private CheckBox chkContactForm;
        private CheckBox chkIdentityServer;

        class ComboItem
        {
            public string Key { get; set; }
            public string Text { get; set; }
        }

        public UserInputForm()
        {
            // set the form size
            this.Size = new System.Drawing.Size(600, 590);
            this.Text = "cloudscribe Options";

            cbDataStorage = new ComboBox();
            cbDataStorage.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDataStorage.DataSource = new ComboItem[] {
                new ComboItem{ Key = "MSSQL", Text = "Use Microsoft SqlServer" },
                new ComboItem{ Key = "MySql", Text = "Use MySql" },
                new ComboItem{ Key = "pgsql", Text = "Use PostgreSql" },
                new ComboItem{ Key = "NoDb", Text = "Use NoDb (no database) file system storage" }
            };
            cbDataStorage.ValueMember = "Key";
            cbDataStorage.DisplayMember = "Text";
            cbDataStorage.Location = new System.Drawing.Point(10, 20);
            cbDataStorage.Width = 550;
            this.Controls.Add(cbDataStorage);

            chkLogging = new CheckBox();
            chkLogging.Checked = useLogging;
            chkLogging.Text = "Include Logging";
            chkLogging.Location = new System.Drawing.Point(10, 80);
            chkLogging.Width = 550;
            chkLogging.Height = 50;
            this.Controls.Add(chkLogging);

            chkSimpleContent = new CheckBox();
            chkSimpleContent.Checked = useSimpleContent;
            chkSimpleContent.Text = "Include SimpleContent";
            chkSimpleContent.Location = new System.Drawing.Point(10, 140);
            chkSimpleContent.Width = 550;
            chkSimpleContent.Height = 50;
            this.Controls.Add(chkSimpleContent);

            chkContactForm = new CheckBox();
            chkContactForm.Checked = useContactForm;
            chkContactForm.Text = "Include Contact Form";
            chkContactForm.Location = new System.Drawing.Point(10, 200);
            chkContactForm.Width = 550;
            chkContactForm.Height = 50;
            this.Controls.Add(chkContactForm);

            chkKvpProfile = new CheckBox();
            chkKvpProfile.Checked = useKvpProfile;
            chkKvpProfile.Text = "Include (key/value) Custom Registration";
            chkKvpProfile.Location = new System.Drawing.Point(10, 260);;
            chkKvpProfile.Width = 550;
            chkKvpProfile.Height = 50;
            this.Controls.Add(chkKvpProfile);

            chkIdentityServer = new CheckBox();
            chkIdentityServer.Checked = useIdentityServer;
            chkIdentityServer.Text = "Include IdentityServer4 (fork) Integration";
            chkIdentityServer.Location = new System.Drawing.Point(10, 320);
            chkIdentityServer.Width = 550;
            chkIdentityServer.Height = 50;
            this.Controls.Add(chkIdentityServer);



            button1 = new Button();
            button1.Text = "Ok";
            button1.Location = new System.Drawing.Point(250, 400);
            button1.Size = new System.Drawing.Size(50, 50);
            button1.Click += button1_Click;
            this.Controls.Add(button1);

            
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            dataStorage = (string)cbDataStorage.SelectedValue;
            useLogging = chkLogging.Checked;
            useSimpleContent = chkSimpleContent.Checked;
            useContactForm = chkContactForm.Checked;
            useKvpProfile = chkKvpProfile.Checked;
            useIdentityServer = chkIdentityServer.Checked;

            this.Close();
        }
    }



}
