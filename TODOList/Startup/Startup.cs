using IWshRuntimeLibrary;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using TODOList.Properties;

namespace TODOList
{
    public class Startup
    {
        public bool SetStartup()
        {
            if (!Settings.Default.IfStartup)
            {
                WshShellClass wshShell = new WshShellClass();
                IWshRuntimeLibrary.IWshShortcut shortcut;
                string startUpFolderPath =
                  Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                // Create the shortcut //nie pisze sie komentarzy w kodzie
                shortcut =
                  (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(
                    startUpFolderPath + "\\" +
                    Application.Current.MainWindow.GetType().Assembly + ".lnk");

                shortcut.TargetPath = Assembly.GetExecutingAssembly().Location;
                shortcut.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                shortcut.Description = "Launch My Application";
                shortcut.IconLocation = AppDomain.CurrentDomain.BaseDirectory + @"\list.ico";
                shortcut.Save();

                return true;
            }
            else
            {
                string startUpFolderPath =
                Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                DirectoryInfo di = new DirectoryInfo(startUpFolderPath);
                FileInfo[] files = di.GetFiles("*.lnk");

                ShortcutName GetName = new ShortcutName();

                foreach (FileInfo fi in files)
                {
                    string shortcutTargetFile = GetName.GetShortcutTargetFile(fi.FullName);

                    if (shortcutTargetFile.EndsWith("TODOList.exe",
                          StringComparison.InvariantCultureIgnoreCase))
                    {
                        System.IO.File.Delete(fi.FullName);
                    }
                }
                return false;
            }
        }
    }
}
