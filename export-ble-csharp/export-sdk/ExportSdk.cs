using export_sdk_server;
using System;

namespace export_sdk
{
    public class ExportSdk
    {
        private string _Address;
        private ExportSdkServer _Server;
        
        public ExportSdk(string address)
        {
            _Address = address;
            _Server = new ExportSdkServer();
        }
        
        public int TestCommunication()
        {
            try
            {
                if (string.IsNullOrEmpty(_Address))
                    return -1;

                if (_Server == null)
                    return -2;

                _Server.IncrementIter();
                return _Server.GetIter();
            }
            catch (Exception)
            {
                return -3;
            }
        }
    }
}
