using System.Collections.Generic;

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
    public struct RamLocation
    {
        public string Name;
        public byte StartAddress;
        public byte EndAddress;
        public DeviceID DeviceModel;
        public RamLocation(string Name, byte StartAddress, byte EndAddress, DeviceID DeviceModel)
        {
            this.Name = Name;
            this.StartAddress = StartAddress;
            this.EndAddress = EndAddress;
            this.DeviceModel = DeviceModel;
        }
    }
    public abstract partial class CFA63XDisplay
    {
        protected List<RamLocation> _ramLocations;

        public List<RamLocation> RamLocations
        {
            get
            {
                return this._ramLocations;
            }
        }

        internal void BuildRamLocationList()
        {
            _ramLocations = new List<RamLocation>();
            //Setup ram locations for CFA631 & CFA633
            _ramLocations.Add(new RamLocation("CGRAM CFA631 CFA633", 0x40, 0x7F, DeviceID.CFA631 | DeviceID.CFA633));
            _ramLocations.Add(new RamLocation("Line 1 CFA631 CFA633", 0x80, 0x93, DeviceID.CFA631 | DeviceID.CFA633));
            _ramLocations.Add(new RamLocation("Line 2 CFA631 CFA633", 0xC0, 0xD3, DeviceID.CFA631 | DeviceID.CFA633));

            //Setup ram locations for CFA635
            _ramLocations.Add(new RamLocation("CGRAM CFA635", 0x40, 0x7F, DeviceID.CFA635));
            _ramLocations.Add(new RamLocation("Line 0 CFA635", 0x80, 0x93, DeviceID.CFA635));
            _ramLocations.Add(new RamLocation("Line 1 CFA635", 0xA0, 0xB3, DeviceID.CFA635));
            _ramLocations.Add(new RamLocation("Line 2 CFA635", 0xC0, 0xD3, DeviceID.CFA635));
            _ramLocations.Add(new RamLocation("Line 3 CFA635", 0xE0, 0xF3, DeviceID.CFA635));
        }
    }
}
