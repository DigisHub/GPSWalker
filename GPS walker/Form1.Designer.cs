namespace GPS_walker
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.map = new GMap.NET.WindowsForms.GMapControl();
            this.btnSetPosition = new System.Windows.Forms.Button();
            this.txtLat = new System.Windows.Forms.TextBox();
            this.chkFollow = new System.Windows.Forms.CheckBox();
            this.txtLong = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnAdbConnect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.txtdestLat = new System.Windows.Forms.TextBox();
            this.txtDestLong = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkRoads = new System.Windows.Forms.CheckBox();
            this.chkGo = new System.Windows.Forms.CheckBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtETA = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.txtAdbResult = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // map
            // 
            this.map.Bearing = 0F;
            this.map.CanDragMap = true;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.EmptyTileColor = System.Drawing.Color.Navy;
            this.map.GrayScaleMode = false;
            this.map.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.map.LevelsKeepInMemmory = 5;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.MarkersEnabled = true;
            this.map.MaxZoom = 18;
            this.map.MinZoom = 8;
            this.map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            this.map.Name = "map";
            this.map.NegativeMode = false;
            this.map.PolygonsEnabled = true;
            this.map.RetryLoadTile = 0;
            this.map.RoutesEnabled = true;
            this.map.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.map.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.map.ShowTileGridLines = false;
            this.map.Size = new System.Drawing.Size(816, 722);
            this.map.TabIndex = 0;
            this.map.Zoom = 13D;
            this.map.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.map_MouseDoubleClick);
            // 
            // btnSetPosition
            // 
            this.btnSetPosition.Location = new System.Drawing.Point(71, 200);
            this.btnSetPosition.Name = "btnSetPosition";
            this.btnSetPosition.Size = new System.Drawing.Size(75, 23);
            this.btnSetPosition.TabIndex = 1;
            this.btnSetPosition.Text = "Set Position";
            this.btnSetPosition.UseVisualStyleBackColor = true;
            this.btnSetPosition.Click += new System.EventHandler(this.btnSetPosition_Click);
            // 
            // txtLat
            // 
            this.txtLat.Location = new System.Drawing.Point(51, 148);
            this.txtLat.Name = "txtLat";
            this.txtLat.ReadOnly = true;
            this.txtLat.Size = new System.Drawing.Size(100, 20);
            this.txtLat.TabIndex = 5;
            // 
            // chkFollow
            // 
            this.chkFollow.AutoSize = true;
            this.chkFollow.Location = new System.Drawing.Point(45, 233);
            this.chkFollow.Name = "chkFollow";
            this.chkFollow.Size = new System.Drawing.Size(96, 17);
            this.chkFollow.TabIndex = 3;
            this.chkFollow.Text = "Follow Position";
            this.chkFollow.UseVisualStyleBackColor = true;
            // 
            // txtLong
            // 
            this.txtLong.Location = new System.Drawing.Point(51, 170);
            this.txtLong.Name = "txtLong";
            this.txtLong.ReadOnly = true;
            this.txtLong.Size = new System.Drawing.Size(100, 20);
            this.txtLong.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "long:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "lat:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current Position";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "phone IP:";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(74, 24);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 20);
            this.txtIP.TabIndex = 8;
            // 
            // btnAdbConnect
            // 
            this.btnAdbConnect.Location = new System.Drawing.Point(71, 50);
            this.btnAdbConnect.Name = "btnAdbConnect";
            this.btnAdbConnect.Size = new System.Drawing.Size(75, 23);
            this.btnAdbConnect.TabIndex = 9;
            this.btnAdbConnect.Text = "Connect";
            this.btnAdbConnect.UseVisualStyleBackColor = true;
            this.btnAdbConnect.Click += new System.EventHandler(this.btnAdbConnect_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(68, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Destination";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 329);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Coords:";
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(67, 329);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(100, 20);
            this.txtDestination.TabIndex = 12;
            this.txtDestination.Leave += new System.EventHandler(this.txtDestination_Leave);
            // 
            // txtdestLat
            // 
            this.txtdestLat.Location = new System.Drawing.Point(67, 365);
            this.txtdestLat.Name = "txtdestLat";
            this.txtdestLat.ReadOnly = true;
            this.txtdestLat.Size = new System.Drawing.Size(100, 20);
            this.txtdestLat.TabIndex = 16;
            // 
            // txtDestLong
            // 
            this.txtDestLong.Location = new System.Drawing.Point(67, 387);
            this.txtDestLong.Name = "txtDestLong";
            this.txtDestLong.ReadOnly = true;
            this.txtDestLong.Size = new System.Drawing.Size(100, 20);
            this.txtDestLong.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 390);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "long:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 365);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "lat:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 471);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Speed";
            // 
            // chkRoads
            // 
            this.chkRoads.AutoSize = true;
            this.chkRoads.Location = new System.Drawing.Point(45, 552);
            this.chkRoads.Name = "chkRoads";
            this.chkRoads.Size = new System.Drawing.Size(116, 17);
            this.chkRoads.TabIndex = 23;
            this.chkRoads.Text = "Use roads for route";
            this.chkRoads.UseVisualStyleBackColor = true;
            // 
            // chkGo
            // 
            this.chkGo.AutoSize = true;
            this.chkGo.Location = new System.Drawing.Point(45, 576);
            this.chkGo.Name = "chkGo";
            this.chkGo.Size = new System.Drawing.Size(104, 17);
            this.chkGo.TabIndex = 24;
            this.chkGo.Text = "Go automatically";
            this.chkGo.UseVisualStyleBackColor = true;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(66, 611);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 25;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtETA);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.txtPosition);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtSpeed);
            this.groupBox1.Controls.Add(this.btnGo);
            this.groupBox1.Controls.Add(this.chkGo);
            this.groupBox1.Controls.Add(this.chkRoads);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtDestLong);
            this.groupBox1.Controls.Add(this.txtdestLat);
            this.groupBox1.Controls.Add(this.txtDestination);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnAdbConnect);
            this.groupBox1.Controls.Add(this.txtIP);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtLong);
            this.groupBox1.Controls.Add(this.chkFollow);
            this.groupBox1.Controls.Add(this.txtLat);
            this.groupBox1.Controls.Add(this.btnSetPosition);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(816, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 722);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // txtETA
            // 
            this.txtETA.Location = new System.Drawing.Point(55, 677);
            this.txtETA.Name = "txtETA";
            this.txtETA.ReadOnly = true;
            this.txtETA.Size = new System.Drawing.Size(100, 20);
            this.txtETA.TabIndex = 33;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 680);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "ETA:";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(67, 640);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 31;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(51, 122);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(100, 20);
            this.txtPosition.TabIndex = 30;
            this.txtPosition.Leave += new System.EventHandler(this.txtPosition_Leave);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1, 122);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "Coords:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(45, 497);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "(0 for instant jump)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(135, 471);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "kmh";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(67, 468);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(61, 20);
            this.txtSpeed.TabIndex = 26;
            this.txtSpeed.Text = "0";
            this.txtSpeed.TextChanged += new System.EventHandler(this.txtSpeed_TextChanged);
            // 
            // txtAdbResult
            // 
            this.txtAdbResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtAdbResult.Location = new System.Drawing.Point(0, 676);
            this.txtAdbResult.Multiline = true;
            this.txtAdbResult.Name = "txtAdbResult";
            this.txtAdbResult.ReadOnly = true;
            this.txtAdbResult.Size = new System.Drawing.Size(816, 46);
            this.txtAdbResult.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 722);
            this.Controls.Add(this.txtAdbResult);
            this.Controls.Add(this.map);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "GPS Walker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl map;
        private System.Windows.Forms.Button btnSetPosition;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.CheckBox chkFollow;
        private System.Windows.Forms.TextBox txtLong;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnAdbConnect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.TextBox txtdestLat;
        private System.Windows.Forms.TextBox txtDestLong;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkRoads;
        private System.Windows.Forms.CheckBox chkGo;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtETA;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtAdbResult;
    }
}

