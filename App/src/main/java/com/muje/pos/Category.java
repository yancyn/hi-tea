package com.muje.pos;

/**
 * Created by yeang-shing.then on 12/26/13.
 */
public class Category {

    private int id;
    public int getId() {
        return this.id;
    }

    private String name;
    public String getName() {
        return this.name;
    }

    public Category(int id, String name) {
        this.id = id;
        this.name = name;
    }
}
