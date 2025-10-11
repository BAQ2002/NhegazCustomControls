

namespace NhegazCustomControls
{
    public partial class CustomComboBox : CustomControl
    {

       
        public CustomComboBox()
        {
            Size = new Size(121, 23);            
            
            MinimumSize =new Size(5, 5);
            
            InnerControls.Add(selectIndex);
            selectIndex.Text = "Teste123456";
            selectIndex.DoubleClick += (s, e) => { Focus(); base.OnClick(e); };
            selectIndex.Click += (s, e) => { Focus(); base.OnClick(e); };          

            InnerControls.Add(dropDownIcon);
            dropDownIcon.DoubleClick += (s, e) => { Focus(); base.OnClick(e); };
            dropDownIcon.Click += (s, e) => { Focus(); base.OnClick(e); };           
            
            AdjustControlSize();           
            AdjustHoverColors();
        }

        
        
        //dropDownIcon.SetHoverColors(BackgroundColor, HeaderBackgroundColor);
        private void ToggleDropDown()
        {
            if (dropDownInstance != null) // Se o dropdown já estiver aberto, fecha ele
            {
                Form parentForm = FindForm();
                parentForm.Controls.Remove(dropDownInstance);
                dropDownInstance = null; OnFocus = false;
            }
            else
            {
                dropDownInstance = new ComboBoxDropDown(this);
                Form parentForm = this.FindForm();
                if (parentForm == null)
                {
                    return;
                }

                Point screenLocation = this.Parent.PointToScreen(this.Location);
                Point formLocation = parentForm.PointToClient(screenLocation);

                dropDownInstance.Location = new Point(formLocation.X, formLocation.Y + Height + 1);
                dropDownInstance.BringToFront();
                parentForm.Controls.Add(dropDownInstance);
                parentForm.Controls.SetChildIndex(dropDownInstance, 0);
                OnFocus = true;
                Invalidate();
            }
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ToggleDropDown();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            OnClick(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            if (dropDownInstance != null)
            {
                var mousePos = Cursor.Position;
                var form = this.FindForm();

                if (form != null)
                {
                    Control controlAtMouse = form.GetChildAtPoint(form.PointToClient(mousePos));

                    if (controlAtMouse != null && controlAtMouse == dropDownInstance)
                    {
                        // Clique ocorreu no dropdown — NÃO REMOVER
                        return;
                    }
                }

                form?.Controls.Remove(dropDownInstance);
                dropDownInstance = null;
                OnFocus = false;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustInnerLocations();
            Invalidate();
        }     
    }  
}
