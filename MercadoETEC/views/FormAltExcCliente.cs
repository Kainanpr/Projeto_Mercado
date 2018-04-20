using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MercadoETEC.model.domain;
using MercadoETEC.model.conexao;
using MercadoETEC.model.dao;
using MercadoETEC.model.service;

using MercadoETEC.model.service.exception;

namespace MercadoETEC.views
{

    /* View responsavel pelo formulario de clientes na parte de exclusao, pesquisa e alterações
     * Os eventos de botões representam a camada de controller(C) do MVC */
    public partial class FormAltExcCliente : Form
    {

        //Atributo responsavel por ter as regras de negocio relacionadas ao DAO
        private ClienteService clienteService = new ClienteService();

        public FormAltExcCliente()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            //Irá tentar encontrar um cliente
            try
            {

                dataGridViewClientes.Rows.Clear();
                
                /*Encontra o cliente de acordo com seu ID 
                 *(Metodo pode lançar uma exceção caso nao encontre o cliente)*/
                List<Cliente> clientes = clienteService.ListAll();

                foreach (Cliente cli in clientes)
                {

                    dataGridViewClientes.Rows.Add(cli.Id, cli.Nome, cli.Cpf);

                }

            }
            
            //Caso não encontre nenhum cliente irá recuperar a exceção que nos lançamos
            catch (ObjetoNotFoundException ex)
            {
                //Recupera a exceção com o erro que nos instanciamos
                MessageBox.Show("Erro: " + ex.Message);

            }
        }
    }
}
