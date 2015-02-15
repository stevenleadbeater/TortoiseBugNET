namespace TurtleBugNET
{
    partial class CommitFinishedForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxSetStatus = new System.Windows.Forms.CheckBox();
            this.checkBoxReassign = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(96, 217);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "OK";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Location = new System.Drawing.Point(240, 217);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Cancel";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Selected issues:";
            // 
            // checkBoxSetStatus
            // 
            this.checkBoxSetStatus.AutoSize = true;
            this.checkBoxSetStatus.Checked = true;
            this.checkBoxSetStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSetStatus.Location = new System.Drawing.Point(12, 25);
            this.checkBoxSetStatus.Name = "checkBoxSetStatus";
            this.checkBoxSetStatus.Size = new System.Drawing.Size(128, 17);
            this.checkBoxSetStatus.TabIndex = 5;
            this.checkBoxSetStatus.Text = "Set Issue Status as: \"";
            this.checkBoxSetStatus.UseVisualStyleBackColor = true;
            // 
            // checkBoxReassign
            // 
            this.checkBoxReassign.AutoSize = true;
            this.checkBoxReassign.Checked = true;
            this.checkBoxReassign.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxReassign.Location = new System.Drawing.Point(12, 49);
            this.checkBoxReassign.Name = "checkBoxReassign";
            this.checkBoxReassign.Size = new System.Drawing.Size(129, 17);
            this.checkBoxReassign.TabIndex = 6;
            this.checkBoxReassign.Text = "Reassign to \"Creator\"";
            this.checkBoxReassign.UseVisualStyleBackColor = true;
            // 
            // CommitFinishedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 252);
            this.Controls.Add(this.checkBoxReassign);
            this.Controls.Add(this.checkBoxSetStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Name = "CommitFinishedForm";
            this.Text = "Issue Actions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxSetStatus;
        private System.Windows.Forms.CheckBox checkBoxReassign;
    }
}