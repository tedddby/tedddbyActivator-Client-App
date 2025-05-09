using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using LibUsbDotNet.DeviceNotify;
using Renci.SshNet;
using System.Net;
using System.Threading;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Linq;



namespace iKingCode_BypassHello
{
    public partial class MainForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        //0 for false, 1 for true.
        private static int deviceInfoLock = 0;

        private static bool isWaiting = true;

        public string iOS = "";

        public string build = "";

        public string type = "";

        private Process iproxy = null;

        private Process ideviceinfo = null;

        public int localShhPort = 2222;

        public int iPhoneShhPort = 44;

        private SshClient sshClient;

        private ScpClient scpClient;

        private bool doNotifix = false;

        private BackgroundWorker checkfotupdatebw;

        private BackgroundWorker checkSerialNumberBw;

        private BackgroundWorker activatorBw;

        private BackgroundWorker NotificationsBw;

        private BackgroundWorker mdmBw;

        public string activationUrl;

        public string recordsUrl;

        public string chainUrl;

        public string service;


        public MainForm()
        {
            InitializeComponent();
            lblStatus.Text = @"Connect a Jailbroken Device";

            labelImei.Text = "Null";
            labelSerial.Text = "Null";
            labelActivationState.Text = "Null";
            labelModel.Text = "Null";
            labelModel1.Text = "Null";
            labelUDID.Text = "Null";
            labelVersion.Text = "Null";
            labelMeid.Text = "Null";
            

            buttonBypass.Enabled = false;
            buttonFixIservices.Enabled = false;
            mdm.Enabled = false;
            carrier.Enabled = false;
            meid.Enabled = false;
            toolStatus.Visible = false;

            this.checkfotupdatebw = new BackgroundWorker();
            this.checkfotupdatebw.DoWork += this.ContactUpdateServer;

            this. checkSerialNumberBw = new BackgroundWorker();
            this.checkfotupdatebw.DoWork += this.checkSN;

            this.activatorBw = new BackgroundWorker();
            this.activatorBw.DoWork += this.Activator;

            this.NotificationsBw = new BackgroundWorker();
            this.NotificationsBw.DoWork += this.NotificationsBwVoid;

            this.mdmBw = new BackgroundWorker();
            this.mdmBw.DoWork += this.mdmBwVoid;
        }

        
        private void ContactUpdateServer(object sender, DoWorkEventArgs e)
        {
            //
            base.BeginInvoke(new Action(delegate ()
            {
                this.lblStatus.Text = "Contacting API Server...";
                this.buttonBypass.Enabled = false;
            }));

            /////

            Thread.Sleep(100);
            string text = string.Empty;
            String V = HttpUtility.UrlEncode("5.3");
            String Send = String.Format("Version={0}", V);
            string url = "https://api.v2.tedddby.com/callback/?q=check4update&v=5.3";
            byte[] byteArray = Encoding.UTF8.GetBytes(Send);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;

            using (Stream webpageStream = webRequest.GetRequestStream())
            {
                webpageStream.Write(byteArray, 0, byteArray.Length);
            }

            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    byte[] data = Convert.FromBase64String(reader.ReadToEnd());
                    text = Encoding.UTF8.GetString(data);
                }
            }

            try
            {
                if (text.Contains("update"))
                {
                    this.Invoke((MethodInvoker)delegate {
                        this.toolStatusPanel.Visible = true;
                        this.toolStatus.Visible = true;
                        this.toolStatus.Text = text;
                    });

                    System.Diagnostics.Process.Start("https://api.v2.tedddby.com/callback/?q=downloadUpdate&v=5.3");
                }

                if (text.Contains("fixing"))
                {

                    this.Invoke((MethodInvoker)delegate {
                        this.toolStatusPanel.Visible = true;
                        this.toolStatus.Visible = true;
                        this.toolStatus.Text = text;
                    });

                }

                if (text.Contains("Announcement"))
                {

                    MessageBox.Show(text, "ANNOUNCEMENT", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.checkSerialNumberBw.RunWorkerAsync();
                }

                if (text.Contains("Twitter"))
                {
                    System.Diagnostics.Process.Start("https://api.v2.tedddby.com/callback/?q=twitter&v=5.3");
                }

                if (text.Contains("page"))
                {
                    System.Diagnostics.Process.Start("https://api.v2.tedddby.com/callback/?q=twitter&v=5.3");
                }

                if (text.Contains("NoUpdate"))
                {
                    this.checkSerialNumberBw.RunWorkerAsync();
                }

            }
            catch(Exception ex) {
                MessageBox.Show("Are you connected to the internet?", "(tedddbyActivator):Error");
            }
        }

        private void checkSN(object sender, DoWorkEventArgs e)
        {
            string request = check();
            JObject response = JObject.Parse(request);
            string snCheck = Base64Decode(response["status"].ToString());

            if(snCheck == "not_registered")
            {
               
                this.lblStatus.Text = "Device Not Registered | Register at tedddby.com";
                System.Diagnostics.Process.Start("https://tedddby.com/"+iOSDevice.SerialNumber+"/callback");
                return;
            }

            if(snCheck == "registered_gsm")
            {
                activationUrl = Base64Decode(response["activation"].ToString());
                recordsUrl = Base64Decode(response["records"].ToString());
                chainUrl = null;
                service = "gsm";

                this.lblStatus.Text = "Your device is registered for GSM Bypass!";
                buttonBypass.Enabled = true;
                return;
            }

            if (snCheck == "registered_meid")
            {
                activationUrl = Base64Decode(response["activation"].ToString());
                recordsUrl = Base64Decode(response["records"].ToString());
                chainUrl = null;
                service = "meid";

                this.lblStatus.Text = "Your device is registered for MEID (No Signal) Bypass!";
                meid.Enabled = true;
                return;
            }

            if (snCheck == "registered_carrier")
            {
                chainUrl = Base64Decode(response["activation"].ToString());
                service = "carrier";
                
                this.lblStatus.Text = "Your device is registered for Carrier Bypass!";
                carrier.Enabled = true;
            }

            if(snCheck == "registered_mdm")
            {
                activationUrl = Base64Decode(response["activation"].ToString());
                service = "mdm";
                
                this.lblStatus.Text = "Your device is registered for MDM Bypass!";
                mdm.Enabled = true;
            }
        }


        private void getFullIdeviceInfo()
        {
            CheckForIllegalCrossThreadCalls = false;

            if (0 == Interlocked.Exchange(ref deviceInfoLock, 1))
            {
                var res = getIdeviceInfo();

                if (res == false)
                {
                    DeviceDisconnect();

                }
                else
                {
                    //buttonBypass.Enabled = true;
                    DeviceConnected();
                }
            }
            else
            {
                Console.WriteLine("   {0} was denied the lock", Thread.CurrentThread.Name);
                //return false;
            }
        }

        private void DeviceDisconnect()
        {
            lblStatus.Text = @"Connect a Jailbroken Device";

            labelImei.Text = "Null";
            labelSerial.Text = "Null";
            labelActivationState.Text = "Null";
            labelModel.Text = "Null";
            labelModel1.Text = "Null";
            labelUDID.Text = "Null";
            labelVersion.Text = "Null";
            labelMeid.Text = "Null";

            DisableAllButtons();

            isWaiting = true;
        }

        private void DeviceConnected()
        {
            this.checkfotupdatebw.RunWorkerAsync();
            MainForm frm1 = (MainForm)Application.OpenForms["MainForm"];

            /*if (iOSDevice.ActivationState.Contains("Activated"))
            {

                buttonFixIservices.Visible = true;

            }*/
            if (iOSDevice.isMEID)
            {
                labelMeid.Text = "MEID Device";
            }
            else
            {
                labelMeid.Text = "GSM Device";
            }

            //buttonBypass.Enabled = true;

            isWaiting = false;
        }


        private bool getIdeviceInfo(string argument = @"")
        {
            CheckForIllegalCrossThreadCalls = false;

            ideviceinfo = new Process();
            ideviceinfo.StartInfo.FileName = Environment.CurrentDirectory + "/files/ideviceinfo.exe";
            ideviceinfo.StartInfo.Arguments = argument;
            ideviceinfo.StartInfo.UseShellExecute = false;
            ideviceinfo.StartInfo.RedirectStandardOutput = true;
            ideviceinfo.StartInfo.CreateNoWindow = true;

            ideviceinfo.Start();

            iOSDevice.isMEID = false;
            iOSDevice.MEID = "";
            iOSDevice.IMEI = "";

            var lines = 0;

            while (!ideviceinfo.StandardOutput.EndOfStream)
            {
                lines++;

                string line = ideviceinfo.StandardOutput.ReadLine();

                var text2 = line.Replace("\r", "");

                if (text2.StartsWith("UniqueDeviceID: "))
                {
                    var text3 = text2.Replace("UniqueDeviceID: ", "");
                    iOSDevice.UDID = text3.Trim();
                    labelUDID.Text = text3.Trim().ToUpper();
                }
                else if (text2.StartsWith("ProductVersion: "))
                {
                    var text3 = text2.Replace("ProductVersion: ", "");
                    iOS = text3;
                    labelVersion.Text = text3;
                    iOSDevice.IOSVersion = text3;
                }
                else if (text2.StartsWith("BuildVersion: "))
                {
                    var text3 = text2.Replace("BuildVersion: ", "");
                    build = text3;
                }
                else if (text2.StartsWith("ProductType: "))
                {
                    var text3 = text2.Replace("ProductType: ", "");
                    type = text3;
                    labelModel1.Text = type;

                    iOSDevice.setProductType(type);

                    labelModel.Text = iOSDevice.Model;
                }

                var split = line.Split(new char[] { ':' }); //Split the string based on specified value

                if (split[0] == "SerialNumber")
                {
                    iOSDevice.SerialNumber = split[1].Trim();
                    labelSerial.Text = split[1].Trim();
                }

                if (split[0] == "InternationalMobileEquipmentIdentity")
                {
                    iOSDevice.IMEI = split[1].Trim();
                    labelImei.Text = split[1].Trim();
                }

                if (split[0] == "ActivationState")
                {
                    iOSDevice.ActivationState = split[1].Trim();
                    labelActivationState.Text = split[1].Trim();
                }

                if (split[0] == "SIMStatus")
                {
                    iOSDevice.SIMStatus = split[1].Trim();
                    iOSDevice.isSIMInserted = iOSDevice.SIMStatus == "kCTSIMSupportSIMStatusReady" ^ iOSDevice.SIMStatus == "kCTSIMSupportSIMStatusPINLocked";
                }

                if (line.Contains("MobileEquipmentIdentifier"))
                {
                    iOSDevice.MEID = split[1].Trim();
                }
            }

            iOSDevice.isMEID = iOSDevice.clasifyGSMorMEID();

            if (iOSDevice.isMEID)
            {
                labelMeid.Text = "MEID Device"; // IPADS / IPODS / iPhone 5s Global, 6, SE, 6s, 7_CDMA, 8_CDMA, X_CDMA 
            }
            else
            {
                labelMeid.Text = "GSM Device"; // 7_GSM, 8_GSM, X_GSM 
            }

            Interlocked.Exchange(ref deviceInfoLock, 0);

            if (lines <= 2)
            {
                return false;
            }

            return true;
        }

        public static IDeviceNotifier UsbDeviceNotifier = DeviceNotifier.OpenDeviceNotifier();

        private void OnDeviceNotifyEvent(object sender, DeviceNotifyEventArgs e)
        {
            if (e.EventType.ToString() == "DeviceRemoveComplete")
            {
                getFullIdeviceInfo();
            }
            else
            {
                getFullIdeviceInfo();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UsbDeviceNotifier.OnDeviceNotify += OnDeviceNotifyEvent;

            var text = "Double click to copy";

            new ToolTip().SetToolTip(labelImei, text);
            new ToolTip().SetToolTip(labelSerial, text);
            new ToolTip().SetToolTip(labelUDID, text);
            new ToolTip().SetToolTip(labelModel1, text);
            new ToolTip().SetToolTip(labelVersion, text);

            backgroundWorkerEventLoop.RunWorkerAsync();
        }

        private void backgroundWorkerBypass_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

           // buttonBypass.Enabled = false;
            //buttonFixIservices.Enabled = false;
            
        }

        private void Activator(object sender, DoWorkEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            try
            {
                cerrarIProxiesProcess();
            }
            catch (Exception ex)
            {
            }

            try
            {
                iPhoneShhPort = 44;
                RunIProxy(localShhPort, iPhoneShhPort);
                testSshConnection("127.0.0.1", localShhPort, "alpine");
            }
            catch (Exception ex)
            {
                var mensaje = "Jailbreak your device with CHECKRA1N!";
                MessageBox.Show(mensaje, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                reportProgress(0, mensaje);
                return;
            }

            switch (service)
            {
                case "gsm":

                    this.lblStatus.Text = "Identifying service...";

                    string[] supportedDevices = { "14.0", "14.0.1", "14.1", "14.2", "14.3", "14.4", "14.4.1", "14.4.2", "14.5", "14.5.1", "14.7" };

                    int supportedGSM = 1;

                    if (iOSDevice.IOSVersion.Contains("14"))
                    {
                        string iosV = iOSDevice.IOSVersion;
                        if (supportedDevices.Contains(iosV))
                        {
                            supportedGSM = 1;
                            this.lblStatus.Text = "Signal Supported...Processing...";
                        }
                        else
                        {
                            supportedGSM = 0;
                            this.lblStatus.Text = "Signal NOT Supported...Processing...";
                        }
                    }
                    else
                    {
                        supportedGSM = 1;
                    }

                    if(iOSDevice.isMEID == true)
                    {
                        this.lblStatus.Text = "MEID Device.. Doing MEID Bypass...";

                        DisableAllButtons();
                        this.buttonBypass.Text = "Activating...";
                        this.lblStatus.Text = "(tedddbyActivator): Activating iDevice... (MEID $7 USD)";

                        DoBypassMEID(activationUrl, recordsUrl);

                        reportProgress(0, "0");
                        this.buttonBypass.Text = "GSM Bypass";
                        this.buttonBypass.Enabled = true;
                        this.buttonFixIservices.Enabled = true;
                        this.lblStatus.Text = "(tedddbyActivator): Successfully Activated iDevice!";
                        return;
                    }
                    else
                    {
                        if (iOSDevice.isMEID == false && supportedGSM == 1)
                        {
                            DisableAllButtons();
                            this.buttonBypass.Text = "Activating...";
                            this.lblStatus.Text = "(tedddbyActivator): Activating iDevice...(With Signal)";

                            DoBypassGSM(activationUrl, recordsUrl);

                            reportProgress(0, "0");
                            this.buttonBypass.Text = "GSM Bypass";
                            this.buttonBypass.Enabled = true;
                            this.buttonFixIservices.Enabled = true;
                            this.lblStatus.Text = "(tedddbyActivator): Successfully Activated iDevice!";
                            return;
                        }
                        else
                        {
                            DisableAllButtons();
                            this.buttonBypass.Text = "Activating...";
                            this.lblStatus.Text = "(tedddbyActivator): Activating iDevice...(Signal Not Supported)";

                            //string activModified = activationUrl.Replace("/gsm/", "/meid/"), recModified = recordsUrl.Replace("/gsm/", "/meid/");

                            DoBypassGSM_NoSignal(activationUrl, recordsUrl);

                            reportProgress(0, "0");
                            this.buttonBypass.Text = "GSM Bypass";
                            this.buttonBypass.Enabled = true;
                            this.buttonFixIservices.Enabled = true;
                            this.lblStatus.Text = "(tedddbyActivator): Successfully Activated iDevice!";
                            return;
                        }
                    }
                    break;

                case "meid":

                    DisableAllButtons();
                    this.meid.Text = "Activating...";
                    this.lblStatus.Text = "(tedddbyActivator): Activating iDevice...";

                    DoBypassMEID(activationUrl, recordsUrl);

                    reportProgress(0, "0");
                    this.meid.Text = "MEID Bypass";
                    this.meid.Enabled = true;
                    this.buttonFixIservices.Enabled = true;
                    this.lblStatus.Text = "(tedddbyActivator): Successfully Activated iDevice!";
                    return;
                    break;

                case "carrier":
                    DisableAllButtons();
                    this.carrier.Text = "Activating...";
                    this.lblStatus.Text = "(tedddbyActivator): Bypassing Carrier Lock...";

                    DoBypassCarrier(chainUrl);

                    reportProgress(0, "0");
                    this.carrier.Text = "Carrier Bypass";
                    this.carrier.Enabled = true;
                    this.lblStatus.Text = "(tedddbyActivator): Successfully Bypassed Carrier!";
                    break;

                default:
                    MessageBox.Show("Unknow service -10999, Message support on Telegram @tedddby", "(tedddbyActivator): Unknown Service");
                    break;
            }
        }



        public void DoBypassGSM(string activLink, string recLink)
        {
           GSMHelloBypass GsmBypass = new GSMHelloBypass();
           GsmBypass.DoBypass(activLink, recLink);
        }

        public void DoBypassGSM_NoSignal(string activLink, string recLink)
        {
            MEIDHelloBypass MEIDBypass = new MEIDHelloBypass();
            MEIDBypass.DoBypass_GSM_No_Signal(activLink, recLink);
        }

        public void DoBypassMEID(string activLink, string recLink)
        {
            MEIDHelloBypass MEIDBypass = new MEIDHelloBypass();
            MEIDBypass.DoBypass(activLink, recLink);
        }

        public void DoBypassNotiFix()
        {
            GSMHelloBypass GsmBypass = new GSMHelloBypass();
            GsmBypass.DoNotiFix();
        }

        public void DoBypassCarrier(string chainLink)
        {
            GSMHelloBypass CarrierBypass = new GSMHelloBypass();
            CarrierBypass.carrier(chainLink);
        }

        public void DoBypassMDM(string chainLink)
        {
            GSMHelloBypass MDMBypass = new GSMHelloBypass();
            MDMBypass.MDM_ACTIVATION(activationUrl, build);
        }

        private void mdmBwVoid(object sender, DoWorkEventArgs e)
        {
            DisableAllButtons();
            this.mdm.Text = "Activating...";
            this.lblStatus.Text = "(tedddbyActivator): Bypassing MDM Lock...";

            DoBypassMDM(chainUrl);

            reportProgress(0, "0");
            this.mdm.Text = "MDM Byass (No Jailbreak)";
            this.mdm.Enabled = true;
            this.lblStatus.Text = "(tedddbyActivator): Successfully Bypassed MDM!";
        }

        private void cerrarIProxiesProcess()
        {
            var processesByName = Process.GetProcessesByName("iproxy");
            if (processesByName.Length >= 1)
            {
                foreach (var process in processesByName) process.Kill();
                iproxy = null;
            }
        }

        public void RunIProxy(int localPort, int remotePort)
        {
            var text = Environment.CurrentDirectory + "/files/iproxy.exe";

            if (iproxy == null && File.Exists(text) || iproxy.HasExited)
            {
                iproxy = new Process();
                iproxy.StartInfo.FileName = text;
                iproxy.StartInfo.Arguments = localPort.ToString() + " " + remotePort.ToString();
                iproxy.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                iproxy.Start();
            }
            else if (!File.Exists(text))
            {
                return;
            }
        }

        public void testSshConnection(string host, int port, string password)
        {
            var authenticationMethods = new AuthenticationMethod[]
            {
                new PasswordAuthenticationMethod("root", password)
            };
            var connectionInfo = new ConnectionInfo(host, port, "root", authenticationMethods);

            var sshClient = new SshClient(connectionInfo);

            sshClient.Connect();
            sshClient.Disconnect();
        }

        private void backgroundWorkerBypass_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerBypass_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            progressBar1.Value = 0;
            //buttonBypass.Enabled = true;
            //buttonFixIservices.Enabled = true;
        }

        public void reportProgress(int perCent, string label)
        {
            backgroundWorkerBypass.ReportProgress(perCent);
            labelDebug.Text = perCent.ToString() + "%";
            if (iOSDevice.debugMode) Console.WriteLine(label);
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public string Base64Decode(string base64EncodedData)
        {
            byte[] bytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(bytes);
        }

        public string check()
        {
            this.lblStatus.Text = "Checking Serial...";
            String SerialNumber = iOSDevice.SerialNumber; //
            String Send = String.Format("auth={0}", "/////");

            string CheckResponse = string.Empty;

            string url = "https://api.v2.tedddby.com/callback/serialCheck/" + SerialNumber;
            byte[] byteArray = Encoding.UTF8.GetBytes(Send);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            using (Stream webpageStream = webRequest.GetRequestStream())
            {
                webpageStream.Write(byteArray, 0, byteArray.Length);
            }
            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    CheckResponse = Base64Decode(reader.ReadToEnd());

                }
            }
            return CheckResponse;
        }

        private static void RunAsSTAThread(Action goForIt)
        {
            AutoResetEvent @event = new AutoResetEvent(false);
            Thread thread = new Thread((ThreadStart)(() =>
            {
                goForIt();
                @event.Set();
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            @event.WaitOne();
        }

        private void siticoneControlBoxClose_Click(object sender, EventArgs e)
        {
            try
            {
                cerrarIProxiesProcess();
            }
            catch (Exception ex)
            {
            }

            Environment.Exit(0);
        }

        private void backgroundWorkerEventLoop_DoWork_1(object sender, DoWorkEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            MainForm frm1 = (MainForm)Application.OpenForms["MainForm"];

            Thread.Sleep(200);

            string pairResult = "NO_DEVICE_FOUND";

            isWaiting = true;

            while (true)
            {

                if (isWaiting == true)
                {
                    isWaiting = false;

                    while (true)
                    {
                        frm1.Invoke((MethodInvoker)(() =>
                        {
                            pairResult = frm1.PairValidate();
                        }));

                        if (pairResult == "NO_DEVICE_FOUND")
                        {
                            //continue
                        }
                        else if (pairResult == "SUCCESS")
                        {
                            break;
                        }
                        else
                        {

                            frm1.Invoke((MethodInvoker)(() =>
                            {
                                pairResult = frm1.PairPair();
                            }));

                            if (pairResult == "NO_DEVICE_FOUND")
                            {
                                //continue
                            }

                            if (pairResult == "SUCCESS")
                            {
                                break;
                            }

                            if (pairResult == "TRUST_ERROR")
                            {
                                MessageBox.Show(this, "Accept trust dialog on the iPhone screen", "Important", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            if (pairResult == "PASSCODE_ERROR")
                            {
                                MessageBox.Show(this, "Please enter the passcode on the device and retry", "Important", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            if (pairResult == "UNKNOW_ERROR")
                            {
                                //continue
                            }
                        }

                        Thread.Sleep(2000);

                        Console.WriteLine("Event loop 1");
                    }

                    frm1.Invoke((MethodInvoker)(() => frm1.getFullIdeviceInfo()));

                    Console.WriteLine("Event loop 2");
                }

                Thread.Sleep(1000);
            }
        }

        public string PairValidate()
        {
            string path = Directory.GetCurrentDirectory();
            Process proc;

            try
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

                try
                {
                    proc.Start();

                    StreamReader reader = proc.StandardOutput;
                    string result = reader.ReadToEnd();

                    reader.Dispose();

                    if (result.Contains("SUCCESS"))
                    {
                        return "SUCCESS";
                    }
                    else if (result.Contains("No device found"))
                    {
                        return "NO_DEVICE_FOUND";
                    }
                    else
                    {
                        return "UNKNOW_ERROR";
                    }
                }
                catch
                {
                    Thread.Sleep(1000);
                    try
                    {
                        proc.Kill();
                    }
                    catch
                    {
                    }
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                //MessageBox.Show("UNKNOW_ERROR");
            }

            return "UNKNOW_ERROR";
        }

        public string PairPair()
        {
            string path = Directory.GetCurrentDirectory();
            Process proc;

            try
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

                try
                {
                    proc.Start();

                    StreamReader reader = proc.StandardOutput;
                    string result = reader.ReadToEnd();

                    reader.Dispose();

                    if (result.Contains("SUCCESS"))
                    {
                        return "SUCCESS";
                    }
                    else if (result.Contains("No device found"))
                    {
                        return "NO_DEVICE_FOUND";
                    }
                    else if (result.Contains("ERROR: Please accept the trust dialog"))
                    {
                        return "TRUST_ERROR";
                    }
                    else if (result.Contains("ERROR: Could not validate with device"))
                    {
                        return "PASSCODE_ERROR";
                    }
                    else
                    {
                        return "UNKNOW_ERROR";
                    }
                }
                catch
                {
                    Thread.Sleep(1000);
                    try
                    {
                        proc.Kill();
                    }
                    catch
                    {
                    }
                }

            }
            catch (System.ComponentModel.Win32Exception)
            {
                //MessageBox.Show("UNKNOW_ERROR");
            }

            return "UNKNOW_ERROR";
        }

        public void DisableAllButtons()
        {
            buttonBypass.Enabled = false;
            buttonFixIservices.Enabled = false;
            mdm.Enabled = false;
            carrier.Enabled = false;
            meid.Enabled = false;
        }

        private void NotificationsBwVoid(object sender, DoWorkEventArgs e) {
            DisableAllButtons();
            this.buttonFixIservices.Text = "Fixing...";
            this.lblStatus.Text = "(tedddbyActivator): Fixing Notifications...";

            DoBypassNotiFix();

            reportProgress(0, "0");
            this.buttonFixIservices.Text = "Fix Apple iServices";
            this.buttonFixIservices.Enabled = true;
            this.lblStatus.Text = "(tedddbyActivator): Notifications Fixed Successfully!";
        }


        private void buttonFixIservices_Click(object sender, EventArgs e)
        {
            this.NotificationsBw.RunWorkerAsync();
        }

        private void buttonBypass_Click(object sender, EventArgs e)
        {
            activatorBw.RunWorkerAsync();
        }

        private void buttonCopySerial_Click(object sender, EventArgs e)
        {
            MainForm.RunAsSTAThread((Action)(() => Clipboard.SetText(iOSDevice.SerialNumber)));
        }

        private void pnMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolStatusPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void carrier_Click(object sender, EventArgs e)
        {
            this.activatorBw.RunWorkerAsync();
        }

        private void meid_Click(object sender, EventArgs e)
        {
            this.activatorBw.RunWorkerAsync();
        }

        private void mdm_Click(object sender, EventArgs e)
        {
            this.mdmBw.RunWorkerAsync();
        }
    }
}