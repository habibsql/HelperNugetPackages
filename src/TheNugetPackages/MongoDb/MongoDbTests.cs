using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Helper.Nuget.Packages.MongoDb
{
    /* Nuget Package Dependency 1) MongoDB.Driver 2) MongoDB.Driver.Core */

    /// <summary>
    /// Accessing nosql mongodb database. Database setup & create test_database & collections are prerequisit.
    /// </summary>
    public class MongoTests
    {
        [Fact]
        public void ShouldConnectMongoWhenValidCredentials()
        {
            IMongoDatabase database = GetMongoDatabase();
            IMongoCollection<BsonDocument> products = database.GetCollection<BsonDocument>("Companies");

            long result = products.CountDocuments(Builders<BsonDocument>.Filter.Empty);

            Assert.True(result > 0);
        }

        [Fact]
        public void ShouldFetchCandidateListWhenSectorWiseSearch()
        {
            IMongoDatabase database = GetMongoDatabase();
            IMongoCollection<BsonDocument> candidateCollection = database.GetCollection<BsonDocument>("Candidates");
            IMongoCollection<BsonDocument> companyCollection = database.GetCollection<BsonDocument>("Companies");

            var sectors = new[] { "java" };

            IAggregateFluent<BsonDocument> candidateAgg = candidateCollection.Aggregate();

            var let = new BsonDocument("compId", "$PreferredCompanies");
            var operands = new BsonArray();
            operands.Add("$$compId").Add("$_id");

            var expression = new BsonDocument("$expr", new BsonDocument("$eq", operands));

            PipelineDefinition<BsonDocument, BsonDocument> pipeline = PipelineDefinition<BsonDocument, BsonDocument>.Create(new BsonDocument("$match", expression));

            candidateAgg = candidateAgg.Lookup(companyCollection, let, pipeline, "Array");

            candidateAgg = candidateAgg.Unwind("Array");

            FilterDefinition<BsonDocument> sectorFilters = Builders<BsonDocument>.Filter.In("Array.sectors", sectors);

            candidateAgg = candidateAgg.Match(sectorFilters);

            var fields = new BsonDocument
            {
                {  "_id", "$_id" },
                {  "CompanyId", new BsonDocument{ {"$first", "$CompanyId"} } }
            };

            candidateAgg = candidateAgg.Group(fields);

            IEnumerable<BsonDocument> resultList = candidateAgg.ToList();

            Assert.NotNull(resultList);
        }

        [Fact]
        public void Should_FetchCandidateListAfterGroupBy_When_FieldGroup()
        {
            IMongoDatabase database = GetMongoDatabase();
            IMongoCollection<BsonDocument> products = database.GetCollection<BsonDocument>("Products");

            var agg = products.Aggregate();

            var fields = new BsonDocument
            {
                { "Name",  "$Name"} ,
                { "Country",  "$Country"},
                { "Tags", "$Tags"}
            };

            var id = new BsonDocument
            {
                {"_id",  fields}
            };

            var r = agg.Group(id);
            r = r.Match(Builders<BsonDocument>.Filter.In("_id.Country", new[] { "Bangladesh", "India" }));

            var rr = r.ToList();

            foreach (var x in rr)
            {
                var xx = x[0];
                Console.WriteLine(xx);
            }

            var resultList = r.ToList();

            Assert.NotNull(resultList);
        }

        private IMongoDatabase GetMongoDatabase()
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017");

            return client.GetDatabase("test_database");
        }

    }
}
