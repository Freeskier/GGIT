package pl.ggit.pharmaceuticathesaurus.s3.exceptions;

public class S3PutException extends RuntimeException {
  public S3PutException(Exception e) {
    super("Błąd podaczas wgrywania pliku.", e);
  }
}
