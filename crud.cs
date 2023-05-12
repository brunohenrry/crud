using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace crud
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=<nome_do_servidor>:<porta>/<nome_do_banco_de_dados>;User Id=<usuario>;Password=<senha>;";

        private OracleConnection connection;
        private OracleDataAdapter adapter;
        private DataTable dataTable;

        public Form1()
        {
            InitializeComponent();

            connection = new OracleConnection(connectionString);
            adapter = new OracleDataAdapter();
            dataTable = new DataTable();

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM contatos";

                adapter.SelectCommand = new OracleCommand(query, connection);

                connection.Open();

                dataTable.Clear();
                adapter.Fill(dataTable);

                dgvContatos.DataSource = dataTable;

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                string nome = txtNome.Text;
                string telefone = txtTelefone.Text;
                string email = txtEmail.Text;

                string query = "INSERT INTO contatos (nome, telefone, email) VALUES (:nome, :telefone, :email)";

                OracleCommand command = new OracleCommand(query, connection);
                command.Parameters.Add(":nome", OracleDbType.Varchar2).Value = nome;
                command.Parameters.Add(":telefone", OracleDbType.Varchar2).Value = telefone;
                command.Parameters.Add(":email", OracleDbType.Varchar2).Value = email;

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();

                MessageBox.Show("Contato salvo com sucesso!");

                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar o contato: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvContatos.SelectedRows.Count > 0)
                {
                    string id = dgvContatos.SelectedRows[0].Cells["id"].Value.ToString();

                    string nome = txtNome.Text;
                    string telefone = txtTelefone.Text;
                    string email = txtEmail.Text;

                    string query = "UPDATE contatos SET nome = :nome, telefone = :telefone, email = :email WHERE id = :id";

                    OracleCommand command = new OracleCommand(query, connection);
                    command.Parameters.Add(":nome", OracleDbType.Varchar2).Value = nome;
                    command.Parameters.Add(":telefone", OracleDbType.Varchar2).Value = telefone;
                    command.Parameters.Add(":email", OracleDbType.Varchar2).Value = email;
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();

                    MessageBox.Show("Contato atualizado com sucesso!");

                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Nenhum contato selecionado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar o contato: " + ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvContatos.SelectedRows.Count > 0)
                {
                    string id = dgvContatos.SelectedRows[0].Cells["id"].Value.ToString();

                    string query = "DELETE FROM contatos WHERE id = :id";

                    OracleCommand command = new OracleCommand(query, connection);
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();

                    MessageBox.Show("Contato excluído com sucesso!");

                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Nenhum contato selecionado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir o contato: " + ex.Message);
            }
        }

        private void dgvContatos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvContatos.SelectedRows.Count > 0)
            {
                string id = dgvContatos.SelectedRows[0].Cells["id"].Value.ToString();
                string nome = dgvContatos.SelectedRows[0].Cells["nome"].Value.ToString();
                string telefone = dgvContatos.SelectedRows[0].Cells["telefone"].Value.ToString();
                string email = dgvContatos.SelectedRows[0].Cells["email"].Value.ToString();

                txtNome.Text = nome;
                txtTelefone.Text = telefone;
                txtEmail.Text = email;
            }
        }

        private void ClearFields()
        {
            txtNome.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }
    }
}