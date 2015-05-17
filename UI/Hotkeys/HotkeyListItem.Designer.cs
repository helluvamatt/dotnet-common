namespace Common.UI.Hotkeys
{
	partial class HotkeyListItem
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
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.hotkeyButton = new System.Windows.Forms.Button();
			this.enabledCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox
			// 
			this.groupBox.Controls.Add(this.enabledCheckBox);
			this.groupBox.Controls.Add(this.descriptionLabel);
			this.groupBox.Controls.Add(this.hotkeyButton);
			this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox.Location = new System.Drawing.Point(0, 0);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new System.Drawing.Size(500, 80);
			this.groupBox.TabIndex = 0;
			this.groupBox.TabStop = false;
			this.groupBox.Text = "### Name ###";
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.descriptionLabel.Location = new System.Drawing.Point(7, 39);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(245, 27);
			this.descriptionLabel.TabIndex = 2;
			this.descriptionLabel.Text = "### Description ###";
			// 
			// hotkeyButton
			// 
			this.hotkeyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.hotkeyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.hotkeyButton.Location = new System.Drawing.Point(258, 19);
			this.hotkeyButton.Name = "hotkeyButton";
			this.hotkeyButton.Size = new System.Drawing.Size(236, 47);
			this.hotkeyButton.TabIndex = 0;
			this.hotkeyButton.Text = "Not Set";
			this.hotkeyButton.UseVisualStyleBackColor = true;
			// 
			// enabledCheckBox
			// 
			this.enabledCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.enabledCheckBox.Location = new System.Drawing.Point(7, 19);
			this.enabledCheckBox.Name = "enabledCheckBox";
			this.enabledCheckBox.Size = new System.Drawing.Size(245, 17);
			this.enabledCheckBox.TabIndex = 3;
			this.enabledCheckBox.Text = "Enabled";
			this.enabledCheckBox.UseVisualStyleBackColor = true;
			// 
			// HotkeyListItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox);
			this.Name = "HotkeyListItem";
			this.Size = new System.Drawing.Size(500, 80);
			this.groupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Button hotkeyButton;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.CheckBox enabledCheckBox;
	}
}
