using AFFA.src.DEPO.DataBase.Context;
using System.Data;

namespace AFFA.src.UI
{
    public partial class StandingsForm : Form
    {
        Label lblStandings;
        DataGridView dgvStandings;

        private readonly AFFADbContext _dbContext = new AFFADbContext();

        public StandingsForm()
        {
            Component();
            LoadStandings();
        }

        private void Component()
        {
            this.dgvStandings = new DataGridView();
            this.lblStandings = new Label();

            this.SuspendLayout();

            this.lblStandings.Text = "Turnir Cədvəli:";
            this.lblStandings.Font = new Font("Arial", 14, FontStyle.Bold);
            this.lblStandings.Location = new Point(20, 20);

            this.dgvStandings.Location = new Point(20, 60);
            this.dgvStandings.Size = new Size(400, 300);

            this.ClientSize = new Size(450, 400);
            this.Controls.Add(this.dgvStandings);
            this.Controls.Add(this.lblStandings);
            this.ResumeLayout(false);
        }


        private void LoadStandings()
        {
            var standings = _dbContext.Standings
                .OrderByDescending(s => s.Points)
                .Select(s => new
                {
                    Club = s.Club.Name,
                    s.Points,
                    s.MatchesPlayed
                }).ToList();

            dgvStandings.DataSource = standings;
        }
    }
}
