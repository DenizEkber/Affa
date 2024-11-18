using AFFA.src.DEPO.DataBase.Context;
using AFFA.src.DEPO.DataBase.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AFFA.src.UI
{
    public partial class ClubForm : Form
    {
        TextBox txtClubName, txtCity;
        Label lblClubName, lblCity;
        Button btnAddClub;
        DataGridView dgvClubs;

        private readonly AFFADbContext _dbContext = new AFFADbContext();

        public ClubForm()
        {
            Component();
            LoadClubs();
        }

        private void Component()
        {
            this.txtClubName = new TextBox();
            this.txtCity = new TextBox();
            this.btnAddClub = new Button();
            this.dgvClubs = new DataGridView();
            this.lblClubName = new Label();
            this.lblCity = new Label();

            this.SuspendLayout();

            // Club Name Label
            this.lblClubName.Text = "Klub Adı:";
            this.lblClubName.Location = new Point(20, 20);

            // Club Name TextBox
            this.txtClubName.Location = new Point(120, 20);
            this.txtClubName.Size = new Size(200, 20);

            // City Label
            this.lblCity.Text = "Şəhər:";
            this.lblCity.Location = new Point(20, 60);

            // City TextBox
            this.txtCity.Location = new Point(120, 60);
            this.txtCity.Size = new Size(200, 20);

            // Add Club Button
            this.btnAddClub.Text = "Klub Əlavə Et";
            this.btnAddClub.Location = new Point(120, 100);
            this.btnAddClub.Click += new EventHandler(this.btnAddClub_Click);

            // Clubs DataGridView
            this.dgvClubs.Location = new Point(20, 140);
            this.dgvClubs.Size = new Size(400, 200);

            // Form Settings
            this.ClientSize = new Size(450, 400);
            this.Controls.Add(this.txtClubName);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.btnAddClub);
            this.Controls.Add(this.dgvClubs);
            this.Controls.Add(this.lblClubName);
            this.Controls.Add(this.lblCity);
            this.ResumeLayout(false);
        }

        private void LoadClubs()
        {
            dgvClubs.DataSource = _dbContext.Clubs.ToList();
        }

        private void btnAddClub_Click(object sender, EventArgs e)
        {
            var club = new Club
            {
                Name = txtClubName.Text,
                City = txtCity.Text
            };
            _dbContext.Clubs.Add(club);
            _dbContext.SaveChanges();
            AddedStandingClub(club.ClubId);
            LoadClubs();
        }
        private void AddedStandingClub(int clubId)
        {
            var standing = new Standing
            {
                ClubId = clubId,
                MatchesPlayed = 0,
                Points = 0
            };
            _dbContext.Standings.Add(standing);
            _dbContext.SaveChanges();
        }
    }
}
