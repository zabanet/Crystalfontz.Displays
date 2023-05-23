using System;

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


    public class CFAReadDataEventArgs : EventArgs
    {
        public CFAReadDataEventArgs()
        {
        }

        public CFAReadDataEventArgs(byte[] Data, int AddressCode)
        {
            this.Data = Data;
            this.AddressCode = AddressCode;
        }
        public byte[] Data { get; set; }
        public int AddressCode { get; set; }
    }

    public class CFAPacketEventArgs : EventArgs
    {
        public CFAPacketEventArgs()
        {
        }

        public CFAPacketEventArgs(CFAPacket Packet)
        {
            this.Packet = Packet;
        }
        public CFAPacket Packet { get; set; }
    }

    public class CFAHardwareFirmwareEventArgs : EventArgs
    {
        public CFAHardwareFirmwareEventArgs()
        {
        }

        public CFAHardwareFirmwareEventArgs(string Information)
        {
            this.Information = Information;
        }
        public string Information { get; set; }
    }


    public class CFAKeyEventArgs : EventArgs
    {
        public CFAKeyEventArgs()
        {
        }

        public CFAKeyEventArgs(Keys Key)
        {
            this.Key = Key;
        }
        public Keys Key { get; set; }
    }

    public class CFAReadUserFlashEventArgs : EventArgs
    {
        public CFAReadUserFlashEventArgs()
        {
        }

        public CFAReadUserFlashEventArgs(byte[] Data)
        {
            this.Data = Data;
        }
        public byte[] Data { get; set; }
    }

    public class CFAReportingAndStatusEventArgs : EventArgs
    {
        public CFAReportingAndStatusEventArgs()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Binary Fans { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Binary Sensors1_8 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Binary Sensors9_15 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Binary Sensors16_23 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Binary Sensors24_32 { get; set; }

        public byte KeyReleases { get; set; }

        public byte KeyPresses { get; set; }

        public Binary ATXPowerSwitch { get; set; }

        public byte WatchdogCounter { get; set; }

        public byte Fan1Glitch { get; set; }

        public byte Fan2Glitch { get; set; }

        public byte Fan3Glitch { get; set; }

        public byte Fan4Glitch { get; set; }

        public int Backlight { get; set; }
        
        public int Contrast { get; set; }
    }
}
