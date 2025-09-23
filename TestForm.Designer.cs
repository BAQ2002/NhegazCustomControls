namespace NhegazCustomControls
{
    partial class TestForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DropDownFeature dropDownFeature1 = new DropDownFeature();
            customDatePicker1 = new CustomDatePicker();
            customDataGridView1 = new CustomDataGridView();
            SuspendLayout();
            // 
            // customDatePicker1
            // 
            customDatePicker1.BackColor = Color.Transparent;
            customDatePicker1.BackgroundColor = SystemColors.Window;
            customDatePicker1.BorderColor = SystemColors.WindowFrame;
            customDatePicker1.BorderRadius = 3;
            customDatePicker1.BorderWidth = 1;
            dropDownFeature1.HeaderBackgroundColor = SystemColors.GrayText;
            dropDownFeature1.HeaderForeColor = SystemColors.ControlText;
            customDatePicker1.DropDownFeatures = dropDownFeature1;
            customDatePicker1.DropDownsHeaderBorderRadius = 6;
            customDatePicker1.DropDownsHeaderColor = SystemColors.Info;
            customDatePicker1.DropDownsHeaderHeightMode = HeaderHeightMode.RelativeToFont;
            customDatePicker1.DropDownsHeaderHight = 1;
            customDatePicker1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customDatePicker1.HorizontalPadding = 5;
            customDatePicker1.HoverColor = SystemColors.Highlight;
            customDatePicker1.Location = new Point(393, 107);
            customDatePicker1.MinimumSize = new Size(142, 22);
            customDatePicker1.Name = "customDatePicker1";
            customDatePicker1.OnFocusBool = false;
            customDatePicker1.OnFocusBorderColor = SystemColors.Highlight;
            customDatePicker1.OnFocusBorderExtraWidth = 1;
            customDatePicker1.PaddingMode = PaddingMode.Absolute;
            customDatePicker1.PaddingRelativePercent = 0.6F;
            customDatePicker1.SecondaryBackgroundColor = SystemColors.ActiveCaption;
            customDatePicker1.SecondaryForeColor = SystemColors.GrayText;
            customDatePicker1.Size = new Size(197, 55);
            customDatePicker1.TabIndex = 0;
            customDatePicker1.VerticalPadding = 12;
            // 
            // customDataGridView1
            // 
            customDataGridView1.BackColor = Color.Transparent;
            customDataGridView1.BackgroundColor = SystemColors.Control;
            customDataGridView1.BorderColor = SystemColors.WindowFrame;
            customDataGridView1.BorderRadius = 5;
            customDataGridView1.BorderWidth = 1;
            customDataGridView1.ColumnWidthMode = ColumnWidthMode.HeaderWidth;
            customDataGridView1.DifferentColorsBetweenRows = true;
            customDataGridView1.FixedCharCount = 10;
            customDataGridView1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customDataGridView1.HeaderBackgroundColor = Color.Red;
            customDataGridView1.HeaderBorderRadius = 0;
            customDataGridView1.HeaderBounds = new Rectangle(0, 0, 0, 0);
            customDataGridView1.HeaderHeightMode = HeaderHeightMode.Absolute;
            customDataGridView1.HeaderHeightRelativePercent = 1F;
            customDataGridView1.HorizontalPadding = 1;
            customDataGridView1.HoverColor = SystemColors.Highlight;
            customDataGridView1.LinesBetweenColumns = true;
            customDataGridView1.LinesBetweenRows = true;
            customDataGridView1.LinesWidth = 1;
            customDataGridView1.Location = new Point(33, 170);
            customDataGridView1.Name = "customDataGridView1";
            customDataGridView1.OnFocusBool = false;
            customDataGridView1.OnFocusBorderColor = SystemColors.Highlight;
            customDataGridView1.OnFocusBorderExtraWidth = 1;
            customDataGridView1.PaddingMode = PaddingMode.RelativeToFont;
            customDataGridView1.PaddingRelativePercent = 1F;
            customDataGridView1.SecondaryBackgroundColor = SystemColors.ControlLightLight;
            customDataGridView1.SecondaryForeColor = SystemColors.GrayText;
            customDataGridView1.Size = new Size(302, 209);
            customDataGridView1.TabIndex = 1;
            customDataGridView1.VerticalPadding = 1;
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 450);
            Controls.Add(customDataGridView1);
            Controls.Add(customDatePicker1);
            Name = "TestForm";
            Text = "Form1";
            Load += TestForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private CustomDatePicker customDatePicker1;
        private CustomDataGridView customDataGridView1;
    }
}
