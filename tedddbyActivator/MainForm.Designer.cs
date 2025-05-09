namespace iKingCode_BypassHello
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label14 = new System.Windows.Forms.Label();
            this.pnMain = new System.Windows.Forms.Panel();
            this.toolStatusPanel = new System.Windows.Forms.Panel();
            this.toolStatus = new System.Windows.Forms.RichTextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.meid = new Siticone.UI.WinForms.SiticoneGradientButton();
            this.mdm = new Siticone.UI.WinForms.SiticoneGradientButton();
            this.carrier = new Siticone.UI.WinForms.SiticoneGradientButton();
            this.labelModel = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.buttonFixIservices = new Siticone.UI.WinForms.SiticoneGradientButton();
            this.buttonBypass = new Siticone.UI.WinForms.SiticoneGradientButton();
            this.progressBar1 = new Siticone.UI.WinForms.SiticoneProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelActivationState = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelImei = new System.Windows.Forms.Label();
            this.labelMeid = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelDebug = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelUDID = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSerial = new System.Windows.Forms.Label();
            this.labelModel1 = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.siticoneControlBox2 = new Siticone.UI.WinForms.SiticoneControlBox();
            this.siticoneControlBoxClose = new Siticone.UI.WinForms.SiticoneControlBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorkerEventLoop = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerBypass = new System.ComponentModel.BackgroundWorker();
            this.pnMain.SuspendLayout();
            this.toolStatusPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(168, 308);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(0, 23);
            this.label14.TabIndex = 29;
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnMain
            // 
            this.pnMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(40)))), ((int)(((byte)(54)))));
            this.pnMain.Controls.Add(this.toolStatusPanel);
            this.pnMain.Controls.Add(this.meid);
            this.pnMain.Controls.Add(this.mdm);
            this.pnMain.Controls.Add(this.carrier);
            this.pnMain.Controls.Add(this.labelModel);
            this.pnMain.Controls.Add(this.label15);
            this.pnMain.Controls.Add(this.buttonFixIservices);
            this.pnMain.Controls.Add(this.buttonBypass);
            this.pnMain.Controls.Add(this.progressBar1);
            this.pnMain.Controls.Add(this.lblStatus);
            this.pnMain.Controls.Add(this.label12);
            this.pnMain.Controls.Add(this.labelActivationState);
            this.pnMain.Controls.Add(this.label10);
            this.pnMain.Controls.Add(this.labelImei);
            this.pnMain.Controls.Add(this.labelMeid);
            this.pnMain.Controls.Add(this.label3);
            this.pnMain.Controls.Add(this.labelDebug);
            this.pnMain.Controls.Add(this.label6);
            this.pnMain.Controls.Add(this.label5);
            this.pnMain.Controls.Add(this.labelUDID);
            this.pnMain.Controls.Add(this.label4);
            this.pnMain.Controls.Add(this.label2);
            this.pnMain.Controls.Add(this.labelSerial);
            this.pnMain.Controls.Add(this.labelModel1);
            this.pnMain.Controls.Add(this.labelVersion);
            this.pnMain.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnMain.Location = new System.Drawing.Point(0, 42);
            this.pnMain.Margin = new System.Windows.Forms.Padding(2);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(659, 492);
            this.pnMain.TabIndex = 5;
            this.pnMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnMain_Paint);
            // 
            // toolStatusPanel
            // 
            this.toolStatusPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(40)))), ((int)(((byte)(54)))));
            this.toolStatusPanel.Controls.Add(this.toolStatus);
            this.toolStatusPanel.Controls.Add(this.label20);
            this.toolStatusPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStatusPanel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStatusPanel.Location = new System.Drawing.Point(2, 2);
            this.toolStatusPanel.Margin = new System.Windows.Forms.Padding(2);
            this.toolStatusPanel.Name = "toolStatusPanel";
            this.toolStatusPanel.Size = new System.Drawing.Size(659, 492);
            this.toolStatusPanel.TabIndex = 59;
            this.toolStatusPanel.Visible = false;
            this.toolStatusPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.toolStatusPanel_Paint);
            // 
            // toolStatus
            // 
            this.toolStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(40)))), ((int)(((byte)(54)))));
            this.toolStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.toolStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStatus.ForeColor = System.Drawing.Color.White;
            this.toolStatus.Location = new System.Drawing.Point(13, 27);
            this.toolStatus.Margin = new System.Windows.Forms.Padding(4);
            this.toolStatus.Name = "toolStatus";
            this.toolStatus.ReadOnly = true;
            this.toolStatus.Size = new System.Drawing.Size(628, 376);
            this.toolStatus.TabIndex = 22;
            this.toolStatus.Text = "";
            this.toolStatus.Visible = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(842, 438);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(29, 17);
            this.label20.TabIndex = 21;
            this.label20.Text = "0%";
            // 
            // meid
            // 
            this.meid.BorderRadius = 5;
            this.meid.CheckedState.Parent = this.meid;
            this.meid.CustomImages.Parent = this.meid;
            this.meid.FillColor = System.Drawing.Color.LightBlue;
            this.meid.FillColor2 = System.Drawing.Color.Cyan;
            this.meid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.meid.ForeColor = System.Drawing.Color.Black;
            this.meid.HoveredState.Parent = this.meid;
            this.meid.Location = new System.Drawing.Point(336, 254);
            this.meid.Margin = new System.Windows.Forms.Padding(2);
            this.meid.Name = "meid";
            this.meid.ShadowDecoration.Parent = this.meid;
            this.meid.Size = new System.Drawing.Size(305, 58);
            this.meid.TabIndex = 58;
            this.meid.Text = "MEID Bypass";
            this.meid.Click += new System.EventHandler(this.meid_Click);
            // 
            // mdm
            // 
            this.mdm.BorderRadius = 5;
            this.mdm.CheckedState.Parent = this.mdm;
            this.mdm.CustomImages.Parent = this.mdm;
            this.mdm.FillColor = System.Drawing.Color.LightBlue;
            this.mdm.FillColor2 = System.Drawing.Color.Cyan;
            this.mdm.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mdm.ForeColor = System.Drawing.Color.Black;
            this.mdm.HoveredState.Parent = this.mdm;
            this.mdm.Location = new System.Drawing.Point(336, 316);
            this.mdm.Margin = new System.Windows.Forms.Padding(2);
            this.mdm.Name = "mdm";
            this.mdm.ShadowDecoration.Parent = this.mdm;
            this.mdm.Size = new System.Drawing.Size(305, 58);
            this.mdm.TabIndex = 57;
            this.mdm.Text = "MDM Bypass (No Jailbreak)";
            this.mdm.Click += new System.EventHandler(this.mdm_Click);
            // 
            // carrier
            // 
            this.carrier.BorderRadius = 5;
            this.carrier.CheckedState.Parent = this.carrier;
            this.carrier.CustomImages.Parent = this.carrier;
            this.carrier.FillColor = System.Drawing.Color.LightBlue;
            this.carrier.FillColor2 = System.Drawing.Color.Cyan;
            this.carrier.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carrier.ForeColor = System.Drawing.Color.Black;
            this.carrier.HoveredState.Parent = this.carrier;
            this.carrier.Location = new System.Drawing.Point(27, 316);
            this.carrier.Margin = new System.Windows.Forms.Padding(2);
            this.carrier.Name = "carrier";
            this.carrier.ShadowDecoration.Parent = this.carrier;
            this.carrier.Size = new System.Drawing.Size(305, 58);
            this.carrier.TabIndex = 56;
            this.carrier.Text = "Carrier Bypass";
            this.carrier.Click += new System.EventHandler(this.carrier_Click);
            // 
            // labelModel
            // 
            this.labelModel.AutoSize = true;
            this.labelModel.BackColor = System.Drawing.Color.Transparent;
            this.labelModel.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModel.ForeColor = System.Drawing.Color.White;
            this.labelModel.Location = new System.Drawing.Point(96, 165);
            this.labelModel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(41, 23);
            this.labelModel.TabIndex = 55;
            this.labelModel.Text = "N/A";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(35, 165);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 23);
            this.label15.TabIndex = 54;
            this.label15.Text = "Model:";
            // 
            // buttonFixIservices
            // 
            this.buttonFixIservices.BorderRadius = 5;
            this.buttonFixIservices.CheckedState.Parent = this.buttonFixIservices;
            this.buttonFixIservices.CustomImages.Parent = this.buttonFixIservices;
            this.buttonFixIservices.FillColor = System.Drawing.Color.LightBlue;
            this.buttonFixIservices.FillColor2 = System.Drawing.Color.Cyan;
            this.buttonFixIservices.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFixIservices.ForeColor = System.Drawing.Color.Black;
            this.buttonFixIservices.HoveredState.Parent = this.buttonFixIservices;
            this.buttonFixIservices.Location = new System.Drawing.Point(27, 379);
            this.buttonFixIservices.Margin = new System.Windows.Forms.Padding(2);
            this.buttonFixIservices.Name = "buttonFixIservices";
            this.buttonFixIservices.ShadowDecoration.Parent = this.buttonFixIservices;
            this.buttonFixIservices.Size = new System.Drawing.Size(614, 58);
            this.buttonFixIservices.TabIndex = 53;
            this.buttonFixIservices.Text = "Fix Apple iServices";
            this.buttonFixIservices.Click += new System.EventHandler(this.buttonFixIservices_Click);
            // 
            // buttonBypass
            // 
            this.buttonBypass.BorderRadius = 5;
            this.buttonBypass.CheckedState.Parent = this.buttonBypass;
            this.buttonBypass.CustomImages.Parent = this.buttonBypass;
            this.buttonBypass.FillColor = System.Drawing.Color.LightBlue;
            this.buttonBypass.FillColor2 = System.Drawing.Color.Cyan;
            this.buttonBypass.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBypass.ForeColor = System.Drawing.Color.Black;
            this.buttonBypass.HoveredState.Parent = this.buttonBypass;
            this.buttonBypass.Location = new System.Drawing.Point(27, 254);
            this.buttonBypass.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBypass.Name = "buttonBypass";
            this.buttonBypass.ShadowDecoration.Parent = this.buttonBypass;
            this.buttonBypass.Size = new System.Drawing.Size(305, 58);
            this.buttonBypass.TabIndex = 51;
            this.buttonBypass.Text = "GSM Bypass";
            this.buttonBypass.Click += new System.EventHandler(this.buttonBypass_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.BorderRadius = 3;
            this.progressBar1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(68)))), ((int)(((byte)(86)))));
            this.progressBar1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.progressBar1.Location = new System.Drawing.Point(28, 453);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.ProgressColor = System.Drawing.Color.LightBlue;
            this.progressBar1.ProgressColor2 = System.Drawing.Color.Cyan;
            this.progressBar1.ShadowDecoration.Parent = this.progressBar1;
            this.progressBar1.Size = new System.Drawing.Size(613, 13);
            this.progressBar1.TabIndex = 50;
            this.progressBar1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(34, 29);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(267, 25);
            this.lblStatus.TabIndex = 44;
            this.lblStatus.Text = "Connect a Jailbroken Device ";
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(35, 135);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(143, 23);
            this.label12.TabIndex = 43;
            this.label12.Text = "Activation Status:";
            // 
            // labelActivationState
            // 
            this.labelActivationState.AutoSize = true;
            this.labelActivationState.BackColor = System.Drawing.Color.Transparent;
            this.labelActivationState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelActivationState.ForeColor = System.Drawing.Color.White;
            this.labelActivationState.Location = new System.Drawing.Point(181, 138);
            this.labelActivationState.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelActivationState.Name = "labelActivationState";
            this.labelActivationState.Size = new System.Drawing.Size(41, 23);
            this.labelActivationState.TabIndex = 38;
            this.labelActivationState.Text = "N/A";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(387, 75);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 23);
            this.label10.TabIndex = 41;
            this.label10.Text = "iDeviceType:";
            // 
            // labelImei
            // 
            this.labelImei.AutoSize = true;
            this.labelImei.BackColor = System.Drawing.Color.Transparent;
            this.labelImei.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelImei.ForeColor = System.Drawing.Color.White;
            this.labelImei.Location = new System.Drawing.Point(82, 75);
            this.labelImei.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelImei.Name = "labelImei";
            this.labelImei.Size = new System.Drawing.Size(63, 23);
            this.labelImei.TabIndex = 10;
            this.labelImei.Text = "N/AAA";
            // 
            // labelMeid
            // 
            this.labelMeid.AutoSize = true;
            this.labelMeid.BackColor = System.Drawing.Color.Transparent;
            this.labelMeid.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMeid.ForeColor = System.Drawing.Color.White;
            this.labelMeid.Location = new System.Drawing.Point(492, 75);
            this.labelMeid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMeid.Name = "labelMeid";
            this.labelMeid.Size = new System.Drawing.Size(41, 23);
            this.labelMeid.TabIndex = 40;
            this.labelMeid.Text = "N/A";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(35, 75);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "IMEI:";
            // 
            // labelDebug
            // 
            this.labelDebug.AutoSize = true;
            this.labelDebug.BackColor = System.Drawing.Color.Transparent;
            this.labelDebug.ForeColor = System.Drawing.Color.White;
            this.labelDebug.Location = new System.Drawing.Point(842, 438);
            this.labelDebug.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDebug.Name = "labelDebug";
            this.labelDebug.Size = new System.Drawing.Size(29, 17);
            this.labelDebug.TabIndex = 21;
            this.labelDebug.Text = "0%";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(387, 105);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 23);
            this.label6.TabIndex = 16;
            this.label6.Text = "Product Type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(387, 135);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 23);
            this.label5.TabIndex = 14;
            this.label5.Text = "iOS Version:";
            // 
            // labelUDID
            // 
            this.labelUDID.AutoSize = true;
            this.labelUDID.BackColor = System.Drawing.Color.Transparent;
            this.labelUDID.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUDID.ForeColor = System.Drawing.Color.White;
            this.labelUDID.Location = new System.Drawing.Point(85, 195);
            this.labelUDID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelUDID.Name = "labelUDID";
            this.labelUDID.Size = new System.Drawing.Size(41, 23);
            this.labelUDID.TabIndex = 13;
            this.labelUDID.Text = "N/A";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(35, 195);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 23);
            this.label4.TabIndex = 12;
            this.label4.Text = "UDID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(35, 105);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Serial Number:";
            // 
            // labelSerial
            // 
            this.labelSerial.AutoSize = true;
            this.labelSerial.BackColor = System.Drawing.Color.Transparent;
            this.labelSerial.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSerial.ForeColor = System.Drawing.Color.White;
            this.labelSerial.Location = new System.Drawing.Point(158, 105);
            this.labelSerial.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSerial.Name = "labelSerial";
            this.labelSerial.Size = new System.Drawing.Size(41, 23);
            this.labelSerial.TabIndex = 8;
            this.labelSerial.Text = "N/A";
            // 
            // labelModel1
            // 
            this.labelModel1.AutoSize = true;
            this.labelModel1.BackColor = System.Drawing.Color.Transparent;
            this.labelModel1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModel1.ForeColor = System.Drawing.Color.White;
            this.labelModel1.Location = new System.Drawing.Point(500, 105);
            this.labelModel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelModel1.Name = "labelModel1";
            this.labelModel1.Size = new System.Drawing.Size(41, 23);
            this.labelModel1.TabIndex = 17;
            this.labelModel1.Text = "N/A";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.BackColor = System.Drawing.Color.Transparent;
            this.labelVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Location = new System.Drawing.Point(490, 135);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(46, 23);
            this.labelVersion.TabIndex = 15;
            this.labelVersion.Text = "N/A ";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(68)))), ((int)(((byte)(86)))));
            this.panel2.Controls.Add(this.siticoneControlBox2);
            this.panel2.Controls.Add(this.siticoneControlBoxClose);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(659, 45);
            this.panel2.TabIndex = 23;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            // 
            // siticoneControlBox2
            // 
            this.siticoneControlBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.siticoneControlBox2.BackColor = System.Drawing.Color.Transparent;
            this.siticoneControlBox2.ControlBoxType = Siticone.UI.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.siticoneControlBox2.FillColor = System.Drawing.Color.Transparent;
            this.siticoneControlBox2.HoveredState.Parent = this.siticoneControlBox2;
            this.siticoneControlBox2.IconColor = System.Drawing.Color.White;
            this.siticoneControlBox2.Location = new System.Drawing.Point(581, 2);
            this.siticoneControlBox2.Margin = new System.Windows.Forms.Padding(2);
            this.siticoneControlBox2.Name = "siticoneControlBox2";
            this.siticoneControlBox2.ShadowDecoration.Parent = this.siticoneControlBox2;
            this.siticoneControlBox2.Size = new System.Drawing.Size(32, 38);
            this.siticoneControlBox2.TabIndex = 3;
            // 
            // siticoneControlBoxClose
            // 
            this.siticoneControlBoxClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.siticoneControlBoxClose.FillColor = System.Drawing.Color.Transparent;
            this.siticoneControlBoxClose.HoveredState.Parent = this.siticoneControlBoxClose;
            this.siticoneControlBoxClose.IconColor = System.Drawing.Color.White;
            this.siticoneControlBoxClose.Location = new System.Drawing.Point(621, 2);
            this.siticoneControlBoxClose.Margin = new System.Windows.Forms.Padding(2);
            this.siticoneControlBoxClose.Name = "siticoneControlBoxClose";
            this.siticoneControlBoxClose.ShadowDecoration.Parent = this.siticoneControlBoxClose;
            this.siticoneControlBoxClose.Size = new System.Drawing.Size(32, 38);
            this.siticoneControlBoxClose.TabIndex = 2;
            this.siticoneControlBoxClose.Click += new System.EventHandler(this.siticoneControlBoxClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(34, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "tedddbyActivator V5.0.3";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // backgroundWorkerEventLoop
            // 
            this.backgroundWorkerEventLoop.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerEventLoop_DoWork_1);
            // 
            // backgroundWorkerBypass
            // 
            this.backgroundWorkerBypass.WorkerReportsProgress = true;
            this.backgroundWorkerBypass.WorkerSupportsCancellation = true;
            this.backgroundWorkerBypass.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerBypass_DoWork);
            this.backgroundWorkerBypass.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerBypass_ProgressChanged);
            this.backgroundWorkerBypass.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerBypass_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(40)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(659, 535);
            this.Controls.Add(this.pnMain);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "tedddbyActivator V5.0.3";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnMain.ResumeLayout(false);
            this.pnMain.PerformLayout();
            this.toolStatusPanel.ResumeLayout(false);
            this.toolStatusPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelActivationState;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.Label labelImei;
        private System.Windows.Forms.Label labelMeid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelDebug;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelUDID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelSerial;
        private System.Windows.Forms.Label labelModel1;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label14;
        private Siticone.UI.WinForms.SiticoneProgressBar progressBar1;
        private Siticone.UI.WinForms.SiticoneControlBox siticoneControlBox2;
        private Siticone.UI.WinForms.SiticoneControlBox siticoneControlBoxClose;
        private Siticone.UI.WinForms.SiticoneGradientButton buttonBypass;
        private Siticone.UI.WinForms.SiticoneGradientButton buttonFixIservices;
        public System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.Label label15;
        private System.ComponentModel.BackgroundWorker backgroundWorkerEventLoop;
        private System.Windows.Forms.Label lblStatus;
        private Siticone.UI.WinForms.SiticoneGradientButton mdm;
        private Siticone.UI.WinForms.SiticoneGradientButton carrier;
        private Siticone.UI.WinForms.SiticoneGradientButton meid;
        private System.Windows.Forms.Panel toolStatusPanel;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.RichTextBox toolStatus;
        private System.ComponentModel.BackgroundWorker backgroundWorkerBypass;
    }
}

