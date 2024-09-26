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
            //contato com os dados da tela
            contato c = new contato();
            c.nome = txtName.Text;
            c.email = txtEmail.Text;
            c.telefone = txtTelephone.Text;

            // se a operação for igual a inserir
               //eu insiro o contato
            if (operation == "inserir")
            {
                //gravar no banco de dados
                using (agendaEntities ctx = new agendaEntities())
                {
                    ctx.contatos.Add(c); // vai adicionar os objetos da classe contatos
                    ctx.SaveChanges(); // vai salvar no banco 
                }
            }
            if (operation == "Alterar")
            {

            }
            this.ListContacts();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            this.operation = "Inserir";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ListContacts();
        }

        private void ListContacts()
        {
            using (agendaEntities ctx = new agendaEntities())
            {
                var consulta = ctx.contatos;
                //Datagrid tem a propriedade itemSource que recebe a lista da consulta, a lista está associada ao ItemsSource
                dgData.ItemsSource = consulta.ToList();
            }
        }
    }
}
