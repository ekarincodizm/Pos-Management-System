using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Pos_Management_System
{
    public static class PhysicalAddressPOS
    {
        public static PhysicalAddressPC ShowNetworkInterfaces()
        {
            try
            {
                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                //Console.WriteLine("Interface information for {0}.{1}     ",
                //        computerProperties.HostName, computerProperties.DomainName);
                PhysicalAddressPC pc = new PhysicalAddressPC();
                pc.ComputerName = computerProperties.HostName;
                if (nics == null || nics.Length < 1)
                {
                    //Console.WriteLine("  No network interfaces found.");
                    pc.ComputerName = computerProperties.HostName;
                    pc.EthernetAddress = computerProperties.HostName;
                    pc.WirelessAddress = computerProperties.HostName;
                    return pc;
                }

                //Console.WriteLine("  Number of interfaces .................... : {0}", nics.Length);

                foreach (NetworkInterface adapter in nics)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties(); //  .GetIPInterfaceProperties();
                    Console.WriteLine();
                    //Console.WriteLine(adapter.Description);
                    //Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                    //Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                    if (adapter.NetworkInterfaceType.ToString().Contains("Ethernet"))
                    {
                        //Console.Write("  Physical address ........................ : ");
                        PhysicalAddress address = adapter.GetPhysicalAddress();
                        byte[] bytes = address.GetAddressBytes();
                        string no = "";
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            // Display the physical address in hexadecimal.
                            //Console.Write("{0}", bytes[i].ToString("X2"));
                            no = no + bytes[i].ToString("X2").ToString();
                            // Insert a hyphen after each byte, unless we are at the end of the 
                            // address.
                            if (i != bytes.Length - 1)
                            {
                                //Console.Write("-");
                                no = no + "-";
                            }
                        }
                        pc.EthernetAddress = no;
                    }
                    else if (adapter.NetworkInterfaceType.ToString().Contains("Wireless"))
                    {
                        //Console.Write("  Physical address ........................ : ");
                        PhysicalAddress address = adapter.GetPhysicalAddress();
                        byte[] bytes = address.GetAddressBytes();
                        string no = "";
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            // Display the physical address in hexadecimal.
                            //Console.Write("{0}", bytes[i].ToString("X2"));
                            no = no + bytes[i].ToString("X2").ToString();
                            // Insert a hyphen after each byte, unless we are at the end of the 
                            // address.
                            if (i != bytes.Length - 1)
                            {
                                //Console.Write("-");
                                no = no + "-";
                            }
                        }
                        pc.WirelessAddress = no;
                    }
                    if (pc.EthernetAddress != null && pc.WirelessAddress != null)
                    {
                        return pc;
                    }
                    else if (pc.EthernetAddress == null)
                    {
                        pc.EthernetAddress = "Undefine";
                    }
                    else if (pc.WirelessAddress == null)
                    {
                        pc.WirelessAddress = "Undefine";
                    }

                    Console.WriteLine();
                }
                return pc;
            }
            catch (Exception ex)
            {
                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                PhysicalAddressPC pc = new PhysicalAddressPC();
                pc.ComputerName = computerProperties.HostName;
                pc.EthernetAddress = computerProperties.HostName;
                pc.WirelessAddress = computerProperties.HostName;
                return pc;
            }

        }
    }
}
