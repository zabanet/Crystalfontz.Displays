using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using System.Management;
using System.IO;
using System.IO.Ports;

/*
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation version 3 of the License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
    
    Copyright Robert M. Meffe February 2009.
*/

namespace Crystalfontz.Displays
{
    public class SerialPorts
    {
        /// <summary>
        /// 
        /// </summary>
        public static ComDevice[] ComPorts
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                //is this linux?
                if (p != 4 && p != 128 && p != 6)
                {
                    //First we want to get a list of port names
                    //I would have went full WMI but I had problems finding Virtual Com Ports
                    string[] _portNames = SerialPort.GetPortNames().Distinct().ToArray<string>();
                    showPorts(_portNames);
                    //Create an bindable array of ComDevice(s) to return
                    ComDevice[] _return = new ComDevice[_portNames.Length];

                    //Loop threw all PortNames From SerialPort
                    for (int _count = 0; _count < _portNames.Length; _count++)
                    {
                        //Query WMI for all PNP (Plug and Play) Devices on the computer that contain (COMxx) in the caption.
                        //http://msdn.microsoft.com/en-us/library/aa394353(VS.85).aspx Win32_PnpEnity Object
                        ManagementObjectSearcher deviceList = new ManagementObjectSearcher("Select Caption from Win32_PnPEntity WHERE Caption LIKE '%(" + _portNames[_count] + ")%'");

                        //Make sure we found the device
                        if (deviceList != null)
                        {
                            //Sould only be one but you never know right?
                            //Could have thrown error on more the one but I will just allow
                            foreach (ManagementObject _comDevice in deviceList.Get())
                            {
                                //Set the ComDevices Name and PortName
                                _return[_count].PortName = _portNames[_count];
                                _return[_count].Name = _comDevice.GetPropertyValue("Caption").ToString();
                                break;
                            }//End foreach device in list

                        }//End deviceList Null Check

                    }//End foreach loop

                    return _return;
                }
                else
                {
                    //First we want to get a list of port names
                    //I would have went full WMI but I had problems finding Virtual Com Ports
                    string[] _portNames = getPortNames();
                    showPorts(_portNames);
                    //Create an bindable array of ComDevice(s) to return
                    ComDevice[] _return = new ComDevice[_portNames.Length];

                    //Loop threw all PortNames From SerialPort
                    if (_portNames != null)
                    {
                        for (int _count = 0; _count < _portNames.Length; _count++)
                        {

                            foreach (string _portName in _portNames)
                            {
                                //Set the ComDevices Name and PortName
                                _return[_count].PortName = _portName;
                                _return[_count].Name = _portName;
                            }
                        }
                        return _return;
                    }
                    return null;
                }
            }//end get

        }//end prop

        private static void showPorts(string[] _portNames)
        {
            string ports = "";
            foreach (string port in _portNames)
            {
                ports += port + ", ";
            }

            File.WriteAllText("debug.txt", ports.TrimEnd(new char[] { ',', ' '}));
        }

        public static string[] getPortNames()
        {
            int p = (int)Environment.OSVersion.Platform;
            List<string> serial_ports = new List<string>();

            string[] ttys = Directory.GetFiles("/dev/", "tty*");
            foreach (string dev in ttys)
            {
                if (dev.StartsWith("/dev/ttyS") || dev.StartsWith("/dev/ttyUSB"))
                    serial_ports.Add(dev);
            }

            return serial_ports.ToArray();
        }

    }//end class
}