﻿using System;
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

namespace MercadoETEC.views
{

    /* View responsavel pelo formulario de clientes, associando cliente a endereço
     * Os eventos de botões representam a camada de controller(C) do MVC */
    public partial class FormCliente : Form
    {

        //Atributo responsavel por inserir uma pessoa no banco de dados
        private PessoaDAO pessoaDAO = new PessoaDAO();

        //Atributo responsavel por inserir um cliente no banco de dados
        private ClienteDAO clienteDAO = new ClienteDAO();

        //Atributo responsavel por inserir um endereco no banco de dados
        private EnderecoDAO enderecoDAO = new EnderecoDAO();

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
            //Retorna um objeto Cliente, captura os dados da view pelo metodo privado GetDTO()
            Cliente cliente = GetDTO();

            /* Grava o endereço no banco de dados e retorna o ultimo endereco inserido no banco 
             * ja com seu id setado para ser associado ao cliente correta */
            cliente.Endereco = enderecoDAO.Create(cliente.Endereco);

            /* Guarda a pessoa no banco de dados 
             * (O metodo retorna a ultima pessoa inserida no banco já com seu id setado). */
            Pessoa pessoa = pessoaDAO.Create(cliente);

            /* Associa o id do cliente ao id da pessoa retornada do banco
             * Esse passo é necessario devido a herança no C# e a especialização no banco  */
            cliente.Id = pessoa.Id;

            /* Guardar o cliente no banco de dados */
            clienteDAO.Create(cliente);

            //Comandos abaixos apenas para resetar o layout
            DesabilitarCamposGeral();

            btnSalvar.Enabled = false;
            btnCancelar.Enabled = false;
            btnPesquisar.Enabled = true;
            btnAlterar.Enabled = true;
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            txtCodigo.Enabled = true;
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            txtCodigo.Enabled = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCamposGeral();
            DesabilitarCamposGeral();

            btnCancelar.Enabled = false;
            btnSalvar.Enabled = false;
            btnPesquisar.Enabled = true;
            btnAlterar.Enabled = true;
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
        private Cliente GetDTO()
        {
            Cliente cliente = new Cliente();
            cliente.Nome = txtNome.Text;
            cliente.Cpf = txtCpf.Text;
            cliente.Email = txtEmail.Text;

            Telefone tel = new Telefone();
            tel.Numero = txtTelefone.Text;

            Endereco end = new Endereco();
            end.Rua = txtEndereco.Text;
            end.Numero = int.Parse(txtNumero.Text);
            end.Cep = int.Parse(txtCep.Text);
            end.Cidade = txtCidade.Text;
            end.Estado = txtEstado.Text;

            //Associa o cliente a um telefone
            cliente.Telefones.Add(tel);

            //Associa o cliente a um endereço
            cliente.Endereco = end;

            return cliente;
        }



    }
}
