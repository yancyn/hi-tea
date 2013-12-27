package com.muje.pos;

import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.util.Log;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;

/**
 * Pos business logic singleton class.
 * Created by yeang-shing.then on 12/24/13.
 */
public class PosReader {

    private static PosReader instance;
    public static final String DATABASE_NAME = "/sdcard/Download/pos.db3"; // TODO: Put in config

    private final String GROUP_BY_DAY_QUERY = "SELECT strftime('%Y-%m-%d',Created) AS Date, COUNT(Id) AS Orders, SUM(Total) AS Sales FROM 'Order' GROUP BY strftime('%Y-%m-%d',Created) ORDER BY strftime('%Y-%m-%d',Created) DESC;";
    private ArrayList<Sales> sales;
    public ArrayList<Sales> getSales() {
        return this.sales;
    }

    private ArrayList<Sales> monthlySales;
    public ArrayList<Sales> getMonthlySales() {
        return this.monthlySales;
    }

    private final String GROUP_BY_MENU_QUERY = "SELECT OrderItem.MenuId, Category.Id AS CategoryId, Menu.Code, Menu.Name, Menu.Price, Count(OrderItem.MenuId) AS Count FROM OrderItem JOIN Menu ON OrderItem.MenuId=Menu.Id JOIN Category ON Menu.CategoryId=Category.Id GROUP BY OrderItem.MenuId ORDER BY Count(OrderItem.MenuId) DESC;";
    private ArrayList<Counter> counters;
    public ArrayList<Counter> getCounter() {
        return this.counters;
    }

    public PosReader() {
        // TODO: Handle database file exist
        retrieve();
    }

    public static synchronized PosReader getInstance() {
        if(instance == null) {
            instance = new PosReader();
        }
        return instance;
    }

    private void retrieve() {

        Log.d("DEBUG", "PosReader.retrieve()");

        SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd");
        SimpleDateFormat monthFormat = new SimpleDateFormat("yyyy-MM");
        SQLiteDatabase database = SQLiteDatabase.openDatabase(DATABASE_NAME, null, SQLiteDatabase.OPEN_READONLY);// helper.getReadableDatabase();

        // get sales
        Map<String, Sales> map = new HashMap<String, Sales>();
        this.sales = new ArrayList<Sales>();
        Cursor cursor = database.rawQuery(GROUP_BY_DAY_QUERY, null);
        cursor.moveToFirst();
        while(!cursor.isAfterLast()) {
            Date date = null;
            try {
                date = dateFormat.parse(cursor.getString(0));
            } catch (ParseException e) {
                e.printStackTrace();
            }

            int count = cursor.getInt(1);
            double sale = cursor.getDouble(2);
            this.sales.add(new Sales(date, count, sale));

            // accumulate in monthly sales collection
            String key = monthFormat.format(date);
            if(!map.containsKey(key)) {
                Calendar calendar = Calendar.getInstance();
                calendar.set(date.getYear()+1900, date.getMonth(), 1);
                map.put(key, new Sales(calendar.getTime(), count, sale));
            } else {
                Sales sales = map.get(key);
                sales.addCount(count);
                sales.addAmount(sale);
            }

            cursor.moveToNext();
        }
        cursor.close();

        // dump to monthly sales collection
        this.monthlySales = new ArrayList<Sales>();
        for(Map.Entry<String, Sales> s: map.entrySet()) {
            this.monthlySales.add(s.getValue());
        }

        // get top selling menu
        this.counters = new ArrayList<Counter>();
        cursor = database.rawQuery(GROUP_BY_MENU_QUERY, null);
        cursor.moveToFirst();
        while(!cursor.isAfterLast()) {

            int count = cursor.getInt(5);
            double price = cursor.getDouble(4);
            Menu menu = new Menu(cursor.getInt(0), cursor.getInt(1), cursor.getString(2), cursor.getString(3), price);
            this.counters.add(new Counter(menu, count));
            cursor.moveToNext();
        }
        cursor.close();

    }
}
