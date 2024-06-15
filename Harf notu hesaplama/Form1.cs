using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Harf_notu_hesaplama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-DDOVUAJ;Initial Catalog=Notes;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

        private void showDatas()
        {
            listView1.Items.Clear();
            connect.Open();
            SqlCommand cmd = new SqlCommand("select *from note", connect);
            SqlDataReader dr = cmd.ExecuteReader();

            while(dr.Read())
            {
                ListViewItem item = new ListViewItem();
                item.Text = dr["id"].ToString();
                item.SubItems.Add(dr["coursename"].ToString());
                item.SubItems.Add(dr["acts"].ToString());
                item.SubItems.Add(dr["letternote"].ToString());
                listView1.Items.Add(item);
            }
            connect.Close();
            textBox1.Clear();
            textBox2.Clear();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            showDatas();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("insert into note (coursename, acts, letternote) values ('" + textBox1.Text.ToString() + "', '" + textBox2.Text.ToString() + "','" + comboBox1.Text.ToString() + "')", connect);
            cmd.ExecuteNonQuery();
            connect.Close();
            showDatas();
            textBox1.Clear();
            textBox2.Clear();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        int id = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("delete from note where id =("+id+") ", connect);
            cmd.ExecuteNonQuery();
            connect.Close();
            showDatas();
        }
        
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[2].Text;
            comboBox1.Text = listView1.SelectedItems[0].SubItems[3].Text;
        }


        double factor(string letters)
        {
            switch (letters)
            {
                case "AA": return 4.00;  break;
                case "BA": return 3.50; break;
                case "BB": return 3.00; break;
                case "CB": return 2.50; break;
                case "CC": return 2.00; break;
                case "DC": return 1.50; break;
                case "DD": return 1.00; break;
                case "FD": return 0.50; break;
                case "FF": return 0.00; break;
                default: return 0.00;
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("select *from note", connect);
            SqlDataReader dr = cmd.ExecuteReader();
            List<int> akts = new List<int>();
            List<string> letters = new List<string>();
            string letterNote = null;
            while (dr.Read())
            {

                int acts = int.Parse(dr["acts"].ToString());
                letterNote = dr["letternote"].ToString();

                akts.Add(acts);
                letters.Add(letterNote);
                
            }
            connect.Close();

            int totalActs = 0;
            double totalFactor = 0;
            for (int i = 0; i < akts.Count; i++)
            {
                totalActs += akts[i];
                totalFactor += (akts[i] * factor(letters[i]));
            }

            double mean = totalFactor / totalActs;

            button5.Text = mean.ToString();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("update note set coursename='" +textBox1.Text.ToString() + "', acts='" +textBox2.Text.ToString() + "',letternote='" +comboBox1.Text.ToString() + "' where id=" + id+"", connect);
            cmd.ExecuteNonQuery();
            connect.Close();
            showDatas();
        }
    }
}
