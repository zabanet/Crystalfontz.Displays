using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

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
    /// <summary>
    /// This is the base class for all CFA63X Displays
    /// </summary>
    public abstract partial class CFA63XDisplay : IDisposable
    {
        //public bool DeviceConnected { get; set; }//zabanet
        public event EventHandler<EventArgs> DisplayConnected;
        private bool isLinux = true; //zabanet

        //Store the name of the port we are using
        protected string _portName = "";

        //The thread we are reading on
        private Thread _spReader;

        //Default baud rate to 115,200
        protected int _baudRate = 115200;

        //use to track when BaudRate Change command is issued
        private int _baudRateChange = 0;

        //Defualt Max Charaters
        protected int _maxCharaters = 20;

        //the serialPort we will be using
        protected SerialPort _sp;

        public DeviceID DeviceModel;

        /// <summary>
        /// Returns the baudrate
        /// </summary>
        public int BaudRate
        {
            get { return _baudRate; }
        }

        /// <summary>
        /// Returns the name of the port we are connected to.
        /// </summary>
        public string PortName
        {
            get { return _portName; }
        }

        /// <summary>
        /// We will automaticaly try to connect to a port.
        /// </summary>
        public CFA63XDisplay()
        {

        }

        /// <summary>
        /// Creates the class and opens the port that was passed in.
        /// </summary>
        /// <param name="PortName">Name of the port to open/use.<example>COM1, COM2</example> For list of ports use: <seealso cref="SerialPorts"/></param>
        public CFA63XDisplay(string PortName)
        {
            _portName = PortName;
            this.Initialize();
        }

        /// <summary>
        /// Creates the class and opens the port that was passed in using the baud rate.
        /// </summary>
        /// <param name="PortName">Name of the port to open/use.<example>COM1, COM2</example> For list of ports use: <seealso cref="SerialPorts"/></param>
        /// <param name="BaudRate">The BaudRate to use with the port.</param>
        public CFA63XDisplay(string PortName, int BaudRate)
        {
            _portName = PortName;
            _baudRate = BaudRate;
            this.Initialize();
        }

        private bool checkIfLinux() //zabanet
        {
            int p = (int) Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
        }

        /// <summary>
        /// Initialize the serial port.
        /// </summary>
        protected void Initialize()
        {
            isLinux = checkIfLinux(); 
            //build a list of ram locations
            BuildRamLocationList();

            _sp = new SerialPort(_portName)
            {
                BaudRate = _baudRate, DtrEnable = true, RtsEnable = true, Handshake = Handshake.None
            };

            _spReader = new Thread(searchForData) {Name = "Serial Reader Thread"};
            
            Task.Run(() => Connect());
        }

        private bool _spReaderStarted = false;
        private void Connect()
        {
            while (!IsOpen)
            {
                try
                {
                    _sp.Open();
                    Console.WriteLine("LCD panel connected");
                    if(!_spReaderStarted) _spReader.Start();

                    _spReaderStarted = true;
                    DisplayConnected?.Invoke(_sp, EventArgs.Empty);
                }
                catch (Exception e)
                {
                    Thread.Sleep(3000);
                    Debug.WriteLine(e.Message);
                }
            }
        }
        
        /// <summary>
        /// Returns true if SerialPort is connected.
        /// </summary>
        public bool IsOpen => _sp != null && _sp.IsOpen;

        /// <summary>
        /// These will trigger event that are common in all 3 CFA usb lcds
        /// </summary>
        /// <param name="Packet">That Packet</param>
        protected virtual void parseData(CFAPacket Packet)
        {
            if (PacketRecived != null)
            {
                PacketRecived(this, Packet);
            }

            switch (Packet.Command)
            {
                //Ping Command Return
                case 0x40:
                    break;

                //Hardware Firmware Version return
                case 0x41:
                    if (HardwareFirmwareVersion != null)
                    {
                        this.HardwareFirmwareVersion(this,
                            new CFAHardwareFirmwareEventArgs(ASCIIEncoding.ASCII.GetString(Packet.Data)));
                    }

                    break;

                //Write User Flash Area
                case 0x42:

                    break;

                //Read user Flash Area
                case 0x43:
                    if (ReadUserFlash != null)
                    {
                        this.ReadUserFlash(this, new CFAReadUserFlashEventArgs(Packet.Data));
                    }

                    break;

                //Response from Store Current State
                case 0x44:
                    break;

                //Response from Reboot CFA,Reset host, Power Off host
                case 0x45:
                    break;

                //Response from Clear LCDd
                case 0x46:
                    break;

                //Return from set special charater data
                case 0x49:
                    break;

                //Response from Read 8 bytes of lcd memeroy data
                case 0x4A:
                    if (ReadData != null)
                    {
                        int _address = Packet.Data[0];
                        byte[] _data = new byte[Packet.DataLength - 1];
                        Array.Copy(Packet.Data, 1, _data, 0, Packet.DataLength - 1);
                        this.ReadData(this, new CFAReadDataEventArgs(_data, _address));
                    }

                    break;

                //Response from Set Cursor Position
                case 0x4B:
                    break;

                //Response from Set Cursor Style
                case 0x4C:
                    break;

                //Response from Set Cursor Contrast
                case 0x4D:
                    break;

                //Response from Set LCD / Keypad Backlight
                case 0x4E:
                    break;

                //Response from Send Command directly to lcd controller
                case 0x56:

                    break;

                //Response from read reporting and status
                case 0x5E:
                    if (ReportingAndStatus != null)
                    {
                        CFAReportingAndStatusEventArgs _eventArgs = new CFAReportingAndStatusEventArgs();

                        _eventArgs.Fans = IntToEnum<Binary>(Packet.Data[0]);
                        _eventArgs.Sensors1_8 = IntToEnum<Binary>(Packet.Data[1]);
                        _eventArgs.Sensors9_15 = IntToEnum<Binary>(Packet.Data[2]);
                        _eventArgs.Sensors16_23 = IntToEnum<Binary>(Packet.Data[3]);
                        _eventArgs.Sensors24_32 = IntToEnum<Binary>(Packet.Data[4]);
                        _eventArgs.KeyPresses = Packet.Data[5];
                        _eventArgs.KeyReleases = Packet.Data[6];
                        _eventArgs.ATXPowerSwitch = IntToEnum<Binary>(Packet.Data[7]);
                        _eventArgs.WatchdogCounter = Packet.Data[8];
                        _eventArgs.Fan1Glitch = Packet.Data[9];
                        _eventArgs.Fan2Glitch = Packet.Data[10];
                        _eventArgs.Fan3Glitch = Packet.Data[11];
                        _eventArgs.Fan4Glitch = Packet.Data[12];
                        _eventArgs.Contrast = Packet.Data[13];
                        _eventArgs.Backlight = Packet.Data[14];

                        this.ReportingAndStatus(this, _eventArgs);
                    }

                    break;

                //Response from Send Data to LCD
                case 0x5F:
                    break;

                //Response from set baud rate
                case 0x61:
                    //We switch after we get the packet that the device has switched
                    _sp.BaudRate = _baudRateChange;
                    this._baudRate = _baudRateChange;
                    _baudRateChange = 0;
                    break;

                #region KeyPressed

                //Key Pressed Packet
                case 0x80:
                    if (Packet.DataLength != 1)
                    {
                        //We got an error....
                        return;
                    }

                    int _keyByte = Packet.Data[0];
                    //zabanet: to minimize operation count, react only on key releases
                    //Less then or equall to 6 = KeyPress
                    /*    
                        if (_keyByte <= 6)
                        {
                            if (KeyPressed != null)
                            {
                                KeyPressed(this, new CFAKeyEventArgs(this.IntToEnum<Keys>(_keyByte)));
                            }
                        }
                        */
                    //grater then 6 = KeyRelease, Must be less then or equall to 12
                    if (_keyByte > 6 && _keyByte <= 12)
                    {
                        if (KeyReleased != null)
                        {
                            KeyReleased(this, new CFAKeyEventArgs(this.IntToEnum<Keys>(_keyByte - 6)));
                        }
                    }

                    break;

                #endregion


            }
        }
        
        
        private void searchForData()
        {
            while (_sp.IsOpen)
            {
                var bytesToRead = 0;
                try
                {
                    bytesToRead = _sp.BytesToRead;
                }
                catch (Exception e)
                {
                    //connection with the panel is lost
                    Console.WriteLine("LCD connection lost!");
                    _sp?.Close();
                    Connect();
                }
                //on the off case there is nothing to read we sleep!
                if (bytesToRead == 0)
                {
                    //This will only make OUR thread sleep :)
                    Thread.Sleep(50); //zabanet, taken down from 250
                }
                else
                {
                    Thread.Sleep(10); //wait untile buffer fills
                    byte[] readout = null;
                    try
                    {
                        readout = Encoding.ASCII.GetBytes(_sp.ReadExisting());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("LCD connection lost!");
                        _sp?.Close();
                        _spReader?.Abort();
                        Connect();
                    }

                    if (readout == null || readout.Length == 0)
                        return;


                    if (readout[0] == 63) //don't know why, but that's the keypad command 
                    {
                        var _Packet = new CFAPacket
                        {
                            Command = 128,
                            DataLength = 1,
                            Data = new byte[1]
                        };
                        //this is keypad command as it should be
                        //default for kepad command
                        _Packet.Data[0] = readout.Length > 2 ? readout[2] : (byte) 0;
                        _Packet.CRC = 0; //for simplicity

                        //if (isLinux)
                        {
                            _sp.DiscardInBuffer(); //zabanet - this must be here for real device, but it brakes things on emulator!
                        }

                        parseData(_Packet);
                    }
                    else if (readout[0] == 65)
                    {
                        var _Packet = new CFAPacket
                        {
                            Command = 65,
                            DataLength = 16,
                            Data = new byte[16]
                        };

                        Array.Copy(readout, 2, _Packet.Data, 0, 16);
                        _Packet.CRC = 0; //for simplicity

                        //if (isLinux)
                        {
                            _sp.DiscardInBuffer(); //zabanet - this must be here for real device, but it brakes things on emulator!
                        }

                        parseData(_Packet);
                    }
                }

            } //End While Loop
        }

        

        /// <summary>
        /// Convert an integer to Enum
        /// </summary>
        /// <typeparam name="T">The Enum (The type)</typeparam>
        /// <param name="number">The value to convert</param>
        /// <returns>Returns Enum</returns>
        public T IntToEnum<T>(int number)
        {
            return (T)Enum.ToObject(typeof(T), number);
        }

        /// <summary>
        /// Sends command to the LCD. CRC is automatically appended!
        /// </summary>
        /// <param name="Command">The Command to Send</param>
        /// <param name="Data">Any data that should also be sent.</param>
        public void SendCommand(Commands Command, byte[] Data)
        {
            List<byte> _sendList = new List<byte>();

            //Add the Command Byte
            _sendList.Add(Convert.ToByte(Command));

            //Add the Data Length
            _sendList.Add(Convert.ToByte(Data.Length));

            //Add the byte array
            _sendList.AddRange(Data);

            //Append the CRC
            CRC.CalculateCRC(ref _sendList);

            //Register Packet Event if we sent one!           
            //Write the command out

            //if (Command == Commands.PING_COMMAND || DeviceConnected && _sp.IsOpen) //zabanet
            if(Command == Commands.PING_COMMAND || _sp.IsOpen)
            {
                _sp.Write(_sendList.ToArray(), 0, _sendList.Count);
            }
        }

        /// <summary>
        /// Converts the Command Enum to a byte
        /// </summary>
        /// <param name="Command">The Command we want to convert</param>
        /// <returns>The bye vaule of the enum</returns>
        protected byte CommandByte(Commands Command)
        {
            return Convert.ToByte((int)Command);
        }

        #region Commands

        public void Ping(string Text)
        {
            if (Text != null)
            {
                this.SendCommand(Commands.PING_COMMAND, ASCIIEncoding.ASCII.GetBytes(Text));
            }
            else
            {
                this.SendCommand(Commands.PING_COMMAND, new byte[0]);
            }
        }

        public void GetHardwareFirmwareVersion()
        {
            this.SendCommand(Commands.GET_HARDWARE_FIRMWARE_VERSION, new byte[0]);
        }

        public void StoreCurrentStateAsBootState()
        {
            this.SendCommand(Commands.STORE_CURRENT_STATE_AS_BOOT_STATE, new byte[0]);
        }

        public void ClearLCD()
        {
            this.SendCommand(Commands.CLEAR_LCD_SCREEN, new byte[0]);
        }

        public void SetCustomCharater(byte CharaterIndex, byte[] Data)
        {
            List<byte> _bytes = new List<byte>();
            _bytes.Add(Convert.ToByte(CharaterIndex));
            _bytes.AddRange(Data);
            if (CharaterIndex < 0 || CharaterIndex < 7)
            {
                throw new IndexOutOfRangeException("CharaterIndex must be 0-7");
            }
            if (Data.Length != 8)
            {
                throw new IndexOutOfRangeException("Data must be 8 bytes long.");
            }
            this.SendCommand(Commands.SET_LCD_SPECIAL_CHARACTER_DATA, _bytes.ToArray());
        }

        public void Read8BytesOfLCDMemory(byte Address)
        {
            this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { Convert.ToByte(Address) });
        }


        /// <summary>
        /// Changes cursor style
        /// </summary>
        /// <param name="Style">The Style of the cursor.</param>
        public void SetCursorStyle(CursorStyle Style)
        {
            //Sends the Command to change the Cursor Style
            this.SendCommand(Commands.SET_LCD_CURSOR_STYLE, new byte[1] { Convert.ToByte((int)Style) });
        }

        /// <summary>
        /// Sets where the cursor is on the screen.
        /// </summary>
        /// <param name="Column">0-19</param>
        /// <param name="Row">0-1 CFA631 & CFA633, 0-3 CFA635</param>
        public void SetCursorPosition(byte Column, byte Row)
        {
            if (Row > 19 || Row < 0)
            {
                throw new IndexOutOfRangeException("Column index is greater/less then charaters on lcd.");
            }

            if (DeviceModel == DeviceID.CFA631 || DeviceModel == DeviceID.CFA633)
            {
                if (Row > 1 || Row < 0)
                {
                    throw new IndexOutOfRangeException("Row index is greater/less then rows on lcd.");
                }
            }
            if (DeviceModel == DeviceID.CFA635)
            {
                if (Row > 3 || Row < 0)
                {
                    throw new IndexOutOfRangeException("Row index is greater/less then rows on lcd.");
                }
            }
            this.SendCommand(Commands.SET_LCD_CURSOR_POSITION, new byte[2] { Column, Row });
        }

        /// <summary>
        /// Sets the back light On/Off or Brightness.
        /// </summary>
        /// <param name="Brightness">0 = Off, 100 = On, 1-99 Brightness</param>
        public void SetBacklight(int Brightness)
        {
            this.SendCommand(Commands.SET_LCD_AND_KEYPAD_BACKLIGHT, new byte[1] { Convert.ToByte(Brightness) });
        }

        /// <summary>
        /// Sets the Contrast.
        /// </summary>
        /// <param name="Brightness">0 - 65 Very light, 66 light, 95 about right, 125 dark, 126-255 Very Dark</param>
        public void SetContrast(int Contrast)
        {
            this.SendCommand(Commands.SET_LCD_CONTRAST, new byte[1] { Convert.ToByte(Contrast) });
        }

        public void SendCommandDirectyToLCDController(ControllerRegister Register, byte[] Data)
        {
             List<byte> _bytes = new List<byte>();
            _bytes.Add(Convert.ToByte(Register));
            _bytes.AddRange(Data);
            this.SendCommand(Commands.SEND_COMMAND_DIRECTLY_TO_LCD_CONTROLLER, _bytes.ToArray());
        }

        public void ReadReportingStatus()
        {
            this.SendCommand(Commands.READ_REPORTING_AND_STATUS, new byte[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BaudRate">0 = 12300, 1= 115200</param>
        public void SetBaudRate(int BaudRate)
        {
            _baudRateChange = BaudRate;
            this.SendCommand(Commands.SET_BAUD_RATE, new byte[1] { Convert.ToByte(BaudRate) });
        }

        public void SetUserFlashArea(byte[] Data)
        {
            this.SendCommand(Commands.WRITE_USER_FLASH_AREA, Data);
        }

        public void ReadUserFlashArea()
        {
            this.SendCommand(Commands.READ_USER_FLASH_AREA, new byte[0] { });
        }

        /// <summary>
        /// Writes a full line to the LCD.
        /// </summary>
        /// <param name="LineNumber">The line number to write line to. Vaild lines 0 - 3.</param>
        /// <param name="Text">The text to write to the lcd. Max 20 Chars.</param>
        /// <param name="XPOS">The Charater Postion in line to write to.</param>
        /// <param name="Padded">If the screen should fill the line. If false will not over write text.</param>
        public void WriteLine(int LineNumber, string Text, int XPOS, bool Padded)
        {
            if (!IsOpen || Text == null) return;
            if(Text.Length > 20)
            {
                Text = Text.Substring(0, 20);
            }
            char[] _textToArray = Text.ToCharArray();

            List<byte> _toWrite = new List<byte>();

            List<byte> _sendList = new List<byte>();
            for (int _count = 0; _count < _textToArray.Length; _count++)
            {
                //Check for backslash
                if (_textToArray[_count] != '\\')
                {
                    _toWrite.Add(Convert.ToByte(_textToArray[_count]));
                }
                else
                {
                    //check to see if this is a dec number 3 digits
                    string _strDecValue = new string(_textToArray, _count + 1, 3);
                    int _intDecValue = 0;
                    if (Int32.TryParse(_strDecValue, out _intDecValue))
                    {
                        //This means we got the parse back right!
                        _toWrite.Add(Convert.ToByte(_intDecValue));
                        _count += 3;
                    }
                    else
                    {
                        //Guess we didnt have an integer
                        _toWrite.Add(Convert.ToByte(_textToArray[_count]));
                    }
                }
            }

            if (_toWrite.Count > _maxCharaters)
            {
                throw new IndexOutOfRangeException("To many charaters passed in.");
            }

            if (LineNumber > 3 || LineNumber < 0)
            {
                throw new IndexOutOfRangeException("Line Number out of range. Vaild 0 - 3");
            }
            if (_sp.IsOpen == false)
            {
                //zabanet
                //throw new InvalidOperationException("Serial Port is not open");
            }

            //If Padded is pass in we pad to max LCD Length 22
            if (Padded)
            {
                int padAmount = _maxCharaters - _toWrite.Count;
                for (int _count = 0; _count < padAmount; _count++)
                {
                    //32 is space btw.
                    _toWrite.Add(32);
                }
            }

            //X POS = 0 line start
            _sendList.Add(Convert.ToByte(XPOS));

            //Y POS = Line Number
            _sendList.Add(Convert.ToByte(LineNumber));

            //Add the text back
            _sendList.AddRange(_toWrite);

            //Now that we have a formatted byte list we can write it out!
            this.SendCommand(Commands.SEND_DATA_TO_LCD, _sendList.ToArray());
        }

        #endregion


        public void Read8Bytes(RamLocation RamLoc)
        {

        }

        #region IDisposable Members

        public void Dispose()
        {
            _spReader?.Abort();
            _sp.Close();
        }

        #endregion
    }
}
