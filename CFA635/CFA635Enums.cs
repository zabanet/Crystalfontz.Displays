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
    public sealed partial class CFA635
    {

        public enum KeysMask
        {
            UP = 1,
            Enter = 2,
            Cancel = 4,
            Left = 8,
            Right = 16,
            Down = 32
        }

        public enum LEDColor
        {
            Green = 0,
            Red = 1
        }


    }
}