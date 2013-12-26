package com.muje.pos;

/**
 * Created by yeang-shing.then on 12/26/13.
 */
public class Menu {
    private int id;
    public int getId() {
        return this.id;
    }

    private int categoryId;
    public int getCategoryId() {
        return this.categoryId;
    }

    private String code;
    public String getCode() {
        return this.code;
    }

    private String name;
    public String getName() {
        return this.name;
    }

    private double price;
    public double getPrice() {
        return this.price;
    }

    public Menu(int id, int categoryId, String code, String name, double price) {
        this.id = id;
        this.categoryId = categoryId;
        this.code = code;
        this.name = name;
        this.price = price;
    }
}
