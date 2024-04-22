using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Media;
namespace Sudoku
{
    public partial class MainWindow : Window
    {
        int[,] sudoku = new int[9, 9];
        private bool programmaticChange = false;
        int[] arrayToCompare = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public MainWindow()
        {
            InitializeComponent();
            int i, j;
            Generate(sudoku);
            while (!Check(sudoku))
            {
                Generate(sudoku);
            }
            for (i = 0; i < 9; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.HorizontalAlignment = HorizontalAlignment.Center;
                    textBox.VerticalAlignment = VerticalAlignment.Center;
                    textBox.BorderThickness = new Thickness(0);
                    textBox.BorderBrush = Brushes.Transparent;
                    textBox.MaxLength = 1;
                    textBox.FontSize = 23;
                    Border cellBorder = new Border();
                    if (sudoku[i, j] == 0)
                    {
                        textBox.Text = "";
                        cellBorder.Background = Brushes.LightGray;
                        textBox.Background = Brushes.LightGray;
                    }
                    else
                    {
                        textBox.Text = $"{sudoku[i, j]}";
                        cellBorder.Background = Brushes.DarkGray;
                        textBox.Background = Brushes.DarkGray;
                        textBox.FontWeight = FontWeights.Bold;
                        textBox.IsReadOnly = true;
                    }
                    textBox.TextChanged += TextBox_TextChanged;
                    textBox.MouseEnter += TextBox_MouseEnter;
                    textBox.MouseLeave += TextBox_MouseLeave;
                    cellBorder.BorderBrush = Brushes.Black;
                    Thickness t = new Thickness(1, 1, 1, 1);
                    //cellBorder.BorderThickness = new Thickness(1,1,1,1);

                    if (i == 0)
                    {
                        t.Top = 4;
                    }
                    if (j == 0)
                    {
                        t.Left = 4;
                    }
                    if (j == 8 || j == 2 || j == 5)
                    {
                        t.Right = 4;
                    }
                    if (i == 8 || i == 2 || i == 5)
                    {
                        t.Bottom = 4;
                    }
                    cellBorder.BorderThickness = t;
                    cellBorder.Child = textBox;
                    Grid.SetRow(cellBorder, i);
                    Grid.SetColumn(cellBorder, j);
                    Grid1.Children.Add(cellBorder);
                }
            }

        }
        public void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (programmaticChange)
            {
                programmaticChange = false;
                return;
            }
            Border cellBorder = FindParent<Border>(textBox);
            if (textBox.Text == " " || textBox.Text == "")
            {
                sudoku[Grid.GetRow(cellBorder), Grid.GetColumn(cellBorder)] = 0;
                textBox.Background = Brushes.LightGray;
                cellBorder.Background = Brushes.LightGray;
                for (int i = 0; i < 9; i++)
                {
                    Border cellBorder2 = GetCellBorder(i, Grid.GetColumn(cellBorder));
                    TextBox textBox2 = (TextBox)cellBorder2.Child;
                    if (!PositionCheck(i, Grid.GetColumn(cellBorder), sudoku) && !(textBox2.IsReadOnly))
                    {
                        cellBorder2.Background = Brushes.Red;
                        textBox2.Background = Brushes.Red;
                    }
                    else if (!(textBox2.IsReadOnly))
                    {
                        cellBorder2.Background = Brushes.LightGray;
                        textBox2.Background = Brushes.LightGray;
                    }
                    cellBorder2 = GetCellBorder(Grid.GetRow(cellBorder), i);
                    textBox2 = (TextBox)cellBorder2.Child;
                    if (!PositionCheck(Grid.GetRow(cellBorder), i, sudoku) && !(textBox2.IsReadOnly))
                    {
                        cellBorder2.Background = Brushes.Red;
                        textBox2.Background = Brushes.Red;
                    }
                    else if (!(textBox2.IsReadOnly))
                    {
                        cellBorder2.Background = Brushes.LightGray;
                        textBox2.Background = Brushes.LightGray;
                    }
                }
                int a1, b1;
                if (Grid.GetRow(cellBorder) < 3 && Grid.GetRow(cellBorder) < 6) a1 = 0;
                else if (Grid.GetRow(cellBorder) >= 3 && Grid.GetRow(cellBorder) < 6) a1 = 3;
                else a1 = 6;
                if (Grid.GetColumn(cellBorder) < 3 && Grid.GetColumn(cellBorder) < 6) b1 = 0;
                else if (Grid.GetColumn(cellBorder) >= 3 && Grid.GetColumn(cellBorder) < 6) b1 = 3;
                else b1 = 6;
                for (int i = a1; i < a1 + 3; i++)
                {
                    for (int j = b1; j < b1 + 3; j++)
                    {
                        Border cellBorder2 = GetCellBorder(i, j);
                        TextBox textBox2 = (TextBox)cellBorder2.Child;
                        if (!PositionCheck(i, j, sudoku) && !(textBox2.IsReadOnly))
                        {
                            cellBorder2.Background = Brushes.Red;
                            textBox2.Background = Brushes.Red;
                        }
                        else if (!(textBox2.IsReadOnly))
                        {
                            cellBorder2.Background = Brushes.LightGray;
                            textBox2.Background = Brushes.LightGray;
                        }
                    }
                }
            }
            else if (int.TryParse(textBox.Text, out int value) && value >= 1 && value <= 9)
            {
                sudoku[Grid.GetRow(cellBorder), Grid.GetColumn(cellBorder)] = value;
                if (!PositionCheck(Grid.GetRow(cellBorder), Grid.GetColumn(cellBorder), sudoku))
                {
                    textBox.Background = Brushes.Red;
                    cellBorder.Background = Brushes.Red;
                }
                else
                {
                    textBox.Background = Brushes.LightGray;
                    cellBorder.Background = Brushes.LightGray;
                }
                for (int i = 0; i < 9; i++)
                {
                    Border cellBorder2 = GetCellBorder(i, Grid.GetColumn(cellBorder));
                    TextBox textBox2 = (TextBox)cellBorder2.Child;
                    if (!PositionCheck(i, Grid.GetColumn(cellBorder), sudoku) && !(textBox2.IsReadOnly))
                    {
                        cellBorder2.Background = Brushes.Red;
                        textBox2.Background = Brushes.Red;
                    }
                    else if (!(textBox2.IsReadOnly))
                    {
                        cellBorder2.Background = Brushes.LightGray;
                        textBox2.Background = Brushes.LightGray;
                    }
                    cellBorder2 = GetCellBorder(Grid.GetRow(cellBorder), i);
                    textBox2 = (TextBox)cellBorder2.Child;
                    if (!PositionCheck(Grid.GetRow(cellBorder), i, sudoku) && !(textBox2.IsReadOnly))
                    {
                        cellBorder2.Background = Brushes.Red;
                        textBox2.Background = Brushes.Red;
                    }
                    else if (!(textBox2.IsReadOnly))
                    {
                        cellBorder2.Background = Brushes.LightGray;
                        textBox2.Background = Brushes.LightGray;
                    }
                }
                int a1, b1;
                if (Grid.GetRow(cellBorder) < 3 && Grid.GetRow(cellBorder) < 6) a1 = 0;
                else if (Grid.GetRow(cellBorder) >= 3 && Grid.GetRow(cellBorder) < 6) a1 = 3;
                else a1 = 6;
                if (Grid.GetColumn(cellBorder) < 3 && Grid.GetColumn(cellBorder) < 6) b1 = 0;
                else if (Grid.GetColumn(cellBorder) >= 3 && Grid.GetColumn(cellBorder) < 6) b1 = 3;
                else b1 = 6;
                for (int i = a1; i < a1 + 3; i++)
                {
                    for (int j = b1; j < b1 + 3; j++)
                    {
                        Border cellBorder2 = GetCellBorder(i, j);
                        TextBox textBox2 = (TextBox)cellBorder2.Child;
                        if (!PositionCheck(i, j, sudoku) && !(textBox2.IsReadOnly))
                        {
                            cellBorder2.Background = Brushes.Red;
                            textBox2.Background = Brushes.Red;
                        }
                        else if (!(textBox2.IsReadOnly))
                        {
                            cellBorder2.Background = Brushes.LightGray;
                            textBox2.Background = Brushes.LightGray;
                        }
                    }
                }
            }
            else
            {
                // Display an error message directly on the window or provide visual feedback.
                MessageBox.Show("Value should be between 1 and 9", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                // Set the flag to true to prevent the handler from running again for programmatic changes.
                programmaticChange = true;
                sudoku[Grid.GetRow(textBox), Grid.GetColumn(textBox)] = 0;
                textBox.Text = "";
            }
            if (FinalCheck(sudoku))
            {
                FinalWindow finalWindow = new FinalWindow { PromptMessage = "Congratulations!Sudoku Completed" };
                bool? result = finalWindow.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    MainWindow newMainWindow = new MainWindow();
                    Close();
                    newMainWindow.Show();
                }
                else
                {
                    Close();
                }
            }
        }
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            T parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }
        public void Generate(int[,] sudoku)
        {
            Random random = new Random();
            int i, j;
            for (i = 0; i < 9; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    sudoku[i, j] = 0;
                }
            }
            for (i = 0; i + 2 < 9; i = i + 2)
            {
                for (j = 0; j + 2 < 9; j = j + 2)
                {
                    sudoku[random.Next(i, i + 3), random.Next(j, j + 3)] = random.Next(1, 10);
                }
            }
        }
        private Border GetCellBorder(int row, int column)
        {
            foreach (var child in Grid1.Children)
            {
                if (child is Border cellBorder && Grid.GetRow(cellBorder) == row && Grid.GetColumn(cellBorder) == column)
                {
                    return cellBorder;
                }
            }

            // Return null if the specified cell was not found
            return null;
        }

        public Boolean Check(int[,] sudoku)
        {
            ArrayList a = new ArrayList();
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    if (sudoku[i, j] == 0) continue;
                    if (!a.Contains(sudoku[i, j]))
                    {
                        a.Add(sudoku[i, j]);
                    }
                    else
                    {
                        return false;
                    }
                }
                a.Clear();
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    if (sudoku[j, i] == 0) continue;
                    if (!a.Contains(sudoku[j, i]))
                    {
                        a.Add(sudoku[j, i]);
                    }
                    else
                    {
                        return false;
                    }
                }
                a.Clear();
            }

            for (int i = 0; i < 9; i = i + 3)
            {
                for (int j = 0; j < 9; j = j + 3)
                {
                    a.Clear();
                    for (int k = 0; k <= 2; k++)
                    {
                        if (!a.Contains(sudoku[i + k, j]))
                        {
                            if (sudoku[i + k, j] != 0)
                            {
                                a.Add(sudoku[i + k, j]);
                            }
                        }
                        else return false;
                        if (!a.Contains(sudoku[i + k, j + 1]))
                        {
                            if (sudoku[i + k, j + 1] != 0)
                            {
                                a.Add(sudoku[i + k, j + 1]);
                            }
                        }
                        else return false;
                        if (!a.Contains(sudoku[i + k, j + 2]))
                        {
                            if (sudoku[i + k, j + 2] != 0)
                            {
                                a.Add(sudoku[i + k, j + 2]);
                            }
                        }
                        else return false;
                    }
                }
            }
            return true;
        }
        public bool ArraysAreEqual(ArrayList arrayList, int[] array)
        {
            if (arrayList.Count != array.Length)
            {
                return false;
            }

            for (int i = 0; i < arrayList.Count; i++)
            {
                if (!arrayList[i].Equals(array[i]))
                {
                    return false;
                }
            }

            return true;
        }
        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Border cellBorder = FindParent<Border>(textBox);

            if (!(textBox.IsReadOnly))
            {
                textBox.Background = Brushes.White;
                cellBorder.Background = Brushes.White;
            }

        }
        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Border cellBorder = FindParent<Border>(textBox);
            if (!PositionCheck(Grid.GetRow(cellBorder), Grid.GetColumn(cellBorder), sudoku) && !(textBox.IsReadOnly))
            {
                textBox.Background = Brushes.Red;
                cellBorder.Background = Brushes.Red;
            }
            else if (!(textBox.IsReadOnly))
            {
                textBox.Background = Brushes.LightGray;
                cellBorder.Background = Brushes.LightGray;
            }
        }

        public Boolean PositionCheck(int a, int b, int[,] sudoku)
        {
            // ArrayList n = new ArrayList();
            int value = sudoku[a, b];
            for (int i = 0; i < 9; i++)
            {
                if (sudoku[i, b] == 0 || i == a)
                {
                    continue;
                }
                else if (sudoku[i, b] == value)
                {
                    return false;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (sudoku[a, i] == 0 || i == b)
                {
                    continue;
                }
                else if (sudoku[a, i] == value)
                {
                    return false;
                }
            }

            int a1, b1;
            if (a < 3 && a < 6) a1 = 0;
            else if (a >= 3 && a < 6) a1 = 3;
            else a1 = 6;
            if (b < 3 && b < 6) b1 = 0;
            else if (b >= 3 && b < 6) b1 = 3;
            else b1 = 6;
            for (int i = a1; i < a1 + 3; i++)
            {
                for (int j = b1; j < b1 + 3; j++)
                {
                    if (sudoku[i, j] == 0 || (i == a && j == b))
                    {
                        continue;
                    }
                    else if (sudoku[i, j] == value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public Boolean FinalCheck(int[,] sudoku)
        {
            ArrayList a = new ArrayList();
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    if (!a.Contains(sudoku[i, j]))
                    {
                        a.Add(sudoku[i, j]);
                    }
                    else
                    {
                        return false;
                    }
                }
                a.Sort();
                if (!ArraysAreEqual(a, arrayToCompare))
                {
                    return false;
                }
                a.Clear();
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {

                    if (!a.Contains(sudoku[j, i]))
                    {
                        a.Add(sudoku[j, i]);
                    }
                    else
                    {
                        return false;
                    }
                }
                a.Sort();
                if (!ArraysAreEqual(a, arrayToCompare))
                {
                    return false;
                }
                a.Clear();
            }
            for (int i = 0; i < 9; i = i + 3)
            {
                for (int j = 0; j < 9; j = j + 3)
                {
                    for (int k = 0; k <= 2; k++)
                    {
                        if (!a.Contains(sudoku[i + k, j]))
                        {
                            if (sudoku[i + k, j] != 0)
                            {
                                a.Add(sudoku[i + k, j]);
                            }
                        }
                        else return false;
                        if (!a.Contains(sudoku[i + k, j + 1]))
                        {
                            if (sudoku[i + k, j + 1] != 0)
                            {
                                a.Add(sudoku[i + k, j + 1]);
                            }
                        }
                        else return false;
                        if (!a.Contains(sudoku[i + k, j + 2]))
                        {
                            if (sudoku[i + k, j + 2] != 0)
                            {
                                a.Add(sudoku[i + k, j + 2]);
                            }
                        }
                        else return false;
                    }
                    a.Sort();
                    if (!ArraysAreEqual(a, arrayToCompare))
                    {
                        return false;
                    }
                    a.Clear();
                }
            }
            return true;
        }
    }
}