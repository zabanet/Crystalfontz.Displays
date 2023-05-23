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
    public class ScrollingText: IDisposable
    {
        private CFA63XDisplay _display;
        private Thread _scrollingThread;

        private bool _scrolling = false;

        private int _strIdx = 0;

        /// <summary>
        /// Creates a scrolling text class
        /// </summary>
        /// <param name="Display">The display you are using. CFA631, CFA633, CFA635</param>
        public ScrollingText(CFA63XDisplay Display)
        {
            _display = Display;
            this.ScrollSpeed = 750;
            this.PadString = 5;
            this.Line = 0;
            _scrollingThread = new Thread(this.scrollingText);
        }

        /// <summary>
        /// This is the text that will scroll.
        /// </summary>
        public string  Text { get; set; }

        public int PadString { get; set; }

        public int Line { get; set; }

        /// <summary>
        /// This is the speed of the scrolling text in millseconds
        /// </summary>
        public int ScrollSpeed { get; set; }

        /// <summary>
        /// Starts the text Scrolling on the screen
        /// </summary>
        public void StartScroll()
        {
            this._scrolling = true;
            this._scrollingThread.Start();
        }

        /// <summary>
        /// Stops the text scrolling on the screen.
        /// </summary>
        public void StopScroll()
        {
            this._scrolling = false;
            this._scrollingThread.Abort();
        }

        /// <summary>
        /// Stops the text scrolling on the screen.
        /// </summary>
        /// <param name="ResetText">If true will reset the scrolling text pos.</param>
        public void StopScroll(bool ResetText)
        {
            this._scrolling = false;
            this._scrollingThread.Abort();
            if (ResetText)
            {
                _display.WriteLine(this.Line, this.Text.Substring(0,20), 0, false);
            }
        }

        /// <summary>
        /// This is the thread that scrolls the text
        /// </summary>
        private void scrollingText()
        {
            string _return = "";
            string _toWrite = "";
            while (_scrolling)
            {
                _toWrite = this.Text.PadRight(this.Text.Length + this.PadString);
                _return = "";
                if (_strIdx + 20 < _toWrite.Length)
                {
                    _return = _toWrite.Substring(_strIdx, 20);
                }
                else
                {
                    _return = _toWrite.Substring(_strIdx);

                    _return += _toWrite.Substring(0, 20 - _return.Length);
                }
                _strIdx++;
                if (_strIdx >= _toWrite.Length)
                {
                    _strIdx = 0;
                }
                _display.WriteLine(this.Line, _return,0,false);
                Thread.Sleep(this.ScrollSpeed);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            _scrolling = false;
            this._scrollingThread.Abort();
        }

        #endregion
    }
}
