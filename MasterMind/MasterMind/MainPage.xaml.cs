using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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


        string pathName;
        string fullFileName;
        

        int[] _sequence = new int[4];

        Color originalColour = Color.Gray;
        Color hintColour;
        Color newColour;

        Button _currentButton;
        Random random;

        List<Button> LstSequenceBtns = new List<Button>();
        List<Button> LstPegs = new List<Button>();
        List<Button> LstButtonsToCheck = new List<Button>();
        public MainPage()
        {
            InitializeComponent();
            CreateGameBoard();
            CreateColorGrid();
            
        }


        private void BtnStartGame_Clicked(object sender, EventArgs e)
        {
            _currentRow = 1;
            foreach (var btn in LstPegs) // goes through each of the pegs disables it and sets the background colour to original
            {
                btn.BackgroundColor = originalColour;
                btn.IsEnabled = false;
            }
            ChooseSequence();
            GrdColorBoard.IsVisible = true; 
            GrdBoard.IsVisible = true;
            newColour = originalColour;


            foreach (var btn in GrdBoard.Children)// enables the first row of buttons
            {
                if(btn.StyleId==_currentRow.ToString())
                {
                    btn.IsEnabled = true;
                }
                
            }
            foreach (var btn in LstSequenceBtns)
            {
                btn.IsVisible = false;
            }
            
        }// end of BtnStartGame_Clicked
        private void CreateGameBoard()
        {
            Button b;
            BoxView d;
            Grid GrdHints;
            // create a tap gesture recogniser for each peg
            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            t.Tapped += T_Tapped;
            for (int i = 0; i < ROW_GAME_BOARD; i++) // for loop to create the box views which will be resemble the holes in the mastermind board
            {
                
                for (int j = 0; j < COLUMN_GAME_BOARD; j++)
                {
                    if (j < 4)
                    {
                        // creating the standard buttons/pegs
                        b = new Button();
                        b.BackgroundColor = originalColour;
                        b.CornerRadius = 20;
                        b.SetValue(Grid.RowProperty, i);
                        b.SetValue(Grid.ColumnProperty, j);
                        b.GestureRecognizers.Add(t);
                        b.StyleId = styleId.ToString();
                        LstPegs.Add(b);
                        b.IsEnabled = false;
                        GrdBoard.Children.Add(b);
                        

                        if (i == 0) // if statement disables the buttons on the top row so they cant be pressed as they will contain the answer
                        {
                            // creating the top buttons to hold the sequnce
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
                        // creating the grid for the hint pegs
                        GrdHints = new Grid();
                        GrdHints.SetValue(Grid.RowProperty, i);
                        GrdHints.SetValue(Grid.ColumnProperty, 4);
                        GrdHints.StyleId = i.ToString();
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
                                // creating the box views for the hints
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
            // creating the colour palette
            int numRows = 2;
            int numCols = 4;
            int styleId = 0;
            Button b;

            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            t.Tapped += SelectColor; // this gesture recogniser allows the colour to be the colour the user wants the peg to be

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
            styleId = Convert.ToInt32(currB.StyleId);
            // uses the styleI to set the color the user wants
        }// end of SelectColor

        private void T_Tapped(object sender, EventArgs e)
        {
            // this method sets the background colour of the peg tapped to the colour of the button the user pressed down the bottom
            Button currB = (Button)sender;
            _currentButton = currB;
            currB.BackgroundColor = newColour;
            currB.StyleId = styleId.ToString();
            BtnCheck.IsVisible = true;
            LstButtonsToCheck.Add(currB);
            _currentButton = null;
        }

        private void ChooseSequence()
        {
            // uses random numbers to pick the sequnce which are converted to colours using the SetColor() method
            Button b;
            if (random == null) random = new Random();

            _sequence[0] = random.Next(8);
            _sequence[1] = random.Next(8);
            _sequence[2] = random.Next(8);
            _sequence[3] = random.Next(8);

            // commentdd code was used for testing
            //_sequence[0] = 0;
            //_sequence[1] = 1;
            //_sequence[2] = 2;
            //_sequence[3] = 3;
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

            for(int i = 0; i < 4; i++)
            {
                b = new Button();
                b.CornerRadius = 20;
                b.SetValue(Grid.RowProperty, 0);
                b.SetValue(Grid.ColumnProperty, i);
                b.StyleId = _sequence[i].ToString();
                b.BackgroundColor = SetColor(b);
                b.IsVisible = false;
                LstSequenceBtns.Add(b);
                GrdBoard.Children.Add(b);
            }
           
            
        }// end of ChooseSequence

        private Color SetColor(Button b)
        {
            // switch statment uses the styleid to convert the background colour
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

        private async void BtnCheck_Clicked(object sender, EventArgs e)
        {
            _currentRow++;
            
            foreach (var btn in GrdBoard.Children)// enables the buttons of the next row so only one row at a time can be used
            {
                if (btn.StyleId == _currentRow.ToString())
                {
                    btn.IsEnabled = true;
                   
                }
            }
            if(_currentRow == 11)// displays sequence if runs out of guesses
            {
                DisplaySequence();
            }

           await CheckSequence();// checks to see if the sequnce selected is the winning one
            

        }// end of BtnCheck_Clicked

        private void DisplaySequence()
        {
           //displays the buttons at the top of the board containing the sequence
            foreach (var button in LstSequenceBtns)
            {
                button.IsVisible = true;
            }
            
        }
        private async Task CheckSequence()
        {

            if(LstButtonsToCheck[0].StyleId == LstSequenceBtns[0].StyleId)
            {
                hintColour = Color.Red;
            }// this if statement was used to check for hint

            // this if statement checks if the selected sequence matches the winning one
            if(LstButtonsToCheck[0].StyleId == LstSequenceBtns[0].StyleId && LstButtonsToCheck[1].StyleId == LstSequenceBtns[1].StyleId
                && LstButtonsToCheck[2].StyleId == LstSequenceBtns[2].StyleId && LstButtonsToCheck[3].StyleId == LstSequenceBtns[3].StyleId)
            {
                await Win();
            }
        }// end of CheckSequence()

        private void DisplayHint()
        {
            
        }

        private async Task Win()
        {
            // this method displays the winning sequnce and disalys an alert to the user
            DisplaySequence();
            await DisplayAlert("Congratulations!", "You have guessed the correct sequence", "Continue");
            foreach (var button in GrdBoard.Children)
            {
                button.IsEnabled = false;
            }
        }

        private void BtnLoad_Clicked(object sender, EventArgs e)
        {
            //NOT COMPLETED DOESNT WORK
            _currentRow = 1;
            foreach (var btn in LstPegs)
            {
                btn.BackgroundColor = originalColour;
                btn.IsEnabled = false;
            }
            ChooseSequence();
            GrdColorBoard.IsVisible = true;
            GrdBoard.IsVisible = true;
            newColour = originalColour;


            foreach (var btn in GrdBoard.Children)
            {
                if (btn.StyleId == _currentRow.ToString())
                {
                    btn.IsEnabled = true;
                }

            }
            foreach (var btn in LstSequenceBtns)
            {
                btn.IsVisible = false;
            }

            ReadFile(fullFileName);
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            //NOT COMPLETED DOESNT WORK

            // save the sequence LstSequence
            // save guesses LstButtonsToCheck
            // save row number _currentRow
            //
            string pathName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullFileName = Path.Combine(pathName, "SaveGame.txt");

            WriteFile(fullFileName);

        }
        private void ReadFile(String fileName)
        {
            string[] line = new string[4];
            using (var reader = new StreamReader(fullFileName))
            {
                for(int i=0; i<4; i++)
                {
                    LstSequenceBtns[i].StyleId = reader.Read().ToString();
                }
                for (int i = 0; i < 4; i++)
                {
                    LstButtonsToCheck[i].StyleId = reader.Read().ToString();
                }
                _currentRow = reader.Read();

            }
        }
        private void WriteFile(String fileName)
        {
            using (var writer = new StreamWriter(fullFileName, true))
            {
                writer.WriteLine(LstSequenceBtns);
                writer.WriteLine(LstButtonsToCheck);
                writer.WriteLine(_currentRow);
            }
        }
    }
}
