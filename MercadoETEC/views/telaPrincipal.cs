using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MercadoETEC.views
{
    /* View responsavel pelo tela principal */
    public partial class telaPrincipal : Form
    {
        public telaPrincipal()
        {
            InitializeComponent();
        }

        private void btnCadCliente_Click(object sender, EventArgs e)
        {
            //instancia uma view de formulario de cliente
            new FormCliente().ShowDialog();
        }
    }
}
