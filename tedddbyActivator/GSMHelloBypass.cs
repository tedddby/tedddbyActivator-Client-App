using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Claunia.PropertyList;
using iKingCode_BypassHello.Resources;
using iKingCode_Removal;
using Renci.SshNet;
using RestSharp;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using System.IO.Compression;



namespace iKingCode_BypassHello
{
    public class GSMHelloBypass
    {

        private SshClient sshClient;
        private ScpClient scpClient;
        public string host = "127.0.0.1";
        public string password = "alpine";
        public int port = 22;

        string Folder_Activa = "";
        string Folder_ArkInt = "";
        string Folder_ActRec = "";
        string Folder_LBPref = "/var/mobile/Library/Preferences/";
        string Folder_WLPref = "/var/wireless/Library/Preferences/";
        string Folder_LBLock = "/var/root/Library/Lockdown/";

        string Route_ActRec = "///activation_record.plist";
        string Route_CommCe = "///com.apple.commcenter.device_specific_nobackup.plist";
        string Route_DatArk = "///data_ark.plist";
        string Route_ArkInt = "///data_ark.plist";
        string Route_Purple = "///com.apple.purplebuddy.plist";

        public static string ToolDir = Directory.GetCurrentDirectory();
        public static string SwapPCDir = ToolDir + "\\files\\swp\\";
        public static string UDID = iOSDevice.UDID;
        public static string SwapIdevDir = @"/tmp/Backup";

        private string WildcardTicket;

        private int stage = 0;
        private int step = 0;
        public string errcarrier;

        private SuccessBox successBox;

        public void DoBypass(string activationLink, string recordsLink)
        {
            try
            {

                //======================================================================================================
                //STAGE 1 : RUN AND CONNECT SSH & CLEANING PREVIOUS ACTIVATION FILES
                stage = 1;

                p(5);

                try
                {
                    //throw new ApplicationException("Testing Error.");

                    CleanSwapFolder();
                    p(6);

                    PairLoop();
                    p(8);

                    RunSSHServer();
                    p(10);

                    Mount();
                    p(12);

                    FindActivationRoutes();
                    p(14);

                    Deactivate();
                    p(16);

                    DeleteSubstrate();
                    p(18);

                    DeleteActivationFiles();
                    p(20);

                    RestartAllDeamons();
                    p(22);

                    SnappyRename();
                    p(24);

                }
                catch (Exception ex)
                {
                    ReportErrorMessage(ex);
                    return;
                }

                //======================================================================================================
                //STAGE 2 : ACTIVATON
                stage = 2;

                try
                {
                    //SSH("cp /System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/FactoryActivation.pem /System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/RaptorActivation.pem");
                    //UploadResource(Resources.GSM.gsm_raptor, "/System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/RaptorActivation.pem"); 

                    SSH("cp /System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/FactoryActivation.pem /System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/RaptorActivation.pem");
                    SSH("chown 444 /System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/RaptorActivation.pem");
                    p(26);

                    RestartLockdown();
                    p(28);

                    CreateActivationFolders();
                    p(29);

                    PairLoop();
                    p(30);

                    bool activateResult = false;
                    int intento = 1;

                    while (activateResult == false && intento <= 5)
                    {
                        p(30 + intento); //33,34,35

                        Deactivate();
                        activateResult = Activate(activationLink);
                        intento++;
                        Thread.Sleep(1000);
                    }

                    if (activateResult == false)
                    {
                        ReportErrorMessage();
                        return;
                    }

                    p(36);
                }
                catch (Exception ex)
                {
                    ReportErrorMessage(ex);
                    return;
                }

                try
                {

                    SSH("chflags nouchg " + Route_Purple);
                    p(38);

                    FindActivationRoutes();
                    p(42);

                    DownloadActivationFiles();
                    p(45);

                    CreateActivationRecFile(recordsLink);
                    p(48);

                    ModifyCommcenterDsnFile();
                    p(50);

                    ModifyDataArkFile();
                    p(55);

                    UploadActivationFiles();
                    p(65);

                }
                catch (Exception ex)
                {
                    CleanSwapFolder();

                    ReportErrorMessage(ex);
                    return;
                }

                //======================================================================================================
                //STAGE 3 : UNTETHERED && FACETIME/IMESSENGER && NOTIFICATION && -----> ???PENDING SIRI???
                stage = 3;

                try
                {
                    InstallSubstrate();
                    p(75);

                    UploadAntiResetSettings();
                    p(82);

                    DisableOTAUpdates();
                    p(84);

                    DeleteLogs();
                    p(85);

                    SkipSetup();
                    p(86);

                    ldrestart2();
                    p(90);

                }
                catch (Exception ex)
                {
                    if (step >= 85)
                    {
                        //Si es el errro 86 SSH Error pues entonces dar success sin eliminar el substrate
                        ReportAsSuccess();
                        p(100);
                        successBox = new SuccessBox();
                        successBox.ShowDialog(getMainForm());
                        CleanSwapFolder();
                        return;
                    }

                    ReportErrorMessage(ex);
                    return;
                }

                //======================================================================================================
                //STAGE 4 : POSTACTIVATION
                stage = 4;

                try
                {

                    DeleteSubstrate();
                    p(94);

                    CleanSwapFolder();
                    p(99);

                    //Report as succes to the server
                    ReportAsSuccess();
                   
                    p(100);

                }
                catch (Exception ex)
                {
                    CleanSwapFolder();
                }
                successBox = new SuccessBox();
                successBox.ShowDialog(getMainForm());
            }
            catch (Exception ex)
            {
                ReportErrorMessage(ex);
            }
        }

        public void carrier(string chainLink)
        {
            try
            {
                if (iOSDevice.SIMStatus != "kCTSIMSupportSIMStatusReady")
                {
                    MessageBox.Show("Please put simcard and connect to WiFi then click OK", "Sim detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                MessageBox.Show(getMainForm(), "Before starting please make sure that device is connected to WiFi and the WiFi has valid internet connection", "IMPORTANT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                errcarrier = string.Empty;
                errcarrier = "Couldn't stablish SSH Server";
                RunSSHServer();
                errcarrier = "Pair failed!";
                PairLoop();
                p(7);
                errcarrier = "Failed to mount device";
                Mount();
                errcarrier = "Failed to install substrate";
                p(10);
                DeleteSubstrate();
                p(15);
                /*Substrate Installation*/
                UploadResource(Resources.GSM.lzma, "/sbin/lzma");
                UploadResource(Resources.GSM.Library_tar, "/lib.tar"); p(6);
                UploadResource(Resources.GSM.substrate_gsm_frr, "/foo.tar.lzma");

                SSH("tar -xvf /lib.tar -C / &&"+" chmod 777 /sbin/lzma &&"+" lzma -d -v /foo.tar.lzma &&"+" tar -xvf /foo.tar -C /"); p(7);
                SSH("/usr/libexec/substrate");
                SSH("rm /sbin/lzma && rm /lib.tar && rm /foo.tar && rm /foo.tar.lzma");
                /*End-Substrate Installation*/
                p(25);
                
                    /////////////////
                    errcarrier = "Failed to contact activation server errcode (-2)";
                    RestClient restClientt = new RestClient(chainLink);
                    restClientt.Timeout = -1;
                    RestRequest restRequestt = new RestRequest(Method.POST);
                    restRequestt.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                    restRequestt.AddHeader("serial", iOSDevice.SerialNumber);
                    restRequestt.AddHeader("udid", iOSDevice.UDID);
                    restRequestt.AddHeader("signature", iOSDevice.UDID+"-"+iOSDevice.SerialNumber+"-"+iOSDevice.IMEI);
                    IRestResponse restResponsee = restClientt.Execute(restRequestt);

                    if(restResponsee.Content == "sorry - your serial isnt registered")
                    {
                    MessageBox.Show("Unregistered Serial", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }
                    p(30);
                    errcarrier = "INVALID activation policy";
                    byte[] byteArray = Encoding.UTF8.GetBytes(restResponsee.Content);
                    MemoryStream chain = new MemoryStream(byteArray);
                    p(40);
                    SSH("mkdir /private/var/tmp/com.apple.mobileactivationd");
                    SSH("chown -R mobile:mobile /private/var/tmp/com.apple.mobileactivationd");
                    scpClient.Upload(chain, "/private/var/tmp/com.apple.mobileactivationd/chain.plist");
                    ///////////////

                    SSH("rm /Library/MobileSubstrate/DynamicLibraries/GrayRhino.dylib && /Library/MobileSubstrate/DynamicLibraries/GrayRhino.plist");
                    UploadResource(Resources.GSM.GrayRhinoDylib, "/Library/MobileSubstrate/DynamicLibraries/GrayRhino.dylib");
                    p(50);
                    UploadResource(Resources.GSM.GrayRhinoPlist, "/Library/MobileSubstrate/DynamicLibraries/GrayRhino.plist");
                    p(90);
                    Thread.Sleep(2000);
                    SSH("killall -9 SpringBoard");
                    Thread.Sleep(3000);
                    SSH("killall -9 backboardd mobileactivationd");
                    Thread.Sleep(5000);

                restart();

                p(100);
                    MessageBox.Show("Successfully bypassed carrier for (" + iOSDevice.SerialNumber + ")! \n\nDevice Rebooting...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                
            }
            catch
            {
                MessageBox.Show(errcarrier, "Activation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public void MDM_ACTIVATION(string url, string BuildVersion)
        {
            try
            {
                p(5); //Progress 5%

                string DeviceName = string.Empty;
                string FileType = string.Empty;

                if (iOSDevice.Model.Contains("iPad"))
                {
                    DeviceName = iOSDevice.Model.Substring(0, 4);
                }
                else
                {
                    DeviceName = "iPhone";
                }

                p(10); //Progress 10%

                string type = string.Empty;

                if (iOSDevice.UDID.Contains("-"))
                {
                    type = "mega-new";
                }
                else
                {
                    string[] oldShit = { "7", "8", "9", "10" };

                    if (iOSDevice.UDID.Length == 40 && oldShit.Contains(iOSDevice.IOSVersion[0].ToString()))
                    {
                        type = "old";
                    }
                    else
                    {
                        type = "new";
                    }
                }


                string DownloadLink = string.Empty;

                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();

                    data["udid"] = iOSDevice.UDID;
                    data["SerialNumber"] = iOSDevice.SerialNumber;
                    data["IMEI"] = iOSDevice.IMEI;
                    data["ProductType"] = iOSDevice.ProductType;
                    data["DeviceName"] = DeviceName;
                    data["DisplayName"] = DeviceName;
                    data["ProductName"] = DeviceName;
                    data["ProductVersion"] = iOSDevice.IOSVersion;
                    data["BuildVersion"] = BuildVersion;
                    data["type"] = type;

                    var response = wb.UploadValues(url, "POST", data);
                    DownloadLink = Encoding.UTF8.GetString(response);
                }
                p(30); //Progress 30%
                if (DownloadLink.Contains("/download-backup/") == false)
                {
                    MessageBox.Show("Something Went Wrong");
                    p(0); //Reset Progress Bar
                    return;
                }
                else
                {

                    Thread.Sleep(5000); //Backup being prepared in server!

                    string LocalStorage = Environment.CurrentDirectory + "/files/" + UDID + "/BACKUP.zip";

                    Directory.CreateDirectory(Environment.CurrentDirectory + "/files/" + UDID);

                    WebClient myWebClient = new WebClient();
                    myWebClient.DownloadFile(DownloadLink, LocalStorage);

                    p(50); //Progress 50%

                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(LocalStorage))
                    {
                        zip.ExtractAll(Environment.CurrentDirectory + "/files/" + UDID);
                    }

                    p(60); //Progress 60%

                    File.Delete(LocalStorage);

                    p(70); //Progress 70%

                    string str = "";
                    using (Process process = new Process())
                    {
                        string restore = Environment.CurrentDirectory + "/files";

                        process.StartInfo.FileName = ToolDir+"\\files\\idevicebackup2.exe";
                        //process.StartInfo.WorkingDirectory = ToolDir + "\\files";
                        process.StartInfo.Arguments = "restore " + restore;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                        str = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                    }

                    p(80); //Progress 80%

                    Directory.Delete(Environment.CurrentDirectory + "/files/" + UDID, true); //Delete backup :)

                    if (str == null || str == "")
                    {
                        p(0); //Reset progress.
                        MessageBox.Show(getMainForm(), "MDM Restore Error", "Error");
                        return;

                    }
                    else
                    {
                        if(str.Contains("Find My iPhone"))
                        {
                            p(0); //Reset progress.
                            MessageBox.Show(getMainForm(), "This device IS NOT MDM Locked. \nFind My iPhone: ON", "Find My iPhone Alert");
                            return;
                        }
                        else
                        {
                            p(100); //Bypass done.
                            MessageBox.Show(getMainForm(), "MDM Device Successfully Activated!", "SUCCESS");
                            return;
                        }
                    }

                }
            }
            catch
            {
                MessageBox.Show(getMainForm(), "Bypass failed", "Error");
            }
        }

        

        public void CreateActivationFolders()
        {
            SSH("mkdir -p " + Folder_WLPref + " && mkdir -p " + Folder_ActRec + " && mkdir -p " + Folder_ArkInt + " && mkdir -p " + Folder_LBLock);
        }

        public void DownloadActivationFiles()
        {
            downloadFile(Route_CommCe, SwapPCDir + @"com.apple.commcenter.device_specific_nobackup.plist");
            downloadFile(Route_ArkInt, SwapPCDir + @"data_ark.plist");
        }

        public void UploadAntiResetSettings()
        {
            try
            {
                if (iOSDevice.IOSVersion.StartsWith("13.") || iOSDevice.IOSVersion.StartsWith("14."))
                {
                    //Ios 13 and 14
                    SSH("mkdir -p /System/Library/PrivateFrameworks/Settings/GeneralSettingsUI.framework &&" +
                        " chmod -R 777 /System/Library/PrivateFrameworks/Settings/GeneralSettingsUI.framework");

                    UploadResource(GSM.i13update,
                        "/System/Library/PrivateFrameworks/Settings/GeneralSettingsUI.framework/General.plist");
                    UploadResource(GSM.i13reset,
                        "/System/Library/PrivateFrameworks/Settings/GeneralSettingsUI.framework/Reset.plist");
                }
                else
                {
                    //Ios 12
                    SSH("mkdir -p /System/Library/PrivateFrameworks/PreferencesUI.framework &&" +
                        " chmod -R 777 /System/Library/PrivateFrameworks/PreferencesUI.framework");

                    UploadResource(GSM.i12update,
                        "/System/Library/PrivateFrameworks/PreferencesUI.framework/General.plist");
                    UploadResource(GSM.i12reset,
                        "/System/Library/PrivateFrameworks/PreferencesUI.framework/Reset.plist");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }


        public void DisableOTAUpdates()
        {
            //Disable automatic updates
            try
            {
                SSH("launchctl unload -w /System/Library/LaunchDaemons/com.apple.mobile.obliteration.plist &&" +
                    " launchctl unload -w /System/Library/LaunchDaemons/com.apple.OTACrashCopier.plist &&" +
                    " launchctl unload -w /System/Library/LaunchDaemons/com.apple.mobile.softwareupdated.plist &&" +
                    " launchctl unload -w /System/Library/LaunchDaemons/com.apple.OTATaskingAgent.plist");

                SSH("rm -rf /System/Library/LaunchDaemons/com.apple.softwareupdateservicesd.plist &&" +
                    " rm -rf /System/Library/LaunchDaemons/com.apple.mobile.softwareupdated.plist && " +
                    " rm -rf /System/Library/LaunchDaemons/com.apple.mobile.obliteration && " +
                    " rm -rf /System/Library/LaunchDaemons/com.apple.OTATaskingAgent");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to Disable OTA: " + e.Message);
            }
        }

        public void restart()
        {
            string text = "";
            using (Process process2 = new Process())
            {
                process2.StartInfo.FileName = ToolDir + @"\files\idevicediagnostics.exe";
                process2.StartInfo.Arguments = "restart";
                process2.StartInfo.UseShellExecute = false;
                process2.StartInfo.RedirectStandardOutput = true;
                process2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process2.StartInfo.CreateNoWindow = true;
                process2.Start();
                text = process2.StandardOutput.ReadToEnd();
                process2.WaitForExit();
            }
        }

        public void p(int number, string message = "")
        {
            step = number;

            MainForm frm1 = (MainForm)Application.OpenForms["MainForm"];
            frm1.Invoke((MethodInvoker)(() => frm1.reportProgress(number, number.ToString())));

            if (iOSDevice.debugMode == true)
            {
                Console.WriteLine(number.ToString() + message);
            }
        }

        public void downloadFile(string remoteFile = "", string localFile = "")
        {
            Stream localFileStream = File.Create(localFile);

            Stream _stream = localFileStream;

            try
            {
                scpClient.Download(remoteFile, _stream);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            _stream.Close();
        }

        public void PairLoop()
        {
            //Pairing
            while (Pair("pair") == false)
            {
                MessageBox.Show(getMainForm(), "Unlock device and press Trust", "[TRUST PC]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public bool Pair(string argument)
        {
            string path = Directory.GetCurrentDirectory();
            Process proc;

            try
            {
                if (argument == "pair")
                {
                    proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = path + @"\files\idevicepair.exe",
                            Arguments = "pair",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                }
                else
                {
                    proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = path + @"\files\idevicepair.exe",
                            Arguments = "validate",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                }

                try
                {
                    proc.Start();
                    StreamReader reader = proc.StandardOutput;
                    string result = reader.ReadToEnd();

                    Thread.Sleep(2000);
                    try { proc.Kill(); }
                    catch { }
                    if (result.Contains("SUCCESS"))
                    {
                        reader.Dispose();
                        return true;
                    }
                    else { return false; }
                }
                catch { }

            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show(getMainForm(), "Idevicepair not found");
                return false;
            }

            return false;
        }

        public void StartIproxy()
        {
            Process proc;

            KillIproxy();

            try
            {

                proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ToolDir + @"\files\iproxy.exe",
                        Arguments = port + " 44",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show(getMainForm(), "iproxy not found");
            }
        }

        public void KillIproxy()
        {
            foreach (var process in Process.GetProcessesByName("iproxy"))
            {
                process.Kill();
            }

            if (File.Exists(@"%USERPROFILE%\.ssh\known_hosts"))
            {
                File.Delete(@"%USERPROFILE%\.ssh\known_hosts");
            }
        }

        public void FindActivationRoutes()
        {
            SshCommand commandX;
            commandX = SSH("find /private/var/containers/Data/System/ -iname 'internal'");
            Folder_Activa = commandX.Result.Replace("Library/internal", "").Replace("\n", "").Replace("//", "/");

            Folder_ArkInt = Folder_Activa + "Library/internal/";
            Folder_ActRec = Folder_Activa + "Library/activation_records/";
            Folder_LBPref = "/var/mobile/Library/Preferences/";
            Folder_WLPref = "/var/wireless/Library/Preferences/";
            Folder_LBLock = "/var/root/Library/Lockdown/";

            Route_ActRec = Folder_ActRec + "activation_record.plist";
            Route_CommCe = Folder_WLPref + "com.apple.commcenter.device_specific_nobackup.plist";
            Route_ArkInt = Folder_ArkInt + "data_ark.plist";
            Route_DatArk = Folder_LBLock + "data_ark.plist";

            Route_Purple = Folder_LBPref + "com.apple.purplebuddy.plist";
        }

        public bool Activate(string url)
        {
            string UrlActivation = url;

            string str = "";
            using (Process process = new Process())
            {
                process.StartInfo.FileName = ToolDir + @"\files\ideviceactivation.exe";
                process.StartInfo.Arguments = "activate -s " + UrlActivation;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                str = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            //MessageBox.Show(str);

            string text = "";
            using (Process process2 = new Process())
            {
                process2.StartInfo.FileName = ToolDir + @"\files\ideviceactivation.exe";
                process2.StartInfo.Arguments = "state";
                process2.StartInfo.UseShellExecute = false;
                process2.StartInfo.RedirectStandardOutput = true;
                process2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process2.StartInfo.CreateNoWindow = true;
                process2.Start();
                text = process2.StandardOutput.ReadToEnd();
                process2.WaitForExit();
            }
            //MessageBox.Show(text);

            if (text.Contains("Activated"))
            {
                return true;
            }

            return false;
        }

        public void RestartLockdown()
        {
            SSH("launchctl unload /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist > /dev/null 2>&1 && launchctl load /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist > /dev/null 2>&1");
        }

        public void DeleteActivationFiles()
        {
            SSH("chflags nouchg " + Route_ActRec + " && rm " + Route_ActRec);

            SSH("chflags nouchg " + Route_CommCe + " && rm " + Route_CommCe +
                " && chflags nouchg " + Route_DatArk + " && rm " + Route_DatArk +
                " && chflags nouchg " + Route_ArkInt + " && rm " + Route_ArkInt +
                " && chflags nouchg " + Route_Purple + " && rm " + Route_Purple);
        }

        public void InstallSubstrate()
        {
            UploadResource(Resources.GSM.lzma, "/sbin/lzma");
            UploadResource(Resources.GSM.Library_tar, "/lib.tar"); p(70);
            UploadResource(Resources.GSM.substrate_gsm_frr, "/foo.tar.lzma");

            SSH("tar -xvf /lib.tar -C / &&" +
                " chmod 777 /sbin/lzma &&" +
                " lzma -d -v /foo.tar.lzma &&" +
                " tar -xvf /foo.tar -C /"); p(74);

            SSH("/usr/libexec/substrate");

            SSH("rm /sbin/lzma && rm /lib.tar && rm /foo.tar && rm /foo.tar.lzma");

            UploadResource(Resources.NOTI_FIX.fix_tedddby, "/Library/MobileSubstrate/DynamicLibraries/iuntethered.dylib");
            UploadResource(Resources.NOTI_FIX.fix_iuntethered_plist, "/Library/MobileSubstrate/DynamicLibraries/iuntethered.plist");
        }

        public bool UploadResource(byte[] resource, string remoteFilePath)
        {
            try
            {
                if (!scpClient.IsConnected) { scpClient.Connect(); }

                MemoryStream stream = new MemoryStream(resource);

                scpClient.Upload(stream, remoteFilePath);

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Upload Resource File" + ex.Message);
            }
        }

        public void RunSSHServer()
        {
            for (; ; )
            {

                KillIproxy();

                Thread.Sleep(2000);

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = ToolDir + @"\files\iproxy.exe"; ;
                    process.StartInfo.Arguments = port + " 44";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                }

                try
                {
                    ConnectSshClient();
                }
                catch (Exception ex)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    if (MessageBox.Show(getMainForm(), "SSH Connection Error. Try again.", "SSH Connection Error", buttons) == DialogResult.Yes)
                    {
                        continue;
                    }
                    throw new ApplicationException("Error SSH " + ex.Message);
                }

                break;
            }
        }

        public void ConnectSshClient()
        {
            AuthenticationMethod[] authenticationMethods = new AuthenticationMethod[]
            {
                new PasswordAuthenticationMethod("root", password)
            };
            ConnectionInfo connectionInfo = new ConnectionInfo(host, port, "root", authenticationMethods);
            connectionInfo.Timeout = TimeSpan.FromSeconds(20);

            sshClient = new SshClient(connectionInfo);
            scpClient = new ScpClient(connectionInfo);

            if (!sshClient.IsConnected) { sshClient.Connect(); }
            if (!scpClient.IsConnected) { scpClient.Connect(); }
        }

        public void Mount()
        {
            SSH("mount -o rw,union,update /");
            //SSH("echo \"\" >> /.mount_rw");
        }

        public void DeleteSubstrate()
        {
            SSH("rm /Library/MobileSubstrate/DynamicLibraries/iuntethered.dylib && rm /Library/MobileSubstrate/DynamicLibraries/iuntethered.plist");
            SSH("rm -rf /Library/MobileSubstrate/DynamicLibraries/");
            SSH("rm -rf /Library/Frameworks/CydiaSubstrate.framework");
            SSH("rm /Library/MobileSubstrate/MobileSubstrate.dylib");
            SSH("rm /Library/MobileSubstrate/DynamicLibraries && rm /Library/MobileSubstrate/ServerPlugins");
            SSH("rm /usr/bin/cycc && rm /usr/bin/cynject");
            SSH("rm /usr/include/substrate.h");
            SSH("rm /usr/lib/cycript0.9/com/saurik/substrate/MS.cy");
            SSH("rm -rf /usr/lib/substrate");
            SSH("rm /usr/lib/libsubstrate.dylib");
            SSH("rm /usr/libexec/substrate && rm /usr/libexec/substrated");
        }

        public void DeleteLogs()
        {
            SSH("rm -rf /private/var/mobile/Library/Logs/* > /dev/null 2>&1");
            SSH("rm -rf /private/var/mobile/Library/Logs/* > /dev/null 2>&1");
        }

        public void UploadActivationFiles()
        {
            SSH("rm -rf " + SwapIdevDir);
            SSH("mkdir " + SwapIdevDir + " && chown -R mobile:mobile " + SwapIdevDir + " && chmod -R 755 " + SwapIdevDir); p(58);

            SSH("mkdir -p " + Folder_WLPref + " && chmod 775 " + Folder_WLPref);
            ConvertPlist(SwapPCDir + "com.apple.commcenter.device_specific_nobackup.plist", 2); p(60);
            UploadLocalFile(SwapPCDir + "com.apple.commcenter.device_specific_nobackup.plist", SwapIdevDir + "/Route_CommCe.plist");
            SSH("mv -f " + SwapIdevDir + "/Route_CommCe.plist " + Route_CommCe + " && chflags uchg " + Route_CommCe);

            SSH("mkdir -p " + Folder_ActRec + " && chmod 775 " + Folder_ActRec);
            ConvertPlist(SwapPCDir + "activation_record.plist", 2);
            UploadLocalFile(SwapPCDir + "activation_record.plist", SwapIdevDir + "/Route_ActRec.plist");
            SSH("mv -f " + SwapIdevDir + "/Route_ActRec.plist " + Route_ActRec +
                " && chflags uchg " + Route_ActRec); p(62);

            ConvertPlist(SwapPCDir + "data_ark.plist", 2);
            UploadLocalFile(SwapPCDir + "data_ark.plist", SwapIdevDir + "/Route_DataArk.plist"); p(64);
            SSH("cp -f " + SwapIdevDir + "/Route_DataArk.plist " + Route_ArkInt +
                " && mv -f " + SwapIdevDir + "/Route_DataArk.plist " + Route_DatArk +
                " && chflags uchg " + Route_ArkInt + " && chflags uchg " + Route_DatArk);
        }

        public bool ConvertPlist(string fileName, int method)
        {
            bool result;
            try
            {
                Process process = new Process();
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.FileName = ToolDir + @"\files\win-plutil.exe";
                if (method == 1)
                {
                    string str = string.Format("\"{0}\"", fileName);
                    processStartInfo.Arguments = "-convert xml1 " + str;
                    process.StartInfo = processStartInfo;
                    process.Start();
                    process.WaitForExit();
                    result = true;
                }
                else
                {
                    string str2 = string.Format("\"{0}\"", fileName);
                    processStartInfo.Arguments = "-convert binary1 " + str2;
                    process.StartInfo = processStartInfo;
                    process.Start();
                    process.WaitForExit();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Convert Error: " + ex.Message);
            }

            return result;
        }

        public void SnappyRename()
        {
            string str = DeleteLines(SSH("snappy -f / -l").Result, 1).Replace("\n", "").Replace("\r", "");
            string command = "snappy -f / -r " + str + " --to orig-fs";
            SSH(command);
        }

        public void RestartAllDeamons()
        {
            SSH("launchctl unload -F /System/Library/LaunchDaemons/* && launchctl load -w -F /System/Library/LaunchDaemons/*");
        }

        public SshCommand SSH(string command)
        {
            if (!sshClient.IsConnected)
            {
                sshClient.ConnectionInfo.Timeout = TimeSpan.FromSeconds(15);
                sshClient.Connect();
            }

            try
            {
                SshCommand commando = this.sshClient.CreateCommand(command);

                commando.CommandTimeout = TimeSpan.FromSeconds(30);
                commando.Execute();

                if (iOSDevice.debugMode)
                {
                    Console.WriteLine("=================");
                    Console.WriteLine("Command Name = {0} " + commando.CommandText);
                    Console.WriteLine("Return Value = {0}", commando.ExitStatus);
                    Console.WriteLine("Error = {0}", commando.Error);
                    Console.WriteLine("Result = {0}", commando.Result);
                }

                return commando;

            }
            catch
            {
                switch (command)
                {
                    case "ls":
                        break;
                    case "uicache --all":
                        break;
                    default:
                        if (iOSDevice.debugMode == true)
                        {
                            Console.WriteLine("SSH Error caused by:" + command);
                        }
                        else { Thread.Sleep(2000); }
                        StartIproxy();
                        SSH(command);
                        break;
                }

                return null;
            }
        }

        public void ldrestart2()
        {
            UploadResource(Resources.GSM.ldrestart, "/usr/bin/ldrestart"); p(82);

            if (!sshClient.IsConnected)
            {
                sshClient.ConnectionInfo.Timeout = TimeSpan.FromSeconds(15);
                sshClient.Connect();
            }

            try
            {
                sshClient.CreateCommand("chmod 755 /usr/bin/ldrestart && /usr/bin/ldrestart").Execute();
                sshClient.Disconnect();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                //Console.WriteLine(e.StackTrace);
                if (File.Exists(@"%USERPROFILE%\.ssh\known_hosts"))
                {

                    File.Delete(@"%USERPROFILE%\.ssh\known_hosts");
                }
            }

            Thread.Sleep(11000);

            PairLoop(); p(86);

            RunSSHServer(); p(87);

            Thread.Sleep(1000);

            Mount(); p(88);
        }

        public void UploadLocalFile(string localFile, string remoteFile)
        {
            Stream localFileStream = File.Open(localFile, FileMode.Open);
            Stream stream = localFileStream;
            scpClient.Upload(stream, remoteFile);
            stream.Close();
        }

        public void Deactivate()
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = ToolDir + @"\files\ideviceactivation.exe"; ;
                process.StartInfo.Arguments = "deactivate";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
        }

        public void CreateActivationRecFile(string link)
        {
            string serverResponse = GetActivationRecordFromServer(link);
            //MessageBox.Show(serverResponse);

            serverResponse = serverResponse.ToString().Replace("\n", "").Replace("\r", "").Replace("\t", "");

            File.WriteAllText(SwapPCDir + "act_rec.plist.tmp", serverResponse);

            NSDictionary actRecDictionary = (NSDictionary)PropertyListParser.Parse(new FileInfo(SwapPCDir + "act_rec.plist.tmp"));

            //global variable
            WildcardTicket = GetPlistProperty(actRecDictionary, "WildcardTicketToRemove", 4);
            //MessageBox.Show(WildcardTicket);

            actRecDictionary.Remove("WildcardTicketToRemove");

            PropertyListParser.SaveAsXml(actRecDictionary, new FileInfo(SwapPCDir + "activation_record.plist"));
        }


        public string GetActivationRecordFromServer(string url)
        {
            string requestUrl = url+"/"+iOSDevice.SerialNumber;

            HttpWebRequest request = HttpWebRequest.CreateHttp(requestUrl);

            // Optionally, set properties of the HttpWebRequest, such as:
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = 12000;

            // Submit the request, and get the response body.
            string responseBodyFromRemoteServer;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    responseBodyFromRemoteServer = reader.ReadToEnd();
                }
            }

            return responseBodyFromRemoteServer;
        }

        public void ModifyCommcenterDsnFile()
        {
            NSDictionary ComCenterDictionary = (NSDictionary)PropertyListParser.Parse(new FileInfo(SwapPCDir + "com.apple.commcenter.device_specific_nobackup.plist"));

            try
            {
                ComCenterDictionary.Remove("kPostponementTicket");
            }
            catch
            {
            }

            p(49);

            ComCenterDictionary.Add("kPostponementTicket", new NSDictionary
            {
                {
                    "ActivationState",
                    "Activated"
                },
                {
                    "ActivationTicket",
                    WildcardTicket //Global variable
                },
                {
                    "ActivityURL",
                    "https://albert.apple.com/deviceservices/activity"
                },
                {
                    "PhoneNumberNotificationURL",
                    "https://albert.apple.com/deviceservices/phoneHome"
                }
            });

            PropertyListParser.SaveAsXml(ComCenterDictionary, new FileInfo(SwapPCDir + "com.apple.commcenter.device_specific_nobackup.plist"));
        }

        public void ModifyDataArkFile()
        {
            NSDictionary DataArkDictionary =
                (NSDictionary)PropertyListParser.Parse(new FileInfo(SwapPCDir + "data_ark.plist"));

            try
            {
                DataArkDictionary.Remove("-UCRTOOBForbidden");
                DataArkDictionary.Remove("ActivationState");
                DataArkDictionary.Remove("-ActivationState");
                DataArkDictionary.Add("ActivationState", "Activated");
            }
            catch (Exception e)
            {

            }

            PropertyListParser.SaveAsXml(DataArkDictionary, new FileInfo(SwapPCDir + "data_ark.plist"));
        }

        public string GetPlistProperty(NSDictionary plist, string NombreObjeto, int LineasArriba = 4)
        {
            NSObject nsobject;
            plist.TryGetValue(NombreObjeto, out nsobject);
            return DeleteLines(nsobject.ToXmlPropertyList().ToString(), LineasArriba).Replace("\n", "").Replace("\r", "").Replace("</data>", "").Replace("</plist>", "").Replace("</string>", "").Replace("<string>", "").Trim();
        }

        public string DeleteLines(string str, int linesToRemove)
        {
            return str.Split(Environment.NewLine.ToCharArray(), linesToRemove + 1).Skip(linesToRemove).FirstOrDefault<string>();
        }

        public void CleanSwapFolder()
        {
            try
            {
                if (Directory.Exists(SwapPCDir))
                {
                    Directory.Delete(SwapPCDir, true);
                }
                Thread.Sleep(1000);
                Directory.CreateDirectory(SwapPCDir);
            }
            catch (Exception e)
            {

            }
        }

        private void ReportAsSuccess()
        {
            try
            {
                //Report the success bypass to the server if you want [Not necesary]
                //Network.SuccessBypass();
            }
            catch { }
        }

        private void ReportErrorMessage(Exception e = null)
        {
            string errmsg = "Activation error!";

            if (e != null)
            {
                errmsg = e.Message + e.StackTrace;
            }

            string errorStr = "Unknow error occured. \nError body: [" + e + "]";

            MessageBox.Show(getMainForm(), errorStr, "Activation Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void DoNotiFix()
        {
            try
            {

                //======================================================================================================
                //STAGE 1 : RUN AND CONNECT SSH & CLEANING PREVIOUS ACTIVATION FILES
                stage = 1;

                p(5);

                try
                {
                    //throw new ApplicationException("Testing Error.");

                    PairLoop();
                    p(10);

                    RunSSHServer();
                    p(20);

                    Mount();
                    p(30);

                    SnappyRename();
                    p(60);

                    InstallSubstrate();
                    p(80);

                    DisableOTAUpdates();
                    p(84);

                    ldrestart2();
                    p(90);

                    DeleteSubstrate();
                    p(96);

                    ReportAsSuccessNotiFix();
                    restart();
                    p(100);
                }
                catch (Exception ex)
                {
                    ReportErrorMessage(ex);
                    return;
                }

                SuccessBoxNotiFix successBox = new SuccessBoxNotiFix();
                successBox.ShowDialog(getMainForm());

            }
            catch (Exception ex)
            {
                ReportErrorMessage(ex);
            }
        }

        private void ReportAsSuccessNotiFix()
        {
            try
            {
                //Report the success notifix to the server if you want [Not necesary]
                //Network.SuccessNotifix();
            }
            catch { }
        }

        private MainForm getMainForm()
        {
            MainForm frm1 = (MainForm)Application.OpenForms["MainForm"];
            return frm1;
        }

        private void SkipSetup()
        {
            UploadResource(GSM.purple, "/var/mobile/Library/Preferences/com.apple.purplebuddy.plist");
            SSH("chmod 600 /var/mobile/Library/Preferences/com.apple.purplebuddy.plist");
        }

    }
}