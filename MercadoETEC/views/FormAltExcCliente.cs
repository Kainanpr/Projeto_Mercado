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

        //Atributo para poder ser manipulado pelos metodos
        Cliente cliente;

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

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            
            try
            {
                //Pega o id do cliente da linha do datagrid que estiver selecionado
                int id = int.Parse(dataGridViewClientes.CurrentRow.Cells[0].Value.ToString());

                //Pesquisa o cliente selecionado
                cliente = clienteService.Read(id);

                //Envia o cliente para setar a view
                SetDTO(cliente);

                //Comandos abaixos apenas para resetar o layout
                AbilitarCamposGeral();

                btnSalvar.Enabled = true;
                btnCancelar.Enabled = true;
                btnExcluir.Enabled = true;

                btnListar.Enabled = false;
                btnAlterar.Enabled = false;
                btnPesquisar.Enabled = false;

            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro: Selecione uma linha válida da tabela");
            }

            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCamposGeral();
            DesabilitarCamposGeral();

            btnCancelar.Enabled = false;
            btnSalvar.Enabled = false;
            btnExcluir.Enabled = false;

            btnListar.Enabled = true;
            btnAlterar.Enabled = true;
            btnPesquisar.Enabled = true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            //Recupera os dados digitados na view
            cliente = GetDTOCadastro(cliente);

            //Envia o cliente para a camada de service que sera responsavel pela atualização do cliente
            clienteService.Update(cliente);

            //Comandos abaixos apenas para resetar o layout
            DesabilitarCamposGeral();

            btnSalvar.Enabled = false;
            btnCancelar.Enabled = false;
            btnExcluir.Enabled = false;

            btnListar.Enabled = true;
            btnAlterar.Enabled = true;
            btnPesquisar.Enabled = true;
        }


        //Metodos auxiliares
        private void AbilitarCamposGeral()
        {
            txtNome.Enabled = true;
            txtTelefone.Enabled = true;
            txtCidade.Enabled = true;
            txtCep.Enabled = true;
            txtEndereco.Enabled = true;
            txtEmail.Enabled = true;
            txtNumero.Enabled = true;
            txtEstado.Enabled = true;
            txtCpf.Enabled = true;
        }

        private void DesabilitarCamposGeral()
        {
            txtNome.Enabled = false;
            txtTelefone.Enabled = false;
            txtCidade.Enabled = false;
            txtCep.Enabled = false;
            txtEndereco.Enabled = false;
            txtEmail.Enabled = false;
            txtNumero.Enabled = false;
            txtEstado.Enabled = false;
            txtCpf.Enabled = false;
        }

        private void LimparCamposGeral()
        {
            txtNome.Clear();
            txtTelefone.Clear();
            txtCidade.Clear();
            txtCep.Clear();
            txtEndereco.Clear();
            txtEmail.Clear();
            txtNumero.Clear();
            txtEstado.Clear();
            txtCpf.Clear();
        }

        /* Metodos auxiliares (DTO). 
         * Coleta os dados da visão e passa os dados para o modelo */
        private Cliente GetDTOCadastro(Cliente cliente)
        {
            cliente.Nome = txtNome.Text == "" ? null : txtNome.Text;
            cliente.Cpf = txtCpf.Text == "" ? null : txtCpf.Text;
            cliente.Email = txtEmail.Text == "" ? null : txtEmail.Text;

            cliente.Endereco.Rua = txtEndereco.Text == "" ? null : txtEndereco.Text;
            cliente.Endereco.Numero = int.Parse(txtNumero.Text);
            cliente.Endereco.Cep = int.Parse(txtCep.Text);
            cliente.Endereco.Cidade = txtCidade.Text == "" ? null : txtCidade.Text;
            cliente.Endereco.Estado = txtEstado.Text == "" ? null : txtEstado.Text;

            //Verifica se o cliente tem telefones na sua lista
            if(cliente.Telefones.Count > 0)
                cliente.Telefones[0].Numero = txtNumero.Text == "" ? null : txtNumero.Text;

            return cliente;
        }

        /* Metodos auxiliares (DTO). 
         * Coloca as informações do modelo na visão */
        private void SetDTO(Cliente cliente)
        {

            txtNome.Text = cliente.Nome;
            txtCpf.Text = cliente.Cpf;
            txtEmail.Text = cliente.Email;

            //Verifica se o usuario possui telefones na lista (nessa caso queremos mostrar apenas o 1º telefone)
            if (cliente.Telefones.Count > 0)
                txtTelefone.Text = cliente.Telefones[0].Numero;
            //Caso ele nao tenha telefone cadastrado o campo deve estar em braco
            else
                txtTelefone.Text = "";


            //Verifica se o cliente possui endereço cadastrado
            if (cliente.Endereco != null)
            {
                txtEndereco.Text = cliente.Endereco.Rua;
                txtNumero.Text = cliente.Endereco.Numero == 0 ? "" : cliente.Endereco.Numero.ToString();
                txtCep.Text = cliente.Endereco.Cep == 0 ? "" : cliente.Endereco.Cep.ToString();
                txtCidade.Text = cliente.Endereco.Cidade;
                txtEstado.Text = cliente.Endereco.Estado;
            }
            //Caso ele nao tenha endereco cadastrado os campos devem estar em braco
            else
            {
                txtEmail.Text = "";
                txtNumero.Text = "";
                txtCep.Text = "";
                txtCidade.Text = "";
                txtEstado.Text = "";
            }
        }
    

    }//Fim da classe
}//Fim da namespace
