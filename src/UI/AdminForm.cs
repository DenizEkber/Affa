using AFFA.src.DEPO.DataBase.Context;
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
    public partial class AdminForm : Form
    {
        TextBox txtMatchId, txtResult, txtBetId;
        Label lblMatchId, lblResult, lblBetId;
        Button btnSetResult, btnMarkAsPaid;
        DataGridView dgvBets;

        private readonly AFFADbContext _dbContext = new AFFADbContext();

        public AdminForm()
        {
            Component();
            LoadBets();
        }

        private void Component()
        {
            this.dgvBets = new DataGridView();
            this.txtMatchId = new TextBox();
            this.txtResult = new TextBox();
            this.txtBetId = new TextBox();
            this.btnSetResult = new Button();
            this.btnMarkAsPaid = new Button();
            this.lblMatchId = new Label();
            this.lblResult = new Label();
            this.lblBetId = new Label();

            this.SuspendLayout();

            this.dgvBets.Location = new Point(20, 20);
            this.dgvBets.Size = new Size(400, 200);

            this.lblMatchId.Text = "Matç ID:";
            this.lblMatchId.Location = new Point(20, 240);
            this.txtMatchId.Location = new Point(120, 240);
            this.txtMatchId.Size = new Size(200, 20);


            this.lblResult.Text = "Nəticə:";
            this.lblResult.Location = new Point(20, 280);


            this.txtResult.Location = new Point(120, 280);
            this.txtResult.Size = new Size(200, 20);


            this.btnSetResult.Text = "Nəticəni yenilə";
            this.btnSetResult.Location = new Point(350, 240);
            this.btnSetResult.Click += new EventHandler(this.btnSetResult_Click);


            this.lblBetId.Text = "Bahis ID:";
            this.lblBetId.Location = new Point(20, 320);


            this.txtBetId.Location = new Point(120, 320);
            this.txtBetId.Size = new Size(200, 20);


            this.btnMarkAsPaid.Text = "Ödənişi təsdiqlə";
            this.btnMarkAsPaid.Location = new Point(350, 320);
            this.btnMarkAsPaid.Click += new EventHandler(this.btnMarkAsPaid_Click);


            this.ClientSize = new Size(450, 400);
            this.Controls.Add(this.dgvBets);
            this.Controls.Add(this.txtMatchId);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtBetId);
            this.Controls.Add(this.btnSetResult);
            this.Controls.Add(this.btnMarkAsPaid);
            this.Controls.Add(this.lblMatchId);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblBetId);
            this.ResumeLayout(false);
        }


        private void LoadBets()
        {
            dgvBets.DataSource = _dbContext.Bets
                .Select(b => new
                {
                    b.BetId,
                    b.Customer.CustomerName,
                    Team = b.TeamId, 
                    b.Amount,
                    b.IsPaid
                }).ToList();
        }

        private void btnSetResult_Click(object sender, EventArgs e)
        {
            int matchId = int.Parse(txtMatchId.Text);
            string result = txtResult.Text;

            var match = _dbContext.Matches.FirstOrDefault(m => m.MatchId == matchId);
            if (match != null)
            {
                /*match.Result = result;
                _dbContext.SaveChanges();*/
                MessageBox.Show("Nəticə yeniləndi!");
            }
        }

        private void btnMarkAsPaid_Click(object sender, EventArgs e)
        {
            int betId = int.Parse(txtBetId.Text);
            var bet = _dbContext.Bets.FirstOrDefault(b => b.BetId == betId);
            if (bet != null)
            {
                bet.IsPaid = true;
                _dbContext.SaveChanges();
                MessageBox.Show("Ödəniş təsdiqləndi!");
            }
        }
    }
}
