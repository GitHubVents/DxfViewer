namespace DxfAndPDFViewer
{
    partial class UserControlEdr
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlEdr));
            this.axEModelViewControl1 = new AxEModelView.AxEModelViewControl();
            ((System.ComponentModel.ISupportInitialize)(this.axEModelViewControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axEModelViewControl1
            // 
            this.axEModelViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axEModelViewControl1.Enabled = true;
            this.axEModelViewControl1.Location = new System.Drawing.Point(0, 0);
            this.axEModelViewControl1.Name = "axEModelViewControl1";
            this.axEModelViewControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axEModelViewControl1.OcxState")));
            this.axEModelViewControl1.Size = new System.Drawing.Size(543, 434);
            this.axEModelViewControl1.TabIndex = 0;
            // 
            // UserControlEdr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axEModelViewControl1);
            this.Name = "UserControlEdr";
            this.Size = new System.Drawing.Size(543, 434);
            ((System.ComponentModel.ISupportInitialize)(this.axEModelViewControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxEModelView.AxEModelViewControl axEModelViewControl1;
    }
}
