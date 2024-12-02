using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

public class MinIOService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public MinIOService(IConfiguration configuration)
    {
        var endpoint = configuration["MinIO:Endpoint"];
        var accessKey = configuration["MinIO:AccessKey"];
        var secretKey = configuration["MinIO:SecretKey"];
        

        var config = new AmazonS3Config
        {
            ServiceURL = endpoint,
            ForcePathStyle = true // Yêu cầu khi kết nối với MinIO
        };

        _s3Client = new AmazonS3Client(accessKey, secretKey, config);
    }

    public async Task<bool> UploadFileAsync(Stream fileStream, string fileName, string _bucketName)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = "application/octet-stream" // hoặc kiểu phù hợp
            };


            var response = await _s3Client.PutObjectAsync(putRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi upload file: {ex.Message}");
            return false;
        }
    }


    public async Task<List<string>> ListFilesAsync()
    {
        var files = new List<string>();
        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName
            };

            var response = await _s3Client.ListObjectsV2Async(request);

            foreach (var entry in response.S3Objects)
            {
                files.Add(entry.Key); // Lấy tên file
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching files: {ex.Message}");
        }

        return files;
    }
    // Phương thức liệt kê tất cả bucket
    public async Task<List<string>> ListBucketsAsync()
    {
        var buckets = new List<string>();
        var response = await _s3Client.ListBucketsAsync();
        foreach (var bucket in response.Buckets)
        {
            buckets.Add(bucket.BucketName);
        }
        return buckets;
    }

    // Phương thức tạo bucket mới
    public async Task<bool> CreateBucketAsync(string bucketName)
    {
        try
        {
            var request = new PutBucketRequest { BucketName = bucketName };
            var response = await _s3Client.PutBucketAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi tạo bucket: {ex.Message}");
            return false;
        }
    }

    // Phương thức xóa bucket
    public async Task<bool> DeleteBucketAsync(string bucketName)
    {
        try
        {
            var response = await _s3Client.DeleteBucketAsync(bucketName);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi xóa bucket: {ex.Message}");
            return false;
        }
    }

    // Phương thức lấy danh sách file từ một bucket cụ thể
    public async Task<List<string>> ListFilesInBucketAsync(string bucketName)
    {
        var files = new List<string>();
        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            files.AddRange(response.S3Objects.Select(o => o.Key));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi lấy file từ bucket {bucketName}: {ex.Message}");
        }

        return files;
    }



}
