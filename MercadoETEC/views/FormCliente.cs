using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MercadoETEC.model.conexao;

namespace MercadoETEC.views
{
    public partial class FormCliente : Form
    {
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

            abilitarCamposGeral();
            limparCamposGeral();
        }
    

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            desabilitarCamposGeral();

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
            limparCamposGeral();
            desabilitarCamposGeral();

            btnCancelar.Enabled = false;
            btnSalvar.Enabled = false;
            btnPesquisar.Enabled = true;
            btnAlterar.Enabled = true;
        }

        //Metodos auxiliares
        private void abilitarCamposGeral()
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

        private void desabilitarCamposGeral()
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

        private void limparCamposGeral()
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



    }
}
