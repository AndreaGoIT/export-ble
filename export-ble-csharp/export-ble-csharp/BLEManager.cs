using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Locations;
using Android.Runtime;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace export_ble_csharp
{
    [Register("export_ble_csharp.BLEManager")]
    public class BLEManager : ScanCallback
    {
        private static BLEManager _Instance;
        private BluetoothManager _CentralManager;
        private BLEConnectCallback _ConnectCallback;
        private Timer _ScanTimeout;
        private List<string> _DeviceNames;

        private bool _IsScanning;

        #region CTOR
        private BLEManager()
        {
            _CentralManager = (BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService);
            _ConnectCallback = new BLEConnectCallback();
            _DeviceNames = new List<string>();

            _IsScanning = false;
        }
        #endregion

        [Export("GetInstance")]
        public static BLEManager GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new BLEManager();
            }
            return _Instance;
        }

        [Export("IsEnable")]
        public bool IsEnable()
        {
            try
            {
                var locationManager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);
                return _CentralManager.Adapter.IsEnabled && locationManager.IsProviderEnabled(LocationManager.GpsProvider);
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Export("IsScanning")]
        public bool IsScanning()
        {
            return _IsScanning;
        }

        [Export("StartScan")]
        public int StartScan(int scanTimeoutMs)
        {
            if (scanTimeoutMs <= 0)
                return -2;

            if (!IsEnable())
                return -3;
            
            try
            {
                _ScanTimeout = new Timer((obj) =>
                {
                    StopScan();
                }, null, scanTimeoutMs, Timeout.Infinite);

                _IsScanning = true;
                _DeviceNames.Clear();
                _CentralManager?.Adapter?.BluetoothLeScanner?.StartScan(this);
            }
            catch (Exception)
            {
                return -1;
            }

            return 0;
        }

        [Export("StopScan")]
        public void StopScan()
        {
            _IsScanning = false;
            _ScanTimeout?.Change(Timeout.Infinite, Timeout.Infinite);
            _CentralManager?.Adapter?.BluetoothLeScanner?.StopScan(this);
        }

        public string[] ScanResult
        {
            [Export("GetScanResult")]
            get => _DeviceNames.ToArray();
        }

        public int ScanResultCount
        {
            [Export("GetScanResultCount")]
            get => _DeviceNames.Count;
        }

        public override void OnScanResult(ScanCallbackType callbackType, ScanResult result)
        {
            try
            {
                if (!string.IsNullOrEmpty(result.Device.Address) && !_DeviceNames.Any(x => x.Equals(result.Device.Address)))
                {
                    // update device list
                    _DeviceNames.Add(result.Device.Address);
                }
            }
            catch (Exception)
            { }
        }

        [Export("GetExportCall")]
        public int GetExportCall()
        {
            var export = new export_sdk.ExportSdk("wow");
            return export.TestCommunication();
        }
    }
}
