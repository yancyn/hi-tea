package com.muje.pos;

import android.annotation.TargetApi;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.content.res.Configuration;
import android.media.Ringtone;
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.preference.EditTextPreference;
import android.preference.ListPreference;
import android.preference.Preference;
import android.preference.PreferenceActivity;
import android.preference.PreferenceCategory;
import android.preference.PreferenceFragment;
import android.preference.PreferenceManager;
import android.preference.RingtonePreference;
import android.text.TextUtils;
import android.util.Log;

import java.util.List;

/**
 * Created by yeang-shing.then on 9/24/13.
 */
public class SettingsActivity extends PreferenceActivity implements SharedPreferences.OnSharedPreferenceChangeListener {

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        addPreferencesFromResource(R.xml.settings);

        try {
            // display database path
            SharedPreferences sharedPreferences = PreferenceManager.getDefaultSharedPreferences(this);
            String path = sharedPreferences.getString("prefDatabase", getResources().getString(R.string.database_path));// "/sdcard/Download/pos.db3");
            if (!path.isEmpty()) {
                EditTextPreference prefDatabase = (EditTextPreference) findPreference("prefDatabase");
                prefDatabase.setSummary(path);
            }

            // set version
            Preference prefVersion = findPreference("prefVersion");
            String version = getPackageManager().getPackageInfo(getPackageName(), 0).versionName;
            prefVersion.setSummary(version);

            Preference prefEmail = findPreference("prefEmail");
            prefEmail.setOnPreferenceClickListener(new Preference.OnPreferenceClickListener() {
                @Override
                public boolean onPreferenceClick(Preference preference) {
                    Intent intent = new Intent(Intent.ACTION_SEND);
                    intent.putExtra(Intent.EXTRA_EMAIL, new String[]{getResources().getString(R.string.contact_email)});
                    intent.setType("message/rfc822");
                    startActivity(Intent.createChooser(intent, "Sending email"));
                    return false;
                }
            });
        } catch (PackageManager.NameNotFoundException e) {
            Log.e("ERROR", e.getMessage());
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        getPreferenceScreen().getSharedPreferences().registerOnSharedPreferenceChangeListener(this);
    }

    @Override
    protected void onPause() {
        super.onPause();
        getPreferenceScreen().getSharedPreferences().unregisterOnSharedPreferenceChangeListener(this);
    }

    @Override
    public void onSharedPreferenceChanged(SharedPreferences sharedPreferences, String s) {
        Preference pref = findPreference(s);
        if (pref instanceof EditTextPreference) {
            EditTextPreference editText = (EditTextPreference) pref;
            String summary = editText.getText();
            if (editText.getKey().equals("prefDatabase")) {
                editText.setSummary(summary);
            }
        }
    }
}
