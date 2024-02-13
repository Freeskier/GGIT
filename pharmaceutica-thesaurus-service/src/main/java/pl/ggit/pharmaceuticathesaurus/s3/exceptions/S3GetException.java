package pl.ggit.pharmaceuticathesaurus.s3.exceptions;

public class S3GetException extends RuntimeException {
  public S3GetException(Exception e) {
    super("Błąd podaczas pobierania pliku.", e);
  }
}
