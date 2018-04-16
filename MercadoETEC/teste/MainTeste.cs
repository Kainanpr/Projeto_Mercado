/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MercadoETEC.model.domain;

namespace MercadoETEC.teste
{
    class MainTeste
    {
        static void Main(string[] args)
        {
            Cliente p1 = new Cliente();
            Pessoa p2 = new Cliente();
            Endereco end1 = new Endereco("Rua um", 1000, "Santa Maria", "13582-31", "São Carlos", "SP");
            Endereco end2 = new Endereco();
            Telefone tel1 = new Telefone("3344-2211");
            Telefone tel2 = new Telefone("3312-2541");
           
            p1.Cpf = "442.555.038-26";
            p1.Nome = "Kainan Pereira Ramos";
            p1.Endereco = end1;
            p1.Email = "kainan.pr@gm.com";
            p1.Telefones.Add(tel1);
            p1.Telefones.Add(tel2);

            MessageBox.Show("" + p1.Id);

            Console.Read();
        }
    }
}
*/