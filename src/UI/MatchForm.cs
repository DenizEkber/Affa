using AFFA.src.DEPO.DataBase.Context;
using AFFA.src.DEPO.DataBase.Entity;
using System.Data;
using System.Linq;

namespace AFFA.src.UI
{
    public partial class MatchForm : Form
    {
        Label lblHomeClub, lblAwayClub, lblMatchDate, lblHomeScore, lblAwayScore;
        Button btnAddMatch, btnUpdateResult, btnGoToCustomerForm;
        ComboBox cmbHomeClub, cmbAwayClub;
        DateTimePicker dtpMatchDate;
        NumericUpDown nudHomeScore, nudAwayScore;
        DataGridView dgvMatches;
        System.Windows.Forms.Timer matchTimeCheckTimer;

        private readonly AFFADbContext _dbContext = new AFFADbContext();

        public MatchForm()
        {
            Component();
            LoadMatches();
            LoadClubs();
            InitializeMatchTimeCheckTimer();
        }

        private void InitializeMatchTimeCheckTimer()
        {
            matchTimeCheckTimer = new System.Windows.Forms.Timer
            {
                Interval = 10000 
            };
            matchTimeCheckTimer.Tick += MatchTimeCheckTimer_Tick;
            matchTimeCheckTimer.Start();
        }

        private void MatchTimeCheckTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var matchesToStart = _dbContext.Matches
                    .Where(m => m.MatchDate <= DateTime.Now && m.MatchResult.HomeScore == 0 && m.MatchResult.AwayScore == 0)
                    .ToList();

                if (!matchesToStart.Any()) return;

                Random random = new Random();
                foreach (var match in matchesToStart)
                {
                    var matchResult = _dbContext.MatchResults.FirstOrDefault(r => r.MatchId == match.MatchId);

                    if (matchResult != null)
                    {
                        matchResult.HomeScore = random.Next(1, 11);
                        matchResult.AwayScore = random.Next(1, 11);
                        _dbContext.SaveChanges();
                    }
                }

                
                LoadMatches();
                MessageBox.Show("Matçların vaxtı çatdığı üçün skorlar avtomatik təyin edildi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Vaxt yoxlanarkən xəta baş verdi: {ex.Message}");
            }
        }

        private void Component()
        {
            this.cmbHomeClub = new ComboBox();
            this.cmbAwayClub = new ComboBox();
            this.dtpMatchDate = new DateTimePicker();
            this.btnAddMatch = new Button();
            this.btnUpdateResult = new Button();
            this.nudHomeScore = new NumericUpDown();
            this.nudAwayScore = new NumericUpDown();
            this.dgvMatches = new DataGridView();
            this.lblHomeClub = new Label();
            this.lblAwayClub = new Label();
            this.lblMatchDate = new Label();
            this.lblHomeScore = new Label();
            this.lblAwayScore = new Label();

            this.SuspendLayout();

            this.lblHomeClub.Text = "Ev Komandası:";
            this.lblHomeClub.Location = new Point(20, 20);
            this.cmbHomeClub.Location = new Point(120, 20);
            this.cmbHomeClub.Size = new Size(200, 20);

            this.lblAwayClub.Text = "Qonaq Komanda:";
            this.lblAwayClub.Location = new Point(20, 60);
            this.cmbAwayClub.Location = new Point(120, 60);
            this.cmbAwayClub.Size = new Size(200, 20);

            this.lblMatchDate.Text = "Matç Tarixi və Saatı:";
            this.lblMatchDate.Location = new Point(20, 100);
            this.dtpMatchDate.Location = new Point(180, 100);
            this.dtpMatchDate.Size = new Size(200, 20);
            this.dtpMatchDate.Format = DateTimePickerFormat.Custom;
            this.dtpMatchDate.CustomFormat = "dd.MM.yyyy HH:mm";

            this.lblHomeScore.Text = "Ev Qolu:";
            this.lblHomeScore.Location = new Point(20, 140);
            this.nudHomeScore.Location = new Point(180, 140);
            this.nudHomeScore.Size = new Size(50, 20);

            this.lblAwayScore.Text = "Qonaq Qolu:";
            this.lblAwayScore.Location = new Point(20, 180);
            this.nudAwayScore.Location = new Point(180, 180);
            this.nudAwayScore.Size = new Size(50, 20);

            this.btnAddMatch.Text = "Matç Əlavə Et";
            this.btnAddMatch.Location = new Point(20, 220);
            this.btnAddMatch.Click += btnAddMatch_Click;

            this.btnUpdateResult.Text = "Nəticəni Yenilə";
            this.btnUpdateResult.Location = new Point(150, 220);
            this.btnUpdateResult.Click += btnUpdateResult_Click;

            this.dgvMatches.Location = new Point(20, 260);
            this.dgvMatches.Size = new Size(500, 200);
            this.dgvMatches.CellClick += dgvMatches_CellClick;

            this.btnGoToCustomerForm = new Button();
            this.btnGoToCustomerForm.Text = "Müştəri Formuna Keç";
            this.btnGoToCustomerForm.Location = new Point(300, 220);
            this.btnGoToCustomerForm.Click += btnGoToCustomerForm_Click;


            this.ClientSize = new Size(550, 500);
            this.Controls.AddRange(new Control[] {
                lblHomeClub, cmbHomeClub, lblAwayClub, cmbAwayClub,
                lblMatchDate, dtpMatchDate, lblHomeScore, nudHomeScore,
                lblAwayScore, nudAwayScore, btnAddMatch, btnUpdateResult, btnGoToCustomerForm, dgvMatches
            });
            this.ResumeLayout(false);
        }

        private void btnGoToCustomerForm_Click(object sender, EventArgs e)
        {
            try
            {
                LoginForm customerForm = new LoginForm(); 
                customerForm.Show(); 
                this.Hide(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Müştəri formuna keçərkən xəta baş verdi: {ex.Message}");
            }
        }

        private void LoadMatches()
        {
            try
            {
                dgvMatches.DataSource = _dbContext.Matches
                    .Select(m => new
                    {
                        m.MatchId,
                        EvKomanda = m.HomeClub.Name,
                        QonaqKomanda = m.AwayClub.Name,
                        Tarix = m.MatchDate.ToString("dd.MM.yyyy HH:mm"),
                        m.MatchResult.HomeScore,
                        m.MatchResult.AwayScore
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Matç siyahısı yüklənərkən xəta: {ex.Message}");
            }
        }

        private void LoadClubs()
        {
            try
            {
                var clubs = _dbContext.Clubs.ToList();
                cmbHomeClub.DataSource = new BindingSource(clubs, null);
                cmbHomeClub.DisplayMember = "Name";
                cmbHomeClub.ValueMember = "ClubId";

                cmbAwayClub.DataSource = new BindingSource(clubs, null);
                cmbAwayClub.DisplayMember = "Name";
                cmbAwayClub.ValueMember = "ClubId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Komandalar yüklənərkən xəta: {ex.Message}");
            }
        }

        private void btnAddMatch_Click(object sender, EventArgs e)
        {
            try
            {
                if ((int)cmbHomeClub.SelectedValue == (int)cmbAwayClub.SelectedValue)
                {
                    MessageBox.Show("Ev və qonaq komandaları fərqli olmalıdır.");
                    return;
                }

                var match = new Match
                {
                    HomeClubId = (int)cmbHomeClub.SelectedValue,
                    AwayClubId = (int)cmbAwayClub.SelectedValue,
                    MatchDate = dtpMatchDate.Value
                };
                _dbContext.Matches.Add(match);
                _dbContext.SaveChanges();

                CreateInitialMatchResult(match);

                LoadMatches();
                MessageBox.Show("Matç uğurla əlavə edildi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Matçı əlavə edərkən xəta: {ex.Message}");
            }
        }
        private void CreateInitialMatchResult(Match match)
        {
            var matchResult = new MatchResult
            {
                MatchId = match.MatchId,
                HomeScore = 0,
                AwayScore = 0
            };
            _dbContext.MatchResults.Add(matchResult);
            _dbContext.SaveChanges();
        }


        private void btnUpdateResult_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMatches.CurrentRow == null)
                {
                    MessageBox.Show("Matç seçin.");
                    return;
                }

                int matchId = (int)dgvMatches.CurrentRow.Cells["MatchId"].Value;
                var matchResult = _dbContext.MatchResults.FirstOrDefault(r => r.MatchId == matchId);

                if (matchResult == null)
                {
                    MessageBox.Show("Matç nəticəsi tapılmadı.");
                    return;
                }

                matchResult.HomeScore = (int)nudHomeScore.Value;
                matchResult.AwayScore = (int)nudAwayScore.Value;
                _dbContext.SaveChanges();

                UpdateStandings(matchResult);
                LoadMatches();
                MessageBox.Show("Nəticə yeniləndi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nəticəni yeniləyərkən xəta: {ex.Message}");
            }
        }

        private void dgvMatches_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int matchId = (int)dgvMatches.Rows[e.RowIndex].Cells["MatchId"].Value;
                var matchResult = _dbContext.MatchResults.FirstOrDefault(r => r.MatchId == matchId);

                if (matchResult != null)
                {
                    nudHomeScore.Value = matchResult.HomeScore;
                    nudAwayScore.Value = matchResult.AwayScore;
                }
            }
        }

        private void UpdateStandings(MatchResult matchResult)
        {
            UpdateTeamPoints(matchResult.Match.HomeClubId, matchResult.HomeScore > matchResult.AwayScore ? 2 : matchResult.HomeScore == matchResult.AwayScore ? 1 : 0);
            UpdateTeamPoints(matchResult.Match.AwayClubId, matchResult.AwayScore > matchResult.HomeScore ? 2 : matchResult.HomeScore == matchResult.AwayScore ? 1 : 0);
        }

        private void UpdateTeamPoints(int clubId, int points)
        {
            var clubStanding = _dbContext.Standings.FirstOrDefault(s => s.ClubId == clubId);
            if (clubStanding != null)
            {
                clubStanding.Points += points;
                clubStanding.MatchesPlayed++;
                _dbContext.SaveChanges();
            }
        }

    }
}
