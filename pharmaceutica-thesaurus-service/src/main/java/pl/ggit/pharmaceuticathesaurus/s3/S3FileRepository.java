package pl.ggit.pharmaceuticathesaurus.s3;


import java.io.InputStream;
import java.io.OutputStream;
import java.util.Optional;

public interface S3FileRepository {

  void upload(String s3BucketName, String s3ObjectName, InputStream inputStream);

  void upload(String s3BucketName, String s3ObjectName, InputStream inputStream, long fileSize);

  InputStream download(String s3BucketName, String s3ObjectName);

  void download(String s3BucketName, String s3ObjectName, OutputStream output);

  void delete(String s3BucketName, String s3ObjectName);

  String getPresignedDownloadUrl(String s3BucketName, String s3ObjectName, int expireInSeconds);

  String getPresignedUploadUrl(String s3BucketName, String s3ObjectName, int expireInSeconds);

  String getPresignedDeleteUrl(String s3BucketName, String s3ObjectName, int expireInSeconds);

  Optional<StatS3Object> findStatObject(String s3BucketName, String s3ObjectName);
}
