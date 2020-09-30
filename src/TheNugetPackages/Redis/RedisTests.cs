using NPOI.SS.Formula.Functions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HelperNuget.Packages.Redis
{
    /* Nuget Dependency: StackExchange.Redis  
     * Need to setup a local redis server for complete test.   
    */

    /// <summary>
    /// Adding/Deleting/Reading value from Redis server
    /// </summary>
    public class RedisTests
    {
        private const string RedisConnectionString = "127.0.0.1:6379,abortConnect=false,connectTimeout=50000,syncTimeout=5000";
        private readonly IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(RedisConnectionString);
        private readonly IDatabase database;

        public RedisTests()
        {
            database = connectionMultiplexer.GetDatabase();
        }

        [Fact]
        public async Task ShouldWriteToRedis()
        {   
            bool succeed = await database.SetAddAsync("Mykey", "MyValue");

            Assert.True(succeed);
        }

        [Fact]
        public void ShouldReadFromRedis()
        {
            string redisValue = database.StringGet("MyKey");

            Assert.Equal("MyValue", redisValue);
        }

        [Fact]
        public void ShouldDeleteKeyFromRedis()
        {
            bool succeed = database.KeyDelete("MyKey");

            Assert.True(succeed);
        }

    }
}

