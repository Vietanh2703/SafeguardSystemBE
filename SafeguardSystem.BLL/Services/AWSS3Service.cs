using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.AWSSettings;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.BLL.Services;

public class AWSS3Service : IAWSS3Service
{
    private readonly IAmazonS3 _awsS3Client;
    private readonly string _bucketName;
    private readonly double _defaultDuration = 60;
    private readonly IUnitOfWork _unitOfWork;

    public AWSS3Service(IOptions<AwsS3Setting> awsS3Settings, IAmazonS3 amazonS3, IUnitOfWork unitOfWork)
    {
        _bucketName = awsS3Settings.Value.BucketName;
        _awsS3Client = amazonS3;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDTO> DownloadFileAsync(string fileName)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };
            // Find user's Downloads folder
            var downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";

            var localPath = Path.Combine(downloadsFolder, fileName);

            await _awsS3Client.DownloadToFilePathAsync(request.BucketName, request.Key, localPath, null);
            return new ResponseDTO("Download file" + fileName + " successfully", 200, true);
        }
        catch (Exception e)
        {
            return new ResponseDTO(e.Message, 500);
        }
    }

    public async Task<ResponseDTO> DeleteFileAsync(string fileName)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };
            await _awsS3Client.DeleteObjectAsync(request);
            return new ResponseDTO("Delete file " + fileName + " Successfully", 200, true);
        }
        catch (Exception e)
        {
            return new ResponseDTO(e.Message, 500);
        }
    }

    public async Task<ResponseDTO> UploadFileAsync(string key, Stream fileStream, string contentType)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = contentType
            };

            var response = await _awsS3Client.PutObjectAsync(putRequest);
            if (response.HttpStatusCode == HttpStatusCode.OK)
                return new ResponseDTO("Upload Successful", 200, true, key);

            return new ResponseDTO("Upload Failed", (int)response.HttpStatusCode);
        }
        catch (Exception e)
        {
            var detailedMessage =
                $"Exception Type: {e.GetType().Name}, Message: {e.Message}, StackTrace: {e.StackTrace}";
            return new ResponseDTO(detailedMessage, 500);
        }
    }

    public async Task<ResponseDTO> DefaultUploadFileAsync(IFormFile file, string userId)
    {
        if (file == null || file.Length == 0)
            return new ResponseDTO("Invalid file", 400);

        try
        {
            using var newMemoryStream = new MemoryStream();
            await file.CopyToAsync(newMemoryStream);

            var key = $"{userId}_{file.FileName}";
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = key,
                BucketName = _bucketName,
                ContentType = file.ContentType
            };

            var fileTransferUtility = new TransferUtility(_awsS3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);

            return new ResponseDTO("Upload Successful", 200, true, key);
        }
        catch (Exception e)
        {
            return new ResponseDTO(e.Message, 500);
        }
    }


    public async Task<ResponseDTO> GetPreSignedURLAsync(string key)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddMinutes(_defaultDuration),
                Verb = HttpVerb.GET,
                ResponseHeaderOverrides = new ResponseHeaderOverrides
                {
                    ContentDisposition = "inline"
                }
            };
            var url = _awsS3Client.GetPreSignedURL(request);
            return new ResponseDTO("Pre-signed URL generated successfully", 200, true, url);
        }
        catch (Exception e)
        {
            return new ResponseDTO(e.Message, 500);
        }
    }

    public async Task<ResponseDTO> ListUserFilesAsync(string userId)
    {
        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = $"{userId}_"
            };

            var response = await _awsS3Client.ListObjectsV2Async(request);

            if (response.S3Objects.Count == 0) return new ResponseDTO("No files found for the user", 404);

            var fileNames = response.S3Objects.Select(o => o.Key).ToList();
            return new ResponseDTO("Files retrieved successfully", 200, true, fileNames);
        }
        catch (Exception e)
        {
            return new ResponseDTO(e.Message, 500);
        }
    }
}