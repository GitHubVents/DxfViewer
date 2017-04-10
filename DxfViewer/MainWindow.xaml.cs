using System.Windows;
using System.IO;
using System;
using EdmLib;
using DxfAndPDFViewer;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using System.Windows.Controls;
using DxfAndPDFViewer.Classes;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using SolidWorks.Interop.sldworks;
using System.Runtime.InteropServices;
using SolidWorks.Interop.swconst;

namespace DxfViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region Variables
            SldWorks swApp;
            UserControlEdr DXFControl;
            PDFControl pdfControl;
            WindowsFormsHost hostpdf;
            WindowsFormsHost hostDxf;
        #endregion
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FindAndKillProcess("AddInSrv");
                GenerateContextMenu();

                var rowStyle = new Style(typeof(DataGridRow));
                rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent, new MouseButtonEventHandler(Row_DoubleClick)));
                DgFiles.RowStyle = rowStyle;

                 
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + ", \n" + ex.StackTrace);
            }
        }
        private void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var folderDialog = new FolderBrowserDialog())
                {
                    DialogResult result = folderDialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        DgFiles.ItemsSource = EpdmVault.ColumnsBind.GetFiles(folderDialog.SelectedPath);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + ", \n" + ex.StackTrace);
            }
        }
        private void DrawBitmapPreview(IEdmFile5 file)
        {
            //try
            //{
            //    IEdmEnumeratorVariable5 varEnum = default(IEdmEnumeratorVariable5);
            //    varEnum = (IEdmEnumeratorVariable5)file.GetEnumeratorVariable();
            //    IEdmBitmap5 preview = default(IEdmBitmap5);
            //    preview = varEnum.GetThumbnail();
            //    if (preview == null)
            //        return;
            //    preview.Draw(this.Handle.ToInt32(), 45, 220, 0, 0);

            //}
            //catch (System.Runtime.InteropServices.COMException ex)
            //{
            //    MessageBox.Show("HRESULT = 0x" + ex.ErrorCode.ToString("X") + " " + ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        private void DgFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //var item = (EpdmVault.ColumnsBind)DgFiles.SelectedItem;
                var item = default(EpdmVault.ColumnsBind);
                DgFiles.Dispatcher.Invoke(new Action(() => { item = (EpdmVault.ColumnsBind)DgFiles.SelectedItem; }));
                if (item != null)
                {                   
                    var oFolder = default(IEdmFolder5);
                    var aFile = EpdmVault.Vault.GetFileFromPath(item.FilePath, out oFolder);
                    if (aFile == null) 
                    {
                        System.Windows.MessageBox.Show("Файл не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var LocWin = new LoadLocalFile();
                    aFile.GetFileCopy(0, "", item.FolderId, (int)EdmGetFlag.EdmGet_MakeReadOnly + (int)EdmGetFlag.EdmGet_Simple);
                    //aFile.GetFileCopy(0, "", item.FolderId,(int)EdmGetFlag.EdmGet_Refs+ (int)EdmGetFlag.EdmGet_RefsOnlyMissing+ (int)EdmGetFlag.EdmGet_RefsOverwriteLocked+ (int)EdmGetFlag.EdmGet_RefsVerLatest);
                    var extension = Path.GetExtension(item.FilePath);
                    switch (extension.ToLower())
                    {
                        case ".dxf":
                        case ".eprt":
                        case ".easm":
                        case ".edrw":
                        case ".sldprt":
                        case ".sldasm":
                        case ".slddrw":
                            DXFControl = new UserControlEdr();
                            hostDxf = new WindowsFormsHost();
                            hostDxf.Child = DXFControl;
                            HostControl.Children.Add(hostDxf);
                            DXFControl.axEModelViewControl1.OpenDoc(item.FilePath , false, false, true, "");
                            if (pdfControl != null)
                            {
                                pdfControl.Dispose();
                            }
                            break;
                        case ".pdf":
                            pdfControl = new PDFControl();
                            hostpdf = new WindowsFormsHost();
                            hostpdf.Child = pdfControl;
                            HostControl.Children.Add(hostpdf);
                            pdfControl.OpenFile(item.FilePath);
                            if (DXFControl != null)
                            {
                                DXFControl.Dispose();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + ", \n" + ex.StackTrace);
            }
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtBoxSearch.Text)) return;

                DgFiles.ItemsSource = null;

                //DgFiles.ItemsSource = EpdmVault.SearchDoc(TxtBoxSearch.Text);
                //Task.Factory.StartNew(new Action(() => EpdmVault.SearchDoc(TxtBoxSearch.Text)));
                //DgFiles.Dispatcher.Invoke(new Action(() => DgFiles.ItemsSource = EpdmVault.SearchDoc(TxtBoxSearch.Text)));

                Task.Factory.StartNew(new Action(() => { runBindDg(); }));

                //ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                //{
                //    DgFiles.Dispatcher.Invoke(new Action(() => { DgFiles.ItemsSource = EpdmVault.SearchDoc(TxtBoxSearch.Text); }));
                //}));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + ", \n" + ex.StackTrace);
            }
        }
        delegate void SearchInPdm();
        void runBindDg()
        {
            this.Dispatcher.Invoke(new SearchInPdm(BindDgFiles), null);
        }
        void BindDgFiles()
        {
            DgFiles.ItemsSource = EpdmVault.SearchDoc(TxtBoxSearch.Text);
        }
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtnSearch_Click( sender, null);
            }
        }
        #region Click DataGridRow        
            private void GenerateContextMenu()
            {
                DgFiles.ContextMenu = ContextM.BuildContextMenu(OpenSldFile_Click, OpenPdmFolder_Click);
            }
            void OpenSolidWorks()
            {
                try
                {
                    var item = default(EpdmVault.ColumnsBind);
                    DgFiles.Dispatcher.Invoke(new Action(() => { item = (EpdmVault.ColumnsBind)DgFiles.SelectedItem; }));
                    var processes = Process.GetProcessesByName("SLDWORKS");
                    if (processes.Length == 0)
                    {
                        //swApp = new SldWorks() { Visible = true };
                        Process.Start(item.FilePath);
                    }
                    else
                    {
                        swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                        var fileType = Path.GetExtension(item.FilePath);
                        var swDocType = 0;
                        switch (fileType.ToLower())
                        {
                            case ".sldprt":
                                swDocType = (int)swDocumentTypes_e.swDocPART;
                            break;
                            case ".sldasm":
                                swDocType = (int)swDocumentTypes_e.swDocASSEMBLY;
                            break;
                            case ".slddrw":
                                swDocType = (int)swDocumentTypes_e.swDocDRAWING;
                            break;
                        }
                        swApp.OpenDoc(item.FilePath, swDocType);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                }
            }
            void OpenPdf()
            {
                try
                {
                    var item = default(EpdmVault.ColumnsBind);
                    DgFiles.Dispatcher.Invoke(new Action(() => { item = (EpdmVault.ColumnsBind)DgFiles.SelectedItem; }));
               
                    var processes = Process.GetProcessesByName("PDF");
                    if (processes.Length == 0)
                    {
                        //swApp = new SldWorks() { Visible = true };
                        using (Process.Start(item.FilePath)) { };
                    }
                    else
                    {
                        var fileType = Path.GetExtension(item.FilePath);
                        switch (fileType.ToLower())
                        {
                            case ".pdf":
                            using (Process.Start(fileType)) { };
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                }
            }
            private void Row_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                Task.Factory.StartNew(() => {
                    var item = default(EpdmVault.ColumnsBind);
                    DgFiles.Dispatcher.Invoke(new Action(() => { item = (EpdmVault.ColumnsBind)DgFiles.SelectedItem; }));
                    var fileType = Path.GetExtension(item.FilePath);
                    switch (fileType.ToLower())
                    {
                        case ".sldprt":
                            OpenSolidWorks();
                            break;
                        case ".sldasm":
                            OpenSolidWorks();
                            break;
                        case ".slddrw":
                            OpenSolidWorks();
                            break;
                        case ".pdf":
                            OpenPdf();
                            break;
                    }
                });
            }
            private void OpenSldFile_Click(object sender, RoutedEventArgs e)
            {
                Task.Factory.StartNew(() => { OpenSolidWorks(); });
            }
            private void OpenPdmFolder_Click(object sender, RoutedEventArgs e)
            {
                var item = (EpdmVault.ColumnsBind)DgFiles.SelectedItem;
                EpdmVault.Vault8.OpenContainingFolder(Convert.ToInt32(item.FileId), 0);
            }
        #endregion
        public void FindAndKillProcess(string name)
        {
            //foreach (Process clsProcess in Process.GetProcesses())
            //{
            //    if (clsProcess.ProcessName.StartsWith(name))
            //    {
            //        clsProcess.Kill();
            //    }
            //}
            var processes = Process.GetProcessesByName(name);
            foreach (var process in processes)
            {
                process.Kill();
            }
        }
        ~MainWindow()
        {
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (pdfControl != null)
            {
                pdfControl.Dispose();
            }
            if (DXFControl != null)
            {
                DXFControl.Dispose();
            }
        }
    }
}