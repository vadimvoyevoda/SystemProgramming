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
            string choose = Console.ReadLine();

            string path = @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(path, true);
            if (reg == null)
            {
                reg = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            
            if ((int)reg.GetValue("NoWinKeys", 0) == 0)
            {
                reg.SetValue("NoWinKeys", 0x1, RegistryValueKind.DWord);
            }
            else
            {
                reg.SetValue("NoWinKeys", 0x0, RegistryValueKind.DWord);
            }

            reg.Close();
        }
    }
}