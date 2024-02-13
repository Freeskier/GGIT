package pl.ggit.pharmaceuticathesaurus.s3;

import io.minio.*;
import io.minio.errors.*;
import io.minio.http.Method;
import lombok.RequiredArgsConstructor;
import org.apache.commons.compress.utils.IOUtils;
import org.springframework.stereotype.Repository;
import pl.ggit.pharmaceuticathesaurus.s3.exceptions.S3GetException;
import pl.ggit.pharmaceuticathesaurus.s3.exceptions.S3PresignedException;
import pl.ggit.pharmaceuticathesaurus.s3.exceptions.S3PutException;
import pl.ggit.pharmaceuticathesaurus.s3.exceptions.S3RemoveException;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.util.Optional;
import java.util.concurrent.TimeUnit;

@Repository
@RequiredArgsConstructor
class MinioFileRepository implements S3FileRepository {
  private final MinioClient minioClient;
  private static final int MIN_PART_SIZE = 5242880;
  private static final int UNKNOWN_FILE_SIZE = -1;

  @Override
  public void upload(String s3BucketName, String s3ObjectName, InputStream inputStream) {
    upload(s3BucketName, s3ObjectName, inputStream, UNKNOWN_FILE_SIZE);
  }

  @Override
  public void upload(String s3BucketName, String s3ObjectName, InputStream inputStream, long fileSize) {
    try {
      minioClient.putObject(
          PutObjectArgs.builder()
              .bucket(s3BucketName)
              .object(s3ObjectName)
              .stream(inputStream, fileSize, MIN_PART_SIZE)
              .build());
    } catch (MinioException | IOException | NoSuchAlgorithmException | InvalidKeyException e) {
      throw new S3PutException(e);
    }
  }

  @Override
  public InputStream download(String s3BucketName, String s3ObjectName) {
    try {
      return minioClient.getObject(GetObjectArgs.builder()
          .bucket(s3BucketName)
          .object(s3ObjectName)
          .build());
    } catch (MinioException | IOException | InvalidKeyException | NoSuchAlgorithmException e) {
      throw new S3GetException(e);
    }
  }

  @Override
  public void download(String s3BucketName, String s3ObjectName, OutputStream output) {
    try (InputStream input = download(s3BucketName, s3ObjectName)) {
      IOUtils.copy(input, output);
    } catch (IOException e) {
      throw new S3GetException(e);
    }
  }

  @Override
  public void delete(String s3BucketName, String s3ObjectName) {
    try {
      minioClient.removeObject(
          RemoveObjectArgs.builder().bucket(s3BucketName).object(s3ObjectName).build());
    } catch (ErrorResponseException | XmlParserException | ServerException | InsufficientDataException |
             InternalException | InvalidKeyException | InvalidResponseException | NoSuchAlgorithmException |
             IOException e) {
      throw new S3RemoveException(e);
    }
  }

  @Override
  public String getPresignedDownloadUrl(String s3BucketName, String s3ObjectName, int expireInSeconds) {
    try {
      return minioClient.getPresignedObjectUrl(
          GetPresignedObjectUrlArgs.builder()
              .method(Method.GET)
              .bucket(s3BucketName)
              .object(s3ObjectName)
              .expiry(expireInSeconds, TimeUnit.SECONDS)
              .build());
    } catch (InvalidKeyException | MinioException | NoSuchAlgorithmException | IOException e) {
      throw new S3PresignedException(e);
    }
  }

  @Override
  public String getPresignedUploadUrl(String s3BucketName, String s3ObjectName, int expireInSeconds) {
    try {
      return minioClient.getPresignedObjectUrl(
          GetPresignedObjectUrlArgs.builder()
              .method(Method.PUT)
              .bucket(s3BucketName)
              .object(s3ObjectName)
              .expiry(expireInSeconds, TimeUnit.SECONDS)
              .build());
    } catch (InvalidKeyException | MinioException | NoSuchAlgorithmException | IOException e) {
      throw new S3PresignedException(e);
    }
  }

  @Override
  public String getPresignedDeleteUrl(String s3BucketName, String s3ObjectName, int expireInSeconds) {
    try {
      return minioClient.getPresignedObjectUrl(
          GetPresignedObjectUrlArgs.builder()
              .method(Method.DELETE)
              .bucket(s3BucketName)
              .object(s3ObjectName)
              .expiry(expireInSeconds, TimeUnit.SECONDS)
              .build());
    } catch (InvalidKeyException | MinioException | NoSuchAlgorithmException | IOException e) {
      throw new S3PresignedException(e);
    }
  }

  @Override
  public Optional<StatS3Object> findStatObject(String s3BucketName, String s3ObjectName) {
    try {
      var statObject = minioClient.statObject(StatObjectArgs.builder()
          .bucket(s3BucketName)
          .object(s3ObjectName).build());
      return Optional.of(new StatS3Object(statObject.contentType(), statObject.size()));
    } catch (ErrorResponseException e) {
      return Optional.empty();
    } catch (ServerException | InsufficientDataException | IOException | NoSuchAlgorithmException |
             InvalidKeyException | InvalidResponseException | XmlParserException | InternalException e) {
      throw new S3GetException(e);
    }
  }

}