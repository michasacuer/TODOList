using System;
using System.IO;

namespace TODOList
{
    public class ShortcutName
    {
        /// <summary>
        /// In order to determine if an existing shortcut 
        /// file is pointing to our application, we need to be able 
        /// to read the TargetPath from a shortcut file.
        /// </summary>
        /// <param name="shortcutFilename"></param>
        /// <returns></returns>
        public string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = Path.GetFileName(shortcutFilename);

            Shell32.Shell shell = new Shell32.ShellClass();
            Shell32.Folder folder = shell.NameSpace(pathOnly);
            Shell32.FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link =
                  (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return String.Empty; // Not found // nie pisze się komentarzy w kodzie
        }
    }
}
