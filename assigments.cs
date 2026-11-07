using System;
using System.IO;
using System.Windows.Forms;

namespace Lucky_Cafe_Order
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter customer name.", "Error");
                return;
            }

            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity < 1)
            {
                MessageBox.Show("Quantity must be a number of 1 or more.", "Error");
                return;
            }

            string drink;
            decimal price;

            if (rdoTea.Checked)
            {
                drink = "Tea";
                price = 1;
            }
            else if (rdoCoffee.Checked)
            {
                drink = "Coffee";
                price = 2;
            }
            else if (rdoJuice.Checked)
            {
                drink = "Juice";
                price = 3;
            }
            else
            {
                MessageBox.Show("Please select a drink.", "Error");
                return;
            }

            listBox.Items.Clear();

            Random rand = new Random();
            bool gotCookie = false;
            int cookieNumber = 0;

            for (int i = 1; i <= quantity; i++)
            {
                int lucky = rand.Next(1, 51);

                if (lucky == 7 || lucky == 17)
                {
                    gotCookie = true;
                    cookieNumber = lucky;
                }

                listBox.Items.Add(i + ") " + txtName.Text.Trim() + " - " + drink +
                                  " - Lucky: " + lucky + " - $" + price.ToString("F2"));
            }

            decimal totalBefore = price * quantity;
            decimal total = totalBefore;

            if (chkStudent.Checked)
            {
                total = totalBefore - (totalBefore * 0.10m);
            }

            lblTotal.Text = "Total Price: $" + total.ToString("F2");

            string luckyMessage;
            if (gotCookie)
            {
                luckyMessage = "You got a Free Cookie!";
            }
            else
            {
                luckyMessage = "No prize this time.";
            }
            lblLucky.Text = "Lucky Message: " + luckyMessage;

            // Summary lines in the ListBox
            listBox.Items.Add("");
            listBox.Items.Add("Member Discount: " + (chkStudent.Checked ? "10%" : "0%"));
            listBox.Items.Add("Total Before Discount: $" + totalBefore.ToString("F2"));
            listBox.Items.Add("Total After Discount: $" + total.ToString("F2"));

            if (gotCookie)
                listBox.Items.Add("Lucky Message: " + luckyMessage + " (Lucky " + cookieNumber + ")");
            else
                listBox.Items.Add("Lucky Message: " + luckyMessage);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (listBox.Items.Count == 0)
            {
                MessageBox.Show("No order to save.");
                return;
            }

            StreamWriter writer = File.AppendText("orders.txt");

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                writer.WriteLine(listBox.Items[i].ToString());
            }

            writer.WriteLine(lblTotal.Text);
            writer.WriteLine(lblLucky.Text);
            writer.WriteLine("----------------------------");

            writer.Close();

            MessageBox.Show("Order saved.");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists("orders.txt"))
            {
                MessageBox.Show("No file found.");
                return;
            }

            listBox.Items.Clear();

            StreamReader reader = File.OpenText("orders.txt");

            while (!reader.EndOfStream)
            {
                listBox.Items.Add(reader.ReadLine());
            }

            reader.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtQuantity.Clear();
            listBox.Items.Clear();
            lblTotal.Text = "Total Price: $0.00";
            lblLucky.Text = "Lucky Message:";
            chkStudent.Checked = false;
            rdoTea.Checked = false;
            rdoCoffee.Checked = false;
            rdoJuice.Checked = false;
            txtName.Focus();
        }
    }
}