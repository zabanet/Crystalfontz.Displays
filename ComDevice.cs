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
    /// Structure to contain data related to a ComDevice.
    /// </summary>
    public struct ComDevice
    {
        string _name;
        string _portName;
        /// <summary>
        /// This is the logical name that you would see in device manager
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        /// <summary>
        /// This is the name of the port the SerialPort class needs
        /// </summary>
        public string PortName
        {
            get
            {
                return _portName;
            }
            set
            {
                _portName = value;
            }
        }
    }
}