using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

//TODO make window ;arger until full screen depending on how many apps are added
namespace NVidia_Surround_Assistant
{
    public partial class ThumbGridView : UserControl
    {
        //Logger
        Logger logger = LogManager.GetLogger("nvsaLogger");

        int border = 30;
        int x_spacing;
        int y_spacing;

        int numOfColumns;
        int numOfRows;

        ControlCollection myControls;

        public ThumbGridView()
        {
            Thumb tempThumb = new Thumb(null, null);

            InitializeComponent();

            x_spacing = border + tempThumb.Width;
            y_spacing = border + tempThumb.Height;

            myControls = panelApplicationListView.Controls;            
        }

        private void UpdateGridView()
        {
            int controlIndex = 0;
            Point controlLocation = new Point(border, border);

            panelApplicationListView.SuspendLayout();
            panelApplicationListView.AutoScrollPosition = Point.Empty;

            SortThumbs();
            //Update number of rows
            //fix update grid not working correctly anymore with colums not calcl correctly
            numOfColumns = (int)Math.Floor((decimal)panelApplicationListView.Width / x_spacing);
            numOfRows = (int)Math.Ceiling((double)panelApplicationListView.Controls.Count / numOfColumns);
            
            for (int row = 0; (row < numOfRows) && (panelApplicationListView.Controls.Count != controlIndex); row++)
            {
                if (row != 0)
                {
                    controlLocation.X = border;
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
            panelApplicationListView.ResumeLayout();
        }        

        private void SortThumbs()
        {
            IEnumerable<Thumb> sortedList = from thumb in myControls.Cast<Thumb>()
                                            orderby thumb.DisplayName, thumb.FullPath
                                            select thumb;
            int counter = 0;
            foreach(Thumb thumb in sortedList)
            {
                myControls.SetChildIndex(thumb, counter);
                counter++;
            }
        }

        public void AddThumb(Thumb newThumb)
        {
            myControls.Add(newThumb);
            UpdateGridView();
        }

        public void RemoveThumb(Thumb newThumb)
        {
            myControls.RemoveAt(myControls.GetChildIndex(newThumb));
            UpdateGridView();
        }

        private void panelApplicationListView_Layout(object sender, LayoutEventArgs e)
        {
            UpdateGridView();
        }

        private void panelApplicationListView_Resize(object sender, EventArgs e)
        {
            UpdateGridView();
        }
    }
}
