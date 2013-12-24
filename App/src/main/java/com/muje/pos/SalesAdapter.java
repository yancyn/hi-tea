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

/**
 * Created by yeang-shing.then on 12/24/13.
 */
public class SalesAdapter extends ArrayAdapter<Sales> {

    private final Context context;
    private final ArrayList<Sales> values;
    private final SimpleDateFormat weekFormat = new SimpleDateFormat("c");
    private final SimpleDateFormat dateFormat = new SimpleDateFormat("dd-MMM-yyyy");

    public SalesAdapter(Context context, ArrayList<Sales> values) {
        super(context, R.layout.layout_sales, values);
        this.context = context;
        this.values = values;
    }
    private String getWeekDay(int day) {
        switch(day) {
            case 0:
                return "Sun";
            case 1:
                return "Mon";
            case 2:
                return "Tue";
            case 3:
                return "Wed";
            case 4:
                return "Thu";
            case 5:
                return "Fri";
            case 6:
                return "Sat";
        }

        return "";
    }
    /**
     * Round up value to desired decimal places.
     * See http://stackoverflow.com/questions/2808535/round-a-double-to-2-significant-figures-after-decimal-point
     * @param value
     * @param places
     * @return
     */
    public static double round(double value, int places) {
        if(places < 0) throw new IllegalArgumentException();
        BigDecimal bd = new BigDecimal(value);
        bd = bd.setScale(places, BigDecimal.ROUND_HALF_UP);
        return bd.doubleValue();
    }
    public View getView(int position, View contentView, ViewGroup parent) {

        LayoutInflater inflater = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View rowView = inflater.inflate(R.layout.layout_sales, parent, false);

        // Define all components
        Sales sales = values.get(position);
        TextView dayView = (TextView)rowView.findViewById(R.id.textView);
        dayView.setText(weekFormat.format(sales.getDate())); // sales.getDate().getDay()

        TextView dateView = (TextView)rowView.findViewById(R.id.textView2);
        dateView.setText(dateFormat.format(sales.getDate()));

        TextView countView = (TextView)rowView.findViewById(R.id.textView3);
        countView.setText(Integer.toString(sales.getCount()) + " orders");

        TextView totalView = (TextView)rowView.findViewById(R.id.textView4);
        //totalView.setText(Double.toString(round(sales.getAmount(),2))); // TODO: Add local currency
        totalView.setText("Total " + String.format("%.2f%n", sales.getAmount()));

        return rowView;
    }
}
