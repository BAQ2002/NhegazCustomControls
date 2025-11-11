// III.CustomDatePicker.Core.cs  (substitua o construtor + acrescente o roteamento de teclado)
using System;
using System.Windows.Forms;

namespace NhegazCustomControls
{
    public partial class CustomDatePicker : CustomControl, IHasDropDown
    {
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (date is null)
            {
                Date = DateOnly.FromDateTime(DateTime.Now);
            }
        }

        public CustomDatePicker() : base()
        {
            DropDownFeatures.Add<DropDownDay>();
            DropDownFeatures.Add<DropDownMonth>();
            DropDownFeatures.Add<DropDownYear>();

            // ===== InnerTextBoxes (ADICIONA como InnerControlsCollection, não Controls) =====
            dayTextBox.SizeBasedOnText = false;
            dayTextBox.MaxLength = 2;
            dayTextBox.CharFilter = char.IsDigit;
            dayTextBox.UseEllipsis = false;       // sem reticências
            dayTextBox.InvalidateParent = Invalidate;
            dayTextBox.Click += (s, e) => { activeInnerTextBox = dayTextBox; this.Focus(); OnClick(e); };
            dayTextBox.DoubleClick += (s, e) => { activeInnerTextBox = dayTextBox; this.Focus(); OnClick(e); };
            //dayTextBox.GotFocus += (s, e) => dayTextBox.OnInnerGotFocus();
            //dayTextBox.LostFocus += (s, e) => dayTextBox.OnInnerLostFocus();
            InnerControls.Add(dayTextBox);
          
            monthTextBox.SizeBasedOnText = false;
            monthTextBox.MaxLength = 2;
            monthTextBox.CharFilter = char.IsDigit;
            monthTextBox.UseEllipsis = false;
            monthTextBox.InvalidateParent = Invalidate;
            monthTextBox.Click += (s, e) => { activeInnerTextBox = monthTextBox; this.Focus(); OnClick(e); };
            monthTextBox.DoubleClick += (s, e) => { activeInnerTextBox = monthTextBox; this.Focus(); OnClick(e); };
            //monthTextBox.GotFocus += (s, e) => monthTextBox.OnInnerGotFocus();
            //monthTextBox.LostFocus += (s, e) => monthTextBox.OnInnerLostFocus();
            InnerControls.Add(monthTextBox);

            yearTextBox.SizeBasedOnText = false;
            yearTextBox.MaxLength = 4;
            yearTextBox.CharFilter = char.IsDigit;
            yearTextBox.UseEllipsis = false;
            yearTextBox.InvalidateParent = Invalidate;
            yearTextBox.Click += (s, e) => { activeInnerTextBox = yearTextBox; this.Focus(); OnClick(e); };
            yearTextBox.DoubleClick += (s, e) => { activeInnerTextBox = yearTextBox; this.Focus(); OnClick(e); };
            //yearTextBox.GotFocus += (s, e) => yearTextBox.OnInnerGotFocus();
            //yearTextBox.LostFocus += (s, e) => yearTextBox.OnInnerLostFocus();
            InnerControls.Add(yearTextBox);

            // ===== Barras e ícones (igual ao anterior) =====
            InnerControls.Add(daySlashMonth);
            daySlashMonth.Text = "/";

            InnerControls.Add(dayDropDownIcon);
            dayDropDownIcon.DoubleClick += (s, e) => OnClick(e, typeof(DropDownDay));
            dayDropDownIcon.Click += (s, e) => OnClick(e, typeof(DropDownDay));

            InnerControls.Add(monthDropDownIcon);
            monthDropDownIcon.DoubleClick += (s, e) => OnClick(e, typeof(DropDownMonth));
            monthDropDownIcon.Click += (s, e) => OnClick(e, typeof(DropDownMonth));

            InnerControls.Add(monthSlashYear);
            monthSlashYear.Text = "/";

            InnerControls.Add(yearDropDownIcon);
            yearDropDownIcon.DoubleClick += (s, e) => OnClick(e, typeof(DropDownYear));
            yearDropDownIcon.Click += (s, e) => OnClick(e, typeof(DropDownYear));

            UpdateLayout();
            AdjustHoverColors();
        }

        protected override void AdjustHoverColors()
        {
            dayDropDownIcon.MouseEnter += (s, e) =>
            {
                dayDropDownIcon.ForeColor = BackgroundColor;
                dayDropDownIcon.BackgroundColor = HoverBackgroundColor;
            };
            dayDropDownIcon.MouseLeave += (s, e) =>
            {
                dayDropDownIcon.ForeColor = ForeColor;
                dayDropDownIcon.BackgroundColor = BackgroundColor;
            };

            monthDropDownIcon.MouseEnter += (s, e) =>
            {
                monthDropDownIcon.ForeColor = BackgroundColor;
                monthDropDownIcon.BackgroundColor = HoverBackgroundColor;
            };
            monthDropDownIcon.MouseLeave += (s, e) =>
            {
                monthDropDownIcon.ForeColor = ForeColor;
                monthDropDownIcon.BackgroundColor = BackgroundColor;
            };

            yearDropDownIcon.MouseEnter += (s, e) =>
            {
                yearDropDownIcon.ForeColor = BackgroundColor;
                yearDropDownIcon.BackgroundColor = HoverBackgroundColor;
            };
            yearDropDownIcon.MouseLeave += (s, e) =>
            {
                yearDropDownIcon.ForeColor = ForeColor;
                yearDropDownIcon.BackgroundColor = BackgroundColor;
            };
        }

        // Clique que abre os dropdowns (comportamento preservado)
        protected void OnClick(EventArgs e, Type dropDownType)
        {
            base.OnClick(e);
            if (dropDownInstance == null)
            {
                OpenDropDown((CustomControl)Activator.CreateInstance(dropDownType, this));
            }
            else if (dropDownInstance.GetType() != dropDownType)
            {
                CloseDropDownInstance();
                OpenDropDown((CustomControl)Activator.CreateInstance(dropDownType, this));
            }
            else
            {
                CloseDropDownInstance();
            }
        }

        public void CloseDropDownInstance()
        {
            if (FindForm() == null || dropDownInstance == null) return;
            Form parentForm = FindForm();
            parentForm.Controls.Remove(dropDownInstance);
            dropDownInstance.Dispose();
            dropDownInstance = null;
        }

        protected void OpenDropDown(CustomControl dropDown)
        {
            Form parentForm = FindForm();
            if (parentForm == null) return;

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

            dropDownInstance.UpdateLayout();

            dropDownInstance.BringToFront();
            parentForm.Controls.Add(dropDownInstance);
            parentForm.Controls.SetChildIndex(dropDownInstance, 0);
            Invalidate();
        }

        public (int, int) GetInnerPaddings(CustomControl dropDown)
        {
            using var ddDay = new DropDownDay(this); Size dDaySize = ddDay.GetControlSize(); ddDay.Dispose();
            using var ddYear = new DropDownYear(this); Size dYearSize = ddYear.GetControlSize(); ddYear.Dispose();

            Size newSize = new(Math.Max(dDaySize.Width, dYearSize.Width), Math.Max(dDaySize.Height, dYearSize.Height));

            if (dropDown is DropDownDay)
            {
                int h = ControlPadding.EffectiveInnerHorizontal + ((newSize.Width - dDaySize.Width) / 6);
                int v = ControlPadding.EffectiveInnerVertical + ((newSize.Height - dDaySize.Height) / 7);
                return (h, v);
            }
            else if (dropDown is DropDownYear)
            {
                int h = ControlPadding.EffectiveInnerHorizontal + ((newSize.Width - dYearSize.Width) / 3);
                int v = ControlPadding.EffectiveInnerVertical + ((newSize.Height - dYearSize.Height) / 4);
                return (h, v);
            }
            else if (dropDown is DropDownMonth)
            {
                int h = ControlPadding.EffectiveInnerHorizontal + ((newSize.Width - dYearSize.Width) / 3);
                int v = ControlPadding.EffectiveInnerVertical + ((newSize.Height - dYearSize.Height) / 4);
                return (h, v);
            }
            else return (0, 0);
        }

        // Garante caret ON quando o controle ganha foco pelo teclado (sem mouse)
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            //activeInnerTextBox?.OnInnerGotFocus();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            //activeInnerTextBox?.OnInnerLostFocus();
        }
    }
}
