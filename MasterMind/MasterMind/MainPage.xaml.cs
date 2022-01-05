using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MasterMind
{
    public partial class MainPage : ContentPage
    {
        int ROW_GAME_BOARD = 11;
        int COLUMN_GAME_BOARD = 5;
        int ROW_HINT_GRID = 2;
        int COLUMN_HINT_GRID = 2;

        int _currentRow = 1;
        int styleId = 11;

        int[] _sequence = new int[4];

        Color originalColour = Color.Gray;
        Color textColour = Color.Black;
        Color newColour;

        Button _currentButton;
        Random random;
        public MainPage()
        {
            InitializeComponent();
        }


        private void BtnStartGame_Clicked(object sender, EventArgs e)
        {
            CreateGameBoard();
            CreateColorGrid();
            GrdColorBoard.IsVisible = true; 
            GrdBoard.IsVisible = true;
            

            foreach (var btn in GrdBoard.Children)
            {
                if(btn.StyleId==_currentRow.ToString())
                {
                    btn.IsEnabled = true;
                }
            }
            
        }
        private void CreateGameBoard()
        {
            Button b;
            BoxView d;
            Grid GrdHints;
            // create a tap gesture recogniser
            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            t.Tapped += T_Tapped;
            for (int i = 0; i < ROW_GAME_BOARD; i++) // for loop to create the box views which will be resemble the holes in the mastermind board
            {
                
                for (int j = 0; j < COLUMN_GAME_BOARD; j++)
                {
                    if (j < 4)
                    {
                        b = new Button();
                        b.BackgroundColor = originalColour;
                        b.CornerRadius = 20;
                        b.SetValue(Grid.RowProperty, i);
                        b.SetValue(Grid.ColumnProperty, j);
                        b.GestureRecognizers.Add(t);
                        b.StyleId = styleId.ToString();
                        b.IsEnabled = false;
                        GrdBoard.Children.Add(b);
                        

                        if (i == 0) // if statement disables the buttons on the top row so they cant be pressed as they will contain the answer
                        {
                            b = new Button();
                            b.BackgroundColor = originalColour;
                            b.CornerRadius = 20;
                            b.SetValue(Grid.RowProperty, i);
                            b.SetValue(Grid.ColumnProperty, j);
                            b.Text = "?";
                            b.IsEnabled = false;
                            GrdBoard.Children.Add(b);
                        }// end of if statement
                    }

                    else if (i > 0 && j == 4)
                    {
                        GrdHints = new Grid();
                        GrdHints.SetValue(Grid.RowProperty, i);
                        GrdHints.SetValue(Grid.ColumnProperty, 4);
                        for (int x = 0; x < ROW_HINT_GRID; x++)
                        {
                            GrdHints.RowDefinitions.Add(new RowDefinition());

                        }
                        for (int y = 0; y < COLUMN_HINT_GRID; y++)
                        {
                            GrdHints.ColumnDefinitions.Add(new ColumnDefinition());
                        }
                        GrdBoard.Children.Add(GrdHints);

                        for (int a = 0; a < ROW_HINT_GRID; a++)
                        {
                            for (int c = 0; c < COLUMN_HINT_GRID; c++)
                            {
                                d = new BoxView();
                                d.BackgroundColor = originalColour;
                                d.CornerRadius = 20;
                                d.SetValue(Grid.RowProperty, a);
                                d.SetValue(Grid.ColumnProperty, c);
                                GrdHints.Children.Add(d);
                            }

                        }
                    }// end of if statement
                }// end of column for loop
                styleId--;
            }// end of row for loop


        }// end of CreateGameBoard()

        private void CreateColorGrid()
        {
            int numRows = 2;
            int numCols = 4;
            int styleId = 0;
            Button b;

            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            t.Tapped += SelectColor;

            for (int x = 0; x < numRows; x++)
            {
                GrdColorBoard.RowDefinitions.Add(new RowDefinition());
            }// end of loop

            for (int y = 0; y < numCols; y++)
            {
                GrdColorBoard.ColumnDefinitions.Add(new ColumnDefinition());
            }// end of loop

            for (int i = 0; i < numRows; i++)
            {
                for(int j = 0; j < numCols; j++)
                {
                    b = new Button();
                    b.CornerRadius = 20;
                    b.SetValue(Grid.RowProperty, i);
                    b.SetValue(Grid.ColumnProperty, j);
                    b.StyleId = styleId.ToString();
                    b.BackgroundColor =  SetColor(b);
                    b.GestureRecognizers.Add(t);
                    GrdColorBoard.Children.Add(b);
                    styleId++;
                }
            }// end of for loop

            
        }

        private void SelectColor(object sender, EventArgs e)
        {
            Button currB = (Button)sender;
            
            newColour = currB.BackgroundColor;
        }

        private void T_Tapped(object sender, EventArgs e)
        {
            Button currB = (Button)sender;
            _currentButton = currB;
            currB.BackgroundColor = newColour;
            BtnCheck.IsVisible = true;
        }

        private void ChooseSequence()
        {
            if (random == null) random = new Random();

            _sequence[0] = random.Next(8);
            _sequence[1] = random.Next(8);
            _sequence[2] = random.Next(8);
            _sequence[3] = random.Next(8);
            if (_sequence[0] == _sequence[1] || _sequence[0] == _sequence[2] || _sequence[0] == _sequence[3])
            {
                _sequence[0] = random.Next(8);
            }
            else if (_sequence[1] == _sequence[2] || _sequence[1] == _sequence[3])
            {
                _sequence[1] = random.Next(8);
            }
            else if (_sequence[2] == _sequence[3])
            {
                _sequence[2] = random.Next(8);
            }

        }

        private Color SetColor(Button b)
        {
            Button currB = b;
            Color colour = Color.Gray;
            int colourNum;
            colourNum = Convert.ToInt32(currB.StyleId);
            switch (colourNum)
            {
                case 0:
                    colour = Color.Red;
                    break;

                case 1:
                    colour = Color.Green;
                    break;

                case 2:
                    colour = Color.Blue;
                    break;

                case 3:
                    colour = Color.Yellow;
                    break;

                case 4:
                    colour = Color.Brown;
                    break;

                case 5:
                    colour = Color.Orange;
                    break;

                case 6:
                    colour = Color.Black;
                    break;

                case 7:
                    colour = Color.White;
                    break;

            }// end of switch 
            return colour;
        }

        private void BtnCheck_Clicked(object sender, EventArgs e)
        {
            _currentRow++;
            foreach (var btn in GrdBoard.Children)
            {
                if (btn.StyleId == _currentRow.ToString())
                {
                    btn.IsEnabled = true;
                }
            }
        }

        //private void EnableRow()
        //{
        //    if (_currentButton.StyleId.Contains(_currentRow.ToString()))
        //    {
        //        for(int i = _buttonNum; i < 4; i++)
        //        {
        //            F
        //        }
        //    }
        //}
    }
}
