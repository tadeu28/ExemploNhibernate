
using ExNHibernate.DataBase.Model;
using ExNHibernate.DataBase.Repository;
using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ExNHibernate.DataBase
{
    public class DBFactory
    {
        private static DBFactory _instance = null;

        private ISessionFactory _sessionFactory;

        public PessoaRepository PessoaRepository { get; set; }

        private DBFactory()
        {
            this.Conectar();

            this.PessoaRepository = new PessoaRepository(this.Session);
        }

        public static DBFactory Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DBFactory();
                }

                return _instance;
            }
        }

        private void Conectar()
        {
            try
            {
                var stringConexao = "Persist Security Info=False;server=localhost;"+
                                    "port=3306;database=pessoas;uid=root;pwd=aluno";

                var mySql = new MySqlConnection(stringConexao);
                try
                {
                    mySql.Open();
                }
                finally
                {
                    if (mySql.State == ConnectionState.Open)
                    {
                        mySql.Close();
                    }
                }

                this.ConfigNhibernate(stringConexao);
            }catch(Exception ex)
            {
                throw new Exception("Não foi Possível Conecta ao Banco de Dados", ex);
            }
        }

        private void ConfigNhibernate(string stringConexao)
        {
            //Cria a configuração com o NH
            var config = new Configuration();
            try
            {
                //Integração com o Banco de Dados
                config.DataBaseIntegration(c =>
                {
                    //Dialeto de Banco
                    c.Dialect<NHibernate.Dialect.MySQLDialect>();
                    //Conexao string
                    c.ConnectionString = stringConexao;
                    //Drive de conexão com o banco
                    c.Driver<NHibernate.Driver.MySqlDataDriver>();
                    // Provedor de conexão do MySQL 
                    c.ConnectionProvider<NHibernate.Connection.DriverConnectionProvider>();
                    // GERA LOG DOS SQL EXECUTADOS NO CONSOLE
                    c.LogSqlInConsole = true;
                    // DESCOMENTAR CASO QUEIRA VISUALIZAR O LOG DE SQL FORMATADO NO CONSOLE
                    c.LogFormattedSql = true;
                    // CRIA O SCHEMA DO BANCO DE DADOS SEMPRE QUE A CONFIGURATION FOR UTILIZADA
                    c.SchemaAction = SchemaAutoAction.Update;
                });

                //Realiza o mapeamento das classes
                var maps = this.Mapeamento();
                config.AddMapping(maps);

                if(HttpContext.Current == null)
                {
                    config.CurrentSessionContext<ThreadStaticSessionContext>();
                }else
                {
                    config.CurrentSessionContext<WebSessionContext>();
                }

                this._sessionFactory = config.BuildSessionFactory();
            } catch(Exception ex){
                throw new Exception("Não foi possível config Nhibernate", ex);
            }
        }

        private HbmMapping Mapeamento()
        {
            try
            {
                var mapper = new ModelMapper();

                mapper.AddMappings(
                    Assembly.GetAssembly(typeof(PessoaMap)).GetTypes()
                );

                return mapper.CompileMappingForAllExplicitlyAddedEntities();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível realizar o mapeamento do modelo.", ex);
            }
        }

        private ISession Session
        {
            get
            {
                try
                {
                    if (CurrentSessionContext.HasBind(_sessionFactory))
                        return _sessionFactory.GetCurrentSession();

                    var session = _sessionFactory.OpenSession();
                    CurrentSessionContext.Bind(session);

                    return session;
                }
                catch (Exception ex)
                {
                    throw new Exception("Não foi possível criar a Sessão.", ex);
                }
            }
        }
    }
}
