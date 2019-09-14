namespace _12036ByTicket
{
    partial class CaptchaCheck
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.pb_image = new System.Windows.Forms.PictureBox();
            this.刷新 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pb_image)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(114, 135);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(8, 8);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pb_image
            // 
            this.pb_image.Location = new System.Drawing.Point(3, 3);
            this.pb_image.Name = "pb_image";
            this.pb_image.Size = new System.Drawing.Size(229, 200);
            this.pb_image.TabIndex = 1;
            this.pb_image.TabStop = false;
            this.pb_image.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // 刷新
            // 
            this.刷新.Location = new System.Drawing.Point(174, 3);
            this.刷新.Name = "刷新";
            this.刷新.Size = new System.Drawing.Size(58, 23);
            this.刷新.TabIndex = 2;
            this.刷新.Text = "刷新";
            this.刷新.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(60, 173);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // CaptchaCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.刷新);
            this.Controls.Add(this.pb_image);
            this.Controls.Add(this.button1);
            this.Name = "CaptchaCheck";
            this.Size = new System.Drawing.Size(238, 227);
            ((System.ComponentModel.ISupportInitialize)(this.pb_image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pb_image;
        private System.Windows.Forms.Button 刷新;
        private System.Windows.Forms.Button button2;
    }
}
