using System;
using System.Text;
using System.Windows.Forms;

namespace EncriptacaoRijndael
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnEncriptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbRandom.Checked)
                {
                    using (var smpCrypt = new SimpleAES(txtChave.Text))
                    {
                        txtOutput.Text = smpCrypt.EncryptStr(txtInput.Text);
                    }
                }
                else
                {
                    txtOutput.Text = CriptografiaHelper.Encriptar(txtChave.Text, txtVetor.Text, txtInput.Text);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnDecriptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbRandom.Checked)
                {
                    using (var smpCrypt = new SimpleAES(txtChave.Text))
                    {
                        txtOutput.Text = smpCrypt.DecryptStr(txtInput.Text);
                    }
                }
                else
                {
                    txtOutput.Text = CriptografiaHelper.Decriptar(txtChave.Text, txtVetor.Text, txtInput.Text);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void ShowError(Exception ex)
        {
            StringBuilder sbLog = new StringBuilder();

            if (ex != null)
            {
                sbLog.Append(" Exception -> " + ex.Message);

                if (ex.InnerException != null)
                {
                    sbLog.Append(Environment.NewLine);
                    sbLog.Append(" InnerException -> " + ex.InnerException.ToString());
                }

                if (!string.IsNullOrWhiteSpace(ex.StackTrace))
                {
                    sbLog.Append(Environment.NewLine);
                    sbLog.Append(" StackTrace -> " + ex.StackTrace);
                }

            }

            MessageBox.Show(sbLog.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
