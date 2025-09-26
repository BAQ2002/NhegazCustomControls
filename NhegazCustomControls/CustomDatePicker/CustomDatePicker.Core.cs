using NhegazCustomControls.PL.CustomControls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;


namespace NhegazCustomControls
{
    public partial class CustomDatePicker : CustomControl, IHasDropDown
    {
        [Category("DropDowns")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DropDownFeature DropDownFeatures { get; } = new();

        public CustomDatePicker() : base() 
        {
            

            DropDownFeatures.Add<DropDownDay>();
            DropDownFeatures.Add<DropDownMonth>();
            DropDownFeatures.Add<DropDownYear>(); // enquanto não migrar, funciona do mesmo jeito

            Controls.Add(selectedDay);
            selectedDay.Name = Name + "selectedDay";
            selectedDay.Text = DateTime.Now.Day.ToString("D2");
            selectedDay.BorderStyle = BorderStyle.None;
            selectedDay.DoubleClick += (s, e) => { this.Focus(); this.OnClick(e); };
            selectedDay.Click += (s, e) => { this.Focus(); this.OnClick(e); };

            InnerControls.Add(daySlashMonth);
            daySlashMonth.Text = "/";

            InnerControls.Add(dayDropDownIcon);    
            dayDropDownIcon.DoubleClick += (s, e) => { OnClick(e, new DropDownDay(this)); };
            dayDropDownIcon.Click += (s, e) => { OnClick(e, new DropDownDay(this)); };
          
            Controls.Add(selectedMonth);
            selectedMonth.Text = DateTime.Now.Month.ToString("D2");
            selectedMonth.BorderStyle = BorderStyle.None;
            selectedMonth.DoubleClick += (s, e) => { this.Focus(); this.OnClick(e); };
            selectedMonth.Click += (s, e) => { this.Focus(); this.OnClick(e); };
           
            InnerControls.Add(monthDropDownIcon);
            monthDropDownIcon.DoubleClick += (s, e) => { OnClick(e, new DropDownMonth(this)); };
            monthDropDownIcon.Click += (s, e) => {  OnClick(e, new DropDownMonth(this)); };

            InnerControls.Add(monthSlashYear);
            monthSlashYear.Text = "/";

            Controls.Add(selectedYear);
            selectedYear.Text = DateTime.Now.Year.ToString();
            selectedYear.BorderStyle = BorderStyle.None;
            selectedYear.DoubleClick += (s, e) => { this.Focus(); this.OnClick(e); };
            selectedYear.Click += (s, e) => { this.Focus(); this.OnClick(e); };
     
          
            InnerControls.Add(yearDropDownIcon);
            yearDropDownIcon.DoubleClick += (s, e) => { OnClick(e, new DropDownYear(this)); };
            yearDropDownIcon.Click += (s, e) => { OnClick(e, new DropDownYear(this)); };

            AdjustControlSize();
            AdjustHoverColors();
        }

        protected override void AdjustHoverColors()
        {
            dayDropDownIcon.MouseEnter += (s, e) =>
            {
                dayDropDownIcon.ForeColor = BackgroundColor;
                dayDropDownIcon.BackgroundColor = HoverColor;                
            };
            dayDropDownIcon.MouseLeave += (s, e) =>
            {
                dayDropDownIcon.ForeColor = ForeColor;
                dayDropDownIcon.BackgroundColor = BackgroundColor;                
            };
            monthDropDownIcon.MouseEnter += (s, e) =>
            {
                monthDropDownIcon.ForeColor = BackgroundColor;
                monthDropDownIcon.BackgroundColor = HoverColor;
            };
            monthDropDownIcon.MouseLeave += (s, e) =>
            {
                monthDropDownIcon.ForeColor = ForeColor;
                monthDropDownIcon.BackgroundColor = BackgroundColor;
            };
            yearDropDownIcon.MouseEnter += (s, e) =>
            {
                yearDropDownIcon.ForeColor = BackgroundColor;
                yearDropDownIcon.BackgroundColor = HoverColor;
            };
            yearDropDownIcon.MouseLeave += (s, e) =>
            {
                yearDropDownIcon.ForeColor = ForeColor;
                yearDropDownIcon.BackgroundColor = BackgroundColor;
            };
        }

        //Sobrescrever o Click para ter o comportamento adequado
        protected void OnClick(EventArgs e, CustomControl dropDown)
        {
            base.OnClick(e);
            if (dropDownInstance == null) 
            {
                OpenDropDown(dropDown);

            }
            else if (dropDownInstance != null)
            {
                Form parentForm = FindForm();

                if (dropDownInstance.GetType() != dropDown.GetType())
                {
                    parentForm.Controls.Remove(dropDownInstance);
                    OpenDropDown(dropDown);
                    return;
                }
                
                parentForm.Controls.Remove(dropDownInstance);
                dropDownInstance = null; //Define o dropDownInstance como Null
                OnFocus = false; //Define que o elemento nao esta em foco

            }
        }

        protected void OpenDropDown(CustomControl dropDown)
        {           
            Form parentForm = FindForm();
            if (parentForm == null)
            {
                return;
            }

            dropDownInstance = dropDown;
            Point screenLocation = Parent.PointToScreen(Location);
            Point formLocation = parentForm.PointToClient(screenLocation);

            dropDownInstance.Location = new Point(formLocation.X, formLocation.Y + Height + 1);
            dropDownInstance.BringToFront();
            parentForm.Controls.Add(dropDownInstance);
            parentForm.Controls.SetChildIndex(dropDownInstance, 0);
            OnFocus = true;
            Invalidate();
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);           
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (dropDownInstance != null)
            {
                Form parentForm = FindForm();
                parentForm.Controls.Remove(dropDownInstance);
                dropDownInstance = null;
                OnFocus = false;
                return;
            }
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustControlSize();
            Invalidate();
        }
    }    
}
