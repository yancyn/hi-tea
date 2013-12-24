package com.muje.pos;

import java.util.Date;

/**
 * Represent a unique order from Pos system.
 * Created by yeang-shing.then on 12/24/13.
 */
public class Order {
    private int id;
    public int getId() {
        return this.id;
    }
    private Date created;
    public Date getCreated() {
        return this.created;
    }
    private String tableNo;
    public String getTableNo() {
        return this.tableNo;
    }
    private String queueNo;
    public String getQueueNo() {
        return this.queueNo;
    }
    private double total;
    public double getTotal() {
        return this.total;
    }
    private Date receiptDate;
    public Date getReceiptDate() {
        return this.receiptDate;
    }
    public void setReceiptDate(Date receiptDate) {
        this.receiptDate = receiptDate;
    }
    private int memberId;
    public int getMemberId() {
        return this.memberId;
    }
    public void setMemberId(int memberId) {
        this.memberId = memberId;
    }

    public Order(int id, Date created, String tableNo, String queueNo, double total) {
        this.id = id;
        this.created = created;
        this.tableNo = tableNo;
        this.queueNo = queueNo;
        this.total = total;
    }
}
