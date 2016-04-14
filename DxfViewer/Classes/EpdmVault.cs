using System;
using EdmLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DxfAndPDFViewer.Classes
{
    public class EpdmVault
    {
        static public IEdmVault7 Vault;
        static public IEdmVault8 Vault8;
        static void CheckPdmVault()
        {
            try
            {
                if (Vault == null)
                {
                    Vault = new EdmVault5();
                }

                var ok = Vault.IsLoggedIn;

                if (!ok)
                {
                    //vault.LoginAuto("Tets_debag", 0);
                    Vault.LoginAuto("Vents-PDM", 0);
                }

                Vault8 = (IEdmVault8)Vault;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "; " + ex.StackTrace);
            }
        }
        public class ColumnsBind
        {
            public string FilePath { get; set; }
            public string FileName //{ get; set; }
            { get { return Path.GetFileNameWithoutExtension(FilePath); } set { } }
            //public string FileType { get; set; }
            //{ get { return Path.GetExtension(FilePath); } set { } }
            public string ImageSrc { get; set; }
            public static List<ColumnsBind> GetFiles(string path)
            {
                var files = Directory.GetFiles(path);
                var list = files.Select(file => new ColumnsBind
                {
                    FilePath = file
                }).ToList();
                return list;
            }
            public int FileId { get; set; }
            public int FolderId { get; set; }
            public string PartNumber { get; set; }
            public int Version { get; set; }
        }
        static public List<ColumnsBind> SearchDoc(string name)
        {
            var namedoc = new List<ColumnsBind>();

            CheckPdmVault();

            var search = (IEdmSearch5)Vault.CreateUtility(EdmUtility.EdmUtil_Search);
            search.FindFiles = true;
            search.FindFolders = false;

            search.FileName = "%" + name + "%";
            var result = search.GetFirstResult();

            while ((result != null))
            {
                var columnClass = new ColumnsBind()
                {
                    FileName = result.Name,
                    FileId = result.ID,
                    FolderId = result.ParentFolderID,
                    FilePath = result.Path,
                    Version = result.Version,
                    ImageSrc = ImageSrc(result.Path)
                };
                namedoc.Add(columnClass);
                result = search.GetNextResult();
            }
            return namedoc;
        }
        static string ImageSrc(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".pdf":
                    return @"\Resource\Pdm Icons\Adobe-ReaderIco.png";
                case ".sldasm":
                    return @"\Resource\Pdm Icons\assemblyIco.png";
                case ".slddrw":
                case ".dxf":
                    return @"\Resource\Pdm Icons\drawingIco.png";
                case ".sldprt":
                    return @"\Resource\Pdm Icons\partIco.png";
                case ".eprt":
                case ".edrw":
                case ".easm":
                    return @"\Resource\Pdm Icons\edrwprtIco.jpg";
                default: return null;
            }
        }
    }
}