using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Utitlity.Nuget.Packages.Aws
{
    /* 
       Nuget package dependencies :  
       1) AWSSDK.Core 
       2)AWSSDK.S3 
    */

    /// <summary>
    /// Amazon support OAUTH protocol. So to understand OAuth protocol is prerequisit
    /// </summary>
    public class S3Tests
    {
        private const string RootBucketName = "RootBucket";
        private const string RootFolderName = "RootFolder";
        private readonly AmazonS3Client client;

        public S3Tests()
        {
            client = CreateAwsClient();
        }

        [Fact]
        public async Task Should_GetFile()
        {
            var fileKey = @"devs/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.pdf";

            var request = new GetObjectRequest
            {
                Key = fileKey,
                BucketName = RootFolderName
            };

            GetObjectResponse response = await client.GetObjectAsync(request);

            client.Dispose();
        }


        [Fact]
        public async Task Should_FetchFileNameList()
        {
            var list = await client.ListBucketsAsync();
            var bucketName = "myBucket/devs";
            S3Bucket bucket = list.Buckets.Where(item => item.BucketName == bucketName).FirstOrDefault();

            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Delimiter = "/"
            };

            ListObjectsV2Response objectList = await client.ListObjectsV2Async(request);

            foreach (S3Object obj in objectList.S3Objects)
            {
                Console.WriteLine(obj.Key);
            }

        }

        [Fact]
        public void Should_GeneratePresignedUrl()
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = "ecap-falcon",
                //Key = "secret_plans.txt",
                Expires = DateTime.Now.AddHours(1),
                Protocol = Protocol.HTTP
            };
            string url = client.GetPreSignedURL(request);
        }

        [Fact]
        public async Task Should_TransferUtitlity()
        {
            var utility = new TransferUtility(client);

            var request = new TransferUtilityDownloadRequest
            {
                BucketName = "ecap-falcon",
                Key = "03CB552E-A3FE-4EF9-A600-909FBA63E900/000c4fcd95e34d90958a5605f2976b6e/461d8d9c-cd33-425e-bb11-3f78b5f98089/000c4fcd95e34d90958a5605f2976b6e.png",
                FilePath = @"d:\abcd.png",

            };

            await utility.DownloadAsync(request);

            //client.CopyObjectAsync();
        }

        [Fact]
        public async Task Should_Rename()
        {
            var sourceFileId = @"test03122020.pdf";
            var sourceFileName = "test03122020.pdf";
            string sourceFileVersionId = null;
            var targetFileId = "82D07BF9-CC75-477D-A286-F1A19A9FA0EA";
            string targetFileName = "test03122021.pdf";

            await MoveFileAsync(sourceFileId, sourceFileName, sourceFileVersionId, targetFileId, targetFileName);

        }

        private async Task MoveFileAsync(string sourceFileId, string sourceFileName, string sourceFileVersionId, string targetFileId, string targetFileName)
        {
            var sourceFileKey = CreateFileKey(sourceFileId, sourceFileVersionId, sourceFileName);
            var targetFileKey = CreateFileKey(targetFileId, targetFileId, targetFileName);

            var copyRequest = new CopyObjectRequest
            {
                SourceBucket = RootFolderName,
                DestinationBucket = RootFolderName,
                SourceKey = sourceFileKey,
                DestinationKey = targetFileKey,
                SourceVersionId = sourceFileVersionId
            };

            CopyObjectResponse copyResponse = await client.CopyObjectAsync(copyRequest);
            if (copyResponse.HttpStatusCode == System.Net.HttpStatusCode.OK || copyResponse.HttpStatusCode == System.Net.HttpStatusCode.Created)
            {
                // await DeleteFileAsync(sourceFileId, new string[] { sourceFileVersionId }, sourceFileName, AccessModifier.Public);
            }
        }

        private string CreateFileKey(string fileId, string fileVersionId, string fileName)
        {
            var fileKey = $@"dev/{RootBucketName}/{fileId}/{fileVersionId}/{fileName}";

            return fileKey;
        }

        private async Task DeleteFileAsync(string fileId, IEnumerable<string> fileVersionIds, string fileName, AccessModifier accessModifier)
        {
            var KeyVersions = fileVersionIds.Select(fileVersionId => CreateFileKey(fileId, fileVersionId, fileName)).Select(fileKey => new KeyVersion { Key = fileKey, VersionId = null }).ToList();

            var deleteObjectsRequest = new DeleteObjectsRequest()
            {
                Objects = KeyVersions,
                BucketName = RootFolderName
            };

            var deleteObjectsResponse = await client.DeleteObjectsAsync(deleteObjectsRequest);
        }

        /// <summary>
        /// Authentication is must before access aws cloud resources and that is OAuth protocol
        /// </summary>
        /// <returns></returns>
        private AmazonS3Client CreateAwsClient()
        {
            var accessKey = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; // fake. You have to use your own
            var secrateKey = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb"; // fake

            var options = new AmazonS3Config
            {
                SignatureVersion = "v4",
                RegionEndpoint = RegionEndpoint.EUCentral1,
            };
            var client = new AmazonS3Client(accessKey, secrateKey, options);

            return client;
        }

        public enum AccessModifier : int
        {
            Private = 0, Public, Secure
        }
    }
}
