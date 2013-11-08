﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using HiTea.Pos;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Markup;

namespace PosWPF
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            PosManager posManager = (this.DataContext as PosManager);

            // remove queue no if failed to make any order
            if (posManager.SelectedOrder.Items.Count == 0)
            {
                if (posManager.CarryBasket.Contains(posManager.SelectedOrder))
                    posManager.CarryBasket.Remove(posManager.SelectedOrder);
                else
                {
                    for (int i = 0; i < posManager.TableBasket.Count; i++)
                    {
                        if (posManager.TableBasket[i].TableNo == posManager.SelectedOrder.TableNo)
                        {
                            Order newOrder = new Order();
                            newOrder.TableNo = posManager.SelectedOrder.TableNo;
                            newOrder.Created = DateTime.Now;
                            posManager.TableBasket[i] = newOrder;
                            posManager.SelectedOrder = posManager.TableBasket[i];

                            if (this.Tag is TableControl)
                                (this.Tag as TableControl).Binding(posManager);
                            break;
                        }
                    }
                }

                posManager.SelectedOrder.QueueNo = string.Empty;
            }

            // remove from cache once paid
            if (posManager.SelectedOrder.ReceiptDate.HasValue)
            {
                if (posManager.CarryBasket.Contains(posManager.SelectedOrder))
                    posManager.CarryBasket.Remove(posManager.SelectedOrder);
                else
                {
                    for (int i = 0; i < posManager.TableBasket.Count; i++)
                    {
                        if (posManager.TableBasket[i].TableNo == posManager.SelectedOrder.TableNo)
                        {
                            Order newOrder = new Order();
                            newOrder.TableNo = posManager.SelectedOrder.TableNo;
                            newOrder.Created = DateTime.Now;
                            posManager.TableBasket[i] = newOrder;
                            posManager.SelectedOrder = posManager.TableBasket[i];

                            if (this.Tag is TableControl)
                                (this.Tag as TableControl).Binding(posManager);
                            break;
                        }
                    }
                }
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            PosManager posManager = (PosManager)this.DataContext;
            if (posManager.SelectedOrder.Items.Count == 0)
            {
                if (posManager.CarryBasket.Contains(posManager.SelectedOrder))
                    posManager.CarryBasket.Remove(posManager.SelectedOrder);
                posManager.SelectedOrder.QueueNo = string.Empty;
            }

            this.Close();
        }

        // TODO: Print order list
        private void OrderListButton_Click(object sender, RoutedEventArgs e)
        {
            Order order = (this.DataContext as PosManager).SelectedOrder;
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            Numpad numpad = new Numpad();
            numpad.Owner = this;
            numpad.Topmost = true;
            numpad.DataContext = this.DataContext;
            numpad.ShowDialog();
        }

        private void ReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.PrintDialog pd = new System.Windows.Forms.PrintDialog();
            PrintDocument pdoc = new PrintDocument();
            pd.Document = pdoc;
            pdoc.PrintPage += new PrintPageEventHandler(Receipt_PrintPage);
            pdoc.Print();
        }
        // TODO: Consider to convert to xaml layout for easier maintenance
        // http://www.bradcurtis.com/document-and-report-generation-using-xaml-wpf-data-binding-and-xps/
        private void Receipt_PrintPage(object sender, PrintPageEventArgs e)
        {
            string underline = "--------------------------------------------------";
            string tab = "    ";

            // TODO: Receipt print format
            Order order = (this.DataContext as PosManager).SelectedOrder;

            Graphics graphics = e.Graphics;
            Font font = new Font("SimSun Regular", 10);// TODO: SimHei Regular
            float fontHeight = font.GetHeight();
            int startX = 10;
            int startY = 10;
            int offset = 10;
            graphics.DrawString("Welcome to Hi Tea", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            string feed = string.Empty;
            feed += "#" + order.ID;
            feed += tab + "Table: " + order.TableNo;
            feed += tab + "Queue: " + order.QueueNo;
            graphics.DrawString(feed, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            graphics.DrawString("Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            graphics.DrawString(underline, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            int i = 0;
            foreach (OrderItem item in order.Items)
            {
                i++;
                offset += 20;

                string line = i + ". " + item.Menu.Code + " " + item.Menu.Name;
                string price = item.Menu.Price.ToString("###,##0.00");
                graphics.DrawString(line, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);
                graphics.DrawString(price, font, new SolidBrush(System.Drawing.Color.Black), startX + 160, startY + offset);
            }

            offset += 20;
            graphics.DrawString(underline, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            feed = "Total: ";
            string total = order.Total.ToString("###,##0.00");
            graphics.DrawString(feed, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);
            graphics.DrawString(total, font, new SolidBrush(System.Drawing.Color.Black), startX + 160 - 10, startY + offset);

            offset += 20;
            graphics.DrawString(underline, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            graphics.DrawString("Thank you", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            graphics.DrawString("", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset); // HACK: Add a footer margin
        }

        private void LabelButton_Click(object sender, RoutedEventArgs e)
        {
            Order order = (this.DataContext as PosManager).SelectedOrder;
            foreach (OrderItem item in order.Items)
            {
                if (item.Menu.CategoryID == 3)
                {
                    System.Windows.Controls.PrintDialog pd = new System.Windows.Controls.PrintDialog();
                    pd.PrintQueue = new PrintQueue(new PrintServer(), "Bar");

                    // add content to first page
                    /*Canvas canvas = new Canvas();

                    TextBlock feed = new TextBlock();
                    feed.FontSize = 12;
                    feed.Foreground = System.Windows.Media.Brushes.Black;
                    feed.Text = "No: " + order.QueueNo;
                    Canvas.SetLeft(feed, 4);
                    Canvas.SetTop(feed, 4);
                    canvas.Children.Add(feed);

                    feed = new TextBlock();
                    feed.FontSize = 12;
                    feed.Foreground = System.Windows.Media.Brushes.Black;
                    feed.Text = item.Menu.Name;
                    Canvas.SetLeft(feed, 4);
                    Canvas.SetTop(feed, 14);
                    canvas.Children.Add(feed);
                    */
                    // FAILED: pd.PrintVisual(canvas, "Drink Label");

                    FlowDocument flowDocument = new FlowDocument();
                    Paragraph paragraph = new Paragraph(new Run("No: " + order.QueueNo));
                    flowDocument.Blocks.Add(paragraph);
                    paragraph = new Paragraph(new Run(item.Menu.Name));
                    flowDocument.Blocks.Add(paragraph);
                    pd.PrintDocument(((IDocumentPaginatorSource)flowDocument).DocumentPaginator, "Drink Label");
                }
            }
        }
    }
}