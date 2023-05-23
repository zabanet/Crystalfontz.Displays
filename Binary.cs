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
    [Flags]
    public enum Binary : byte
    {
        Bit1 = 1,
        Bit2 = 2,
        Bit3 = 4,
        Bit4 = 8,
        Bit5 = 16,
        Bit6 = 32,
        Bit7 = 64,
        Bit8 = 128
    }

    public static class BinaryHelper
    {
        /// <summary>
        /// Toggle a bit on/off in a bitmask
        /// </summary>
        /// <param name="BitMask">Copy of the Binary bitmask</param>
        /// <param name="BitIndex">The Index of the bit 0-7</param>
        /// <param name="Enabled">Is the bit on/off? False = Off, True = On</param>
        /// <param name="StartAtZero">Should the First Index be 0 or 1? True = 0, False = 1</param>
        /// <returns></returns>
        public static Binary ToggleBit(Binary BitMask, byte BitIndex, bool Enabled, bool StartAtZero)
        {
            byte _count = 0;
            if (StartAtZero == false)
            {
                _count = 1;
            }
            foreach (byte _byteValue in Enum.GetValues(typeof(Binary)))
            {
                if (_count == BitIndex)
                {
                    if (Enabled)
                    {
                        BitMask = BitMask | (Binary)Enum.ToObject(typeof(Binary), _byteValue);
                    }
                    else
                    {
                        BitMask = BitMask &~ (Binary)Enum.ToObject(typeof(Binary), _byteValue);
                    }
                }
                    _count++;
            }
            return BitMask;
        }
    }




}
