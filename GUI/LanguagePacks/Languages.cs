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
            General_Cancel = "Cancel",
            General_Close = "Close",
            General_Connect = "Connect",
            General_Connection = "Connection",
            General_Error = "Error",
            General_IP = "IP",
            General_Port = "Port",
            General_InvalidInput = "Invalid Input",

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
            ControlMenu_Lock_Text = "The host has locked the drawing board for others!",
            
            Dialog_ConnectionInfo_Infotext = "Others can connect to you by using this data.",
            Dialog_ChangeConnection_InvalidIP = "Invalid IP",
            Dialog_ChangeConnection_InvalidPort = "Invalid Port",
            Dialog_ConnectionError_ErrorMsg = "Failed to establish a connection to your partner!",

            ExitDlg_Text = "Do you really want to exit?",
            ExitDlg_Caption = "Exit"
        };

        public static Language German = new Language()
        {
            General_Cancel = "Abbrechen",
            General_Close = "Schließen",
            General_Connect = "Verbinden",
            General_Connection = "Verbindung",
            General_Error = "Fehler",
            General_IP = "IP-Adresse",
            General_Port = "Port",
            General_InvalidInput = "Ungültige Eingabe",

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
            ControlMenu_Lock_Text = "Der Host hat das Zeichenbrett für andere gesperrt!",
            
            Dialog_ConnectionInfo_Infotext = "Mit diesen Daten können sich andere mit dir verbinden.",
            Dialog_ChangeConnection_InvalidIP = "Ungültige IP-Adresse",
            Dialog_ChangeConnection_InvalidPort = "Ungültiger Port",
            Dialog_ConnectionError_ErrorMsg = "Es konnte keine Verbindung zum Partner hergestellt werden!",

            ExitDlg_Text = "Wirklich beenden?",
            ExitDlg_Caption = "Beenden"
        };
    }
}
