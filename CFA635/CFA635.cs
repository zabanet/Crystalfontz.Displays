using System.Collections.Generic;
using System.Threading;

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
    public sealed partial class CFA635: CFA63XDisplay
    {

        /// <summary>
        /// Creates class and auto connects if possiable
        /// </summary>
        public CFA635() : base()
        {
            this.initialize();

            ComDevice[] _devices = SerialPorts.ComPorts;
            ComDevice _myDevice = _devices[0];// (from cd in _devices where cd.Name.Contains(this.DeviceModel.ToString()) select cd).First();

            base._portName = _myDevice.PortName;
            base.Initialize();
        }

        /// <summary>
        /// Creates the class and opens the port that was passed in.
        /// </summary>
        /// <param name="PortName">Name of the port to open/use.<example>COM1, COM2</example> For list of ports use: <seealso cref="Prepatch.LCD.SerialPorts"/></param>
        public CFA635(string PortName):base(PortName)
        {
            this.initialize();
        }

        /// <summary>
        /// Creates the class and opens the port that was passed in using the baud rate.
        /// </summary>
        /// <param name="PortName">Name of the port to open/use.<example>COM1, COM2</example> For list of ports use: <seealso cref="Prepatch.LCD.SerialPorts"/></param>
        /// <param name="BaudRate">The BaudRate to use with the port.</param>
        public CFA635(string PortName, int BaudRate):base(PortName,BaudRate)
        {
            this.initialize();
        }

        /// <summary>
        /// We just need to init the defaults for the following vars.
        /// </summary>
        private void initialize()
        {
            this.DeviceModel = DeviceID.CFA635;
        }

        /// <summary>
        /// Sets the color and brightness of the led.
        /// </summary>
        /// <param name="LEDIndex">The index of the LED. 0 = Bottom, 3 = Top.</param>
        /// <param name="Color">The Color to use. Red or Green</param>
        /// <param name="Brightness">0 = off, 100 = on, 1-99 Brightness</param>
        public void SetLED(int LEDIndex, LEDColor Color, int Brightness)
        {
            List<byte> _sendList = new List<byte>();
            //LEDs start at 5
            //Green = +0
            //Red = +1
            //Example:
            //  Bottom LED Green = GPIO 5
            //  Bottom LED Red = GPIO 6
            //Thanks to cool stuff like that we get slick code!
            //Once Again Bottom LED is GPIO 5 our Index is 0, see the cool factor?

            //Okay, this is a long command but really simple
            //Bassicaly we are sending the command
            //Next: New Byte Array Length 2
            //First Byte LED
            //Second Brightness
            //this.SendCommand(Commands.SET_CONFIGURE_GPIO_PIN, new byte[2] { Convert.ToByte(5 + LEDIndex + (int)Color), Convert.ToByte(Brightness) });
            int gpio = 5 + (LEDIndex * 2) + (int)Color;
            this.SendCommand(Commands.SET_CONFIGURE_GPIO_PIN, new[] { (byte)gpio, (byte)Brightness });
        }

        /// <summary>
        /// Reads a line of text from the display.
        /// </summary>
        /// <param name="LineNumber">The line number. Vaild 0-3.</param>
        public void ReadLine(int LineNumber)
        {
            switch (LineNumber)
            {
                case 0:
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0x80 });
                    Thread.Sleep(100);
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0x88 });
                    Thread.Sleep(100);
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0x90 });
                    break;
                case 1:
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0xA0 });
                    Thread.Sleep(100);
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0xA8 });
                    Thread.Sleep(100);
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0xB0 });
                    break;
                case 2:
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0xC0 });
                    Thread.Sleep(100);
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0xC8 });
                    Thread.Sleep(100);
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0xD0 });
                    break;
                case 3:
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0xE0 });
                    Thread.Sleep(100);
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0xE8 });
                    Thread.Sleep(100);
                    this.SendCommand(Commands.READ_8_BYTES_LCD_MEMORY, new byte[1] { 0xF0 });
                    break;
            }
        }
        
    }
}
