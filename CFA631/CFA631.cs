using System.Linq;

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
    public sealed partial class CFA631: CFA63XDisplay
    {
        /// <summary>
        /// Creates class and auto connects if possiable
        /// </summary>
        public CFA631() : base()
        {
            this.initialize();

            ComDevice[] _devices = SerialPorts.ComPorts;
            ComDevice _myDevice = (from cd in _devices where cd.Name.Contains(this.DeviceModel.ToString()) select cd).First();

            base._portName = _myDevice.PortName;
            base.Initialize();
        }

        /// <summary>
        /// Creates the class and opens the port that was passed in.
        /// </summary>
        /// <param name="PortName">Name of the port to open/use.<example>COM1, COM2</example> For list of ports use: <seealso cref="Prepatch.LCD.SerialPorts"/></param>
        public CFA631(string PortName):base(PortName)
        {
            this.initialize();
        }

        /// <summary>
        /// Creates the class and opens the port that was passed in using the baud rate.
        /// </summary>
        /// <param name="PortName">Name of the port to open/use.<example>COM1, COM2</example> For list of ports use: <seealso cref="Prepatch.LCD.SerialPorts"/></param>
        /// <param name="BaudRate">The BaudRate to use with the port.</param>
        public CFA631(string PortName, int BaudRate):base(PortName,BaudRate)
        {
            this.initialize();
        }

        /// <summary>
        /// We just need to init the defaults for the following vars.
        /// </summary>
        private void initialize()
        {
            this.DeviceModel = DeviceID.CFA631;
        }

        protected override void parseData(CFAPacket Packet)
        {
            base.parseData(Packet);
        }
    }
}
