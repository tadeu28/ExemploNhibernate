using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExNHibernate.DataBase.Repository
{
    public abstract class RepositoryBase<T> where T : class

    {
        public ISession Session = null;

        protected RepositoryBase(ISession session)
        {
            this.Session = session;
        }

        public virtual IList<T> FindAll()
        {
            return this.Session.CreateCriteria(typeof(T)).List<T>();
        }

        public virtual T FirstOrDefault()
        {
            return this.Session.Query<T>().FirstOrDefault();
        }

        public void Delete(T entity)
        {
            try
            {
                var transaction = this.Session.BeginTransaction();

                this.Session.Delete(entity);

                transaction.Commit();
            }catch(Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) + 
                                    "\nErro:" + ex.Message);
            }
        }

        public void Delete(T entity, int id)
        {
            try
            {
                var sql = "delete from {0} where id = {1};";                

                this.Session.CreateQuery(
                    String.Format(sql, 
                                  typeof(T), 
                                  id)
                ).ExecuteUpdate();

                this.Session.Clear();           
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) +
                                    "\nErro:" + ex.Message);
            }
        }

        public virtual T Save(T entity)
        {
            try
            {
                this.Session.Clear();

                var transacao = this.Session.BeginTransaction();
                this.Session.SaveOrUpdate(entity);
                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível gravar " + typeof(T) +
                                    "\nErro:" + ex.Message);
            }
        }
    }
}
