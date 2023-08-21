using InstallerLenelOnGuard.Entidades;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace InstallerLenelOnGuard
{

    public class Installer
    {
        public event EventHandler<showLblsMessageArgs> showLblsMessage;
        public event EventHandler<printMessageEventArgs> PrintMessage;
        public event EventHandler<enableButtonArgs> enableButton;
        public event EventHandler<encryptedPasswordArgs> setEncryptedPassword;
        public Form form;
        public printMessageEventArgs printMessageEventArgs;
        private string destination;
        private string DataSource;
        private string DatabaseName;
        private string UserName;
        private string Password;
        List<ModelPorts> Ports;
        public Installer()
        {
        }
        public Installer(string destinationPath, string dataSource, string databaseName, string username, string password, List<ModelPorts> ports)
        {
            printMessageEventArgs = new printMessageEventArgs();
            destination = destinationPath;
            DataSource = dataSource;
            DatabaseName = databaseName;
            UserName = username;
            Password = password;
            Ports = ports;
        }

        #region CONFIGURATION_VALIDATION
        public void ValidateInitConfig(string dataSource, string dBName, string dBUsername, string password, bool test, MainForm main, List<ModelPorts> ListmodelPorts)
        {
            bool connectionOk = false;

            try
            {
                string accessString = $"Data Source={dataSource}; Initial Catalog={dBName}; User ID={dBUsername}; Password={password}; ";

                using (SqlConnection cnn = new SqlConnection(accessString))
                {
                    cnn.Open();
                    cnn.Close();
                    connectionOk = true;
                }

                if (connectionOk)
                {
                    showLblsMessageArgs args = new showLblsMessageArgs
                    {
                        isError = false,
                        message = "Connection information is correct.",
                        enablesInstallation = true
                    };

                    showLblsMessage?.Invoke(main, args);
                    enableButtonArgs btnArgs = new enableButtonArgs
                    {
                        button = "connection"
                    };
                    enableButton?.Invoke(this, btnArgs);
                    Log.GetInstance().DoLog("SQL connection information is correct.", "Shared configuration", null);

                    if (!test) //encrypt password and if this is successfull then change view
                    {
                        EncryptPassword(password);
                    }
                }
            }
            catch (Exception ex)
            {
                showLblsMessageArgs args = new showLblsMessageArgs
                {
                    isError = true,
                    message = "Connection information is incorrect",
                    enablesInstallation = false
                };

                showLblsMessage?.Invoke(this, args);
                Log.GetInstance().DoLog($"Exception while checking SQL connection for the input provided. Exception: {ex.Message}", "Shared configuration", null);
                enableButton?.Invoke(this, new enableButtonArgs { button = "connection" });
                if (!test)
                    enableButton?.Invoke(this, new enableButtonArgs { button = "install" });
            }
        }
        #endregion CONFIGURATION_VALIDATION

        #region PASSWORD_ENCRYPTION
        public void EncryptPassword(string password)
        {
            showLblsMessageArgs args = new showLblsMessageArgs
            {
                isError = false,
                message = "Encrypting password.",
                enablesInstallation = false
            };

            showLblsMessage?.Invoke(this, args);
            Log.GetInstance().DoLog("Encrypting password value.", "Shared configuration", null);

            try
            {
                string encryptedPassword = Crypto.Encript(password);

                args.message = "Password encrypted.";
                showLblsMessage?.Invoke(this, args);

                Log.GetInstance().DoLog("Database user password encrypted successfully.", "Shared configuration", null);
                setEncryptedPassword(this, new encryptedPasswordArgs { encryptedPassword = encryptedPassword });

                Thread.Sleep(1500); //wait a second so the text is readable                        

                form.Invoke(((MainForm)form).changeViewDelegate);
                //changeView?.Invoke(this, new changeViewArgs { encryptedPassword = encryptedPassword });
            }
            catch (Exception ex)
            {
                args.isError = true;
                args.message = "Error encrypting password, please check log for details.";
                showLblsMessage?.Invoke(this, args);

                Log.GetInstance().DoLog("Exception while encrypting password ", "Shared configuration", ex);
            }
        }
        #endregion PASSWORD_ENCRYPTION

        #region API_INSTALLATION
        public static bool CreateAppPool(string module, string poolname, bool enable32bitOn64, ManagedPipelineMode mode, string runtimeVersion)
        {
            Log.GetInstance().DoLog("Will attempt to create a new application pool for this module", module, null);
            bool res = false;
            try
            {
                using (ServerManager serverManager = new ServerManager())
                {
                    if (!serverManager.ApplicationPools.Any(appPool => appPool.Name == poolname))
                    {
                        ApplicationPool newPool = serverManager.ApplicationPools.Add(poolname);
                        newPool.ManagedRuntimeVersion = runtimeVersion;
                        newPool.Enable32BitAppOnWin64 = true;
                        newPool.ManagedPipelineMode = mode;
                        serverManager.CommitChanges();
                        Log.GetInstance().DoLog($"Application pool {poolname} created successfully", module, null);
                    }
                    else
                    {
                        Log.GetInstance().DoLog($"Application pool {poolname} was not created because it already exists", module, null);
                    }
                }
                res = true;
            }
            catch (Exception ex)
            {
                Log.GetInstance().DoLog("Failed creating application pool for this module", module, ex);
                res = false;
            }
            return res;
        }

        public static bool CreateIISAPI(string module, string websiteName, string hostname, string phyPath, string appPool, string port)
        {
            try
            {
                Log.GetInstance().DoLog("Will attempt to create IIS website", module);
                ServerManager iisManager = new ServerManager();

                string domainName = module;
                if (!IIsWebsiteExists(domainName))
                {
                    iisManager.Sites.Add(domainName, "http", $"*:{port}:", phyPath);
                    iisManager.ApplicationDefaults.ApplicationPoolName = appPool;
                    iisManager.CommitChanges();
                }
                else
                {
                    Log.GetInstance().DoLog("IIS website already exists", module);
                    bool deleted = Installer.DeleteIISAPI(module);
                    if (!deleted) //Could not remove the previous installation to install the new one
                    {
                        return false;
                    }
                    else
                    {
                        Log.GetInstance().DoLog("After old website removal will attempt to create new IIS website", module);
                        iisManager.Sites.Add(domainName, "http", $"*:{port}:", phyPath);
                        iisManager.ApplicationDefaults.ApplicationPoolName = appPool;
                        iisManager.CommitChanges();
                    }

                }

                Log.GetInstance().DoLog("IIS website created successfully for this module", module);
                return true;
            }
            catch (Exception ex)
            {
                Log.GetInstance().DoLog("Failed creating IIS website for this module", module, ex);
                return false;
            }
        }
        public static bool DeleteIISAPI(string module)
        {
            try
            {
                Log.GetInstance().DoLog("Will attempt to remove IIS website of this module", module);
                ServerManager iisManager = new ServerManager();

                if (IIsWebsiteExists(module))
                {
                    Microsoft.Web.Administration.Site siteToRemove = iisManager.Sites[module];
                    iisManager.Sites.Remove(siteToRemove);
                    iisManager.CommitChanges();
                    Log.GetInstance().DoLog("Old IIS website removed successfully for this module", module);
                }
                else
                {
                    //this log should never be present since we only call this method if the site exists
                    Log.GetInstance().DoLog("IIS website to remove was not found", module);
                    //will return true to continue with the installation since the site no longer exists anyways
                }

                return true;

            }
            catch (Exception ex)
            {
                Log.GetInstance().DoLog("Failed removing old IIS website for this module", module, ex);
                return false;
            }
        }

        public static bool IIsWebsiteExists(string strWebsitename)
        {
            ServerManager serverMgr = new ServerManager();
            Boolean flagset = false;
            SiteCollection sitecollection = serverMgr.Sites;
            flagset = sitecollection.Any(x => x.Name == strWebsitename);
            return flagset;
        }
        public void InstallAPI(string moduleName, string port)
        {
            //printMessageEventArgs args = new printMessageEventArgs();
            ChargePrintMessageArgs("Starting installation ", moduleName);
            //FILES EXTRACTION
            try
            {
                Log.GetInstance().DoLog("Instalation started, will insert the binaries on the selected folder", moduleName);
                string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string apiPath = System.IO.Path.Combine(sCurrentDirectory, $@"{moduleName}.zip");
                if (Directory.Exists($"{destination}\\{moduleName}"))
                {
                    Log.GetInstance().DoLog("Folder for this module already exists in the directory", moduleName);
                    if (Installer.IIsWebsiteExists(moduleName))
                    {
                        Log.GetInstance().DoLog("IIS website already exists for the old folder", moduleName);
                        bool deleted = Installer.DeleteIISAPI(moduleName);
                        if (!deleted) //Could not remove the previous installation to install the new one
                        {
                            //PrintMessage("Instalation failed, check log for details", moduleName);
                            ChargePrintMessageArgs("Instalation failed, check log for details ", moduleName);
                            return;
                        }
                    }
                    try
                    {
                        Log.GetInstance().DoLog("Will attempt to remove old installation folder", moduleName);

                        DirectoryInfo dirInfo = new DirectoryInfo($"{destination}\\{moduleName}");

                        foreach (FileInfo file in dirInfo.EnumerateFiles())
                        {
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in dirInfo.EnumerateDirectories())
                        {
                            dir.Delete(true);
                        }

                        Directory.Delete($"{destination}\\{moduleName}");

                        Log.GetInstance().DoLog("Successfully removed old installation folder", moduleName);
                    }
                    catch (Exception exx)
                    {
                        Log.GetInstance().DoLog("Failed removing old installation folder", moduleName, exx);
                        throw exx;
                    }
                }

                //Extract the new binaries into a new folder
                ZipFile.ExtractToDirectory(apiPath, destination);
                Log.GetInstance().DoLog("Finished inserting the binaries into selected folder", moduleName);
            }
            catch (Exception ex)
            {
                Log.GetInstance().DoLog($"Installation of {moduleName} failed, error extracting to directory. Error message: {ex.Message}", moduleName);
                //PrintMessage("Instalation failed, check log for details", moduleName);
                ChargePrintMessageArgs("Instalation failed, check log for details ", moduleName);
                return;
            }

            //Load configuration for the web or the api
            if (!moduleName.Equals("AMS AMSWeb"))
            {
                //InitConfig file
                //Update configuration files with database connection
                string path = $"{destination}\\{moduleName}" + "\\" + @"InitConfig.xml";
                LoadConfiguration(path);
            }
            else
            {
                try
                {
                    string path = $"{destination}\\{moduleName}";
                    var applicationSettings = ConfigurationManager.GetSection("ApplicationSettings") as NameValueCollection;
                    //Create a processs that executes a command prompt to install the service
                    string currentDir = Environment.CurrentDirectory;
                    Environment.CurrentDirectory = path;
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    string command = "/c npm install";
                    startInfo.Arguments = command;
                    process.StartInfo = startInfo;
                    process.Start();
                    Environment.CurrentDirectory = currentDir;
                    process.WaitForExit();
                    process.Close();
                    ChangeServerInfoInConfigJs(path, "localhost");
                  //  ChangeServerInfoInConfigXML(path, "localhost");

                }
                catch (Exception ex)
                {
                    Log.GetInstance().DoLog("Failed installing node dependecnies into the folder", moduleName, ex);
                }
            }
            Log.GetInstance().DoLog("Finished loading data in the configuration file ", moduleName);

            //INSTALLATION
            bool appPoolCreated = CreateAppPool(moduleName, moduleName, false, Microsoft.Web.Administration.ManagedPipelineMode.Integrated, "v4.0");
            if (!appPoolCreated)
            {
                //PrintMessage("Instalation failed, check log for details", moduleName);
                ChargePrintMessageArgs("Instalation failed, check log for details ", moduleName);
                return;
            }

            bool apiCreated = CreateIISAPI(moduleName, moduleName, "", $"{destination}\\{moduleName}", moduleName, port);

            if (apiCreated)
            {
                //PrintMessage("Finished installation sucessfully", moduleName);
                ChargePrintMessageArgs("Finished installation sucessfully ", moduleName);
            }
            else
            {
                //PrintMessage("Instalation failed, check log for details", moduleName);
                ChargePrintMessageArgs("Instalation failed, check log for details ", moduleName);
            }

        }

        //Update configuration files with database connection
        public void LoadConfiguration(string archivoConfiguracion)
        {
            if (!File.Exists(archivoConfiguracion))
            {
                File.Delete(archivoConfiguracion);
            }

            XmlDocument xdoc = new XmlDocument();
            XmlElement elementRoot = xdoc.CreateElement("ConfigParameters");
            xdoc.AppendChild(elementRoot);


            //------------------------DataSource
            XmlElement elementDataSource = xdoc.CreateElement("ConfigParameter");
            XmlAttribute attributeDataSource = xdoc.CreateAttribute("id");
            attributeDataSource.Value = "DataSource";
            elementDataSource.Attributes.Append(attributeDataSource);
            XmlAttribute valueAttributeDataSource = xdoc.CreateAttribute("value");
            valueAttributeDataSource.Value = this.DataSource.ToString();
            elementDataSource.Attributes.Append(valueAttributeDataSource);
            elementRoot.AppendChild(elementDataSource);

            //------------------------DatabaseName
            XmlElement elementDatabaseName = xdoc.CreateElement("ConfigParameter");
            XmlAttribute attributeDatabaseName = xdoc.CreateAttribute("id");
            attributeDatabaseName.Value = "DatabaseName";
            elementDatabaseName.Attributes.Append(attributeDatabaseName);
            XmlAttribute valueAttributeDatabaseName = xdoc.CreateAttribute("value");
            valueAttributeDatabaseName.Value = DatabaseName;
            elementDatabaseName.Attributes.Append(valueAttributeDatabaseName);
            elementRoot.AppendChild(elementDatabaseName);

            //------------------------UserName
            XmlElement elementUserName = xdoc.CreateElement("ConfigParameter");
            XmlAttribute attributeUserName = xdoc.CreateAttribute("id");
            attributeUserName.Value = "UserName";
            elementUserName.Attributes.Append(attributeUserName);
            XmlAttribute valueAttributeUserName = xdoc.CreateAttribute("value");
            valueAttributeUserName.Value = UserName;
            elementUserName.Attributes.Append(valueAttributeUserName);
            elementRoot.AppendChild(elementUserName);

            //------------------------Password
            XmlElement elementPassword = xdoc.CreateElement("ConfigParameter");
            XmlAttribute attributePassword = xdoc.CreateAttribute("id");
            attributePassword.Value = "Password";
            elementPassword.Attributes.Append(attributePassword);
            XmlAttribute valueAttributePassword = xdoc.CreateAttribute("value");
            valueAttributePassword.Value = Password;
            elementPassword.Attributes.Append(valueAttributePassword);
            elementRoot.AppendChild(elementPassword);

            xdoc.Save(archivoConfiguracion);
        }

        /// <summary>
        /// Change server info in file config.js
        /// </summary>
        /// <param name="path"></param>
        /// <param name="serverDNS"></param>
        public void ChangeServerInfoInConfigJs(string path, string serverDNS)
        {
            try
            {
                // Javascript path
                path += "\\config\\config.js";

                // Read the content of the file
                string jsContent = File.ReadAllText(path);
                // Find and replace the values 
                string oldServerIP = "ServerIP: \"10.12.10.40\"";
                string newServerIP = $"ServerIP: \"{serverDNS}\"";
                string oldServerPort = "ServerPort: 12000";
                //string newServerPort = $"ServerPort: {Ports.AMSWeb}";
                string newServerPort = $"ServerPort: process.env.PORT";

                // Change the content value
                string newJsContent = jsContent.Replace(oldServerIP, newServerIP).Replace(oldServerPort, newServerPort);

                // Save the changes in the file
                File.WriteAllText(path, newJsContent);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ///// <summary>
        ///// Change values from config XML file
        ///// </summary>
        ///// <param name="path"></param>
        ///// <param name="serverDNS"></param>
        //public void ChangeServerInfoInConfigXML(string path, string serverDNS)
        //{
        //    try
        //    {
        //        path += "\\client\\build\\AMSuiteConfig.xml"; // get the path of the file (.xml)

        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(path); // load the .xml file

        //        XmlNodeList parameterNodes = doc.SelectNodes("/ConfigParameters/ConfigParameter");
        //        if (parameterNodes != null)
        //        {
        //            foreach (XmlNode parameterNode in parameterNodes)
        //            {
        //                XmlAttribute idAttribute = parameterNode.Attributes["id"];
        //                XmlAttribute valueAttribute = parameterNode.Attributes["value"];
        //                if (idAttribute != null && valueAttribute != null)
        //                {
        //                    if (idAttribute.Value == "AMSuite")
        //                    {
        //                        valueAttribute.Value = $"http://{serverDNS}:  {Ports.AMSWeb}/api";
        //                    }
        //                    else if (idAttribute.Value == "AMSuiteAPI")
        //                    {
        //                        valueAttribute.Value = $"http://{serverDNS}:{Ports.AMSAPI}/api";
        //                    }
        //                    else if (idAttribute.Value == "WebSocket")
        //                    {
        //                        valueAttribute.Value = $"ws://{serverDNS}:{Ports.MobileAPI}";
        //                    }

        //                }
        //            }
        //            doc.Save(path);

        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        #endregion API_INSTALLATION


        #region SERVICE_INSTALLATION
        /// <summary>
        /// Creates new Windows Service
        /// </summary>
        /// <param Name of the service module="module"></param>
        /// <param Name of the executable file ="executableName"></param>
        /// <param Destination folder="folderName"></param>
        /// <returns></returns>
        public bool CreateService(string executableName, string folderName, bool directoryExists, string servicePath)
        {
            bool res = false;
            try
            {
                //Create a processs that executes a command prompt to install the service
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                ServiceController service = new ServiceController(executableName);
                bool serviceExists = false;
                foreach (ServiceController sc in ServiceController.GetServices())
                {
                    if (sc.ServiceName == executableName)
                    {
                        //service is found
                        serviceExists = true;
                        break;
                    }
                }

                if (serviceExists)
                {
                    Log.GetInstance().DoLog("The service already exists, will be deleted", executableName);
                    //If the service is in progress to start
                    if (service.Status.Equals(ServiceControllerStatus.StartPending) || service.Status.Equals(ServiceControllerStatus.ContinuePending))
                    {
                        Log.GetInstance().DoLog("The service is in progress to be running", executableName);
                        service.WaitForStatus(ServiceControllerStatus.Running);
                    }
                    //If the service is in progress to pause
                    if (service.Status.Equals(ServiceControllerStatus.PausePending))
                    {
                        Log.GetInstance().DoLog("The service is in progress to be paused", executableName);
                        service.WaitForStatus(ServiceControllerStatus.Paused);
                    }
                    //If the service is in progress to be stopped
                    if (service.Status.Equals(ServiceControllerStatus.StopPending))
                    {
                        Log.GetInstance().DoLog("The service is in progress to be stopped", executableName);
                        service.WaitForStatus(ServiceControllerStatus.Stopped);
                    }
                    //If the service is in progress to be stopped
                    if (!service.Status.Equals(ServiceControllerStatus.Stopped))
                    {
                        //stop the process
                        Log.GetInstance().DoLog("The service is in progress to be stopped", executableName);
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped);
                    }
                    //delete the process
                    Log.GetInstance().DoLog("Delete the service", executableName);
                    startInfo.Arguments = $"/C sc delete {executableName}";
                    process.StartInfo = startInfo;
                    process.Start();
                    process.Close();
                    Thread.Sleep(5000);
                }
                if (directoryExists)
                {
                    DirectoryInfo dir = new DirectoryInfo(folderName);
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        file.Delete();
                    }
                }
                //extratc the files ina folder in the selected path
                ZipFile.ExtractToDirectory(servicePath, folderName);
                //The name of the service (in this case the executableName variable) couldn't have spaces
                string binPath = "/C sc create " + executableName + " binpath=\"" + folderName + "\"\\" + executableName + ".exe start=auto";
                startInfo.Arguments = binPath;
                process.StartInfo = startInfo;
                process.Start();
                process.Close();
                Thread.Sleep(5000);
                //search the process to validate if it was created
                foreach (ServiceController sc in ServiceController.GetServices())
                {
                    if (sc.ServiceName == executableName)
                    {
                        Log.GetInstance().DoLog("Service created, starting the process", executableName);
                        //service is found, then start it
                        service.Start();
                        res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.GetInstance().DoLog("Service creation failed. ", executableName, ex);
                res = false;
            }
            return res;
        }

        public bool InstallSc(string module, string executableName, string folderName)
        {
            enableButtonArgs btnArgs = new enableButtonArgs
            {
                button = "close"
            };
            try
            {
                bool directoryExists = true;
                //PrintMessage("Starting installation", module);
                ChargePrintMessageArgs("Starting installation ", module);
                Log.GetInstance().DoLog("Starting installation ", module);
                Log.GetInstance().DoLog("Will install the binaries on the selected folder ", module);
                string pathToUnzip = folderName + "\\" + module;
                if (!Directory.Exists(pathToUnzip))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(pathToUnzip);
                    directoryExists = false;
                }
                string servicePath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, $@"{module}.zip");
                Log.GetInstance().DoLog("Finished inserting the binaries into selected folder ", folderName);

                //InitConfig file
                //Update configuration files with database connection
                string path = pathToUnzip + "\\" + @"InitConfig.xml";
                LoadConfiguration(path);
                Log.GetInstance().DoLog("Finished loading data in the configuration file ", path);

                bool serviceCreated = CreateService(executableName, pathToUnzip, directoryExists, servicePath);
                 if (serviceCreated)
                {
                    //PrintMessage("Finished installation sucessfully", module);
                    ChargePrintMessageArgs("Finished installation sucessfully ", module);
                }
                else
                {
                    //PrintMessage("Installation failed, check log for details", module);
                    ChargePrintMessageArgs("Installation failed, check log for details", module);
                }
                //btnClose.Enabled = true;
                enableButton?.Invoke(this, btnArgs);
                Log.GetInstance().DoLog("Installation finished", module);
            }
            catch (Exception ex)
            {
                //btnClose.Enabled = true;
                enableButton?.Invoke(this, btnArgs);
                Log.GetInstance().DoLog("Exception installing service: " + ex.ToString(), module);
            }
            return true;
        }
        #endregion

        protected virtual void OnPrintMessage(printMessageEventArgs e)
        {
            EventHandler<printMessageEventArgs> handler = PrintMessage;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void ChargePrintMessageArgs(string message, string module)
        {
            printMessageEventArgs.message = message;
            printMessageEventArgs.module = module;
            OnPrintMessage(printMessageEventArgs);
        }
    }
}
