using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithRegistry
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Enable Mouse Right Button - push 1");
            Console.WriteLine("2. Disable Mouse Right Button - push 2");
            string choose = Console.ReadLine();

            string path = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(path, true);
            if (reg == null)
            {
                reg = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree);
            }

            if (choose == "1")
            {
                Registry.SetValue(path, "NoViewContextMenu", 0x0, RegistryValueKind.DWord);
            }
            else
            {
                Registry.SetValue(path, "NoViewContextMenu", 0x1, RegistryValueKind.DWord);
            }
           
            reg.Close();
        }
    }
}