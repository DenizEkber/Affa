using AFFA.src.DEPO.DataBase.Context;
using AFFA.src.DEPO.DataBase.Entity;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AFFA.src.UI
{
    public partial class CustomerForm : Form
    {
        TextBox txtAmount, txtHomeScore, txtAwayScore;
        Button btnPlaceBet, btnCheckBet;
        Label lblTeam, lblAmount, lblBalance, lblHomeScore, lblAwayScore;
        ComboBox cmbTeams, cmbMatchTeams;
        DataGridView dgvBets;
        private readonly AFFADbContext _dbContext = new AFFADbContext();
        private Customer _currentCustomer;

        public CustomerForm(Customer customer)
        {
            _currentCustomer = customer;
            Component();
            LoadTeams();
            LoadBets();
            LoadMatches();
        }

        private void Component()
        {
            this.ClientSize = new Size(400, 500);
            this.Text = "Müştəri Paneli";
            this.StartPosition = FormStartPosition.CenterScreen;

            this.cmbMatchTeams = new ComboBox
            {
                Location = new Point(450, 140),
                Size = new Size(240, 20),
                //Enabled = false 
            };


            this.lblBalance = new Label
            {
                Text = $"Balans: {_currentCustomer.Balance:C}",
                Location = new Point(20, 20),
                AutoSize = true
            };

            this.lblTeam = new Label
            {
                Text = "Komanda:",
                Location = new Point(20, 60),
                AutoSize = true
            };

            this.cmbTeams = new ComboBox
            {
                Location = new Point(120, 60),
                Size = new Size(240, 20)
            };

            this.lblAmount = new Label
            {
                Text = "Məbləğ:",
                Location = new Point(20, 100),
                AutoSize = true
            };

            this.txtAmount = new TextBox
            {
                Location = new Point(120, 100),
                Size = new Size(240, 20)
            };

            this.lblHomeScore = new Label
            {
                Text = "Ev Sahibi Skoru:",
                Location = new Point(20, 140),
                AutoSize = true
            };

            this.txtHomeScore = new TextBox
            {
                Location = new Point(120, 140),
                Size = new Size(240, 20)
            };

            this.lblAwayScore = new Label
            {
                Text = "Deplasman Skoru:",
                Location = new Point(20, 180),
                AutoSize = true
            };

            this.txtAwayScore = new TextBox
            {
                Location = new Point(120, 180),
                Size = new Size(240, 20)
            };

            this.btnPlaceBet = new Button
            {
                Text = "Bahis yerləşdir",
                Location = new Point(120, 220),
                Size = new Size(120, 30)
            };
            this.btnPlaceBet.Click += new EventHandler(this.btnPlaceBet_Click);

            this.btnCheckBet = new Button
            {
                Text = "Bahis Nəticələrini Yoxla",
                Location = new Point(240, 220),
                Size = new Size(120, 30)
            };
            this.btnCheckBet.Click += new EventHandler(this.btnCheckBet_Click);

            this.dgvBets = new DataGridView
            {
                Location = new Point(20, 260),
                Size = new Size(360, 200),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };

            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblTeam);
            this.Controls.Add(this.cmbTeams);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.lblHomeScore);
            this.Controls.Add(this.txtHomeScore);
            this.Controls.Add(this.lblAwayScore);
            this.Controls.Add(this.txtAwayScore);
            this.Controls.Add(this.btnPlaceBet);
            this.Controls.Add(this.btnCheckBet);
            this.Controls.Add(this.dgvBets);
            this.Controls.Add(this.cmbMatchTeams);
        }

        private void LoadMatches()
        {
            var matches = _dbContext.Matches
                .Where(m => m.MatchDate > DateTime.Now)
                .Select(m => new
                {
                    m.MatchId,
                    DisplayName = $"{m.HomeClub.Name} vs {m.AwayClub.Name}"
                })
                .ToList();

            cmbMatchTeams.DataSource = matches;
            cmbMatchTeams.DisplayMember = "DisplayName";
            cmbMatchTeams.ValueMember = "MatchId";

            cmbMatchTeams.SelectedIndexChanged += (s, e) =>
            {
                if (cmbMatchTeams.SelectedValue is int matchId)
                {
                    LoadTeamsForMatch(matchId);
                }
            };
        }


        private void LoadTeamsForMatch(int matchId)
        {
            var match = _dbContext.Matches
                .Where(m => m.MatchId == matchId)
                .Select(m => new
                {
                    HomeTeamId = m.HomeClubId,
                    HomeTeamName = m.HomeClub.Name,
                    AwayTeamId = m.AwayClubId,
                    AwayTeamName = m.AwayClub.Name
                })
                .FirstOrDefault();

            if (match != null)
            {
                var teams = new[]
                {
                    new { TeamId = match.HomeTeamId, Name = match.HomeTeamName },
                    new { TeamId = match.AwayTeamId, Name = match.AwayTeamName }
                };

                cmbMatchTeams.DataSource = teams.ToList();
                cmbMatchTeams.DisplayMember = "Name";
                cmbMatchTeams.ValueMember = "TeamId";
                cmbMatchTeams.Enabled = true;
            }
            else
            {
                MessageBox.Show("No teams found for the selected match.");
                cmbMatchTeams.DataSource = null;
                cmbMatchTeams.Enabled = false;
            }
        }



        private void LoadTeams()
        {
            cmbTeams.DataSource = _dbContext.Clubs.ToList();
            cmbTeams.DisplayMember = "Name";
            cmbTeams.ValueMember = "ClubId";
        }

        private void LoadBets()
        {
            dgvBets.DataSource = _dbContext.Bets
                .Where(b => b.CustomerId == _currentCustomer.CustomerId)
                .Select(b => new
                {
                    b.BetId,
                    b.Amount,
                    Team = b.Team.Name,
                    b.IsPaid
                })
                .ToList();
        }

        private void btnPlaceBet_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmount.Text, out decimal betAmount) || betAmount <= 0)
            {
                MessageBox.Show("Geçerli bir miktar girin!");
                return;
            }

            if (_currentCustomer.Balance < betAmount)
            {
                MessageBox.Show("Balanınız yeterli değil!");
                return;
            }

            if (cmbTeams.SelectedValue == null || cmbMatchTeams.SelectedValue == null)
            {
                MessageBox.Show("Lütfen bir maç ve takım seçin!");
                return;
            }

            int selectedMatchId = (int)cmbTeams.SelectedValue;
            int selectedTeamId = (int)cmbMatchTeams.SelectedValue;

            if (!int.TryParse(txtHomeScore.Text, out int homeScore) || !int.TryParse(txtAwayScore.Text, out int awayScore))
            {
                MessageBox.Show("Geçerli bir tahmin girin.");
                return;
            }

            var bet = new Bet
            {
                CustomerId = _currentCustomer.CustomerId,
                TeamId = selectedTeamId,
                MatchId = selectedMatchId,
                Amount = betAmount,
                PredictedHomeScore = homeScore,
                PredictedAwayScore = awayScore,
                IsPaid = false
            };

            _dbContext.Bets.Add(bet);
            _currentCustomer.Balance -= betAmount;
            _dbContext.SaveChanges();
            MessageBox.Show("Bahis kabul edildi! Lütfen ödemeyi yapın.");

            SimulatePayment(bet);
        }



        private void SimulatePayment(Bet bet)
        {
            var confirm = MessageBox.Show($"Bahis üçün {bet.Amount:C} ödəniş edilsinmi?", "Ödənişi təsdiq edin", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                bet.IsPaid = true;
                _dbContext.SaveChanges();
                MessageBox.Show("Ödəniş təsdiqləndi!");
            }
            else
            {
                MessageBox.Show("Bahis ödənilmədiyi üçün ləğv edildi.");
                _dbContext.Bets.Remove(bet);
                _dbContext.SaveChanges();
                _currentCustomer.Balance += bet.Amount; 
            }

            LoadBets(); 
            lblBalance.Text = $"Balans: {_currentCustomer.Balance:C}"; 
        }

        private void btnCheckBet_Click(object sender, EventArgs e)
        {
            var bet = _dbContext.Bets.OrderByDescending(b => b.BetId).FirstOrDefault(b => b.CustomerId == _currentCustomer.CustomerId);
            if (bet == null)
            {
                MessageBox.Show("Hesablı bahis tapılmadı.");
                return;
            }

            var matchResult = _dbContext.MatchResults
                .FirstOrDefault(mr => mr.MatchId == bet.TeamId);

            if (matchResult != null)
            {
                EvaluateBet(bet, matchResult);
            }
            else
            {
                MessageBox.Show("Matç nəticəsi tapılmadı.");
            }
        }

        private void EvaluateBet(Bet bet, MatchResult matchResult)
        {
            bool isWinner = (bet.PredictedHomeScore == matchResult.HomeScore) &&
                            (bet.PredictedAwayScore == matchResult.AwayScore);

            decimal payout = 0;

            if (isWinner)
            {
                payout = bet.Amount * 2; 
            }

            if (payout > 0)
            {
                _currentCustomer.Balance += payout; 
                MessageBox.Show($"Tebrikler! {payout:C} kazandınız!");
            }
            else
            {
                MessageBox.Show("Üzgünüz, tahmininiz yanlış oldu.");
            }

            _dbContext.SaveChanges();
            lblBalance.Text = $"Balan: {_currentCustomer.Balance:C}";
        }

    }
}
