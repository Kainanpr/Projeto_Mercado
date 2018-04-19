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

    /* View responsavel pelo formulario de clientes, associando cliente a endereço
     * Os eventos de botões representam a camada de controller(C) do MVC */
    public partial class FormCliente : Form
    {

        //Atributo responsavel por ter as regras de negocio relacionadas ao DAO
        private ClienteService clienteService = new ClienteService();

        public FormCliente()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtCodigo.Enabled = false;
            btnCancelar.Enabled = true;
            btnSalvar.Enabled = true;
            btnPesquisar.Enabled = false;
            btnAlterar.Enabled = false;

            AbilitarCamposGeral();
            LimparCamposGeral();
        }
    

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                //Retorna um objeto Cliente, captura os dados da view pelo metodo privado GetDTOCadastro()
                Cliente cliente = GetDTOCadastro();

                /* Envia os dados do cliente para a camada de service que é responsavel
                 * por chamar os DAOs adequados */
                clienteService.Create(cliente);

            }
            //Captura uma exceção caso o usuario digite algo que esteja incorreto
            catch(FormatException)
            {
                MessageBox.Show("Erro: Dados incorretos");
                LimparCamposGeral();
            }
            

            //Comandos abaixos apenas para resetar o layout
            DesabilitarCamposGeral();

            btnSalvar.Enabled = false;
            btnCancelar.Enabled = false;
            btnPesquisar.Enabled = true;
            btnAlterar.Enabled = true;
            txtCodigo.Enabled = true;
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            
            //Irá tentar encontrar um cliente
            try
            {
                //Captura o id digitado pelo usuario para pesquisar
                int id = int.Parse(txtCodigo.Text);

                /*Encontra o cliente de acordo com seu ID 
                 *(Metodo pode lançar uma exceção caso nao encontre o cliente)*/
                Cliente cliente = clienteService.Read(id);

                //Chama o metodo auxiliar para atualizar a view
                SetDTO(cliente);

            }
            //Captura uma exceção caso o usuario digite algo que não seja números inteiros
            catch(FormatException)
            {
                MessageBox.Show("Erro: Digite apenas números");
                LimparCamposGeral();
            }
            //Caso não encontre nenhum cliente irá recuperar a exceção que nos lançamos
            catch(ObjetoNotFoundException ex)
            {
                //Recupera a exceção com o erro que nos instanciamos
                MessageBox.Show("Erro: " + ex.Message);
                LimparCamposGeral();
            }
 
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            //Captura o id digitado pelo usuario para pesquisar
            int id = int.Parse(txtCodigo.Text);

            /*Encontra o cliente de acordo com seu ID 
             *(Metodo pode lançar uma exceção caso nao encontre o cliente)*/
            Cliente cliente = clienteService.Read(id);
    

            clienteService.Delete(cliente);

            //clienteService.Delete(cliente);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCamposGeral();
            DesabilitarCamposGeral();

            btnCancelar.Enabled = false;
            btnSalvar.Enabled = false;
            btnPesquisar.Enabled = true;
            btnAlterar.Enabled = true;
            txtCodigo.Enabled = true;
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
            txtCodigo.Enabled = false;
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
            txtCodigo.Clear();
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
        private Cliente GetDTOCadastro()
        {
            Cliente cliente = new Cliente();
            cliente.Nome = txtNome.Text == "" ? null : txtNome.Text;
            cliente.Cpf = txtTelefone.Text == "" ? null : txtTelefone.Text;
            cliente.Email = txtEndereco.Text == "" ? null : txtEndereco.Text;

            Telefone tel = new Telefone();
            tel.Numero = txtCpf.Text == "" ? null : txtCpf.Text;

            Endereco end = new Endereco();
            end.Rua = txtEmail.Text == "" ? null : txtEmail.Text;
            end.Numero = int.Parse(txtNumero.Text);
            end.Cep = int.Parse(txtCep.Text);
            end.Cidade = txtCidade.Text == "" ? null : txtCidade.Text;
            end.Estado = txtEstado.Text == "" ? null : txtEstado.Text;

            //Associa o cliente a um telefone
            cliente.Telefones.Add(tel);

            //Associa o cliente a um endereço
            cliente.Endereco = end;

            return cliente;
        }

        /* Metodos auxiliares (DTO). 
         * Coloca as informações do modelo na visão */
        private void SetDTO(Cliente cliente)
        {

            txtNome.Text = cliente.Nome;
            txtTelefone.Text = cliente.Cpf;
            txtEndereco.Text = cliente.Email;

            //Verifica se o usuario possui telefones na lista (nessa caso queremos mostrar apenas o 1º telefone)
            if (cliente.Telefones.Count > 0)
                txtCpf.Text = cliente.Telefones[0].Numero;
            //Caso ele nao tenha telefone cadastrado o campo deve estar em braco
            else
                txtCpf.Text = "";


            //Verifica se o cliente possui endereço cadastrado
            if(cliente.Endereco != null)
            {
                txtEmail.Text = cliente.Endereco.Rua;
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
