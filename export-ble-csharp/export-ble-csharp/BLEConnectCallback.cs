﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace export_ble_csharp
{
    [Register("export_ble_csharp.BLEConnectCallback")]
    public class BLEConnectCallback : BluetoothGattCallback
    {
    }
}