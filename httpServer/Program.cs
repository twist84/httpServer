using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Linq;

namespace httpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = args.Length > 0 ? args[0] : Properties.Settings.Default.Root;
            Console.WriteLine("Type help to get started:");
            while (true)
            {
                string command = Console.ReadLine();
                Commands(root, command);
            }
        }

        static void Commands(string root, string command)
        {
            Console.Clear();
            var Props = Properties.Settings.Default;
            var url = Props.Protocol + "://" + Props.IP + ":" + Props.Port + "/";

            var fileSystem = new PhysicalFileSystem(root);
            var options = new FileServerOptions();

            options.FileSystem = fileSystem;
            options.DefaultFilesOptions.DefaultFileNames.Clear();

            if (Props.DefaultFileList.Contains(","))
            {
                string[] defaultLists = Props.DefaultFileList.Split(',');
                foreach (string defaultList in defaultLists)
                {
                    options.DefaultFilesOptions.DefaultFileNames.Add(defaultList);
                }
            }
            else options.DefaultFilesOptions.DefaultFileNames.Add(Props.DefaultFileList);

            if (Props.CustomFileList.Contains(","))
            {
                string[] customLists = Props.CustomFileList.Split(',');
                foreach (string customList in customLists)
                {
                    options.DefaultFilesOptions.DefaultFileNames.Add(customList);
                }
            }
            else options.DefaultFilesOptions.DefaultFileNames.Add(Props.CustomFileList);

            switch (Props.EnableDirectoryBrowsing)
            {
                case true:
                    options.EnableDirectoryBrowsing = true;
                    options.DefaultFilesOptions.DefaultFileNames.Clear();
                    break;

                case false:
                    options.EnableDirectoryBrowsing = false;
                    options.DefaultFilesOptions.DefaultFileNames.Add(Props.CustomFileList);
                    break;
            }

            switch (command)
            {
                case "Protocol":
                case "protocol":
                    Console.WriteLine("Type the Protocol to use, the default is \"http\":");
                    Console.WriteLine("The current Protocol is \"{0}\":", Props.Protocol);

                    string Protocol = Console.ReadLine();
                    Props.Protocol = Protocol;
                    Props.Save();
                    break;

                case "IP":
                case "ip":
                    Console.WriteLine("Type the IP address to use, the default is \"127.0.0.1\":");
                    Console.WriteLine("The current Protocol is \"{0}\":", Props.IP);

                    string IP = Console.ReadLine();
                    Props.IP = IP;
                    Props.Save();
                    break;

                case "Port":
                case "port":
                    Console.WriteLine("Type the Port address to use, the default is \"8080\":");
                    Console.WriteLine("The current Protocol is \"{0}\":", Props.Port);

                    string Port = Console.ReadLine();
                    Props.Port = Port;
                    Props.Save();
                    break;

                case "Root":
                case "root":
                    Console.WriteLine("Type the Port address to use, the default is \".\" which is the location of httpServer.exe:");
                    Console.WriteLine("The current Protocol is \"{0}\":", Props.Root);

                    string Root = Console.ReadLine();
                    Props.Root = Root;
                    Props.Save();
                    break;

                case "Directory":
                case "directory":
                case "Dir":
                case "dir":
                case "D":
                case "d":
                    Console.WriteLine("Type true or false to enable or disable directory browsing, the default is \"true\":");
                    Console.WriteLine("The current Protocol is \"{0}\":", options.EnableDirectoryBrowsing);

                    string EnableDirectoryBrowsing = Console.ReadLine();
                    Props.EnableDirectoryBrowsing = Convert.ToBoolean(EnableDirectoryBrowsing);
                    options.EnableDirectoryBrowsing = Convert.ToBoolean(EnableDirectoryBrowsing);
                    Props.Save();
                    break;

                case "File":
                case "file":
                case "F":
                case "f":
                    Console.WriteLine("Type the file that will load when the webpage is viewed, the default are:");
                    Console.WriteLine("The current Files are:");

                    for (int i = 0; i < options.DefaultFilesOptions.DefaultFileNames.Count; i++)
                    {
                        if (!options.DefaultFilesOptions.DefaultFileNames[i].Contains(','))
                            Console.WriteLine("\"{0}\"",
                                options.DefaultFilesOptions.DefaultFileNames[i].ToString());
                    }
                    string CustomFileName = Console.ReadLine();
                    Props.CustomFileList = CustomFileName;
                    if (Props.CustomFileList.Contains(","))
                    {
                        string[] customFiles = Props.CustomFileList.Split(',');
                        foreach (string customFile in customFiles)
                        {
                            options.DefaultFilesOptions.DefaultFileNames.Add(customFile);
                        }
                    }
                    else options.DefaultFilesOptions.DefaultFileNames.Add(Props.CustomFileList);
                    Props.Save();
                    break;

                case "Reset":
                case "reset":
                case "Reload":
                case "reload":
                case "R":
                case "r":
                    Props.Reset();
                    Props.Save();
                    Props.Reload();
                    break;

                case "Information":
                case "information":
                case "Info":
                case "info":
                case "I":
                case "i":
                    Console.WriteLine("The current Protocol is: \"{0}\"", Props.Protocol);
                    Console.WriteLine("The current IP is: \"{0}\"", Props.IP);
                    Console.WriteLine("The current Port is: \"{0}\"", Props.Port);
                    Console.WriteLine("The current Root is: \"{0}\"", Props.Root);
                    Console.WriteLine("Is Directory browsing enabled: \"{0}\"", options.EnableDirectoryBrowsing);

                    if (options.EnableDirectoryBrowsing == false)
                    {
                        Console.WriteLine("The current default Files are:");
                        for (int i = 0; i < options.DefaultFilesOptions.DefaultFileNames.Count; i++)
                            if (!options.DefaultFilesOptions.DefaultFileNames[i].Contains(','))
                                Console.WriteLine("\"{0}\"",
                                    options.DefaultFilesOptions.DefaultFileNames[i].ToString());
                    }
                    break;

                case "Start":
                case "start":
                case "S":
                case "s":
                    WebApp.Start(url, builder => builder.UseFileServer(options));
                    Console.WriteLine("Listening at " + url);
                    break;

                case "Quit":
                case "quit":
                case "Exit":
                case "exit":
                case "Stop":
                case "stop":
                case "Q":
                case "q":
                case "X":
                case "x":
                    Environment.Exit(0);
                    break;

                case "Help":
                case "help":
                case "H":
                case "h":
                case "?":
                    Console.WriteLine("Type Help to show this message.\n");
                    Console.WriteLine("Type Start to start the server.\n");
                    Console.WriteLine("Type Quit to stop the server.\n");
                    Console.WriteLine("Type Port to change the port.\n");
                    Console.WriteLine("Type Root to change the root.\n");
                    Console.WriteLine("Type Directory to change the directory.\n");
                    Console.WriteLine("Type File to change the default file loaded.\n");
                    Console.WriteLine("Type Reset to reset.\n");
                    break;

                default:
                    Console.WriteLine("Type Help to show this message.\n");
                    Console.WriteLine("Type Start to start the server.\n");
                    Console.WriteLine("Type Quit to stop the server.\n");
                    Console.WriteLine("Type Port to change the port.\n");
                    Console.WriteLine("Type Root to change the root.\n");
                    Console.WriteLine("Type Directory to change the directory.\n");
                    Console.WriteLine("Type File to change the default file loaded.\n");
                    Console.WriteLine("Type Reset to reset.\n");
                    break;
            }
        }
    }
}