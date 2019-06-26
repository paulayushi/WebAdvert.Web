using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using System.Net;

namespace WebAdvert.Web.Repositories
{
    public class S3FileUploader : IFileUploader
    {
        private readonly IConfiguration _config;

        public S3FileUploader(IConfiguration config)
        {
            _config = config;
        }
        public async Task<bool> FileUploader(string filename, Stream filestream)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentException("File name must be specfied.");
            var bucketName = _config.GetValue<string>("ImageBucket");

            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
            {
                if (filestream != null && filestream.Length > 0)
                {
                    if (filestream.CanSeek)
                    {
                        filestream.Seek(0, SeekOrigin.Begin);
                    }
                }
                var request = new PutObjectRequest
                {
                    AutoCloseStream = true,
                    BucketName = bucketName,
                    InputStream = filestream,
                    Key = filename
                };

                var response = await client.PutObjectAsync(request).ConfigureAwait(false);
                return response.HttpStatusCode == HttpStatusCode.OK;
            }            
        }
    }
}
