using System;

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
    public class LCDProgressBar
    {
        private CFA63XDisplay _display;

        //Helpful info:
        // Full:    208
        //          209
        //          210
        //          211
        // One Bar: 212
        // No Bar:  032

        private int _value = 0;

        public LCDProgressBar(CFA63XDisplay Display)
        {
            this._display = Display;
            if(Display.GetType().Name == "CFA633")
            {
                throw new NotSupportedException("CFA633 is not a supported device for the Progress Bar");
            }
        }

        /// <summary>
        /// Please note: BarSize = How many charaters max 20
        /// </summary>
        public int BarSize { get; set; }

        /// <summary>
        /// This is the line to draw the bar to.
        /// </summary>
        public int DisplayLine { get; set; }

        /// <summary>
        /// This will add [ before and ] after the progress bar. Please account for this!!!! it will incress barsize by two charaters
        /// </summary>
        public bool AddOutline { get; set; }

        /// <summary>
        /// This offsets the x of the bar!
        /// </summary>
        public int BarOffset { get; set; }

        /// <summary>
        /// This is the max value of the bar
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// This is the Percentage of the bar that will be filled
        /// </summary>
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                this.updateProgress();
            }
        }


        /// <summary>
        /// This code is based off code from Programming 32-bit Microcontrollers in C By: Lucio Di Jasio
        /// </summary>
        private void updateProgress()
        {
            //Scale the bar base on size.
            int _width = 0;

            try
            {
                _width = _value * (BarSize * 5) / MaxValue;
            }
            //We really don't care if we divide by zero
            catch { }

            int _remainder = _width % 5;
            
            string _strToWrite = "";

            if(AddOutline){_strToWrite += @"\250";}

            //Here we loop for each 5 in the value we add a full block
            try
            {
                for (int _count = _width / 5; _count > 0; _count--)
                {
                    _strToWrite += @"\208";
                }
            }
            //We really don't care if we divide by zero
            catch  { }

            //If we have a remainder we add the right bar
            switch (_remainder)
            {
                case 1:
                    _strToWrite += @"\212";
                    break;
                case 2:
                    _strToWrite += @"\211";
                    break;
                case 3:
                    _strToWrite += @"\210";
                    break;
                case 4:
                    _strToWrite += @"\209";
                    break;
            }

            //Calc if we need to pad (This is to clear any other chars that might be in our space
            int _pad = BarSize - (_strToWrite.Length / 4);

            //the padd  function
            for (int _count = 0; _count < _pad; _count++)
            {
                _strToWrite += @"\032";
            }

            if(AddOutline){_strToWrite += @"\252";}

            //Lastly we write the string out
            _display.WriteLine(DisplayLine, _strToWrite, BarOffset, false);

        }

        /// <summary>
        /// This will run a demo of a full line progress bar
        /// </summary>
        public void Demo1()
        {
            Thread _demoThread = new Thread(runDemo1);
            _demoThread.Start();
        }

        public void Demo2()
        {
            Thread _demoThread = new Thread(runDemo2);
            _demoThread.Start();
        }
        
        private void runDemo1()
        {
            this.BarOffset = 0;
            this.BarSize = 20;
            this.DisplayLine = 0;
            this.Value = 0;
            this.MaxValue = 100;
            for (int _count = 0; _count < 100; _count++)
            {
                this.Value += 1;
                Thread.Sleep(250);
            }
        }

        private void runDemo2()
        {
            //Basicaly we are clearing the line by writting spaces to it.
            _display.WriteLine(0, "", 0, true);
            this.BarOffset = 10;
            this.BarSize = 5;
            this.DisplayLine = 0;
            this.Value = 0;
            this.MaxValue = 100;
            this.AddOutline = true;
            for (int _count = 0; _count <= 100; _count++)
            {
                _display.WriteLine(0, _count.ToString() + "%", 16, false);
                Thread.Sleep(50);
                this.Value += 1;
                Thread.Sleep(250);
            }
        }

    }
}
