package pl.ggit.pharmaceuticathesaurus.s3.exceptions;

public class S3RemoveException extends RuntimeException {
  public S3RemoveException(Exception e) {
    super("Błąd usuwania pliku.", e);
  }
}
