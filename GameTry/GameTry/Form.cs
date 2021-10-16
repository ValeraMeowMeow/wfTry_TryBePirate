using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTry
{
    public partial class Form : System.Windows.Forms.Form
    {
        // уровни по времени. корабль - тонет. остров - армия
        // 1 - 3х3. 2 - 3х4. 3 - 3х5. 4 - 4х4
        // 1-3 - корабль. 4 - остров
        // 1 - ключи. 2 - лопата. 3 - карта. 4 - сабля
        // 
        // таймер
        private const int SEC_MAX = 300;
        private DateTime startTimerUp;
        // скорость передвижения
        private int speed = 10;
        // начальное положение
        (int x, int y) PlayerLocation = (30, 30);
        // строка
        private int ii = 0;
        // столбец
        private int ij = 0;
        // уровень
        private int il = 1;
        // размер карты
        private static int n = 3;
        private static int m = 3;
        private bool[,] map = new bool[n, m];
        // расположение двери
        private int doori;
        private int doorj;
        Random rand = new Random();
        // инвентарь - ключ - лопата - карта - сабля - сокровища
        private bool[] inventory = new bool[5] { false, false, false, false, false };
        // сундуки
        private int ichest = 3;
        private string[] riches = new string[12] { "Key!", "Saber", "Map", "Shovel", "Papeeer", "Nothing...", "Nothing...", 
                                                   "Rom :D", "Nothing...", "Nothing... Nothing... Nothing", "Nothing... Nothing... Nothing", "Nothing..." };
        private bool[] richesbool = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

        public Form()
        {
            InitializeComponent();
            this.Text = "Try";
            this.Icon = Properties.Resources.Icon;
            this.MinimumSize = this.MaximumSize = this.Size;

            //pictureBox1.Location = new Point(PlayerLocation.x, PlayerLocation.y);
            //this.KeyDown += Form_KeyDown;

            pictureBox1.Image = Properties.Resources.pirat;
            pictureBox2.Image = Properties.Resources.chest;
            pictureBox3.Image = Properties.Resources.door;
            panel1.BackgroundImage = Properties.Resources.ship;

            Launch();
            //CountChest();

            this.KeyDown += Form_KeyDown;

            verticalProgressBar1.Maximum = SEC_MAX;
        }

        // Загрузка приложения
        private void Launch()
        {
            panel1.Visible = false;
            panel1.Enabled = false;
            panel2.Visible = false;
            panel2.Enabled = false;
            panel3.Visible = true;
            panel3.Enabled = true;
            panel4.Visible = false;
            panel4.Enabled = false;
        }

        // Взаимодействие с кнопками
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    GoUp();
                    break;
                case Keys.S:
                    GoDown();
                    break;
                case Keys.D:
                    GoRight();
                    break;
                case Keys.A:
                    GoLeft();
                    break;
                case Keys.Escape:
                    ToMenu();
                    break;
                case Keys.E:
                    OpenChest();
                    break;
                case Keys.Q:
                    OpenDoor();
                    break;
                default:
                    break;
            }
        }

        // Количество сундуков - рандом
        /*private void CountChestRandom()
        {
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        map[i, j] = false;
                        break;
                    }
                    map[i, j] = rand.Next(2) == 0 ? false : true;
                    //map[i, j] = rand.NextBool();
                    if (map[i, j] == true)
                        count++;
                    //label1.Text += map[i,j].ToString();
                }
            }
            // на всякий случай
            if (count == 0)
                CountChest();
            label1.Text = count.ToString();
        }*/

        // Количество сундуков - задано
        private void CountChest()
        {
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    map[i, j] = false;
                }
            }
            for (int i = 0; i < ichest; i++)
            {
                map[rand.Next(0, n), rand.Next(0, m)] = true;
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (map[i, j] == true && i != 0 && j != 0)
                        count++;
                }
            }
            if (count != ichest)
                CountChest();
            // проверка
            //label1.Text = count.ToString();
        }

        // Расстановка сундуков
        private void CreateChest()
        {
            if (il == 4)
            {
                CreateChestEnd();
            }
            else
            {
                if (map[ii, ij] == true)
                {
                    pictureBox2.Visible = true;
                    pictureBox2.Enabled = true;
                }
                else
                {
                    pictureBox2.Visible = false;
                    pictureBox2.Enabled = false;
                }
            }
        }

        // Положение сундука сокровищ
        private void CreateChestEnd()
        {
            if (map[ii, ij] == true && inventory[2] == true)
            {
                pictureBox2.Visible = true;
                pictureBox2.Enabled = true;
            }
            else
            {
                pictureBox2.Visible = false;
                pictureBox2.Enabled = false;
            }
        }

        // Создание положения двери
        private void Door()
        {
            doori = rand.Next(n - 1);
            doorj = rand.Next(m - 1);
            if (doori == 0 && doorj == 0)
                Door();
        }

        // Визуализация двери
        private void CreateDoor()
        {
            if (ii == doori && ij == doorj)
            {
                pictureBox3.Visible = true;
                pictureBox3.Enabled = true;
            }
            else
            {
                pictureBox3.Visible = false;
                pictureBox3.Enabled = false;
            }
        }

        /* Взаимодействие с кнопками - начало */
        private void GoLeft()
        {
            //pictureBox1.Left -= speed;
            if ((pictureBox1.Location.X > 0 && ij == 0) || (ij > 0))
                pictureBox1.Left -= speed;
            if (pictureBox1.Location.X <= -20 && ij > 0)
            {
                pictureBox1.Location = new Point(720, pictureBox1.Location.Y);
                ij--;
                //label1.Text = ii + " " + ij;
            }
            CreateChest();
            CreateDoor();
        }

        private void GoRight()
        {
            if ((pictureBox1.Location.X < 720 && ij == m - 1) || (ij < m - 1))
                pictureBox1.Left += speed;
            if (pictureBox1.Location.X > 720 && ij < m - 1)
            {
                pictureBox1.Location = new Point(-10, pictureBox1.Location.Y);
                ij++;
                //label1.Text = ii + " " + ij;
            }
            CreateChest();
            CreateDoor();
        }

        private void GoDown()
        {
            //Death.Bottom -= speed;
            //if (pictureBox1.Location.Y <= 570)
            if ((pictureBox1.Location.Y < 390 && ii == n - 1) || (ii < n - 1))
                pictureBox1.Top += speed;
            if (pictureBox1.Location.Y >= 420 && ii < n - 1)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, 0);
                ii++;
                //label1.Text = ii + " " + ij;
            }
            CreateChest();
            CreateDoor();
        }

        private void GoUp()
        {
            //if (pictureBox1.Location.Y >= 20)
            if ((pictureBox1.Location.Y > 0 && ii == 0) || (ii > 0))
                pictureBox1.Top -= speed;
            if (pictureBox1.Location.Y <= -20 && ii > 0)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, 400);
                ii--;
                //label1.Text = ii + " " + ij;
            }
            CreateChest();
            CreateDoor();
        }

        private void ToMenu()
        {
            DialogResult result = MessageBox.Show("Are you sure want to quit the game? The entire gameplay will be lost", "Exit?",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                ExitToMenu();
            }
        }

        private void ExitToMenu()
        {
            Launch();
            il = 1;
            m = 3;
            n = 3;
            ResizeArrayStart(ref map, n, m);
            for (int i = 0; i < richesbool.Length; i++)
                richesbool[i] = false;
            for (int i = 0; i < inventory.Length; i++)
                inventory[i] = false;
            slot1.Image = null;
            slot2.Image = null;
            slot3.Image = null;
            slot4.Image = null;
        }

        private void OpenChest()
        {
            if ((il == 1 || il == 2 || il == 3) && pictureBox2.Visible == true && pictureBox2.Enabled == true && 
                pictureBox1.Location.X > 320 && pictureBox1.Location.X < 460 && pictureBox1.Location.Y > 120 && pictureBox1.Location.Y < 260)
            {
                //MessageBox.Show("Key!", "You find...", MessageBoxButtons.OK);
                ChestRandom();
                //MessageBox.Show(ChestRandom(), "You find...", MessageBoxButtons.OK);
                pictureBox2.Visible = false;
                pictureBox2.Enabled = false;
                map[ii, ij] = false;
            }
            if (il == 4 &&
                pictureBox1.Location.X > 320 && pictureBox1.Location.X < 460 && pictureBox1.Location.Y > 120 && pictureBox1.Location.Y < 260)
            {
                OpenChestEnd();
                pictureBox2.Visible = false;
                pictureBox2.Enabled = false;
                map[ii, ij] = false;
            }
        }

        private void OpenChestEnd()
        {
            if (inventory[1] == true)
            {
                MessageBox.Show("Treasure chest!", "You find...", MessageBoxButtons.OK);
                if (inventory[0] == true)
                {
                    MessageBox.Show("I'M RICH!", "YOU ARE RICH!", MessageBoxButtons.OK);
                    inventory[4] = true;
                }
                else
                {
                    MessageBox.Show("I don't have a key :c", "You don't find...", MessageBoxButtons.OK);
                }
            }
            else
            {
                // 30 сек откапывать руками
                //SEC_MAX -= 60;
            }
        }

        private void OpenDoor()
        {
            if (il == 4)
            {
                OpenDoorEnd();
            }
            if ((il == 1 || il == 2 || il == 3) && pictureBox3.Visible == true && pictureBox1.Location.X < 70 && pictureBox1.Location.Y > 330)
            {
                MessageBox.Show("Door!", "New level...", MessageBoxButtons.OK);
                il++;
                pictureBox2.Enabled = false;
                pictureBox2.Visible = false;
                pictureBox3.Enabled = false;
                pictureBox3.Visible = false;
                NewLevel();
            }
        }

        private void OpenDoorEnd()
        {
            MessageBox.Show("You survived", "The End", MessageBoxButtons.OK);
            if (inventory[4] == true)
            {
                MessageBox.Show("You are win! :D", "The End", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("You don't lose c:", "The End", MessageBoxButtons.OK);
            }
            Launch();
            ExitToMenu();
        }
        /* Взаимодействие с кнопками - конец */

        // Кнопка выхода
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Кнопка открытия правил
        private void button2_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            panel4.Enabled = true;
        }

        // Кнопка закрытия правил
        private void button4_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel4.Enabled = false;
        }

        // Кнопка начала игры
        private void button1_Click(object sender, EventArgs e)
        {
            NewLevel();
            startTimerUp = DateTime.Now;
            timer1.Start();
        }

        // CTRL + K + C - закомментировать
        // CTRL + K + U - раскомментировать

        // Кнопка выхода в меню из игры по ESC
        // Button и движение по кнопкам не работают вместе :с
        //private void button5_Click(object sender, EventArgs e)
        //{
        //    DialogResult result = MessageBox.Show("Are you sure want to quit the game? The entire gameplay will be lost", "Exit?",
        //                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //    if (result == DialogResult.Yes)
        //    {
        //        Launch();
        //        il = 1;
        //    }
        //}

        // 
        private void NewLevel()
        {
            panel1.Visible = true;
            panel1.Enabled = true;
            panel2.Visible = true;
            panel2.Enabled = true;
            panel3.Visible = false;
            panel3.Enabled = false;
            LevelSelection();
            CountChest();
            Door();
            pictureBox1.Location = new Point(PlayerLocation.x, PlayerLocation.y);
        }

        // Размер поля от уровня
        private void LevelSelection()
        {   
            ii = 0;
            ij = 0;
            //speed = 10;
            switch(il)
            {
                case 1:
                    panel1.BackgroundImage = Properties.Resources.ship;
                    n = 3;
                    m = 3;
                    ichest = 3;
                    break;
                case 2:
                    n = 3;
                    m = 4;
                    ichest = 4;
                    break;
                case 3:
                    n = 3;
                    m = 5;
                    ichest = 5;
                    break;
                case 4:
                    panel1.BackgroundImage = Properties.Resources.beach;
                    n = 4;
                    m = 4;
                    ichest = 1;
                    break;
                default:
                    break;
            }
            ResizeArray(ref map, n, m);
        }

        // Расширение массива - увеличение
        private void ResizeArray(ref bool[,] original, int cols, int rows) 
        {
            bool[,] newArray = new bool[cols, rows];
            Array.Copy(original, newArray, original.Length);
            original = newArray;
        }
        
        // Стартовый массив - 3х3
        private void ResizeArrayStart(ref bool[,] original, int cols, int rows) 
        {
            bool[,] newArray = new bool[cols, rows];
            Array.Copy(original, newArray, newArray.Length);
            original = newArray;
        }

        // Рандом предмета из сундука
        private void ChestRandom()
        {
            int ix = rand.Next(0, 12);
            if (richesbool[ix] == true)
            {
                ChestRandom();
            }
            else
            {
                richesbool[ix] = true;
                string x = riches[ix];
                switch (x)
                {
                    case "Key!":
                        slot1.Image = Properties.Resources.key;
                        inventory[0] = true;
                        break;
                    case "Shovel":
                        slot2.Image = Properties.Resources.shovel;
                        inventory[1] = true;
                        break;
                    case "Map":
                        slot3.Image = Properties.Resources.map;
                        inventory[2] = true;
                        break;
                    case "Saber":
                        slot4.Image = Properties.Resources.saber;
                        inventory[3] = true;
                        break;
                    default:
                        break;
                }
                MessageBox.Show(x, "You find...", MessageBoxButtons.OK);
            }
        }

        // Таймер
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var x = DateTime.Now - startTimerUp;
                if (x.TotalSeconds > SEC_MAX)
                {
                    timer1.Stop();
                    x = TimeSpan.FromSeconds(SEC_MAX);
                    MessageBox.Show("You lose :c", "The End", MessageBoxButtons.OK);
                    Launch();
                    ExitToMenu();
                }
                else
                {
                    //
                }
                verticalProgressBar1.Value = (int)x.TotalSeconds;
            }
            catch { }
        }
    }
}
