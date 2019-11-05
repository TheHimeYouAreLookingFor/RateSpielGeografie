using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RateSpielGeografie
{
    public partial class Form1 : Form
    {
        String ANS;
        Random r = new Random();
        int A, B, C, D;
        int QCount = 0;
        bool FormDragging = false;
        private DB db = new DB();
        private int mouseStartX;
        private int mouseStartY;
        private int formStartX;
        private int formStartY;
        private int Score;
        List<Question> FforQ = new List<Question>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(424, 336);
            LandGuess.Location = new Point(4, 24);
            FlagGuess.Location = new Point(4, 24);
            CapitalGuess.Location = new Point(4, 24);
            Highscores.Location = new Point(4, 24);
        }

        private void HighscoresStart_Click(object sender, EventArgs e)
        {
            Highscore_List.Items.Clear();
            foreach (Score pl in db.Scores())
            {
                 Highscore_List.Items.Add(pl.PNAME + ": " + pl.PSCORE);
            }
            Start.Visible = false;
            Highscores.Visible = true;
        }

        private void Selected_Player(object sender, EventArgs e)
        {
            ListBox LB = (ListBox)sender;
            String[] Sel = LB.SelectedItem.ToString().Split(' ');
            Highscore_Player.Items.Clear();
            HighscoreLabelPlayer.Text = Sel[0].ToUpper();
            foreach (String st in LB.Items)
            {
                if (st.ToUpper().Contains(Sel[0].ToUpper()))
                {
                    Highscore_Player.Items.Add(st);
                }
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grip_MouseDown(object sender, MouseEventArgs e)
        {
            this.mouseStartX = MousePosition.X;
            this.mouseStartY = MousePosition.Y;
            this.formStartX = this.Location.X;
            this.formStartY = this.Location.Y;
            FormDragging = true;
        }

        private void grip_MouseMove(object sender, MouseEventArgs e)
        { 
            if (FormDragging)
            {
                this.Location = new Point(
                this.formStartX + MousePosition.X - this.mouseStartX,
                this.formStartY + MousePosition.Y - this.mouseStartY
                );
            }
        }

        private void grip_MouseUp(object sender, MouseEventArgs e)
        {
            FormDragging = false;
        }

        private void EnterName(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            TB.Text = TB.Text.Trim();
            if (TB.Text.Length >= 4)
            {
                NameLength.Visible = false;
                SubmitName.Visible = true;
            }
            else
            {
                SubmitName.Visible = false;
                int lng = TB.Text.Length;
                NameLength.Visible = true;
                NameLength.Text = "(" + (4 - lng) + ")";
                NameLength.ForeColor = Color.Red;
                FlagGuessStart.Enabled = false;
                LandGuessStart.Enabled = false;
                CapitalGuessStart.Enabled = false;
            }
        }

        private void SubmtName(object sender, EventArgs e)
        {
            label1Start.Text = NameINStart.Text + " | Score: " + Score.ToString();
            label1Start.Location = new Point(90, 160);
            FlagGuessStart.Enabled = true;
            LandGuessStart.Enabled = true;
            CapitalGuessStart.Enabled = true;
            SubmitName.Visible = false;
            NameINStart.Visible = false;
            Send_Score.Enabled = true;
            End_game.Enabled = true;
        }

        private void Send_Score_Click(object sender, EventArgs e)
        {
            if (Score != 0)
            {
                db.HScore(new Score(NameINStart.Text, Score));
                label1Start.Text = "Please enter your name:";
                label1Start.Location = new Point(45, 160);
                FlagGuessStart.Enabled = false;
                LandGuessStart.Enabled = false;
                CapitalGuessStart.Enabled = false;
                SubmitName.Visible = true;
                NameINStart.Visible = true;
                NameINStart.Text = "";
                Score = 0;
                Send_Score.Enabled = false;
                End_game.Enabled = false;
            }
        }

        private void End_Game_Click(object sender, EventArgs e)
        {
            label1Start.Text = "Please enter your name:";
            label1Start.Location = new Point(45, 160);
            FlagGuessStart.Enabled = false;
            LandGuessStart.Enabled = false;
            CapitalGuessStart.Enabled = false;
            SubmitName.Visible = true;
            NameINStart.Visible = true;
            NameINStart.Text = "";
            Score = 0;
            Send_Score.Enabled = false;
            End_game.Enabled = false;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            FlagGuess.Visible = false;
            LandGuess.Visible = false;
            CapitalGuess.Visible = false;
            Highscores.Visible = false;
            if (label1Start.Text.StartsWith(NameINStart.Text) && NameINStart.Text.Length >= 4)
            {
                label1Start.Text = NameINStart.Text + " | Score: " + Score.ToString();
            }
            Start.Visible = true;
        }




        //Guess the flag
        private void FlagGuessStart_Click(object sender, EventArgs e)
        {
            Start.Visible = false;
            FlagGuess.Visible = true;
            GuessFlagGame();
        }

        private void GuessFlagGame()
        {
            FlagGuessInfo.Visible = true;
            FlagGuessBack.Visible = true;
            FlagGuessStartbtn.Visible = true;

            FlagGuessSubmit.Visible = false;
            FlagGuessFlag.Visible = false;
        }

        private void FlagGuessStartbtn_Click(object sender, EventArgs e)
        {
            QCount = 0;
            FforQ = new List<Question>();
            for (int i = 0; i < 10; i++)
            {
                bool done = false;
                while (!done)
                {
                    A = r.Next(0, db.Lander().Count);
                    B = r.Next(0, db.Lander().Count);
                    C = r.Next(0, db.Lander().Count);
                    D = r.Next(0, db.Lander().Count);
                    if (A != B && A != C && A != D && B != C && B != D && C != D)
                    {
                        done = true;
                    }
                }
                done = false;
                FforQ.Add(new Question(db.Lander()[A].Name, db.Lander()[B].Name, db.Lander()[C].Name, db.Lander()[D].Name));
            }
            FlagGuessInfo.Visible = false;
            FlagGuessBack.Visible = false;
            FlagGuessStartbtn.Visible = false;

            FlagGuessSubmit.Visible = true;
            FlagGuessFlag.Visible = true;
            FlagGuess.BackgroundImage = Image.FromFile("..\\..\\Pictures\\Background.png");
            FQswitch();
        }

        private void FlagGuessSubmit_Click(object sender, EventArgs e)
        {
            FlagGuessSubmit.Enabled = false;
            FlagGuessLabel.Text = "The correct answer was " + FforQ[QCount].AN;
            if (ANS == FforQ[QCount].AN)
            {
                FlagGuessLabel.Text = "Correct! It is " + FforQ[QCount].AN;
                Score += 1;
            }
            if (QCount != 10)
            {
                QCount += 1;
                FlagGuessNext.Visible = true;
            }
            FlagGuess1.Enabled = false;
            FlagGuess2.Enabled = false;
            FlagGuess3.Enabled = false;
            FlagGuess4.Enabled = false;
        }

        private void FlagGuess_answer(object sender, EventArgs e)
        {
            RadioButton RB = (RadioButton)sender;
            ANS = RB.Text;
        }

        private void FlagGuessNext_Click(object sender, EventArgs e)
        {
            if (QCount == 10)
            {
                if (Score == 0)
                {
                    FlagGuess.BackgroundImage = Image.FromFile("..\\..\\Pictures\\Capture.jpg");
                }
                GuessFlagGame();
            }
            else
            {
                FQswitch();
            }
            FlagGuessLabel.Text = "To which land does this Flag belong to ?";
            FlagGuessSubmit.Enabled = true;
            FlagGuessNext.Visible = false;
            FlagGuess1.Enabled = true;
            FlagGuess2.Enabled = true;
            FlagGuess3.Enabled = true;
            FlagGuess4.Enabled = true;
            FlagGuess1.Checked = false;
            FlagGuess2.Checked = false;
            FlagGuess3.Checked = false;
            FlagGuess4.Checked = false;
        }

        private void FQswitch()
        {
            FlagGuess1.Text = FforQ[QCount].QS[0];
            FlagGuess2.Text = FforQ[QCount].QS[1];
            FlagGuess3.Text = FforQ[QCount].QS[2];
            FlagGuess4.Text = FforQ[QCount].QS[3];
            FlagGuessFlag.Image = Image.FromFile("..\\..\\Flaggen\\" + FforQ[QCount].AN + ".png");
        }




        //Guess the land
        private void LandGuessStart_Click(object sender, EventArgs e)
        {
            Start.Visible = false;
            LandGuess.Visible = true;
            GuessLandGame();
        }

        private void GuessLandGame()
        {
            LandGuessInfo.Visible = true;
            LandGuessBack.Visible = true;
            LandGuessStartBtn.Visible = true;

            LandGuess1.Visible = false;
            LandGuess2.Visible = false;
            LandGuess3.Visible = false;
            LandGuess4.Visible = false;
            LandGuessSubmit.Visible = false;
        }

        private void LandGuessStartBtn_Click(object sender, EventArgs e)
        {
            QCount = 0;
            FforQ = new List<Question>();
            for (int i = 0; i < 10; i++)
            {
                bool done = false;
                while (!done)
                {
                    A = r.Next(0, db.Lander().Count);
                    B = r.Next(0, db.Lander().Count);
                    C = r.Next(0, db.Lander().Count);
                    D = r.Next(0, db.Lander().Count);
                    if (A != B && A != C && A != D && B != C && B != D && C != D)
                    {
                        done = true;
                    }
                }
                done = false;
                FforQ.Add(new Question(db.Lander()[A].Name, db.Lander()[B].Name, db.Lander()[C].Name, db.Lander()[D].Name));
            }
            LandGuessInfo.Visible = false;
            LandGuessBack.Visible = false;
            LandGuessStartBtn.Visible = false;

            LandGuessLabel.Visible = true;
            LandGuess1.Visible = true;
            LandGuess2.Visible = true;
            LandGuess3.Visible = true;
            LandGuess4.Visible = true;
            LandGuessSubmit.Visible = true;
            LandGuess.BackgroundImage = Image.FromFile("..\\..\\Pictures\\Background.png");
            LQswitch();
        }

        private void LandGuessSubmit_Click(object sender, EventArgs e)
        {
            LandGuessSubmit.Enabled = false;
            LandGuessLabel.Text = "The correct answer was: " + FforQ[QCount].AN;
            if (ANS == FforQ[QCount].AN)
            {
                LandGuessLabel.Text = "Correct! It is: " + FforQ[QCount].AN;
                Score += 1;
            }
            if (QCount != 10)
            {
                QCount += 1;
                LandGuessNext.Visible = true;
            }
            LandGuess1.Enabled = false;
            LandGuess2.Enabled = false;
            LandGuess3.Enabled = false;
            LandGuess4.Enabled = false;
        }

        private void LandGuess_answer(object sender, EventArgs e)
        {
            RadioButton RB = (RadioButton)sender;
            ANS = RB.Tag.ToString();
        }

        private void LandGuessNext_Click(object sender, EventArgs e)
        {
            if (QCount == 10)
            {
                if (Score == 0)
                {
                    LandGuess.BackgroundImage = Image.FromFile("..\\..\\Pictures\\Capture.jpg");
                }
                GuessLandGame();
            }
            else
            {
                LQswitch();
            }
            LandGuessSubmit.Enabled = true;
            LandGuessNext.Visible = false;
            LandGuess1.Enabled = true;
            LandGuess2.Enabled = true;
            LandGuess3.Enabled = true;
            LandGuess4.Enabled = true;
            LandGuess1.Checked = false;
            LandGuess2.Checked = false;
            LandGuess3.Checked = false;
            LandGuess4.Checked = false;
        }

        private void LQswitch()
        {
            LandGuess1.Tag = FforQ[QCount].QS[0];
            LandGuess2.Tag = FforQ[QCount].QS[1];
            LandGuess3.Tag = FforQ[QCount].QS[2];
            LandGuess4.Tag = FforQ[QCount].QS[3];

            LandGuess1.BackgroundImage = Image.FromFile("..\\..\\Flaggen\\" + FforQ[QCount].QS[0] + ".png");
            LandGuess2.BackgroundImage = Image.FromFile("..\\..\\Flaggen\\" + FforQ[QCount].QS[1] + ".png");
            LandGuess3.BackgroundImage = Image.FromFile("..\\..\\Flaggen\\" + FforQ[QCount].QS[2] + ".png");
            LandGuess4.BackgroundImage = Image.FromFile("..\\..\\Flaggen\\" + FforQ[QCount].QS[3] + ".png");
            LandGuessLabel.Text = "The Flag from " + FforQ[QCount].AN + " is:";
        }




        //Guess the capital
        private void CapitalGuessStart_Click(object sender, EventArgs e)
        {
            Start.Visible = false;
            CapitalGuess.Visible = true;
            GuessCapitalGame();
        }

        private void GuessCapitalGame()
        {
            CapitalGuessInfo.Visible = true;
            CapitalGuessBack.Visible = true;
            CapitalGuessStartBtn.Visible = true;

            CapitalGuess1.Visible = false;
            CapitalGuess2.Visible = false;
            CapitalGuess3.Visible = false;
            CapitalGuess4.Visible = false;
            CapitalGuessFlag.Visible = false;
            CapitalGuessSubmit.Visible = false;
        }

        private void CapitalGuessStartBtn_Click(object sender, EventArgs e)
        {
            QCount = 0;
            FforQ = new List<Question>();
            for (int i = 0; i < 10; i++)
            {
                bool done = false;
                while (!done)
                {
                    A = r.Next(0, db.Lander().Count);
                    B = r.Next(0, db.Lander().Count);
                    C = r.Next(0, db.Lander().Count);
                    D = r.Next(0, db.Lander().Count);
                    if (A != B && A != C && A != D && B != C && B != D && C != D)
                    {
                        done = true;
                    }
                }
                done = false;
                FforQ.Add(new Question(db.Lander()[A].Haupt, db.Lander()[B].Haupt, db.Lander()[C].Haupt, db.Lander()[D].Haupt, db.Lander()[A].Name, db.Lander()[B].Name, db.Lander()[C].Name, db.Lander()[D].Name));
            }
            CapitalGuessInfo.Visible = false;
            CapitalGuessBack.Visible = false;
            CapitalGuessStartBtn.Visible = false;

            CapitalGuessSubmit.Visible = true;
            CapitalGuess1.Visible = true;
            CapitalGuess2.Visible = true;
            CapitalGuess3.Visible = true;
            CapitalGuess4.Visible = true;
            CapitalGuessFlag.Visible = true;
            CapitalGuess.BackgroundImage = Image.FromFile("..\\..\\Pictures\\Background.png");
            CQswitch();
        }

        private void CapitalGuessSubmit_Click(object sender, EventArgs e)
        {
            CapitalGuessSubmit.Enabled = false;
            CapitalGuessLabel.Text = "The correct answer was: " + FforQ[QCount].AN;
            if (ANS == FforQ[QCount].AN)
            {
                CapitalGuessLabel.Text = "Correct! It is: " + FforQ[QCount].AN;
                Score += 1;
            }
            if (QCount != 10)
            {
                QCount += 1;
                CapitalGuessNext.Visible = true;
            }
            CapitalGuess1.Enabled = false;
            CapitalGuess2.Enabled = false;
            CapitalGuess3.Enabled = false;
            CapitalGuess4.Enabled = false;
        }

        private void CapitalGuess_answer(object sender, EventArgs e)
        {
            RadioButton RB = (RadioButton)sender;
            ANS = RB.Text;
        }

        private void CapitalGuessNext_Click(object sender, EventArgs e)
        {
            if (QCount == 10)
            {
                if (Score == 0)
                {
                    CapitalGuess.BackgroundImage = Image.FromFile("..\\..\\Pictures\\Capture.jpg");
                }
                GuessCapitalGame();
            }
            else
            {
                CQswitch();
            }
            CapitalGuessLabel.Text = "The Capital of " + FforQ[QCount].HFLAG + " is:";
            CapitalGuessSubmit.Enabled = true;
            CapitalGuessNext.Visible = false;
            CapitalGuess1.Enabled = true;
            CapitalGuess2.Enabled = true;
            CapitalGuess3.Enabled = true;
            CapitalGuess4.Enabled = true;
            CapitalGuess1.Checked = false;
            CapitalGuess2.Checked = false;
            CapitalGuess3.Checked = false;
            CapitalGuess4.Checked = false;
        }

        private void CQswitch()
        {
            CapitalGuess1.Text = FforQ[QCount].QS[0];
            CapitalGuess2.Text = FforQ[QCount].QS[1];
            CapitalGuess3.Text = FforQ[QCount].QS[2];
            CapitalGuess4.Text = FforQ[QCount].QS[3];
            CapitalGuessFlag.Image = Image.FromFile("..\\..\\Flaggen\\" + FforQ[QCount].HFLAG + ".png");
            CapitalGuessLabel.Text = "The Capital of " + FforQ[QCount].HFLAG + " is:";
        }
    }
}
