using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WERService.MongoDB;
using WERService.MongoModels;

namespace WERService.MongoService
{
    public class UserController
    {
        // Adım adım gidelim
        // UserModel diye bir class yaratıyoruz, bu class'taki her bir alan aslında
        // MongoDB'mizin sütunu olacak. Aynı SQL Mantığı gibi düşünün.
        public static bool Add(UserModel data)
        {
            //Öncelikle Repository tanımlıyoruz. Bu tanımlamada MongoDB'miz öncelikle UserModel adında
            // bir tablonun olup olmadığına bakar. Yoksa otomatik oluşturur. Varsa da var olanı getirir
            MongoDBRepository<UserModel> Repository = new MongoDBRepository<UserModel>();
            // Bu kısımda o tabloya Data'larımızı ekliyoruz insert komutuyla.. 
            WriteConcernResult result = Repository.Insert(data);
            //Hata almazsa true dönsün, hata alırsak false dönsün diyoruz, isterseniz siz burada
            //geliştirme yapıp hata mesajları fırlatabilirsiniz.
            if (!result.HasLastErrorMessage)
            {
                return true;
            }
            return false;
        }

        public static IList<UserModel> GetAll()
        {
            //Yine Repository'de tablomuzun varlığını kontrol ediyoruz
            MongoDBRepository<UserModel> Repository = new MongoDBRepository<UserModel>();
            // Tablomuzda gelen dataların hepsini modele atıyoruz
            IList<UserModel> models = Repository.GetAll();
            //  model'i döndürüyoruz
            return models;
        }
    }
    // siz class'larınızı burada tanımlamayın, örnek olması açısından koydum
    public class UserModel
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Surname {get; set;}
        public string Password {get; set;}
        public string Email {get; set;}
    }
}