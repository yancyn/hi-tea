package com.muje.pos;

import android.app.Fragment;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

/**
 * Created by yeang-shing.then on 12/24/13.
 */
public class MonthFragment extends Fragment {
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        Log.d("DEBUG", "Launch MonthFragment");
        return inflater.inflate(R.layout.fragment_month, container, false);
    }
}