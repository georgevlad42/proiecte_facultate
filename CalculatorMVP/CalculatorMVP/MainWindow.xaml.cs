using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorMVP
{
    public partial class MainWindow : Window
    {
        private String input = "0";
        private String op = String.Empty;
        private String sign = String.Empty;
        private String mem = String.Empty;
        public MainWindow()
        {
            InitializeComponent();
            MainCalc.Text = input;
            MC_Button.IsEnabled = false;
            MR_Button.IsEnabled = false;
            if (System.IO.File.ReadAllText(@"D:\Facultate\Anul 2\Semestrul 2\Medii vizuale de programare\Resurse\dg.txt") == "true")
                DigitGrouping_Button.IsChecked = true;
            else if (System.IO.File.ReadAllText(@"D:\Facultate\Anul 2\Semestrul 2\Medii vizuale de programare\Resurse\dg.txt") == "false")
                DigitGrouping_Button.IsChecked = false;
            else
            {
                System.IO.File.WriteAllText(@"D:\Facultate\Anul 2\Semestrul 2\Medii vizuale de programare\Resurse\dg.txt", "false");
                DigitGrouping_Button.IsChecked = false;
            }
            System.Windows.Forms.NotifyIcon notification = new System.Windows.Forms.NotifyIcon();
            notification.Icon = new System.Drawing.Icon(@"D:\Facultate\Anul 2\Semestrul 2\Medii vizuale de programare\Resurse\calculator.ico");
            notification.Visible = true;
            notification.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);
        }

        private String ElimZero(String s)
        {
            while (s[s.Length - 1].Equals('0') && s.Contains('.'))
                s = s.Remove(s.Length - 1);
            if (s[s.Length - 1].Equals('.'))
                s = s.Remove(s.Length - 1);
            return s;
        }

        private void ExceptieDivZero(Boolean stare)
        {
            Percentage_Button.IsEnabled = stare;
            Sqrt_Button.IsEnabled = stare;
            Squared_Button.IsEnabled = stare;
            OneByX_Button.IsEnabled = stare;
            Divide_Button.IsEnabled = stare;
            Multiply_Button.IsEnabled = stare;
            Minus_Button.IsEnabled = stare;
            Plus_Button.IsEnabled = stare;
            ChangeSign_Button.IsEnabled = stare;
            Dot_Button.IsEnabled = stare;
            MC_Button.IsEnabled = stare;
            MR_Button.IsEnabled = stare;
            MPlus_Button.IsEnabled = stare;
            MMinus__Button.IsEnabled = stare;
            MS_Button.IsEnabled = stare;
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(MainCalc.Text);
            input = "0";
            MainCalc.Text = input;
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(MainCalc.Text);
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            if (Double.TryParse(Clipboard.GetText(), out Double i))
            {
                if (DigitGrouping_Button.IsChecked)
                {
                    MainCalc.Text = ElimZero(Convert.ToDouble(Clipboard.GetText()).ToString("N15", CultureInfo.InvariantCulture));
                    input = MainCalc.Text;
                }
                else
                {
                    MainCalc.Text = Clipboard.GetText();
                    if (MainCalc.Text.Contains(","))
                        MainCalc.Text = MainCalc.Text.Replace(",", "");
                    input = MainCalc.Text;
                }
            }
        }

        private void DigitGrouping_Click(object sender, RoutedEventArgs e)
        {
            if (DigitGrouping_Button.IsChecked)
            {
                System.IO.File.WriteAllText(@"D:\Facultate\Anul 2\Semestrul 2\Medii vizuale de programare\Resurse\dg.txt", "true");
                DigitGrouping_Button.IsChecked = true;
                C_Click(sender, e);
            }
            else
            {
                System.IO.File.WriteAllText(@"D:\Facultate\Anul 2\Semestrul 2\Medii vizuale de programare\Resurse\dg.txt", "false");
                DigitGrouping_Button.IsChecked = false;
                C_Click(sender, e);
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aw = new AboutWindow();
            aw.Procesor.Text = Environment.ProcessorCount.ToString();
            aw.ShowDialog();
        }

        private void MC_Click(object sender, RoutedEventArgs e)
        {
            mem = String.Empty;
            MC_Button.IsEnabled = false;
            MR_Button.IsEnabled = false;
        }

        private void MR_Click(object sender, RoutedEventArgs e)
        {
            if (DigitGrouping_Button.IsChecked)
                MainCalc.Text = ElimZero(Convert.ToDouble(mem).ToString("N15", CultureInfo.InvariantCulture));
            else
                MainCalc.Text = mem;
            input = "0";
        }

        private void M_Plus_Click(object sender, RoutedEventArgs e)
        {
            if (mem.Equals(String.Empty))
            {
                mem = MainCalc.Text;
                MC_Button.IsEnabled = true;
                MR_Button.IsEnabled = true;
            }
            else
                mem = Convert.ToString(Convert.ToDouble(mem) + Convert.ToDouble(MainCalc.Text));
            input = "0";
        }

        private void M_Minus_Click(object sender, RoutedEventArgs e)
        {
            if (mem.Equals(String.Empty))
            {
                if (Convert.ToDouble(MainCalc.Text) < 0)
                    mem = MainCalc.Text.Substring(1);
                else
                    mem = "-" + MainCalc.Text;
                MC_Button.IsEnabled = true;
                MR_Button.IsEnabled = true;
            }
            else
                mem = Convert.ToString(Convert.ToDouble(mem) - Convert.ToDouble(MainCalc.Text));
            input = "0";
        }

        private void MS_Click(object sender, RoutedEventArgs e)
        {
            mem = MainCalc.Text;
            input = "0";
            MC_Button.IsEnabled = true;
            MR_Button.IsEnabled = true;
        }

        private void Percentage_Click(object sender, RoutedEventArgs e)
        {
            if (op != String.Empty && sign != String.Empty)
            {
                if (DigitGrouping_Button.IsChecked)
                {
                    MainCalc.Text = ElimZero(Convert.ToDouble(Convert.ToDouble(op) * (Convert.ToDouble(MainCalc.Text) / 100.0)).ToString("N15", CultureInfo.InvariantCulture));
                    Calcul(sign, op, MainCalc.Text);
                    op = ElimZero(Convert.ToDouble(MainCalc.Text).ToString("N15", CultureInfo.InvariantCulture));
                }
                else
                {
                    MainCalc.Text = Convert.ToString(Convert.ToDouble(op) * (Convert.ToDouble(MainCalc.Text) / 100.0));
                    Calcul(sign, op, MainCalc.Text);
                    op = Convert.ToString(Convert.ToDouble(MainCalc.Text));
                }
                sign = String.Empty;
                input = "0";
                SubCalc.Text = op;
            }
        }

        private void Sqrt_Click(object sender, RoutedEventArgs e)
        {
            if (DigitGrouping_Button.IsChecked)
            {
                MainCalc.Text = ElimZero(Math.Sqrt(Convert.ToDouble(MainCalc.Text)).ToString("N15", CultureInfo.InvariantCulture));
                if (!sign.Equals(String.Empty))
                {
                    Calcul(sign, op, MainCalc.Text);
                    op = ElimZero(Convert.ToDouble(MainCalc.Text).ToString("N15", CultureInfo.InvariantCulture));
                    sign = String.Empty;
                    SubCalc.Text = op;
                }
            }
            else
            {
                MainCalc.Text = Convert.ToString(Math.Sqrt(Convert.ToDouble(MainCalc.Text)));
                if (!sign.Equals(String.Empty))
                {
                    Calcul(sign, op, MainCalc.Text);
                    op = Convert.ToString(Convert.ToDouble(MainCalc.Text));
                    sign = String.Empty;
                    SubCalc.Text = op;
                }
            }
            if(MainCalc.Text == "NaN")
            {
                MainCalc.Text = "Eroare radical";
                ExceptieDivZero(false);
                op = String.Empty;
                sign = String.Empty;
                SubCalc.Text = String.Empty;
            }
            input = "0";
        }

        private void Squared_Click(object sender, RoutedEventArgs e)
        {
            if (DigitGrouping_Button.IsChecked)
            {
                MainCalc.Text = ElimZero(Math.Pow(Convert.ToDouble(MainCalc.Text), 2).ToString("N15", CultureInfo.InvariantCulture));
                if (!sign.Equals(String.Empty))
                {
                    Calcul(sign, op, MainCalc.Text);
                    op = ElimZero(Convert.ToDouble(MainCalc.Text).ToString("N15", CultureInfo.InvariantCulture));
                    sign = String.Empty;
                    SubCalc.Text = op;
                }
            }
            else
            {
                MainCalc.Text = Convert.ToString(Math.Pow(Convert.ToDouble(MainCalc.Text), 2));
                if (!sign.Equals(String.Empty))
                {
                    Calcul(sign, op, MainCalc.Text);
                    op = Convert.ToString(Convert.ToDouble(MainCalc.Text));
                    sign = String.Empty;
                    SubCalc.Text = op;
                }
            }
            input = "0";
        }

        private void One_By_X_Click(object sender, RoutedEventArgs e)
        {
            if (MainCalc.Text == "0")
            {
                MainCalc.Text = "Nu se poate impartirea la zero!";
                ExceptieDivZero(false);
                op = String.Empty;
                sign = String.Empty;
                SubCalc.Text = String.Empty;
            }
            else if (DigitGrouping_Button.IsChecked)
            {
                MainCalc.Text = ElimZero((1.0 / Convert.ToDouble(MainCalc.Text)).ToString("N15", CultureInfo.InvariantCulture));
                if (!sign.Equals(String.Empty))
                {
                    Calcul(sign, op, MainCalc.Text);
                    op = ElimZero(Convert.ToDouble(MainCalc.Text).ToString("N15", CultureInfo.InvariantCulture));
                    sign = String.Empty;
                    SubCalc.Text = op;
                }
            }
            else
            {
                MainCalc.Text = Convert.ToString(1.0 / Convert.ToDouble(MainCalc.Text));
                if (!sign.Equals(String.Empty))
                {
                    Calcul(sign, op, MainCalc.Text);
                    op = Convert.ToString(Convert.ToDouble(MainCalc.Text));
                    sign = String.Empty;
                    SubCalc.Text = op;
                }
            }
            input = "0";
        }

        private void CE_Click(object sender, RoutedEventArgs e)
        {
            if (MainCalc.Text == "Nu se poate impartirea la zero!" || MainCalc.Text == "Eroare radical")
            {
                input = "0";
                MainCalc.Text = input;
                ExceptieDivZero(true);
                if (mem == String.Empty)
                {
                    MC_Button.IsEnabled = false;
                    MR_Button.IsEnabled = false;
                }
            }
            else
            {
                input = "0";
                MainCalc.Text = input;
            }
        }

        private void C_Click(object sender, RoutedEventArgs e)
        {
            if (MainCalc.Text == "Nu se poate impartirea la zero!" || MainCalc.Text == "Eroare radical")
            {
                input = "0";
                MainCalc.Text = input;
                ExceptieDivZero(true);
                if (mem == String.Empty)
                {
                    MC_Button.IsEnabled = false;
                    MR_Button.IsEnabled = false;
                }
            }
            else
            {
                op = String.Empty;
                SubCalc.Text = String.Empty;
                sign = String.Empty;
                input = "0";
                MainCalc.Text = input;
            }
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if ((input[0] == '-' && input.Length == 2) || input == "-0.")
                input = "0";
            else if (MainCalc.Text == "Nu se poate impartirea la zero!" || MainCalc.Text == "Eroare radical")
            {
                ExceptieDivZero(true);
                if (mem == String.Empty)
                {
                    MC_Button.IsEnabled = false;
                    MR_Button.IsEnabled = false;
                }
                input = "0";
            }
            else if (input.Length != 0)
            {
                input = input.Remove(input.Length - 1);
                if (DigitGrouping_Button.IsChecked && !input.Equals(""))
                    if (input.Contains('.') && input[input.Length - 1] != '.')
                        input = Convert.ToDouble(input).ToString("N" + (input.Substring(input.IndexOf('.') + 1).Length), CultureInfo.InvariantCulture);
                    else if (input[input.Length - 1] == '.')
                        input.Remove(input.Length - 1);
                    else
                        input = ElimZero(Convert.ToDouble(input).ToString("N15", CultureInfo.InvariantCulture));
            }
            if (input.Length == 0)
                input = "0";
            MainCalc.Text = input;
        }

        private void Calcul(String sign, String opLeft, String opRight)
        {
            switch (sign)
            {
                case "+":
                    MainCalc.Text = Convert.ToString(Convert.ToDouble(opLeft) + Convert.ToDouble(opRight));
                    break;
                case "-":
                    MainCalc.Text = Convert.ToString(Convert.ToDouble(opLeft) - Convert.ToDouble(opRight));
                    break;
                case "/":
                    MainCalc.Text = Convert.ToString(Convert.ToDouble(opLeft) / Convert.ToDouble(opRight));
                    break;
                case "*":
                    MainCalc.Text = Convert.ToString(Convert.ToDouble(opLeft) * Convert.ToDouble(opRight));
                    break;
                default:
                    break;
            }
            if (DigitGrouping_Button.IsChecked)
                MainCalc.Text = ElimZero(Convert.ToDouble(MainCalc.Text).ToString("N15", CultureInfo.InvariantCulture));
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            Choose_Operation("+");
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            Choose_Operation("-");
        }

        private void Multiply_Click(object sender, RoutedEventArgs e)
        {
            Choose_Operation("*");
        }

        private void Divide_Click(object sender, RoutedEventArgs e)
        {
            Choose_Operation("/");
        }

        private void Choose_Operation(String symbol)
        {
            if (sign.Equals(String.Empty) || SubCalc.Text.Equals(String.Empty))
            {
                sign = symbol;
                if (DigitGrouping_Button.IsChecked)
                    op = ElimZero(Convert.ToDouble(MainCalc.Text).ToString("N15", CultureInfo.InvariantCulture));
                else
                    op = Convert.ToString(Convert.ToDouble(MainCalc.Text));
                input = "0";
                SubCalc.Text = op + " " + sign;
            }
            else if (sign.Equals("/") && Convert.ToDouble(MainCalc.Text) == 0)
            {
                MainCalc.Text = "Nu se poate impartirea la zero!";
                ExceptieDivZero(false);
                op = String.Empty;
                sign = String.Empty;
                SubCalc.Text = String.Empty;
                input = "0";
            }
            else if (input.Equals("0"))
            {
                sign = symbol;
                SubCalc.Text = op + " " + sign;
            }
            else if (!sign.Equals(symbol))
            {
                Calcul(sign, op, MainCalc.Text);
                sign = symbol;
                if (DigitGrouping_Button.IsChecked)
                    op = ElimZero(Convert.ToDouble(MainCalc.Text).ToString("N15", CultureInfo.InvariantCulture));
                else
                    op = Convert.ToString(Convert.ToDouble(MainCalc.Text));
                input = "0";
                SubCalc.Text = op + " " + sign;
            }
            else
            {
                Calcul(sign, op, MainCalc.Text);
                if (DigitGrouping_Button.IsChecked)
                    op = ElimZero(Convert.ToDouble(MainCalc.Text).ToString("N15", CultureInfo.InvariantCulture));
                else
                    op = Convert.ToString(Convert.ToDouble(MainCalc.Text));
                input = "0";
                SubCalc.Text = op + " " + sign;
            }
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (sign != String.Empty && op != String.Empty)
            {
                if (sign.Equals("/") && Convert.ToDouble(MainCalc.Text) == 0)
                {
                    MainCalc.Text = "Nu se poate impartirea la zero!";
                    ExceptieDivZero(false);
                    op = String.Empty;
                    sign = String.Empty;
                    SubCalc.Text = String.Empty;
                    input = "0";
                }
                else
                {
                    Calcul(sign, op, MainCalc.Text);
                    SubCalc.Text = String.Empty;
                    input = "0";
                }
            }
            else if (MainCalc.Text == "Nu se poate impartirea la zero!" || MainCalc.Text == "Eroare radical")
            {
                input = "0";
                MainCalc.Text = input;
                ExceptieDivZero(true);
                if (mem == String.Empty)
                {
                    MC_Button.IsEnabled = false;
                    MR_Button.IsEnabled = false;
                }
            }
            else
            {
                input = "0";
                MainCalc.Text = input;
                SubCalc.Text = String.Empty;
            }
        }

        private void Change_Sign_Click(object sender, RoutedEventArgs e)
        {
            if (Double.TryParse(SubCalc.Text, out Double i))
                SubCalc.Text = String.Empty;
            if (input[0] == '-')
                input = input.Substring(1, input.Length - 1);
            else if (!input.Equals("0"))
                input = "-" + input;
            MainCalc.Text = input;
        }

        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            if (Double.TryParse(SubCalc.Text, out Double i))
                SubCalc.Text = String.Empty;
            if (!input.Contains('.'))
                input += ".";
            MainCalc.Text = input;
        }

        private void Zero_Click(object sender, RoutedEventArgs e)
        {
            Add_Digit("0");
        }

        private void One_Click(object sender, RoutedEventArgs e)
        {
            Add_Digit("1");
        }

        private void Two_Click(object sender, RoutedEventArgs e)
        {
            Add_Digit("2");
        }

        private void Three_Click(object sender, RoutedEventArgs e)
        {
            Add_Digit("3");
        }

        private void Four_Click(object sender, RoutedEventArgs e)
        {
            Add_Digit("4");
        }

        private void Five_Click(object sender, RoutedEventArgs e)
        {
            Add_Digit("5");
        }

        private void Six_Click(object sender, RoutedEventArgs e)
        {
            Add_Digit("6");
        }

        private void Seven_Click(object sender, RoutedEventArgs e)
        {
            Add_Digit("7");
        }

        private void Eight_Click(object sender, RoutedEventArgs e)
        {
            Add_Digit("8");
        }

        private void Nine_Click(object sender, RoutedEventArgs e)
        {
            Add_Digit("9");
        }

        private void Add_Digit(String nr)
        {
            if (Double.TryParse(SubCalc.Text, out Double i))
                SubCalc.Text = String.Empty;
            if (MainCalc.Text == "Nu se poate impartirea la zero!" || MainCalc.Text == "Eroare radical")
            {
                input = "0";
                ExceptieDivZero(true);
                if (mem == String.Empty)
                {
                    MC_Button.IsEnabled = false;
                    MR_Button.IsEnabled = false;
                }
            }
            if (input.Equals("0"))
            {
                if (nr != "0")
                    input = nr;
            }
            else if (input.Count(c => Char.IsDigit(c)) < 15)
            {
                if (DigitGrouping_Button.IsChecked)
                {
                    if (nr.Equals("0") && input.Contains('.'))
                        input += nr;
                    else
                        input = ElimZero(Convert.ToDouble(input + nr).ToString("N15", CultureInfo.InvariantCulture));
                }
                else
                    input += nr;
            }
            MainCalc.Text = input;
        }

    }
}