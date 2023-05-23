namespace Crystalfontz.Displays
{
    public abstract partial class CFA63XDisplay
    {
        public delegate void KeyPressedHandler(object sender, CFAKeyEventArgs e);
        public event KeyPressedHandler KeyPressed;

        public delegate void KeyReleasedHandler(object sender, CFAKeyEventArgs e);
        public event KeyReleasedHandler KeyReleased;

        public delegate void PacketRecivedHandler(object sender, CFAPacket e);
        public event PacketRecivedHandler PacketRecived;

        public delegate void PacketSentHandler(object sender, CFAPacket e);
        public event PacketSentHandler PacketSent;

        public delegate void HardwareFirmwareVersionHandler(object sender, CFAHardwareFirmwareEventArgs e);
        public event HardwareFirmwareVersionHandler HardwareFirmwareVersion;

        public delegate void ReadUserFlashHandler(object sender, CFAReadUserFlashEventArgs e);
        public event ReadUserFlashHandler ReadUserFlash;

        public delegate void ReadDataHandler(object sender, CFAReadDataEventArgs e);
        public event ReadDataHandler ReadData;

        public delegate void BaudRateChangeHandler(object sender);
        public event BaudRateChangeHandler BaudRateChange;

        public delegate void ReportingAndStatusHandler(object sender, CFAReportingAndStatusEventArgs e);
        public event ReportingAndStatusHandler ReportingAndStatus;
    }
}
