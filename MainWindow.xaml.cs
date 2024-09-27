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
            // Cria um novo objeto 'contato' e atribui os valores dos campos de entrada da tela
            contato c = new contato();
            c.nome = txtName.Text; // Cria um novo objeto 'contato' e atribui os valores dos campos de entrada da tela
            c.email = txtEmail.Text; // 'email' recebe o texto digitado no campo txtEmail
            c.telefone = txtTelephone.Text; // 'telefone' recebe o texto digitado no campo txtTelephone

            // Verifica se a operação é "inserir"
            // Se for, adiciona o novo contato ao banco de dados
            if (operation == "inserir")
            {
                // Usa o contexto do banco de dados para adicionar o novo contato
                using (agendaEntities ctx = new agendaEntities())
                {
                    ctx.contatos.Add(c); // 'contatos' recebe um novo contato (c) adicionado à sua lista
                    ctx.SaveChanges(); // Salva as mudanças no banco de dados
                }
            }
            if (operation == "Alterar")
            {
                // Aqui entraria a lógica para alterar o contato existente
            }
            // Atualiza a exibição dos contatos no DataGrid
            this.ListContacts();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            // Define que a operação atual é "Inserir"
            this.operation = "Inserir";
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
                // 'consulta' recebe todos os contatos do banco de dados através do contexto
                var consulta = ctx.contatos;
                // Atribui a lista de contatos (consulta) ao DataGrid para exibição
                dgData.ItemsSource = consulta.ToList();
            }
        }

        private void AlterButton(int options)
        {
            btnToAlter.IsEnabled = false;
            btnInsert.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnLocate.IsEnabled = false;
            btnSave.IsEnabled = false;

            if (options == 1)
            {
                btnInsert.IsEnabled = true;
                btnLocate.IsEnabled = true;
            }
        }
    }
}
