using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System.Configuration;

namespace WERService.MongoDB
{
    //Burada ayarlarımızı yapacağız aşağıdaki açıklamaları okuyun
    public class MongoDBRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private MongoDatabase database;
        private MongoCollection<TEntity> collection;
        public MongoDBRepository()
        {
            GetDataBase();
            GetCollection();
        }

        private void GetCollection()
        {
            collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        private string GetDataBaseName()
        {
            //Web.Config'i açıyoruz appSettings taglarını buluyoruz
            //  <add key="Mongo" value="MongoDatabase" />
            // Ben mongodb'yi oluşturduğumda MongoDatabase diye bir database oluşturmuştum
            //siz hangi database'i oluşturduysanız adını value kısmına giriniz.
            return ConfigurationManager.AppSettings.Get("Mongo");
        }

        private string GetConnectionString()
        {
             //Web.Config'i açıyoruz <appSettings></appSettings> taglarını buluyoruz
            //MongoDB bağlantısı için oluşturulan string değerini value kısmına kopyalıyoruz ve adını
            //ConnectionString olarak tanımlıyoruz
            //<add key="ConnectionString" value="1fj5lfk4gkfd3wkfk" />
            return ConfigurationManager.AppSettings.Get("ConnectionString");
        }

        private void GetDataBase()
        {
            var client = new MongoClient(GetConnectionString());
            var server = client.GetServer();
            database = server.GetDatabase(GetDataBaseName());
        }

        public WriteConcernResult Delete(TEntity entity)
        {
            WriteConcernResult result = collection.Remove(Query.EQ("_id",entity.Id));
            return result;
        }

        public IList<TEntity> GetAll()
        {
            return collection.FindAllAs<TEntity>().ToList();
        }

        public TEntity GetById(string id)
        {
            return collection.FindOneByIdAs<TEntity>(id);
        }

        public WriteConcernResult Insert(TEntity entity)
        {
            entity.Id = ObjectId.GenerateNewId().ToString();
            WriteConcernResult result = collection.Insert(entity);
            return result;
        }

        public IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return collection.AsQueryable<TEntity>().Where(predicate.Compile()).ToList();
        }

        public WriteConcernResult Update(TEntity entity)
        {
            if (entity.Id == null)
            {
                return Insert(entity);
            }
            WriteConcernResult result = collection.Save(entity);
            return result;
        }
    
    }
}