using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Helper.Nuget.Packages.Ftp
{
    /* Nuget Package Dependency: FluentFTP  */

    /// <summary>
    /// You have to setup/get your permissble Ftp server. It is prerequisit
    /// </summary>
    public class FtpTests
    {
        [Fact]
        public async Task ShouldUploadFileToFtpServerPath()
        {
            var ftpClient = new FtpClient("127.0.0.1", "guest", "guest");

            var profile = new FtpProfile();
            profile.Host = "127.0.0.1";
            profile.RetryAttempts = 3;

            string fileTobeUploaded = @"myDirectory/2.jpg";

            var fileInfo = new FileInfo(fileTobeUploaded);
            using FileStream fileStream = fileInfo.OpenRead();

            FtpStatus ftpStatus = await ftpClient.UploadAsync(fileStream, "/myremotpath/");

            Assert.True(ftpStatus.IsSuccess());
        }

        [Fact]
        public async Task ShouldDownloadEntityDirectoryFromFtpServer()
        {
            var ftpClient = new FtpClient("127.0.0.1", "guest", "guest");

            List<FtpResult> ftpStatus = await ftpClient.DownloadDirectoryAsync("c:/Downloads/", "/in");

            Assert.False(ftpStatus.IsBlank());
        }
    }
}
