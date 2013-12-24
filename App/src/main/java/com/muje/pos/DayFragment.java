package com.muje.pos;

import android.app.Fragment;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.TextView;

import java.util.Date;
import java.util.HashMap;
import java.util.Map;

/**
 * Created by yeang-shing.then on 12/24/13.
 */
public class DayFragment extends Fragment {
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        Log.d("DEBUG", "Launch DayFragment");

        PosReader reader = new PosReader(getActivity());
        reader.retrieve();

//        Map<Date, Sales> sales = reader.getSales();
//        for(Map.Entry<Date, Sales> sale: sales.entrySet()) {
//            Log.d("DEBUG", sale.getKey() + ": " + sale.getValue().getAmount() );
//        }

        View view = inflater.inflate(R.layout.fragment_day, container, false);
        SalesAdapter adapter = new SalesAdapter(view.getContext(), reader.getSales());
        ListView listView = (ListView)view.findViewById(R.id.listView);
        listView.setAdapter(adapter);
        return view;
    }
}