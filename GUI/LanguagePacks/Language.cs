using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.LanguagePacks
{
    public class Language
    {
        #region MenuBar
        #region MenuBar Menu
        public string MenuBar_Menu { get; set; }

        public string MenuBar_Menu_Theme { get; set; }
        public string MenuBar_Menu_Theme_Classic { get; set; }
        public string MenuBar_Menu_Theme_GreenBlue { get; set; }

        public string MenuBar_Language { get; set; }
        public string MenuBar_Language_English { get; set; }
        public string MenuBar_Language_German { get; set; }

        public string MenuBar_Menu_Restart { get; set; }
        public string MenuBar_Menu_ReportBug { get; set; }
        public string MenuBar_Menu_Exit { get; set; }
        #endregion MenuBar Menu

        #region MenuBar Share
        public string MenuBar_Share { get; set; }
        #endregion MenuBar Share

        #region MenuBar Join
        public string MenuBar_Join { get; set; }
        #endregion MenuBar Join
        #endregion MenuBar

        #region Connection
        public string Connection_TBl_IP { get; set; }
        public string Connection_TBl_Port { get; set; }
        public string Connection_Status_Connected { get; set; }
        public string Connection_Status_Disconnected { get; set; }
        #endregion Connection

        #region PaintMenu
        public string PaintMenu_Clear_Tooltip { get; set; }
        #endregion PaintMenu

        #region ControlMenu
        public string ControlMenu_Lock_Tooltip { get; set; }
        #endregion ControlMenu

        #region ExitDialog
        public string ExitDlg_Text { get; set; }
        public string ExitDlg_Caption { get; set; }
        #endregion ExitDialog
    }
}
