using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.LanguagePacks
{
    public static class Languages
    {

        public static Language English = new Language()
        {
            MenuBar_Menu = "Menu",

            MenuBar_Menu_Theme = "Theme",
            MenuBar_Menu_Theme_Classic = "Classic",
            MenuBar_Menu_Theme_GreenBlue = "GreenBlue",

            MenuBar_Language = "Language",
            MenuBar_Language_English = "English",
            MenuBar_Language_German = "German",

            MenuBar_Menu_Restart = "Restart",
            MenuBar_Menu_ReportBug = "Report a Bug",
            MenuBar_Menu_Exit = "Exit",

            MenuBar_Share = "Share",

            MenuBar_Join = "Join",

            Connection_TBl_IP = "IP",
            Connection_TBl_Port = "Port",
            Connection_Status_Connected = "Connected",
            Connection_Status_Disconnected = "Disconnected",
            
            PaintMenu_Clear_Tooltip = "Clear the drawing board",
            
            ControlMenu_Lock_Tooltip = "(Un)lock the drawing board for others",

            ExitDlg_Text = "Do you really want to exit?",
            ExitDlg_Caption = "Exit"
        };

        public static Language German = new Language()
        {
            MenuBar_Menu = "Menü",

            MenuBar_Menu_Theme = "Design",
            MenuBar_Menu_Theme_Classic = "Klassisch",
            MenuBar_Menu_Theme_GreenBlue = "Grün-Blau",

            MenuBar_Language = "Sprache",
            MenuBar_Language_English = "Englisch",
            MenuBar_Language_German = "Deutsch",

            MenuBar_Menu_Restart = "Neustarten",
            MenuBar_Menu_ReportBug = "Fehler melden",
            MenuBar_Menu_Exit = "Beenden",

            MenuBar_Share = "Freigeben",

            MenuBar_Join = "Beitreten",

            Connection_TBl_IP = "IP",
            Connection_TBl_Port = "Port",
            Connection_Status_Connected = "Verbunden",
            Connection_Status_Disconnected = "Getrennt",

            PaintMenu_Clear_Tooltip = "Das Kunstwerk verwerfen",

            ControlMenu_Lock_Tooltip = "Für andere die Künstlerfläche (ent)sperren",

            ExitDlg_Text = "Wirklich beenden?",
            ExitDlg_Caption = "Beenden"
        };
    }
}
