using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WERService.MongoDB
{
    public class EntityBase
    { 
        [BsonId]
        public string Id { get; set; }
    }
}