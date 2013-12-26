package com.muje.pos;

import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

/**
 * Created by yeang-shing.then on 12/24/13.
 */
public class PosReader {
    public static final String DATABASE_NAME = "/sdcard/Download/pos.db3"; // TODO: Put in config
    private final String GROUP_BY_DAY_QUERY = "SELECT strftime('%Y-%m-%d',Created) AS Date, COUNT(Id) AS Orders, SUM(Total) AS Sales FROM 'Order' GROUP BY strftime('%Y-%m-%d',Created) ORDER BY strftime('%Y-%m-%d',Created) DESC;";
    private ArrayList<Sales> sales;
    public ArrayList<Sales> getSales() {
        return this.sales;
    }

    public PosReader() {
    }
    public void retrieve() {

        SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd");
        SQLiteDatabase database = SQLiteDatabase.openDatabase(DATABASE_NAME, null, SQLiteDatabase.OPEN_READONLY);// helper.getReadableDatabase();

        this.sales = new ArrayList<Sales>();// HashMap<Date, Sales>();
        Cursor cursor = database.rawQuery(GROUP_BY_DAY_QUERY, null);
        cursor.moveToFirst();
        while(!cursor.isAfterLast()) {
            Date date = null;
            try {
                date = dateFormat.parse(cursor.getString(0));
            } catch (ParseException e) {
                e.printStackTrace();
            }

            //this.sales.put(date, new Sales(date, cursor.getInt(1), cursor.getDouble(2)));
            this.sales.add(new Sales(date, cursor.getInt(1), cursor.getDouble(2)));
            cursor.moveToNext();
        }
        cursor.close();
    }
}
