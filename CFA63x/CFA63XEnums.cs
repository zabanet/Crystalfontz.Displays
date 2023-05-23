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
    public enum Commands
    {
        PING_COMMAND = 0x00,
        GET_HARDWARE_FIRMWARE_VERSION = 0x01,
        WRITE_USER_FLASH_AREA = 0x02,
        READ_USER_FLASH_AREA = 0x03,
        STORE_CURRENT_STATE_AS_BOOT_STATE = 0x04,
        REBOOT_CFA_RESET_HOST_POWER_OFF = 0x05,
        CLEAR_LCD_SCREEN = 0x06,
        SET_LCD_SPECIAL_CHARACTER_DATA = 0x09,
        READ_8_BYTES_LCD_MEMORY = 0x0A,
        SET_LCD_CURSOR_POSITION = 0x0B,
        SET_LCD_CURSOR_STYLE = 0x0C,
        SET_LCD_CONTRAST = 0x0D,
        SET_LCD_AND_KEYPAD_BACKLIGHT = 0x0E,
        SET_UP_FAN_REPORTING = 0x10,
        SET_FAN_POWER = 0x11,
        READ_DOW_DEVICE_INFORMATION = 0x12,
        SET_UP_TEMPERATURE_REPORTING = 0x13,
        ARBITRARY_DOW_TRANSACTION = 0x14,
        SEND_COMMAND_DIRECTLY_TO_LCD_CONTROLLER = 0x16,
        CONFIGURE_KEY_REPORTING = 0x17,
        READ_KEYPAD_POLLED_MODE = 0x18,
        SET_FAN_POWER_FAILSAFE = 0x19,
        SET_FAN_TACHOMETER_GLITCH_FILTER = 0x1A,
        QUERY_FAN_POWER_FAILSAFE_MASK = 0x1B,
        SET_ATX_POWER_SWITCH_FUNCTIONALITY = 0x1C,
        ENABLE_DISABLE_AND_RESET_WATCHDOG = 0x1D,
        READ_REPORTING_AND_STATUS = 0x1E,
        SEND_DATA_TO_LCD = 0x1F,
        KEY_LEGENDS = 0x20,
        SET_BAUD_RATE = 0x21,
        SET_CONFIGURE_GPIO_PIN = 0x22,
        READ_GPIO_PIN_LEVELS_AND_CONFIGURATION_STATE = 0x23,
        SEND_KEY = 0x80

    }

    public enum Keys
    {
        UP = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        Enter = 5,
        Exit = 6,
        UPLeft = 13,
        UPRight = 14,
        LowerLeft = 15,
        LowerRight = 16
    }

    [Flags]
    public enum DeviceID
    {
        CFA631 = 1,
        CFA633 = 2,
        CFA635 = 4
    }

    public enum CursorStyle
    {
        None = 0,
        BlinkingBlock = 1,
        Underscore = 2,
        BlinkingBlockWithUnderscore = 3,
        InvertingBlinkingBlock = 4
    }

    public enum ControllerRegister
    {
        Data = 0,
        ControlRegisterRE0 = 1,
        ControlRegisterRE1 = 2
    }

    /// <summary>
    /// Setup reporting on all devices 0-31
    /// </summary>
    public class TempatureReportingSetup
    {
        private Binary _device_0_7 = new Binary();
        private Binary _device_8_15 = new Binary();
        private Binary _device_16_24 = new Binary();
        private Binary _device_25_31 = new Binary();

        public void SetDevice(byte DeviceIndex, bool Enabled)
        {
            //Is the Device 0-7
            if (DeviceIndex >= 0 && DeviceIndex <= 7)
            {
                _device_0_7 = BinaryHelper.ToggleBit(_device_0_7, DeviceIndex, Enabled, true);
            }
            //Is the Device 8 - 15
            if (DeviceIndex >= 8 && DeviceIndex <= 15)
            {
                _device_8_15 = BinaryHelper.ToggleBit(_device_8_15, DeviceIndex, Enabled, true);
            }
            //Is the Device 16 - 24
            if (DeviceIndex >= 16 && DeviceIndex <= 24)
            {
                _device_16_24 = BinaryHelper.ToggleBit(_device_16_24, DeviceIndex, Enabled, true);
            }
            //Is the Device 25 - 31
            if (DeviceIndex >= 25 && DeviceIndex <= 31)
            {
                _device_25_31 = BinaryHelper.ToggleBit(_device_25_31, DeviceIndex, Enabled, true);
            }
        }

        public byte[] Value
        {
            get
            {
                return new byte[4] { Convert.ToByte(_device_0_7), Convert.ToByte(_device_8_15), Convert.ToByte(_device_16_24), Convert.ToByte(_device_25_31) };
            }
        }

    }

}
