/* SPDX license identifier: MPL-2.0
 * Author: Guido Pennella
 * Copyright ©2014, Magneti Marelli S.p.A. All rights reserved
 * Version: MPL 2.0
 * 
 * This file is part of Magneti Marelli Genivi Document Generator
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 2.0. If a copy of the MPL was not distributed with this file, you can obtain one at http://mozilla.org/MPL/2.0/. 
 * 
 * For further information see http://www.genivi.org/. 
 *
 * ***** END LICENSE BLOCK *****/
namespace MM_GENIVI_EA_Document_Generation_System
{
    partial class mmEAdgs_dataForm
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpData = new System.Windows.Forms.GroupBox();
            this.grpEA = new System.Windows.Forms.GroupBox();
            this.btnOutput = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.btnEAP = new System.Windows.Forms.Button();
            this.txtEAP = new System.Windows.Forms.TextBox();
            this.lblEA = new System.Windows.Forms.Label();
            this.btnImages = new System.Windows.Forms.Button();
            this.lblImage = new System.Windows.Forms.Label();
            this.txtImages = new System.Windows.Forms.TextBox();
            this.btnGen = new System.Windows.Forms.Button();
            this.genDLL = new System.Windows.Forms.GroupBox();
            this.txtDllDir = new System.Windows.Forms.TextBox();
            this.lblDLLdir = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.grpData.SuspendLayout();
            this.grpEA.SuspendLayout();
            this.genDLL.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpData
            // 
            this.grpData.Controls.Add(this.btnOutput);
            this.grpData.Controls.Add(this.txtOutput);
            this.grpData.Controls.Add(this.lblOutput);
            this.grpData.Controls.Add(this.btnTemplate);
            this.grpData.Controls.Add(this.txtTemplate);
            this.grpData.Controls.Add(this.lblTemplate);
            this.grpData.Location = new System.Drawing.Point(3, 3);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(669, 102);
            this.grpData.TabIndex = 0;
            this.grpData.TabStop = false;
            this.grpData.Text = "Document data";
            // 
            // grpEA
            // 
            this.grpEA.Controls.Add(this.btnEAP);
            this.grpEA.Controls.Add(this.lblEA);
            this.grpEA.Controls.Add(this.txtEAP);
            this.grpEA.Controls.Add(this.txtImages);
            this.grpEA.Controls.Add(this.lblImage);
            this.grpEA.Controls.Add(this.btnImages);
            this.grpEA.Location = new System.Drawing.Point(3, 111);
            this.grpEA.Name = "grpEA";
            this.grpEA.Size = new System.Drawing.Size(669, 108);
            this.grpEA.TabIndex = 1;
            this.grpEA.TabStop = false;
            this.grpEA.Text = "Enterprise Architect Connection";
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(539, 56);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(111, 23);
            this.btnOutput.TabIndex = 11;
            this.btnOutput.Text = "Choose Outfile";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(113, 59);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(420, 20);
            this.txtOutput.TabIndex = 10;
            this.txtOutput.TextChanged += new System.EventHandler(this.txtOutput_TextChanged);
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(18, 62);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(89, 13);
            this.lblOutput.TabIndex = 9;
            this.lblOutput.Text = "Output document";
            // 
            // btnTemplate
            // 
            this.btnTemplate.Location = new System.Drawing.Point(539, 24);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(111, 23);
            this.btnTemplate.TabIndex = 8;
            this.btnTemplate.Text = "Choose Template";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // txtTemplate
            // 
            this.txtTemplate.Location = new System.Drawing.Point(113, 24);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new System.Drawing.Size(420, 20);
            this.txtTemplate.TabIndex = 7;
            this.txtTemplate.TextChanged += new System.EventHandler(this.txtTemplate_TextChanged);
            // 
            // lblTemplate
            // 
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Location = new System.Drawing.Point(6, 27);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(101, 13);
            this.lblTemplate.TabIndex = 6;
            this.lblTemplate.Text = "Template document";
            // 
            // btnEAP
            // 
            this.btnEAP.Location = new System.Drawing.Point(536, 59);
            this.btnEAP.Name = "btnEAP";
            this.btnEAP.Size = new System.Drawing.Size(111, 23);
            this.btnEAP.TabIndex = 17;
            this.btnEAP.Text = "Choose EAP link";
            this.btnEAP.UseVisualStyleBackColor = true;
            this.btnEAP.Click += new System.EventHandler(this.btnEAP_Click);
            // 
            // txtEAP
            // 
            this.txtEAP.Location = new System.Drawing.Point(159, 62);
            this.txtEAP.Name = "txtEAP";
            this.txtEAP.Size = new System.Drawing.Size(371, 20);
            this.txtEAP.TabIndex = 16;
            this.txtEAP.TextChanged += new System.EventHandler(this.txtEAP_TextChanged);
            // 
            // lblEA
            // 
            this.lblEA.AutoSize = true;
            this.lblEA.Location = new System.Drawing.Point(18, 65);
            this.lblEA.Name = "lblEA";
            this.lblEA.Size = new System.Drawing.Size(135, 13);
            this.lblEA.TabIndex = 15;
            this.lblEA.Text = "Enterprise Architect Project";
            // 
            // btnImages
            // 
            this.btnImages.Location = new System.Drawing.Point(536, 26);
            this.btnImages.Name = "btnImages";
            this.btnImages.Size = new System.Drawing.Size(111, 23);
            this.btnImages.TabIndex = 14;
            this.btnImages.Text = "Choose Directory";
            this.btnImages.UseVisualStyleBackColor = true;
            this.btnImages.Click += new System.EventHandler(this.btnImages_Click);
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.Location = new System.Drawing.Point(18, 31);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(102, 13);
            this.lblImage.TabIndex = 12;
            this.lblImage.Text = "Temporary Directory";
            // 
            // txtImages
            // 
            this.txtImages.Location = new System.Drawing.Point(159, 28);
            this.txtImages.Name = "txtImages";
            this.txtImages.Size = new System.Drawing.Size(371, 20);
            this.txtImages.TabIndex = 13;
            this.txtImages.TextChanged += new System.EventHandler(this.txtImages_TextChanged);
            // 
            // btnGen
            // 
            this.btnGen.Location = new System.Drawing.Point(37, 310);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(594, 23);
            this.btnGen.TabIndex = 2;
            this.btnGen.Text = "Generate Documentation";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // genDLL
            // 
            this.genDLL.Controls.Add(this.txtDllDir);
            this.genDLL.Controls.Add(this.lblDLLdir);
            this.genDLL.Controls.Add(this.button1);
            this.genDLL.Location = new System.Drawing.Point(3, 225);
            this.genDLL.Name = "genDLL";
            this.genDLL.Size = new System.Drawing.Size(669, 67);
            this.genDLL.TabIndex = 3;
            this.genDLL.TabStop = false;
            this.genDLL.Text = "Generation DLL data";
            // 
            // txtDllDir
            // 
            this.txtDllDir.Location = new System.Drawing.Point(161, 24);
            this.txtDllDir.Name = "txtDllDir";
            this.txtDllDir.Size = new System.Drawing.Size(371, 20);
            this.txtDllDir.TabIndex = 16;
            this.txtDllDir.TextChanged += new System.EventHandler(this.txtDllDir_TextChanged);
            // 
            // lblDLLdir
            // 
            this.lblDLLdir.AutoSize = true;
            this.lblDLLdir.Location = new System.Drawing.Point(20, 27);
            this.lblDLLdir.Name = "lblDLLdir";
            this.lblDLLdir.Size = new System.Drawing.Size(72, 13);
            this.lblDLLdir.TabIndex = 15;
            this.lblDLLdir.Text = "DLL Directory";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(538, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Choose Directory";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // mmEAdgs_dataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.genDLL);
            this.Controls.Add(this.btnGen);
            this.Controls.Add(this.grpEA);
            this.Controls.Add(this.grpData);
            this.Name = "mmEAdgs_dataForm";
            this.Size = new System.Drawing.Size(683, 354);
            this.Load += new System.EventHandler(this.mmEAdgs_dataForm_Load);
            this.grpData.ResumeLayout(false);
            this.grpData.PerformLayout();
            this.grpEA.ResumeLayout(false);
            this.grpEA.PerformLayout();
            this.genDLL.ResumeLayout(false);
            this.genDLL.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpData;
        private System.Windows.Forms.GroupBox grpEA;
        private System.Windows.Forms.Button btnOutput;
        public System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Button btnTemplate;
        public System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.Button btnEAP;
        private System.Windows.Forms.Label lblEA;
        public System.Windows.Forms.TextBox txtEAP;
        public System.Windows.Forms.TextBox txtImages;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.Button btnImages;
        private System.Windows.Forms.Button btnGen;
        private System.Windows.Forms.GroupBox genDLL;
        public System.Windows.Forms.TextBox txtDllDir;
        private System.Windows.Forms.Label lblDLLdir;
        private System.Windows.Forms.Button button1;
    }
}
