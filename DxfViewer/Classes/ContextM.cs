using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DxfAndPDFViewer.Classes
{
    public class ContextM
    {
        static private MenuItem _miaGoToFile;
        static private MenuItem _miaOpenFile;
        static public ContextMenu BuildContextMenu(RoutedEventHandler OpenSldFile_Click, RoutedEventHandler OpenPdmFolder_Click)
        {
            var theMenu = new ContextMenu();

            _miaOpenFile = new MenuItem { Header = "Открыть файл" };
            _miaOpenFile.Click += OpenSldFile_Click;

            _miaGoToFile = new MenuItem { Header = "Перейти к файлу..." };
            _miaGoToFile.Click += OpenPdmFolder_Click;

            theMenu.Items.Add(_miaOpenFile);
            theMenu.Items.Add(_miaGoToFile);

            return theMenu;
        }
    }
}
