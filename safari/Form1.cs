using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.IO;

namespace safari
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (Clipboard.ContainsText() == true)
            {
                //Извлекаем (точнее копируем) его и сохраняем в переменную
                string someText = Clipboard.GetText();

                var tmp = someText.Split('\n');
                //var nameFish = Regex.Split(tmp[0], "-?[0-9]+");
                //MessageBox.Show(nameFish[0]);

                var n = tmp.Length;
                //Выводим показываем сообщение с текстом, скопированным из буфера обмена
                for (var i = 0; i < n - 1; i++)
                {
                    var nameFish = Regex.Split(tmp[i].Trim(), "-?[0-9]+")[0].Trim();
                    string pattern = @"(\d+(?:\.\d+)?)";
                    Regex rgx = new Regex(pattern);
                    var minWeight = tmp[i].Trim().Replace(",", ".");

                    string whereFish = getBase1(nameFish);

                    
                    //MessageBox.Show(whereFish.ToString());
                    

                    dataGridView1.Rows.Add(); // Создаем пустую запись в таблице
                    dataGridView1.Rows[i].Cells[0].Value = nameFish; // Пишем название рыбы
                    dataGridView1.Rows[i].Cells[1].Value = rgx.Matches(minWeight)[0]; // Вес от
                    dataGridView1.Rows[i].Cells[2].Value = rgx.Matches(minWeight)[1]; // Вес До
                    dataGridView1.Rows[i].Cells[3].Value = whereFish.Split(':')[0]; // База
                    dataGridView1.Rows[i].Cells[4].Value = whereFish.Split(':')[1]; // База
                    dataGridView1.Rows[i].Cells[5].Value = whereFish.Split(':')[2]; // База
                    //dataGridView1.Rows[i].Cells[3].Value = baseFish; // Вес До
                    

                }
                

            }
            else
            {
                //Выводим сообщение о том, что в буфере обмена нет текста
                MessageBox.Show(this, "В буфере обмена нет текста", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            lbStatusText.Text = (dataGridView1.Rows.Count - 1).ToString();



        }

        private string getBase1(string nameFish)
        {
            // Получаем где ловить
           string result = String.Empty;

            const string databaseName = @"DataBase.db";
            SQLiteConnection connection =
            new SQLiteConnection(string.Format("Data Source={0};", databaseName));
            connection.Open();

            const string databaseName2 = @"DataBase.db";
            SQLiteConnection connection2 =
            new SQLiteConnection(string.Format("Data Source={0};", databaseName2));
            connection2.Open();



            SQLiteCommand command = new SQLiteCommand($"SELECT BasesFishCont.CountFish, Fishes.Name, Bases.Name, BasesFishCont.MinWeight, BasesFishCont.MaxWeight, Locations.Name, Bait.Name FROM  Bait, Locations, Bases, Fishes, BasesFishCont  WHERE BasesFishCont.IDCatchBait = Bait.ID AND BasesFishCont.IDCatchyLocation = Locations.ID AND BasesFishCont.IDBases = Bases.ID AND BasesFishCont.IDFish = Fishes.ID AND Fishes.Name='{nameFish}'", connection);
            SQLiteDataReader reader = command.ExecuteReader();

            SQLiteCommand command2 = new SQLiteCommand($"SELECT BasesFishCont.CountFish, Fishes.Name, Bases.Name, BasesFishCont.MinWeight, BasesFishCont.MaxWeight, Locations.Name, Bait.Name FROM  Bait, Locations, Bases, Fishes, BasesFishCont  WHERE BasesFishCont.IDCatchBait = Bait.ID AND BasesFishCont.IDCatchyLocation = Locations.ID AND BasesFishCont.IDBases = Bases.ID AND BasesFishCont.IDFish = Fishes.ID AND Fishes.Name='{nameFish}'", connection2);
            SQLiteDataReader reader2 = command2.ExecuteReader();

            int maxСountFish = 0;

            foreach (DbDataRecord record in reader)
            {

                if (maxСountFish < int.Parse($"{record[0]}"))
                    maxСountFish = int.Parse($"{record[0]}");
            }
            
            connection.Close();

            // 0 - Колличесто
            // 1 - Рыба
            // 2 - База
            // 3 - min
            // 4 - max
            // 5 - Локация
            // 6 - Наживка

            foreach (DbDataRecord record in reader2)
                if (int.Parse($"{record[0]}") == maxСountFish)
                {
                    result = $"{record[2]}:{record[5]}:{record[6]}";
                }

            connection2.Close();
            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbStatusText.Text = "0";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //
        }

        // Получаем где ловить


    }
}

