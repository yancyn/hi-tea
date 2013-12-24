package com.muje.pos;

import java.util.ArrayList;
import java.util.Date;

/**
 * Represent a total sales in a day.
 * Created by yeang-shing.then on 12/24/13.
 */
public class Sales {

    private Date date;
    public Date getDate() {
        return this.date;
    }

    private int count;
    public int getCount() {
        return this.count;
    }

    private double amount;
    public double getAmount() {
        return this.amount;
    }

    private ArrayList<Order> orders;
    public ArrayList<Order> getOrders() {
        return this.orders;
    }

    public Sales(Date date, int count, double amount) {
        this.date = date;
        this.orders = new ArrayList<Order>();
        this.count = count;
        this.amount = amount;
    }
    public Sales(Date date, ArrayList<Order> orders) {
        this.date = date;
        this.orders = orders;
        this.count = orders.size();
        this.amount = 0;
        for(Order order: orders) {
            this.amount += order.getTotal();
        }
    }
}
