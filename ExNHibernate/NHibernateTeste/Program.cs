using ExNHibernate.DataBase;
using ExNHibernate.DataBase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateTeste
{
    class Program
    {
        static void Main(string[] args)
        {
            var pessoa = new Pessoa()
            {
                Nome = "Classe.T",
                DtNascimento = new DateTime(1988, 09, 28),
                TipoDocumento = "RG",
                NumDocumento = "MG14654181",
                Telefone = "32988087710"
            };
            
            pessoa = DBFactory.Instance.PessoaRepository.Save(pessoa);

            pessoa.Nome += " Moreira";

            pessoa = DBFactory.Instance.PessoaRepository.Save(pessoa);

            DBFactory.Instance.PessoaRepository.Delete(pessoa);

            Console.Clear();

            var pessoas = DBFactory.Instance.PessoaRepository.FindAll();
            pessoas.ToList().ForEach(f =>
            {
                Console.WriteLine("Nome: " + f.Nome);
            });

            Console.ReadKey();
        }
    }
}
