package com.muje.pos;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Currency;
import java.util.Locale;

/**
 * Created by yeang-shing.then on 12/26/13.
 */
public class MonthAdapter extends ArrayAdapter<Sales> {

    private final Context context;
    private final ArrayList<Sales> values;
    private final SimpleDateFormat dateFormat = new SimpleDateFormat("MMM yyyy");

    public MonthAdapter(Context context, ArrayList<Sales> values) {
        super(context, R.layout.layout_sales, values);
        this.context = context;
        this.values = values;
    }

    @Override
    public View getView(int position, View contentView, ViewGroup parent) {

        LayoutInflater inflater = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View rowView = inflater.inflate(R.layout.layout_month, parent, false);

        // Define layout
        Sales sales = values.get(position);
        TextView monthView = (TextView)rowView.findViewById(R.id.textView);
        monthView.setText(dateFormat.format(sales.getDate()));

        TextView countView = (TextView)rowView.findViewById(R.id.textView2);
        countView.setText(Integer.toString(sales.getCount()) + " orders");

        TextView totalView = (TextView)rowView.findViewById(R.id.textView3);
        totalView.setText(Currency.getInstance(Locale.getDefault()).getSymbol() + String.format("%,.2f", sales.getAmount()));

        return rowView;
    }
}
