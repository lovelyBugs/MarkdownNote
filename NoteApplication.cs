using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Web;

namespace NoteApplication
{
    public class NoteInfoApplication
    {
        public List<NoteMenu> GetMarkdownNoteFiles(string noteBaseDir)
        {
            List<FileDirectoryInfo> fileInfos = new List<FileDirectoryInfo>();
            foreach(string file in Directory.EnumerateFiles(noteBaseDir, "*.md", SearchOption.AllDirectories))
            {
                string fileName = Path.GetFileName(file);
                string directory = Path.GetDirectoryName(file);
                string relative = Path.GetRelativePath(noteBaseDir, file);
                relative.Split("\\").Where(folder => !string.IsNullOrWhiteSpace(folder));
                fileInfos.Add(new FileDirectoryInfo()
                {
                    PathStrArr = relative.Split("\\").Where(folder => !string.IsNullOrWhiteSpace(folder)).ToArray(),
                    FileName = fileName,
                });
            }

            DirectoryStructure root = new DirectoryStructure() {
                SubDirectories = new Dictionary<string, DirectoryStructure>(),
                CurrentFolderName = "notes",
                FileNames = new List<string>(),
                CurrentMenu = new NoteMenu() {
                    Router = "notes",
                    Title = "notes",
                    Folder = "notes",
                    SubMenu = new List<NoteMenu>(),
                    NoteFiles = new List<MarkdownNoteFile>()
                }
            };
            if (fileInfos.Count > 0)
            {
                int minDepth = fileInfos.Min(f => f.PathStrArr.Length);
                int maxDepth = fileInfos.Max(f => f.PathStrArr.Length);
                IOrderedEnumerable<FileDirectoryInfo> sortedFileInfos = fileInfos.OrderBy(f => f.PathStrArr.Length);

                 DirectoryStructure currentDirectory = root;
                NoteMenu currentMenu = root.CurrentMenu;
                //List<NoteMenu> currentMenus = meuns;
                List<string> currentArr = new List<string>();
                foreach (FileDirectoryInfo file in sortedFileInfos)
                {
                    currentDirectory = root;
                    currentMenu = root.CurrentMenu;
                    currentArr.Clear();
                    for (int depth = 0; depth < file.PathStrArr.Length; depth++)
                    {
                        if (depth == file.PathStrArr.Length - 1)
                        {
                            // is file
                            currentDirectory.FileNames.Add(file.PathStrArr[depth]);
                            currentMenu.NoteFiles.Add(new MarkdownNoteFile()
                            {
                                FileName = file.PathStrArr[depth],
                                FilePath = $"/notes/{string.Join("/", currentArr)}/"
                            });
                        }
                        else
                        {
                            // is folder
                            currentArr.Add(HttpUtility.UrlEncode(file.PathStrArr[depth], Encoding.UTF8));
                            if (!currentDirectory.SubDirectories.ContainsKey(file.PathStrArr[depth]))
                            {
                                currentDirectory.SubDirectories.Add(file.PathStrArr[depth], new DirectoryStructure()
                                {
                                    CurrentFolderName = file.PathStrArr[depth],
                                    FileNames = new List<string>(),
                                    SubDirectories = new Dictionary<string, DirectoryStructure>(),
                                    CurrentMenu = new NoteMenu()
                                    {
                                        Router = string.Join("/", currentArr),
                                        Folder = $"/notes/{string.Join("/", currentArr)}",
                                        Title = file.PathStrArr[depth],
                                        NoteFiles = new List<MarkdownNoteFile>(),
                                        SubMenu = new List<NoteMenu>()
                                    }

                                });
                                currentMenu.SubMenu.Add(currentDirectory.SubDirectories[file.PathStrArr[depth]].CurrentMenu);
                            }
                            currentMenu = currentDirectory.SubDirectories[file.PathStrArr[depth]].CurrentMenu;
                            currentDirectory = currentDirectory.SubDirectories[file.PathStrArr[depth]];
                        }
                        
                        
                    }
                }
            }
            return root.SubDirectories.Select(directory => directory.Value.CurrentMenu).ToList();
        }
    }
    public class DirectoryStructure
    {
        public string CurrentFolderName { get; set; }
        public List<string> FileNames { get; set; }
        public NoteMenu CurrentMenu { get; set; }
        public Dictionary<string, DirectoryStructure> SubDirectories { get; set; }
    }
    public class FileDirectoryInfo
    {
        public string[] PathStrArr { get; set; }
        public string FileName { get; set; }
    }
    public class MarkdownNoteFile
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
    public class NoteMenu
    {

        public string Router { get; set; }
        public string Title { get; set; }
        public string Folder { get; set; }
        public List<NoteMenu> SubMenu { get; set; }
        public List<MarkdownNoteFile> NoteFiles { get; set; }
    }
}

// write note of 3d map 
// write note of asp.net CORS config
// write note of angular environment config and deploy setting
// write note of angular view markdown note