package com.example.exportblecsharp;

import android.Manifest;
import android.content.pm.PackageManager;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import java.util.ArrayList;

import export_ble_csharp.*;

public class MainActivity extends AppCompatActivity {

    private static final int PERMISSION_REQUEST_ID = 1;

    private Button _TestButton;
    private Button _StartScan;
    private Button _StopScan;
    private Button _ShowResult;
    private TextView _ResultText;
    private ListView _ListView;

    private BLEManager _BLEManager;
    private String _Text;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        _TestButton = findViewById(R.id.test_button);
        _StartScan = findViewById(R.id.start_scan);
        _StopScan = findViewById(R.id.stop_scan);
        _ShowResult = findViewById(R.id.show_result);
        _ResultText = findViewById(R.id.result_text);
        _ListView = findViewById(R.id.list_view);

        _BLEManager = BLEManager.GetInstance();

        _TestButton.setOnClickListener(v -> {
            boolean res = _BLEManager.IsEnable();
            if (res)
                UpdateText("Ble status: ON");
            else
                UpdateText("Ble status: OFF");
        });

        _StartScan.setOnClickListener(v -> {
            int ret = _BLEManager.StartScan(20000);

            UpdateText("Start scan: " + ret);
        });

        _StopScan.setOnClickListener(v -> {
            if (_BLEManager.IsScanning())
            {
                _BLEManager.StopScan();
                UpdateText("Stop scan");
            }
            else
            {
                UpdateText("Scan already stop");
            }
        });

        _ShowResult.setOnClickListener(v -> {
            if (!_BLEManager.IsScanning())
            {
                String[] result = _BLEManager.GetScanResult();
                ArrayList<String> array_list = new ArrayList<>();

                for (int i=0; i<_BLEManager.GetScanResultCount(); i++)
                    array_list.add(result[i]);

                _ListView.setAdapter(new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, array_list));
            }
            else
            {
                UpdateText("Not valid operation");
            }
        });

        UpdateText("Read: " + Integer.toString(_BLEManager.GetExportCall()));
        EnableButtons(false);
    }

    @Override
    protected void onResume() {
        super.onResume();

        if (ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            // Permission is not granted
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_COARSE_LOCATION}, PERMISSION_REQUEST_ID);
        }
        else {
            EnableButtons(true);
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        switch (requestCode) {
            case PERMISSION_REQUEST_ID: {
                // If request is cancelled, the result arrays are empty.
                if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    // permission was granted, yay! Do the
                    // contacts-related task you need to do.
                    EnableButtons(true);
                } else {
                    // permission denied, boo! Disable the
                    // functionality that depends on this permission.
                    Toast.makeText(MainActivity.this, "Permission denied to access Location", Toast.LENGTH_SHORT).show();
                }
                return;
            }
            // other 'case' lines to check for other
            // permissions this app might request.
        }
    }

    private void EnableButtons(boolean state)
    {
        _TestButton.setActivated(state);
        _TestButton.setEnabled(state);

        _StartScan.setActivated(state);
        _StartScan.setEnabled(state);

        _StopScan.setActivated(state);
        _StopScan.setEnabled(state);
    }
    private void UpdateText(String text)
    {
        _Text = text;
        _ResultText.setText(_Text);
    }
}
