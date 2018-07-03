using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NLog;

namespace NVidia_Surround_Assistant
{
    public partial class ThumbGridView : UserControl
    {
        public int x_spacing { get; private set; }
        public int y_spacing { get; private set; }

        int numOfColumns;
        int numOfRows;
        ControlCollection myControls = null;

        bool redoLayout = true;

        public ThumbGridView()
        {
            Thumb tempThumb = new Thumb(null);

            InitializeComponent();

            x_spacing = tempThumb.Width;
            y_spacing = tempThumb.Height;
            
            myControls = panelApplicationListView.Controls;
        }

        private void UpdateGridView()
        {
            int controlIndex = 0;
            Point controlLocation = new Point(0, 0);

            if (myControls != null && myControls.Count > 0)
            {
                panelApplicationListView.SuspendLayout();
                panelApplicationListView.AutoScroll = false;
                panelApplicationListView.AutoScrollPosition = Point.Empty;

                if (redoLayout)
                {
                    SortThumbs();

                    numOfColumns = (int)Math.Floor((decimal)panelApplicationListView.Width / x_spacing);
                    numOfRows = (int)Math.Ceiling((double)panelApplicationListView.Controls.Count / numOfColumns);

                    for (int row = 0; (row < numOfRows) && (panelApplicationListView.Controls.Count != controlIndex); row++)
                    {
                        if (row != 0)
                        {
                            controlLocation.X = 0;
                            controlLocation.Y += y_spacing;
                        }
                        for (int column = 0; (column < numOfColumns) && (panelApplicationListView.Controls.Count != controlIndex); column++)
                        {
                            if (column != 0)
                                controlLocation.X += x_spacing;
                            panelApplicationListView.Controls[controlIndex].Location = controlLocation;
                            controlIndex++;
                        }
                    }
                    redoLayout = false;
                }
                panelApplicationListView.AutoScroll = true;                
                panelApplicationListView.ResumeLayout();                                
            }
        }

        private void SortThumbs()
        {
            IEnumerable<Thumb> sortedList = from thumb in myControls.Cast<Thumb>()
                                            orderby thumb.DisplayName, thumb.FullPath
                                            select thumb;
            int counter = 0;
            foreach (Thumb thumb in sortedList)
            {
                myControls.SetChildIndex(thumb, counter);
                counter++;
            }
        }

        public void AddThumb(Thumb newThumb)
        {
            myControls.Add(newThumb);
            redoLayout = true;
        }

        public void RemoveThumb(Thumb newThumb)
        {
            myControls.RemoveAt(myControls.GetChildIndex(newThumb));
            redoLayout = true;
        }

        public void SetAutoScroll(bool enabled)
        {
            AutoScroll = enabled;
        }

        public void ResetScrollBar(int value)
        {
            panelApplicationListView.VerticalScroll.Value = value;
            PerformLayout();
        }

        private void panelApplicationListView_Layout(object sender, LayoutEventArgs e)
        {
            panelApplicationListView.Layout -= panelApplicationListView_Layout;
            UpdateGridView();
            panelApplicationListView.Layout += panelApplicationListView_Layout;
        }

        private void panelApplicationListView_Resize(object sender, EventArgs e)
        {            
            panelApplicationListView.Resize -= panelApplicationListView_Resize;
            redoLayout = true;
            UpdateGridView();
            panelApplicationListView.Resize += panelApplicationListView_Resize;
        }
    }
}
