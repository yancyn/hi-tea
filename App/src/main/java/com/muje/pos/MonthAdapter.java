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
    private int maxCounter;
    private double maxAmount;
    private final SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy MMM");

    public MonthAdapter(Context context, ArrayList<Sales> values) {
        super(context, R.layout.layout_sales, values);
        this.context = context;
        this.values = values;

        this.maxCounter = 0;
        this.maxAmount = 0;
        for(Sales sales: values) {
            if(sales.getCount() > maxCounter) maxCounter = sales.getCount();
            if(sales.getAmount() > maxAmount) maxAmount = sales.getAmount();
        }
    }

    @Override
    public View getView(int position, View contentView, ViewGroup parent) {

        Sales sales = values.get(position);
        LayoutInflater inflater = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View rowView = inflater.inflate(R.layout.layout_month, parent, false);

        // paint graph for counter
        int maxWidth = parent.getWidth() / 2;
        TextView counterBar = (TextView)rowView.findViewById(R.id.textView4);
        counterBar.setWidth(sales.getCount() * maxWidth / maxCounter);

        // paint graph for amount
        TextView amountBar = (TextView)rowView.findViewById(R.id.textView5);
        int width = (int)(sales.getAmount() * maxWidth / maxAmount);
        amountBar.setWidth(width);

        // Define layout
        TextView monthView = (TextView)rowView.findViewById(R.id.textView);
        monthView.setText(dateFormat.format(sales.getDate()));

        TextView countView = (TextView)rowView.findViewById(R.id.textView2);
        countView.setText(Integer.toString(sales.getCount()) + " orders");

        TextView totalView = (TextView)rowView.findViewById(R.id.textView3);
        totalView.setText(Currency.getInstance(Locale.getDefault()).getSymbol() + String.format("%,.2f", sales.getAmount()));

        return rowView;
    }
}
