package com.muje.pos;

/**
 * Created by yeang-shing.then on 12/26/13.
 */
public class Counter {

    private int count;
    public int getCount() {
        return this.count;
    }

    private Menu menu;
    public Menu getMenu() {
        return this.menu;
    }

    public double getTotal() {
        return this.count * menu.getPrice();
    }

    public Counter(Menu menu, int count) {
        this.menu = menu;
        this.count = count;
    }

}
