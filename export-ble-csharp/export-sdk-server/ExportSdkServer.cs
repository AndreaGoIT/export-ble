using System;

namespace export_sdk_server
{
    public class ExportSdkServer
    {
        int _Iter;

        public ExportSdkServer()
        {
            _Iter = 0;
        }

        public void IncrementIter()
        {
            _Iter++;
        }

        public int GetIter()
        {
            return _Iter; 
        }
    }
}
