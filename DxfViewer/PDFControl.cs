using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DxfAndPDFViewer
{
    public partial class PDFControl : UserControl
    {
        public PDFControl()
        {
            InitializeComponent();
        }
        public void OpenFile(string path)
        {
            axAcroPDF1.LoadFile(path);
        }

    }
}