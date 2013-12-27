package com.muje.pos;

import android.app.ActionBar;
import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.RelativeLayout;
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

    private int getColor(int categoryId) {
        switch(categoryId) {
            case 2: return R.color.color_set_menu;
            case 3: return R.color.color_food;
            case 4: return R.color.color_snack;
            case 5: return R.color.color_drink;
            case 6: return R.color.color_dessert;
            default: return R.color.color_addon;
        }
    }
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        LayoutInflater inflater = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View rowView = inflater.inflate(R.layout.layout_menu, parent, false);

        Counter counter = values.get(position);
        Menu menu = counter.getMenu();

        // Define layout
        TextView colorView = (TextView)rowView.findViewById(R.id.textView);
        int screenWidth = parent.getWidth();
        int width = counter.getCount() * screenWidth / reader.getMaxCounter();
        colorView.setWidth(width);
        colorView.setBackgroundResource(getColor(menu.getCategoryId()));

        TextView codeView = (TextView)rowView.findViewById(R.id.textView2);
        codeView.setText(menu.getCode());

        TextView counterView = (TextView)rowView.findViewById(R.id.textView3);
        counterView.setText(Integer.toString(counter.getCount()));

        TextView priceView = (TextView)rowView.findViewById(R.id.textView4);
        priceView.setText(Currency.getInstance(Locale.getDefault()).getSymbol() + String.format("%,.2f", counter.getTotal()));
        // handle position for not overlap counter
        double percentage = counter.getCount() * 100 / reader.getMaxCounter();
        if(percentage > 90) {
            RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(
                    RelativeLayout.LayoutParams.WRAP_CONTENT, RelativeLayout.LayoutParams.WRAP_CONTENT);
            params.setMargins(0,0,100,0);
            params.addRule(RelativeLayout.ALIGN_PARENT_TOP);
            params.addRule(RelativeLayout.ALIGN_RIGHT, R.id.textView);
            priceView.setLayoutParams(params);
        }
        if(percentage < 50) {
            RelativeLayout.LayoutParams params = new RelativeLayout.LayoutParams(
                    RelativeLayout.LayoutParams.WRAP_CONTENT, RelativeLayout.LayoutParams.WRAP_CONTENT);
            params.setMargins(0,0,-230,0);
            params.addRule(RelativeLayout.ALIGN_PARENT_TOP);
            params.addRule(RelativeLayout.ALIGN_RIGHT, R.id.textView);
            priceView.setLayoutParams(params);
        }

        TextView nameView = (TextView)rowView.findViewById(R.id.textView5);
        nameView.setText(menu.getName());

        return rowView;
    }
}
