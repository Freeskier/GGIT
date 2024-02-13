package pl.ggit.pharmaceuticathesaurus.s3.exceptions;

public class S3PresignedException extends RuntimeException {
  public S3PresignedException(Exception e) {
    super("Problem z generowaniem url.", e);
  }
}
