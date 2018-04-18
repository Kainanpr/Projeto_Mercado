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
            //Caso não encontre nenhum cliente irá recuperar a exceção que eu lancei
            catch(Exception ex)
            {
                //Recupera a exceção com o erro que eu instanciei
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

            cliente.Endereco.Id = 8;

            clienteService.update(cliente);
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
            txtCpf.Enabled = true;
            txtCidade.Enabled = true;
            txtCep.Enabled = true;
            txtEmail.Enabled = true;
            txtEndereco.Enabled = true;
            txtNumero.Enabled = true;
            txtEstado.Enabled = true;
            txtTelefone.Enabled = true;
        }

        private void DesabilitarCamposGeral()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = false;
            txtCpf.Enabled = false;
            txtCidade.Enabled = false;
            txtCep.Enabled = false;
            txtEmail.Enabled = false;
            txtEndereco.Enabled = false;
            txtNumero.Enabled = false;
            txtEstado.Enabled = false;
            txtTelefone.Enabled = false;
        }

        private void LimparCamposGeral()
        {
            txtCodigo.Clear();
            txtNome.Clear();
            txtCpf.Clear();
            txtCidade.Clear();
            txtCep.Clear();
            txtEmail.Clear();
            txtEndereco.Clear();
            txtNumero.Clear();
            txtEstado.Clear();
            txtTelefone.Clear();
        }

        /* Metodos auxiliares (DTO). 
         * Coleta os dados da visão e passa os dados para o modelo */
        private Cliente GetDTOCadastro()
        {
            Cliente cliente = new Cliente();
            cliente.Nome = txtNome.Text == "" ? null : txtNome.Text;
            cliente.Cpf = txtCpf.Text == "" ? null : txtCpf.Text;
            cliente.Email = txtEmail.Text == "" ? null : txtEmail.Text;

            Telefone tel = new Telefone();
            tel.Numero = txtTelefone.Text == "" ? null : txtTelefone.Text;

            Endereco end = new Endereco();
            end.Rua = txtEndereco.Text == "" ? null : txtEndereco.Text;
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
            txtCpf.Text = cliente.Cpf;
            txtEmail.Text = cliente.Email;

            //Verifica se o usuario possui telefones na lista
            if (cliente.Telefones.Count > 0)
                txtTelefone.Text = cliente.Telefones[0].Numero;
            //Caso ele nao tenha telefone o campo deve estar em braco
            else
                txtTelefone.Text = "";

            txtEndereco.Text = cliente.Endereco.Rua;
            txtNumero.Text = cliente.Endereco.Numero.ToString();
            txtCep.Text = cliente.Endereco.Cep.ToString();
            txtCidade.Text = cliente.Endereco.Cidade ;
            txtEstado.Text = cliente.Endereco.Estado;
        }

    }
}
