package pl.ggit.pharmaceuticathesaurus.s3;

import io.minio.MinioClient;
import lombok.RequiredArgsConstructor;
import okhttp3.OkHttpClient;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.util.concurrent.TimeUnit;

@Configuration
@RequiredArgsConstructor
class MinioConfig {
  @Value("${minio.access.name}")
  private String accessName;
  @Value("${minio.access.secret}")
  private String accessSecret;
  @Value("${minio.url}")
  private String minioUrl;

  @Bean
  public MinioClient generateMinioClient() {
    try {
      OkHttpClient httpClient = new OkHttpClient.Builder()
          .connectTimeout(10, TimeUnit.MINUTES)
          .writeTimeout(10, TimeUnit.MINUTES)
          .readTimeout(30, TimeUnit.MINUTES)
          .build();

      return MinioClient.builder()
          .endpoint(minioUrl)
          .httpClient(httpClient)
          .credentials(accessName, accessSecret)
          .build();

    } catch (Exception e) {
      throw new RuntimeException(e.getMessage());
    }

  }

}