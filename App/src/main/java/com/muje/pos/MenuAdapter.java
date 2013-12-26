package com.muje.pos;

import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.Currency;
import java.util.Locale;

/**
 * Created by yeang-shing.then on 12/26/13.
 */
public class MenuAdapter extends ArrayAdapter<Counter> {
    private final Context context;
    private final ArrayList<Counter> values;
    private PosReader reader;
    public MenuAdapter(Context context, ArrayList<Counter> values) {
        super(context, R.layout.layout_menu, values);
        this.context = context;
        this.values = values;
        this.reader = null;
    }

    public void setReader(PosReader reader) {
        this.reader = reader;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        LayoutInflater inflater = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View rowView = inflater.inflate(R.layout.layout_menu, parent, false);
        Log.d("DEBUG", "Total Width: " + Integer.toString(rowView.getWidth()));

        // Define layout
        Counter counter = values.get(position);
        Menu menu = counter.getMenu();

        TextView colorView = (TextView)rowView.findViewById(R.id.textView);
        //int screenWidth = parent.getWidth();
        //colorView.setBackgroundColor();

        TextView codeView = (TextView)rowView.findViewById(R.id.textView2);
        codeView.setText(menu.getCode());

        TextView counterView = (TextView)rowView.findViewById(R.id.textView3);
        counterView.setText(Integer.toString(counter.getCount()));

        TextView priceView = (TextView)rowView.findViewById(R.id.textView4);
        priceView.setText(Currency.getInstance(Locale.getDefault()).getSymbol() + String.format("%.2f%n", counter.getTotal()));

        TextView nameView = (TextView)rowView.findViewById(R.id.textView5);
        nameView.setText(menu.getName());

        return rowView;
    }
}
