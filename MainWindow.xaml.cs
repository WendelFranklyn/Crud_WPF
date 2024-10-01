using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AgendaCrud
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private string operation;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            // Verifica se a operação é "inserir"
            // Se for, adiciona o novo contato ao banco de dados
            if (operation == "Inserir")
            {
                // Cria um novo objeto 'contato' e atribui os valores dos campos de entrada da tela
                contato c = new contato();
                c.nome = txtName.Text; // Cria um novo objeto 'contato' e atribui os valores dos campos de entrada da tela
                c.email = txtEmail.Text; // 'email' recebe o texto digitado no campo txtEmail
                c.telefone = txtTelephone.Text; // 'telefone' recebe o texto digitado no campo txtTelephone
                // Usa o contexto do banco de dados para adicionar o novo contato
                using (agendaEntities ctx = new agendaEntities())
                {
                    ctx.contatos.Add(c); // 'contatos' recebe um novo contato (c) adicionado à sua lista
                    ctx.SaveChanges(); // Salva as mudanças no banco de dados
                }
            }
            if (operation == "Alterar")
            {
                using (agendaEntities ctx = new agendaEntities())
                {
                    contato c = ctx.contatos.Find(Convert.ToInt32(txtID.Text));
                    if (c != null)
                    {
                        c.nome = txtName.Text;
                        c.email = txtEmail.Text;
                        c.telefone = txtTelephone.Text;
                        ctx.SaveChanges();
                    }
                }
            }
            // Atualiza a exibição dos contatos no DataGrid
            this.ListContacts();
            this.AlterButton(1);
            this.ClearFields();
        }
        private void ClearFields()
        {
            txtID.IsEnabled = true;
            txtID.Clear();
            txtName.Clear();
            txtEmail.Clear();
            txtTelephone.Clear();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            // Define que a operação atual é "Inserir"
            this.operation = "Inserir";
            this.AlterButton(2);
            this.ClearFields();
            txtID.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Carrega e exibe os contatos quando a janela é carregada
            this.ListContacts();
            this.AlterButton(1);
        }

        private void ListContacts()
        {
            using (agendaEntities ctx = new agendaEntities())
            {
                int qtd = ctx.contatos.Count();
                lbQtdContacts.Content = "Número de contatos existentes: " + qtd.ToString();
                // 'consulta' recebe todos os contatos do banco de dados através do contexto
                var consulta = ctx.contatos;
                // Atribui a lista de contatos (consulta) ao DataGrid para exibição
                dgData.ItemsSource = consulta.ToList();
            }
        }

        private void AlterButton(int options)
        {
            // Desabilita todos os botões inicialmente
            btnToAlter.IsEnabled = false;
            btnInsert.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnLocate.IsEnabled = false;
            btnSave.IsEnabled = false;

            // Dependendo do valor de 'options', habilita os botões corretos
            if (options == 1) // Estado inicial
            {
                btnInsert.IsEnabled = true; // Habilita o botão de inserir
                btnLocate.IsEnabled = true; // Habilita o botão de localizar
            }
            if (options == 2) // Inserção em andamento
            {
                btnCancel.IsEnabled = true; // Habilita o botão de cancelar
                btnSave.IsEnabled = true; //Habilita o botão de salvar
            }
            if (options == 3) // Edição em andamento
            {
                btnToAlter.IsEnabled = true; // Habilita o botão de alterar
                btnDelete.IsEnabled = true; // Habilita o botão de deletar
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Restaura o estado inicial dos botões
            this.AlterButton(1);
            // Limpa os campos de entrada
            this.ClearFields();
        }

        private void btnLocate_Click(object sender, RoutedEventArgs e)
        {
            //se a quantidade de caracteres for maior que 0
            if (txtID.Text.Trim().Count() > 0)
            {
                try
                {
                    // Converte o ID de texto para inteiro e tenta localizar o contato no banco
                    int id = Convert.ToInt32(txtID.Text);
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        // Busca o contato pelo ID
                        contato c = ctx.contatos.Find(id);
                        // Exibe o contato no DataGrid
                        dgData.ItemsSource = new contato[1] { c };

                    }
                }
                catch
                {
                    // Tratar exceções (por exemplo, se a conversão de ID falhar)
                }
            }
            // Se o campo Nome tiver algum valor
            if (txtName.Text.Trim().Count() > 0)
            {
                try
                {
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        // Busca contatos cujo nome contenha o texto digitado
                        var consultation = from c in ctx.contatos
                                           where c.nome.Contains(txtName.Text)
                                           select c;
                        // Exibe os contatos correspondentes no DataGrid
                        dgData.ItemsSource = consultation.ToList();

                    }
                }
                catch { }
            }
            // Se o campo Email tiver algum valor
            if (txtEmail.Text.Trim().Count() > 0)
            {
                try
                {
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        // Busca contatos cujo email contenha o texto digitado
                        var consultation = from c in ctx.contatos
                                           where c.email.Contains(txtEmail.Text)
                                           select c;
                        // Exibe os contatos correspondentes no DataGrid
                        dgData.ItemsSource = consultation.ToList();

                    }
                }
                catch { }
            }
            // Se o campo Telefone tiver algum valor
            if (txtTelephone.Text.Trim().Count() > 0)
            {
                try
                {
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        // 'consultation' recebe todos os contatos da tabela 'contatos'
                        // onde o campo 'telefone' contém o texto digitado no campo 'txtTelephone'
                        var consultation = from c in ctx.contatos
                                           where c.telefone.Contains(txtTelephone.Text)
                                           select c;
                        // Exibe os contatos correspondentes no DataGrid
                        dgData.ItemsSource = consultation.ToList();

                    }
                }
                catch { }
            }
        }

        private void dgData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Se algum item do DataGrid estiver selecionado
            if (dgData.SelectedIndex >= 0)
            {
                //contato c = (contato)dgData.Items[dgData.SelectedIndex];
                contato c = (contato)dgData.SelectedItem;

                // Preenche os campos de entrada com os dados do contato selecionado
                txtID.Text = c.id.ToString();
                txtName.Text = c.nome;
                txtEmail.Text = c.email;
                txtTelephone.Text = c.telefone;
                this.AlterButton(3);
            }
        }

        private void btnToAlter_Click(object sender, RoutedEventArgs e)
        {
            this.operation = "Alterar";
            this.AlterButton(2);
            txtID.IsEnabled = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            using (agendaEntities ctx = new agendaEntities())
            {
                contato c = ctx.contatos.Find(Convert.ToInt32(txtID.Text));
                if(c != null)
                {
                    ctx.contatos.Remove(c);
                    ctx.SaveChanges();
                }
                this.ListContacts();
                this.AlterButton(1);
                this.ClearFields();
            }
        }
    }
}
