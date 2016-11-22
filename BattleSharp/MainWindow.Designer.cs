namespace BattleSharp
{
    partial class MainWindow
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
            this.injectButton = new System.Windows.Forms.Button();
            this.dllMods = new System.Windows.Forms.CheckedListBox();
            this.dllMod = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // injectButton
            // 
            this.injectButton.Location = new System.Drawing.Point(12, 168);
            this.injectButton.Name = "injectButton";
            this.injectButton.Size = new System.Drawing.Size(387, 105);
            this.injectButton.TabIndex = 0;
            this.injectButton.Text = "Inject";
            this.injectButton.UseVisualStyleBackColor = true;
            this.injectButton.Click += new System.EventHandler(this.injectButton_Click);
            // 
            // dllMods
            // 
            this.dllMods.FormattingEnabled = true;
            this.dllMods.Items.AddRange(new object[] {
            "Zoom",
            "Fog of War"});
            this.dllMods.Location = new System.Drawing.Point(12, 29);
            this.dllMods.Name = "dllMods";
            this.dllMods.Size = new System.Drawing.Size(387, 94);
            this.dllMods.TabIndex = 1;
            this.dllMods.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.dllMods_ItemCheck);
            // 
            // dllMod
            // 
            this.dllMod.AutoSize = true;
            this.dllMod.Location = new System.Drawing.Point(13, 13);
            this.dllMod.Name = "dllMod";
            this.dllMod.Size = new System.Drawing.Size(358, 13);
            this.dllMod.TabIndex = 2;
            this.dllMod.Text = "DLL Mods - These need to be enabled or disabled before starting Battlerite";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(334, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Aim Hack - Inject after game is loaded. GUI on top left should appear.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(246, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Keyboard button ` toggles lock, 1-6 changes target";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 283);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dllMod);
            this.Controls.Add(this.dllMods);
            this.Controls.Add(this.injectButton);
            this.Name = "MainWindow";
            this.Text = "BattleSharp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button injectButton;
        private System.Windows.Forms.CheckedListBox dllMods;
        private System.Windows.Forms.Label dllMod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

