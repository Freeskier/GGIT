FROM bitnami/spark:3.2.4

COPY script.py /app/
COPY elasticsearch-spark.jar /opt/bitnami/spark/jars/elasticsearch-spark.jar

WORKDIR /app

CMD ["spark-submit", "--jars", "/opt/bitnami/spark/jars/elasticsearch-spark.jar", "--packages", "org.apache.spark:spark-sql-kafka-0-10_2.12:3.2.4", "script.py"]