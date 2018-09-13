using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wallsh
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, int uParam, String pvParam, UInt32 fWinIni);
        private static UInt32 SPI_GETDESKWALLPAPER = 0x73;
        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static int MAX_PATH = 260;
        private static UInt32 SPIF_UPDATEINIFILE = 0x1;
            
        static void Main(string[] args)
        {
            if(args.Length > 0)
                switch (args[0])
                {
                    case "/G":
                        getWallpaper();
                       break;
                    case "/S":
                        if (args.Length == 2)
                            setWallpaper(args[1]);
                        else {
                            Console.Write("No path introduced!");
                            Environment.Exit(1);
                        }
                        break;
                    default:
                        printHelp();
                        break;
                }
            else printHelp();
        }

        private static void getWallpaper()
        {
            string currentWallpaper = new string('\0', MAX_PATH);

            SystemParametersInfo(SPI_GETDESKWALLPAPER, currentWallpaper.Length, currentWallpaper, 0);
            
            Console.Write(currentWallpaper.Substring(0, currentWallpaper.IndexOf('\0')));
        }

        private static void setWallpaper(string filename)
        {
            if (File.Exists(filename))
            {
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Wallpaper", filename);
            }
            else{
                Console.Write("Invalid path introduced!");
                Environment.Exit(1);
            }
        }

        private static void printHelp()
        {
            string message = 
                "\r\n wallsh (v0.1): get and set the desktop wallpaper." +
                "\r\n\r\n Usage:\twallsh [options]" +
                "\r\n\r\n Options:" +
                "\r\n /H      \t\t\tprint help" +
                "\r\n /G      \t\t\tget the current wallpaper" +
                "\r\n /S [path]\t\t\tset a wallpaper" +
                "\r\n\r\n Created by Jaume Segarra";

            Console.Write(message);
        }
    }
}
