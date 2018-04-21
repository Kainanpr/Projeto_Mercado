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
            //Verifica se o cliente digitou algo no campo localizar cliente
            if (txtPesquisarCliente.Text != "")
            {

                //Verifica se o radio name esta selecionado
                if (radioButtonNome.Checked)
                {

                    //Irá tentar encontrar um cliente
                    try
                    {
                        /*Encontra o cliente de acordo com seu nome
                         *(Metodo pode lançar uma exceção caso nao encontre o cliente)*/
                        List<Cliente> clientes = clienteService.FindByName(txtPesquisarCliente.Text.ToLower());

                        //Chama o metodo auxiliar que nos criamos para atualizar a tabela de acordo com os dados
                        AtualizarGrid(clientes);
                    }
                    //Caso não encontre nenhum cliente irá recuperar a exceção que nos lançamos
                    catch (ObjetoNotFoundException ex)
                    {
                        //Recupera a exceção com o erro que nos instanciamos
                        MessageBox.Show("Erro: " + ex.Message);

                        //Limpa todas as rows
                        dataGridViewClientes.Rows.Clear();
                    }

                }//Fim if

                //Verifica se o raio cpf esta selecionado
                else if (radioButtonCpf.Checked)
                {

                    //Irá tentar encontrar um cliente
                    try
                    {
                        /*Encontra o cliente de acordo com seu cpf
                         *(Metodo pode lançar uma exceção caso nao encontre o cliente)*/
                        Cliente cliente = clienteService.FindByCpf(txtPesquisarCliente.Text.ToLower());

                        //Limpa todas as rows
                        dataGridViewClientes.Rows.Clear();

                        //Atualiza a gride
                        dataGridViewClientes.Rows.Add(cliente.Id, cliente.Nome, cliente.Cpf);
                        
                    }
                    //Caso não encontre nenhum cliente irá recuperar a exceção que nos lançamos
                    catch (ObjetoNotFoundException ex)
                    {
                        //Recupera a exceção com o erro que nos instanciamos
                        MessageBox.Show("Erro: " + ex.Message);

                        //Limpa todas as rows
                        dataGridViewClientes.Rows.Clear();
                    }
                }

            }//Fim if
            else
            {
                MessageBox.Show("Campo está em branco");
            }
           
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            //Irá tentar encontrar um cliente
            try
            {
                /*Encontra o cliente de acordo com seu id
                 *(Metodo pode lançar uma exceção caso nao encontre o cliente)*/
                List<Cliente> clientes = clienteService.ListAll();

                //Chama o metodo auxiliar que nos criamos para atualizar a tabela de acordo com os dados
                AtualizarGrid(clientes);
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
            var result = MessageBox.Show(this, "Você tem certeza que deseja atualizar este cliente?", "Sim", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {

                try
                {
                    //Recupera os dados digitados na view
                    cliente = GetDTOCadastro(cliente);

                    //Envia o cliente para a camada de service que sera responsavel pela atualização do cliente
                    clienteService.Update(cliente);
                }
                //Captura uma exceção caso o usuario digite algo que esteja incorreto
                catch (FormatException)
                {
                    MessageBox.Show("Erro: Dados incorretos");
                    LimparCamposGeral();
                }

                //Comandos abaixos apenas para resetar o layout
                DesabilitarCamposGeral();

                btnSalvar.Enabled = false;
                btnCancelar.Enabled = false;
                btnExcluir.Enabled = false;

                btnListar.Enabled = true;
                btnAlterar.Enabled = true;
                btnPesquisar.Enabled = true;
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(this, "Você tem certeza que deseja excluir este cliente?", "Sim", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                clienteService.Delete(cliente);

                //Comandos abaixos apenas para resetar o layout
                DesabilitarCamposGeral();

                btnSalvar.Enabled = false;
                btnCancelar.Enabled = false;
                btnExcluir.Enabled = false;

                btnListar.Enabled = true;
                btnAlterar.Enabled = true;
                btnPesquisar.Enabled = true;

                LimparCamposGeral();

                //Limpa todas as rows
                dataGridViewClientes.Rows.Clear();
            }

        }


        //Evento é disparado sempre que eu escolher uma linha da gride
        private void dataGridViewClientes_SelectionChanged(object sender, EventArgs e)
        {

            try
            {
                //Pega o id do cliente da linha do datagrid que estiver selecionado
                int id = int.Parse(dataGridViewClientes.CurrentRow.Cells[0].Value.ToString());

                //Pesquisa o cliente selecionado
                cliente = clienteService.Read(id);

                //Envia o cliente para setar a view
                SetDTO(cliente);
            }
            catch (ObjetoNotFoundException)
            {
                //Caso não encontre nenhum cliente limpe os campos
                LimparCamposGeral();
            }
            
        }       

        //Metodos auxiliares
        private void AbilitarCamposGeral()
        {
            txtCpf.Enabled = true;
            txtTelefone.Enabled = true;
            txtCidade.Enabled = true;
            txtCep.Enabled = true;
            txtEndereco.Enabled = true;
            txtEmail.Enabled = true;
            txtNumero.Enabled = true;
            txtEstado.Enabled = true;
            txtNome.Enabled = true;
        }

        private void DesabilitarCamposGeral()
        {
            txtCpf.Enabled = false;
            txtTelefone.Enabled = false;
            txtCidade.Enabled = false;
            txtCep.Enabled = false;
            txtEndereco.Enabled = false;
            txtEmail.Enabled = false;
            txtNumero.Enabled = false;
            txtEstado.Enabled = false;
            txtNome.Enabled = false;
        }

        private void LimparCamposGeral()
        {
            txtCpf.Clear();
            txtTelefone.Clear();
            txtCidade.Clear();
            txtCep.Clear();
            txtEndereco.Clear();
            txtEmail.Clear();
            txtNumero.Clear();
            txtEstado.Clear();
            txtNome.Clear();
        }

        private void AtualizarGrid(List<Cliente> clientes)
        {     
            //Limpa todas as rows
            dataGridViewClientes.Rows.Clear();

            //Percorre a lista de clientes
            foreach (Cliente cli in clientes)
            {
                //Adiciona os dados do cliente na row
                dataGridViewClientes.Rows.Add(cli.Id, cli.Nome, cli.Cpf);
            } 
        }

        /* Metodos auxiliares (DTO). 
         * Coleta os dados da visão e passa os dados para o modelo */
        private Cliente GetDTOCadastro(Cliente cliente)
        {
            cliente.Nome = txtNome.Text == "" ? null : txtNome.Text;
            cliente.Cpf = txtCpf.Text == "" ? null : txtCpf.Text;
            cliente.Email = txtEmail.Text == "" ? null : txtEmail.Text;
            cliente.Telefone = txtTelefone.Text == "" ? null : txtTelefone.Text;

            cliente.Endereco.Rua = txtEndereco.Text == "" ? null : txtEndereco.Text;
            cliente.Endereco.Numero = int.Parse(txtNumero.Text);
            cliente.Endereco.Cep = int.Parse(txtCep.Text);
            cliente.Endereco.Cidade = txtCidade.Text == "" ? null : txtCidade.Text;
            cliente.Endereco.Estado = txtEstado.Text == "" ? null : txtEstado.Text;            

            return cliente;
        }

        /* Metodos auxiliares (DTO). 
         * Coloca as informações do modelo na visão */
        private void SetDTO(Cliente cliente)
        {

            txtNome.Text = cliente.Nome;
            txtCpf.Text = cliente.Cpf;
            txtEmail.Text = cliente.Email;
            txtTelefone.Text = cliente.Telefone;

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
