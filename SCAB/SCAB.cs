using System;

using Crystalfontz.Displays;

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

namespace Crystalfontz.Modules
{
    public partial class SCAB
    {
        CFA63XDisplay _display;

        public int Fan1Pulse { get; set; }
        public int Fan2Pulse { get; set; }
        public int Fan3Pulse { get; set; }
        public int Fan4Pulse { get; set; }

        public SCAB(CFA63XDisplay Display)
        {
            _display = Display;
            _display.PacketRecived += new CFA63XDisplay.PacketRecivedHandler(_display_PacketRecived);
            this.initialize();
        }



        /// <summary>
        /// Setsup fan reporting for fans. False = Off, True = On
        /// </summary>
        /// <param name="Fan1"></param>
        /// <param name="Fan2"></param>
        /// <param name="Fan3"></param>
        /// <param name="Fan4"></param>
        public void SetUpFanReporting(bool Fan1, bool Fan2, bool Fan3, bool Fan4)
        {
            Binary _bitMap = new Binary();
            if (Fan1)
            {
                _bitMap = _bitMap | Binary.Bit1;
            }
            if (Fan2)
            {
                _bitMap = _bitMap | Binary.Bit2;
            }
            if (Fan3)
            {
                _bitMap = _bitMap | Binary.Bit3;
            }
            if (Fan4)
            {
                _bitMap = _bitMap | Binary.Bit4;
            }
            _display.SendCommand(Commands.SET_UP_FAN_REPORTING, new byte[1] { Convert.ToByte(_bitMap) });
        }

        public void SetUpFanPower(byte Fan1, byte Fan2, byte Fan3, byte Fan4)
        {
            if (Fan1 > 100 || Fan1 < 0)
            {
                throw new OverflowException("Fan1 Power set greater or less then fan can handle");
            }
            if (Fan2 > 100 || Fan2 < 0)
            {
                throw new OverflowException("Fan1 Power set greater or less then fan can handle");
            }
            if (Fan3 > 100 || Fan3 < 0)
            {
                throw new OverflowException("Fan1 Power set greater or less then fan can handle");
            }
            if (Fan3 > 100 || Fan3 < 0)
            {
                throw new OverflowException("Fan1 Power set greater or less then fan can handle");
            }
            _display.SendCommand(Commands.SET_FAN_POWER, new byte[4] { Fan1, Fan2, Fan3, Fan4 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Fan1"></param>
        /// <param name="Fan2"></param>
        /// <param name="Fan3"></param>
        /// <param name="Fan4"></param>
        /// <param name="TimeoutValue">Timeout is in 1/8 ticks, 1 = 1/8, 2 = 1/4, 255 = 31 7/8</param>
        public void SetFanPowerFailSafe(bool Fan1, bool Fan2, bool Fan3, bool Fan4, byte TimeoutValue)
        {
            Binary _bitMask = new Binary();
            _bitMask = BinaryHelper.ToggleBit(_bitMask, 0, Fan1, true);
            _bitMask = BinaryHelper.ToggleBit(_bitMask, 1, Fan2, true);
            _bitMask = BinaryHelper.ToggleBit(_bitMask, 2, Fan3, true);
            _bitMask = BinaryHelper.ToggleBit(_bitMask, 3, Fan4, true);

            _display.SendCommand(Commands.SET_FAN_POWER_FAILSAFE, new byte[2] { Convert.ToByte(_bitMask), TimeoutValue });
        }

        public void SetATXPowerSwitchFunctionality(ATXPowerMask PowerMask)
        {
            _display.SendCommand(Commands.SET_ATX_POWER_SWITCH_FUNCTIONALITY, new byte[1] { Convert.ToByte(PowerMask) });
        }

        public void SetATXPowerSwitchFunctionality(ATXPowerMask PowerMask, byte PowerPulseLength)
        {
            _display.SendCommand(Commands.SET_ATX_POWER_SWITCH_FUNCTIONALITY, new byte[2] { Convert.ToByte(PowerMask), PowerPulseLength });
        }

        /// <summary>
        /// Enables/Disables or Reset the watchdog. Once enable you must resend this command before the timeout.
        /// </summary>
        /// <param name="Timeout">0 = Disabled, 1-255 = Enabled</param>
        public void EnableDisableResetWatchdog(byte Timeout)
        {
            _display.SendCommand(Commands.ENABLE_DISABLE_AND_RESET_WATCHDOG, new byte[1] { Timeout });
        }


        public void QueryFanPowerAndFailSafe()
        {
            _display.SendCommand(Commands.QUERY_FAN_POWER_FAILSAFE_MASK, new byte[0]);
        }

        public void SetFanTachometerGlitchFilter(byte Fan1, byte Fan2, byte Fan3, byte Fan4)
        {
            _display.SendCommand(Commands.SET_FAN_TACHOMETER_GLITCH_FILTER,new byte[4] { Fan1, Fan2, Fan3, Fan4 });

        }

        public void SetupTemperatureReporting(TempatureReportingSetup TempatureBitmask)
        {
            if (TempatureBitmask == null)
            {
                throw new NullReferenceException("TempatureBitmask can not be null");
            }
            else
            {
                _display.SendCommand(Commands.SET_UP_TEMPERATURE_REPORTING, TempatureBitmask.Value);
            }
        }

        /// <summary>
        /// We just need to init the defaults for the following vars.
        /// </summary>
        private void initialize()
        {
            this.Fan1Pulse = 2;
            this.Fan2Pulse = 2;
            this.Fan3Pulse = 2;
            this.Fan4Pulse = 2;
        }

        void _display_PacketRecived(object sender, CFAPacket e)
        {
            //So we can get out own copy of the packet
            switch (e.Command)
            {
                //Response from setup fan Reporting
                case 0x50:
                    break;

                //Response from Set Fan Power Level
                case 0x51:
                    break;

                //Response from Read DOW device Information
                case 0x52:
                    break;

                //Response from setup temperature Reporting
                case 0x53:
                    break;

                //Arbitray dow transaction
                case 0x54:
                    break;

                //Response from set fan fail safe power
                case 0x59:
                    break;

                //Response From Setup live fan or temperature display
                case 0x55:
                    break;

                //Response from set fan tachometer glitch filter
                case 0x5A:
                    break;

                //Response from Query fan power and fail
                case 0x5B:
                    if (this.QueryFanPower != null)
                    {
                        this.QueryFanPower(this, new CFAQueryFanEventArgs(e.Data[0], e.Data[1], e.Data[2], e.Data[3], _display.IntToEnum<Binary>(e.Data[4])));
                    }
                    break;

                //Set ATX Power Switch Functionality
                case 0x5C:
                    break;

                //Enable/Disable and Reset the Watchdog
                case 0x5D:
                    break;

                //Fan Speed Report
                case 0x81:
                    if (e.DataLength != 4)
                    {
                        //we got an error!
                    }

                    ushort _fanTimerTicks = BitConverter.ToUInt16(e.Data, 2);
                    int _FanTach = e.Data[1];
                    int _RPMS = 0;
                    switch (e.Data[0])
                    {
                        case 0:
                            try
                            {
                                _RPMS = (((27692308 / this.Fan1Pulse) * (Math.Abs(_FanTach - 3))) / (_fanTimerTicks));
                            }
                            catch { }
                            break;
                        case 1:
                            try
                            {
                                _RPMS = (((27692308 / this.Fan2Pulse) * (Math.Abs(_FanTach - 3))) / (_fanTimerTicks));
                            }
                            catch { }
                            break;
                        case 2:
                            try
                            {
                                _RPMS = (((27692308 / this.Fan3Pulse) * (Math.Abs(_FanTach - 3))) / (_fanTimerTicks));
                            }
                            catch { }
                            break;
                        case 3:
                            try
                            {
                                _RPMS = (((27692308 / this.Fan4Pulse) * (Math.Abs(_FanTach - 3))) / (_fanTimerTicks));
                            }
                            catch { }
                            break;
                    }
                    if (FanReport != null)
                    {
                        FanReport(this, new CFAFanReportEventArgs(e.Data[0], _FanTach, _fanTimerTicks, _RPMS));
                    }
                    break;

                //Temperature Sensor Report
                case 0x82:
                    double _tempC = BitConverter.ToUInt16(e.Data, 1) / 16.0;
                    double _tempF = ((_tempC * 9.0) / 5.0) + 32.0;
                    if (TemperatureReport != null)
                    {
                        TemperatureReport(this, new CFATemperatureReportEventArgs(e.Data[0], _tempC, _tempF));
                    }
                    break;


            }

        }
    }
}
