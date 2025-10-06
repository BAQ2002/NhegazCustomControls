using NhegazCustomControls.PL.CustomControls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;


namespace NhegazCustomControls
{
    public partial class CustomDatePicker : CustomControl, IHasDropDown
    {        
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (date is null)
            {
                Date = DateOnly.FromDateTime(DateTime.Now); // usa o setter -> sincroniza textos
            }
        }
        public CustomDatePicker() : base() 
        {
            

            DropDownFeatures.Add<DropDownDay>();
            DropDownFeatures.Add<DropDownMonth>();
            DropDownFeatures.Add<DropDownYear>(); // enquanto não migrar, funciona do mesmo jeito

            Controls.Add(selectedDay);
            selectedDay.BorderStyle = BorderStyle.None;
            selectedDay.DoubleClick += (s, e) => { this.Focus(); this.OnClick(e); };
            selectedDay.Click += (s, e) => { this.Focus(); this.OnClick(e); };

            InnerControls.Add(daySlashMonth);
            daySlashMonth.Text = "/";

            InnerControls.Add(dayDropDownIcon);    
            dayDropDownIcon.DoubleClick += (s, e) => { OnClick(e, new DropDownDay(this)); };
            dayDropDownIcon.Click += (s, e) => { OnClick(e, new DropDownDay(this)); };
          
            Controls.Add(selectedMonth);
            selectedMonth.BorderStyle = BorderStyle.None;
            selectedMonth.DoubleClick += (s, e) => { this.Focus(); this.OnClick(e); };
            selectedMonth.Click += (s, e) => { this.Focus(); this.OnClick(e); };
           
            InnerControls.Add(monthDropDownIcon);
            monthDropDownIcon.DoubleClick += (s, e) => { OnClick(e, new DropDownMonth(this)); };
            monthDropDownIcon.Click += (s, e) => {  OnClick(e, new DropDownMonth(this)); };

            InnerControls.Add(monthSlashYear);
            monthSlashYear.Text = "/";

            Controls.Add(selectedYear);
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
                    dropDownInstance.Dispose();
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

            dropDownInstance.ControlPadding.Mode = PaddingMode.Absolute;
            dropDownInstance.ControlPadding.BorderLeft = ControlPadding.EffectiveBorderLeft;
            dropDownInstance.ControlPadding.BorderTop = ControlPadding.EffectiveBorderTop;
            dropDownInstance.ControlPadding.BorderRight = ControlPadding.EffectiveBorderRight;
            dropDownInstance.ControlPadding.BorderBottom = ControlPadding.EffectiveBorderBottom;

            (dropDownInstance.ControlPadding.InnerHorizontal,
            dropDownInstance.ControlPadding.InnerVertical) = GetInnerPaddings(dropDownInstance);
            dropDownInstance.AdjustControlSize();


            dropDownInstance.BringToFront();
            parentForm.Controls.Add(dropDownInstance);
            parentForm.Controls.SetChildIndex(dropDownInstance, 0);
            OnFocus = true;
            Invalidate();
            
        }

        public (int, int) GetInnerPaddings(CustomControl dropDown)
        {
            using var ddDay = new DropDownDay(this); Size dDaySize = ddDay.GetSize(); ddDay.Dispose();
            using var ddYear = new DropDownYear(this); Size dYearSize = ddYear.GetSize(); ddYear.Dispose();
            //using var ddMonth = new DropDownMonth(this); Size dMonthSize = ddMonth.GetSize(); ddMonth.Dispose();
            Size newSize = new(Math.Max(dDaySize.Width, dYearSize.Width), Math.Max(dDaySize.Height, dYearSize.Height));
            
            if (dropDown is DropDownDay)
            {
                int dDayIdealHorizontalGap = ControlPadding.EffectiveInnerHorizontal + ((newSize.Width - dDaySize.Width) / 6);
                int dDayIdealVerticalGap = ControlPadding.EffectiveInnerVertical + ((newSize.Height - dDaySize.Height) / 7);

                return (dDayIdealHorizontalGap, dDayIdealVerticalGap);
            }

            else if (dropDown is DropDownYear)
            {
                int dYearIdealHorizontalGap = ControlPadding.EffectiveInnerHorizontal + ((newSize.Width - dYearSize.Width) / 3);
                int dYearIdealVerticalGap = ControlPadding.EffectiveInnerVertical + ((newSize.Height - dYearSize.Height) / 4);

                return (dYearIdealHorizontalGap, dYearIdealVerticalGap);
            }
            else if (dropDown is DropDownMonth)
            {
                int dMonthIdealHorizontalGap = ControlPadding.EffectiveInnerHorizontal + ((newSize.Width - dYearSize.Width) / 3);
                int dMonthIdealVerticalGap = ControlPadding.EffectiveInnerVertical + ((newSize.Height - dYearSize.Height) / 4);

                return (dMonthIdealHorizontalGap, dMonthIdealVerticalGap);
            }
            else return (0, 0);


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
