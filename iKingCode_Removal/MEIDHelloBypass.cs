using System;
using System.Collections.Generic;
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

namespace iKingCode_BypassHello
{
    public class MEIDHelloBypass
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
        public static string SwapIdevDir = @"/tmp/Backup";

        private string WildcardTicket;

       private int stage = 0;
        private int step = 0;

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
                        p(31 + intento); //33,34,35

                        Deactivate();
                        activateResult = Activate(activationLink);
                        intento++;
                        Thread.Sleep(1000);
                    }

                    if (activateResult == false)
                    {
                        //MessageBox.Show("Error in Activation Process [STAGE 2 - STEP " + step.ToString() + "]", "Error in Activation Process (1)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                    p(77);

                    DisableOTAUpdates();
                    p(80);

                    ChangeSimStatus();
                    p(82);

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









        //////////////////////////////////////GSM NO SIGNAL////////////////////////////////////////
        public void DoBypass_GSM_No_Signal(string activationLink, string recordsLink)
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
                        p(31 + intento); //33,34,35

                        Deactivate();
                        activateResult = Activate(activationLink);
                        intento++;
                        Thread.Sleep(1000);
                    }

                    if (activateResult == false)
                    {
                        //MessageBox.Show("Error in Activation Process [STAGE 2 - STEP " + step.ToString() + "]", "Error in Activation Process (1)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

                    SSH("launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.softwareupdateservicesd.plist");
                    SSH("launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.mobile.softwareupdated.plist");
                    SSH("launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.OTATaskingAgent.plist");
                    SSH("launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.mobile.obliteration.plist");
                    SSH("launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.CommCenter.plist");
                    SSH("launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.CommCenterMobileHelper.plist");
                    SSH("launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.CommCenterRootHelper.plist");
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
                    p(77);

                    DisableOTAUpdates();
                    p(80);

                    ChangeSimStatus();
                    p(82);

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
        //////////////////////////////////////////////////////////////////////////////////////////


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
                MessageBox.Show(getMainForm(), "Unlock device and press Trust", "[TRUS PC]", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public bool Activate(string UrlActivation)
        {

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

            SSH("rm /sbin/lzma && rm /lib.tar && rm /foo.tar && rm /foo.tar.lzma"); p(70);

            UploadResource(Resources.GSM.lzma, "/sbin/lzma");
            UploadResource(Resources.GSM.Library_tar, "/lib.tar"); p(72);
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
            SSH("rm " + SwapIdevDir);
            SSH("mkdir " + SwapIdevDir + " && chown -R mobile:mobile " + SwapIdevDir + " && chmod -R 755 " + SwapIdevDir); p(58);

            ConvertPlist(SwapPCDir + "com.apple.commcenter.device_specific_nobackup.plist", 2);
            SSH("chflags nouchg " + Route_CommCe + " && mkdir -p " + Folder_WLPref);
            UploadLocalFile(SwapPCDir + "com.apple.commcenter.device_specific_nobackup.plist", SwapIdevDir + "/Route_CommCe.plist"); p(59);
            SSH("mv -f " + SwapIdevDir + "/Route_CommCe.plist " + Route_CommCe);
            //Lectura + Ejecucion para todos, pero no escritura
            SSH("chown root:mobile " + Route_CommCe + " && chmod 555 " + Route_CommCe + " && chflags uchg " + Route_CommCe); p(60);

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
            try
            {
                //renaming snapshot
                SSH("snappy -f / -r `snappy -f / -l | sed -n 2p` -t orig-fs");
            }
            catch (Exception ex)
            {
                //..
            }
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
            UploadResource(Resources.GSM.ldrestart, "/usr/bin/ldrestart"); p(86);

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

            PropertyListParser.SaveAsXml(actRecDictionary, new FileInfo(SwapPCDir + "activation_record.plist"));
        }

        public string GetActivationRecordFromServer(string url)
        {
            string requestUrl = url + "/" + iOSDevice.SerialNumber;

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

            string RandomString = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPCFET0NUWVBFIHBsaXN0IFBVQkxJQyAiLS8vQXBwbGUvL0RURCBQTElTVCAxLjAvL0VOIiAiaHR0cDovL3d3dy5hcHBsZS5jb20vRFREcy9Qcm9wZXJ0eUxpc3QtMS4wLmR0ZCI+CjxwbGlzdCB2ZXJzaW9uPSIxLjAiPgo8ZGljdD4KCTxrZXk+QWN0aXZhdGlvblJlcXVlc3RJbmZvPC9rZXk+Cgk8ZGljdD4KCQk8a2V5PkFjdGl2YXRpb25SYW5kb21uZXNzPC9rZXk+CgkJPHN0cmluZz4zMGI2MGZkMC02Njc0LTQ3NzgtYmIxNC1mNGZhOTQ0MWQ0Yzg8L3N0cmluZz4KCQk8a2V5PkFjdGl2YXRpb25TdGF0ZTwva2V5PgoJCTxzdHJpbmc+VW5hY3RpdmF0ZWQ8L3N0cmluZz4KCQk8a2V5PkZNaVBBY2NvdW50RXhpc3RzPC9rZXk+CgkJPHRydWUvPgoJPC9kaWN0PgoJPGtleT5CYXNlYmFuZFJlcXVlc3RJbmZvPC9rZXk+Cgk8ZGljdD4KCQk8a2V5PkFjdGl2YXRpb25SZXF1aXJlc0FjdGl2YXRpb25UaWNrZXQ8L2tleT4KCQk8dHJ1ZS8+CgkJPGtleT5CYXNlYmFuZEFjdGl2YXRpb25UaWNrZXRWZXJzaW9uPC9rZXk+CgkJPHN0cmluZz5WMjwvc3RyaW5nPgoJCTxrZXk+QmFzZWJhbmRDaGlwSUQ8L2tleT4KCQk8aW50ZWdlcj4xMjM0NTY3PC9pbnRlZ2VyPgoJCTxrZXk+QmFzZWJhbmRNYXN0ZXJLZXlIYXNoPC9rZXk+CgkJPHN0cmluZz44Q0IxMDcwRDk1QjlDRUU0QzgwMDAwNUUyMTk5QkI4RkIxODNCMDI3MTNBNTJEQjVFNzVDQTJBNjE1NTM2MTgyPC9zdHJpbmc+CgkJPGtleT5CYXNlYmFuZFNlcmlhbE51bWJlcjwva2V5PgoJCTxkYXRhPgoJCUVnaGRDdz09CgkJPC9kYXRhPgoJCTxrZXk+SW50ZXJuYXRpb25hbE1vYmlsZUVxdWlwbWVudElkZW50aXR5PC9rZXk+CgkJPHN0cmluZz4xMjM0NTY3ODkxMjM0NTY8L3N0cmluZz4KCQk8a2V5Pk1vYmlsZUVxdWlwbWVudElkZW50aWZpZXI8L2tleT4KCQk8c3RyaW5nPjEyMzQ1Njc4OTEyMzQ1PC9zdHJpbmc+CgkJPGtleT5TSU1TdGF0dXM8L2tleT4KCQk8c3RyaW5nPmtDVFNJTVN1cHBvcnRTSU1TdGF0dXNOb3RJbnNlcnRlZDwvc3RyaW5nPgoJCTxrZXk+U3VwcG9ydHNQb3N0cG9uZW1lbnQ8L2tleT4KCQk8dHJ1ZS8+CgkJPGtleT5rQ1RQb3N0cG9uZW1lbnRJbmZvUFJMTmFtZTwva2V5PgoJCTxpbnRlZ2VyPjA8L2ludGVnZXI+CgkJPGtleT5rQ1RQb3N0cG9uZW1lbnRJbmZvU2VydmljZVByb3Zpc2lvbmluZ1N0YXRlPC9rZXk+CgkJPGZhbHNlLz4KCTwvZGljdD4KCTxrZXk+RGV2aWNlQ2VydFJlcXVlc3Q8L2tleT4KCTxkYXRhPgoJTFMwdExTMUNSVWRKVGlCRFJWSlVTVVpKUTBGVVJTQlNSVkZWUlZOVUxTMHRMUzBLVFVsSlFuaEVRME5CVXpCRFFWRkIKCWQyZFpUWGhNVkVGeVFtZE9Wa0pCVFZSS1JVa3pUbXRSTUU1RlJUVk1WVmt6VGpCUmRFNUZVVEJOYVRBMFVWVktRZzBLCglURlJyZUZKcVdrVlNSRWw1VWtWS1IwNXFSVXhOUVd0SFFURlZSVUpvVFVOV1ZrMTRRM3BCU2tKblRsWkNRV2RVUVd0TwoJYWpaeFNVbHRUbmxXU21WMU5sTTJVak40UVcxT1RXNWFjREpHTDNoRVNIRjViVmxVT1ZoT1JFdzBjRlJaYjFnMmF6QmsKCVFrMVNTWGRGUVZsRVZsRlJTQTBLUlhkc1JHUllRbXhqYmxKd1ltMDRlRVY2UVZKQ1owNVdRa0Z2VkVOclJuZGpSM2hzCglTVVZzZFZsNU5IaEVla0ZPUW1kT1ZrSkJjMVJDYld4UllVYzVkUTBLV2xSRFFtNTZRVTVDWjJ0eGFHdHBSemwzTUVKQgoJUVUxQk1FZERVM0ZIVTBsaU0wUlJSVUpDVVZWQlFUUkhRa0ZETDJ4eWJHVlJUamR3UVEwS00yaEhWVlkwU0ZsU1lXdHYKCWFrazRPV3d4YUZKdmRqQlROREJPTUhBeU1UaHJUV295YkRGT2EzUXdWWEJxV2s5RU5WVldlVGRDT0VsT1FrSm1RMmxNCglNZzBLWnk4dkx5dHpaVVZoVjFjMGFEWXdUM0pOZG5KbFFWQTBNR0psVTJaUFlucE1WR3hYUzJGV2NXRnJNV1JGVGpSSgoJTkd4TVRYaHBlVFVyYjNwSVpqWmlWdzBLVGl0bldFSlVNMjl4WkhWRFF6RldWelZKV25aMlpFUlNWRWx3YUZoNmEyRUsKCVVVVkdRVUZQUW1wUlFYZG5XV3REWjFsRlFYSlVhMVpFZDBGV01IbHRZazVWUm14ME0yeExjMHRCWkEwS2JuYzBTRlpPCglaMEZ1UkhoaWRRMEtRVUpXV1VSMlNGaEJNREZNV0ZOS1F5dHRkamd5VFZSSWQySk5ORVF2V2xJclJFaFpRV1kyWXlzNQoJYVc1TlJtUk9PR2xaV0hSSWFFOXdjV3MwYVd4TlR3MEtZMnRuWWtsNlMwb3lOWFJPWTFKVWMwOXdWVU5CZDBWQlFXRkIKCUxTMHRMUzFGVGtRZ1EwVlNWRWxHU1VOQlZFVWdVa1ZSVlVWVFZDMHRMUzB0Cgk8L2RhdGE+Cgk8a2V5PkRldmljZUlEPC9rZXk+Cgk8ZGljdD4KCQk8a2V5PlNlcmlhbE51bWJlcjwva2V5PgoJCTxzdHJpbmc+RlIxUDJHSDhKOFhIPC9zdHJpbmc+CgkJPGtleT5VbmlxdWVEZXZpY2VJRDwva2V5PgoJCTxzdHJpbmc+ZDk4OTIwOTZjZjM0MTFlYTg3ZDAwMjQyYWMxMzAwMDNmMzQxMWU0Mjwvc3RyaW5nPgoJPC9kaWN0PgoJPGtleT5EZXZpY2VJbmZvPC9rZXk+Cgk8ZGljdD4KCQk8a2V5PkJ1aWxkVmVyc2lvbjwva2V5PgoJCTxzdHJpbmc+MThGMDA8L3N0cmluZz4KCQk8a2V5PkRldmljZUNsYXNzPC9rZXk+CgkJPHN0cmluZz5pUGhvbmU8L3N0cmluZz4KCQk8a2V5PkRldmljZVZhcmlhbnQ8L2tleT4KCQk8c3RyaW5nPkI8L3N0cmluZz4KCQk8a2V5Pk1vZGVsTnVtYmVyPC9rZXk+CgkJPHN0cmluZz5NTExOMjwvc3RyaW5nPgoJCTxrZXk+T1NUeXBlPC9rZXk+CgkJPHN0cmluZz5pUGhvbmUgT1M8L3N0cmluZz4KCQk8a2V5PlByb2R1Y3RUeXBlPC9rZXk+CgkJPHN0cmluZz5pUGhvbmUwLDA8L3N0cmluZz4KCQk8a2V5PlByb2R1Y3RWZXJzaW9uPC9rZXk+CgkJPHN0cmluZz4xNC4wLjA8L3N0cmluZz4KCQk8a2V5PlJlZ2lvbkNvZGU8L2tleT4KCQk8c3RyaW5nPkxMPC9zdHJpbmc+CgkJPGtleT5SZWdpb25JbmZvPC9rZXk+CgkJPHN0cmluZz5MTC9BPC9zdHJpbmc+CgkJPGtleT5SZWd1bGF0b3J5TW9kZWxOdW1iZXI8L2tleT4KCQk8c3RyaW5nPkExMjM0PC9zdHJpbmc+CgkJPGtleT5TaWduaW5nRnVzZTwva2V5PgoJCTx0cnVlLz4KCQk8a2V5PlVuaXF1ZUNoaXBJRDwva2V5PgoJCTxpbnRlZ2VyPjEyMzQ1Njc4OTEyMzQ8L2ludGVnZXI+Cgk8L2RpY3Q+Cgk8a2V5PlJlZ3VsYXRvcnlJbWFnZXM8L2tleT4KCTxkaWN0PgoJCTxrZXk+RGV2aWNlVmFyaWFudDwva2V5PgoJCTxzdHJpbmc+Qjwvc3RyaW5nPgoJPC9kaWN0PgoJPGtleT5Tb2Z0d2FyZVVwZGF0ZVJlcXVlc3RJbmZvPC9rZXk+Cgk8ZGljdD4KCQk8a2V5PkVuYWJsZWQ8L2tleT4KCQk8dHJ1ZS8+Cgk8L2RpY3Q+Cgk8a2V5PlVJS0NlcnRpZmljYXRpb248L2tleT4KCTxkaWN0PgoJCTxrZXk+Qmx1ZXRvb3RoQWRkcmVzczwva2V5PgoJCTxzdHJpbmc+ZmY6ZmY6ZmY6ZmY6ZmY6ZmY8L3N0cmluZz4KCQk8a2V5PkJvYXJkSWQ8L2tleT4KCQk8aW50ZWdlcj4yPC9pbnRlZ2VyPgoJCTxrZXk+Q2hpcElEPC9rZXk+CgkJPGludGVnZXI+MzI3Njg8L2ludGVnZXI+CgkJPGtleT5FdGhlcm5ldE1hY0FkZHJlc3M8L2tleT4KCQk8c3RyaW5nPmZmOmZmOmZmOmZmOmZmOmZmPC9zdHJpbmc+CgkJPGtleT5VSUtDZXJ0aWZpY2F0aW9uPC9rZXk+CgkJPGRhdGE+CgkJTUlJRDB3SUJBakNDQTh3RUlQNEMzc3FRdFAxUzJod0JaekNvSGNzb0gyeE51NWMrYTRRNDVvSjFNS0YzCgkJQkVFRTJlOTNlb1ZPeHVmMGVLUFVxTkVnNnpNbEJzTnEranIrUnFNQXhTaFZBL2NUNW9ua3IwdCtFMEhLCgkJblNkdkhNMi9GZXRyT3FpT0k0RHZIUElEVzBEMnVBUVEzaW9iUHdhQWxGbFhIUFdyOE1KLyt3UVFHVGxuCgkJRVhPMTZOdDJrVUUrdy8vQmxHd1Q4V3hSZXkvSU41SW1NbGtZelpsSnpack83dVl0bHBlZ3k2K3hJaWwyCgkJQjJYbHk0aUd4UlppUld5NXNLcFFvMll6b0pFbW1XU25manUwY1UyL3JiOUZCdnVWaS9rV1NGbkFrdDR5CgkJcVF3NGswaWJ0cDVXK1lVQ0NvZm8zeWVuak0yVWMwbit5SExyU20wRTlPUDNwdExUN3ZHcnJma3IzWFJpCgkJdHdEcGRCT3NzK1h6SEFRWEt1cG85WGkxUW1ObGp1VGoxakpZbzZNc1kyOURYOUVacFdEdmpJc0l5THd4CgkJQjRjbUlTVWY4Qm5yUlFHOURBM01lYzZiaFRkUEJjdUtXZHBCbm5DMlY4V3BmTXBwVUQ2U2RndW5pejZ6CgkJTEcwNmNGR3dvUXZuWXhRa1Vra2pkWWR6NG85eXM5L3ZxQ2JxZnBuNHRjZEkyMWM5Z29Nd0xoRHNoYms1CgkJUENaQnNoNUY0U1JSaWdBV3JBU0NBejk4MkI3bzhwQ0NaL2pZK3laQ3pBb3J6SG5zR2Z2d0tpSlBBTWppCgkJZTA0RzRqSk04cEpRUU5uWmFhUCt0RmVsZGhER1FubzA0dmZKRFkzOEZGTSthZUN3elJyQy9DUGJrZVpRCgkJNXR5NTdMSXNzMUhyUmUzSTFjK0ZMNXBuZmwvaEsxQjF1QTRHRDRWbFkxU0xMMXk1ajRHdUZUM1hTeHpiCgkJWlIvZmJEa1V5VHNUM3I2eGdoWnRNNEJYSW9hNjJaREMzSVBtT2J4S2JobGFLQTRtSzJzM1FCNFZjNlMvCgkJbTZ1YTZQakwvQjE1QzBjTGpyMUNNb0x0Lzc4TFVRV21GRXV5SkhkdnRTNnhIbWtMRG9FZW1tMHlDcGJqCgkJMmhrRmt2d3dISlg2SDFiUm1KWS9HUmY1UXVIWDVKdlk3ZGhOY2YzNENmaVExRHdwZ2VKUkw5eTN2SG0vCgkJZkFSV0JxWDNkWjV1VUpXcUNzMklvMFdIRGdqMTh3cW5vUEw2QnRHcjVhWEJFeGF3WkpGT1ZOcVZjV2lPCgkJOE9LMzhuSDFKaGcxVk44UURBelhmTEpjQ2w0UEN6Mm5sVlpSMDl1WnF0NlpPaXFjVUNyZ3hZbTdIQktaCgkJOS9BRmIyVmxLUFRZTTNueXBDeGh5MmNMQnowK3RCK0V6V0hTbjlzU3FMelN1eFBOdGIxY21FMno5OFNoCgkJMk1UVzJaWk42NWdvYkxrSU5wbzdUb1RBMm50cHY1ZjBqdlhpVnZIV1V1dmhUSVlLZG4vKzA0czNJQ0VLCgkJQVlJQ0NPNjgvakxucDVQUERuRmVsQ3Z1d0dFRTFkb0lMNzZ6UllNOWlrWTJHRVB5NW5XdW1ydXp4U2RCCgkJMURBNnNOeUxQanN2QnBnYUVnWmI0OUpXSjlERU5vYWZKeGQ4dlBoRnpORHZEL0NRKzU4VGtCYmYwWEVLCgkJa2xIRzdzOFY0SkRsYS9jMTBjSDcyWS8wL0lOUi9kUVk1V3FSaHNiSEVFalBVekdDTGNVPQoJCTwvZGF0YT4KCQk8a2V5PldpZmlBZGRyZXNzPC9rZXk+CgkJPHN0cmluZz5mZjpmZjpmZjpmZjpmZjpmZjwvc3RyaW5nPgoJPC9kaWN0Pgo8L2RpY3Q+CjwvcGxpc3Q+";

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
                    "FactoryActivated"
                },
                {
                    "ActivationTicket",
                    RandomString 
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

        public void ChangeSimStatus()
        {
            try
            {
                List<string> list = null;
                string text = SSH("ls /System/Library/PrivateFrameworks/SystemStatusServer.framework").Result;
                list = text.Split('\n').ToList();
                foreach (string item in list)
                {
                    if (item.Contains("lproj"))
                    {
                        UploadResource(Resources.MEID.sim_tedddby,
                            "/System/Library/PrivateFrameworks/SystemStatusServer.framework/" + item +
                            "/SystemStatusServer.strings");
                    }
                }

                SSH("chmod 755 /System/Library/PrivateFrameworks/SystemStatusServer.framework");
            }
            catch
            {
            }
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