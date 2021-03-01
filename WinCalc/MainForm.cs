using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinCalc
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Поле для хранения значения индикатора
        /// </summary>
        private double _x;
        /// <summary>
        /// Значение индикатора
        /// </summary>

        private Operation operation = Operation.None;
        private double y = 0;
        private bool newNumber = true;
        private bool dotPressed = false;
        private int factor;
        private double x
        {
            get
            {
                return _x;
            }
            set
            {
                // Сохранение значения
                _x = value;
                /*Строка формата для преобразования числа
                в строковое представление */
                string format = string.Empty;
                if (dotPressed)
                {
                    if (factor == 0)
                    {
                        format = "0\\.";
                    }
                    else
                    {
                        format = "0.";
                        /*Формируем строку вида 0.000 в соответствии
                         с количеством десятичных цифр */
                        for (int i = 0; i < factor; i++)
                        {
                            format += "0";
                        }
                    }
                }
                indicator.Text = _x.ToString
                    (format, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        public MainForm()
        {
            InitializeComponent();
            x = 0;
        }
        /// <summary>
        /// Обработчик нажатия на кнопку
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        private void buttonDigit_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                string tag = (string)b.Tag;
                double digit = double.Parse(tag);
                if (newNumber)
                {
                    x = digit;
                    newNumber = false;
                }
                else if (dotPressed)
                {
                    //Ввод дробной части числа
                    x += digit / Math.Pow(10, ++factor);
                }
                else
                {
                    //Ввод целой части числа
                    x = 10 * x + digit;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonOperation_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                string tag = (string)b.Tag;
                Enum.TryParse(tag, out operation);
                y = x;
                newNumber = true;
                dotPressed = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (operation)
                {
                    case Operation.Addition:
                        x = y + x;
                        break;
                    case Operation.Multiplication:
                        x = y * x;
                        break;
                    case Operation.Division:
                        x = y / x;
                        break;
                    case Operation.Substraction:
                        x = y - x;
                        break;
                }
                newNumber = true;
                dotPressed = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(indicator.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    if (double.TryParse(Clipboard.GetText(),
                        System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out double value)
                        )
                    {
                        x = value;
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDot_Click(object sender, EventArgs e)
        {
            if (!dotPressed)
            {
                dotPressed = true;
                factor = 0;
                x = x;
            }

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            indicator.Text = string.Empty;
            x = 0;
            y = 0;
        }
    }
}
