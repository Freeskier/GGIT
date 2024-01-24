FROM bitnami/spark:latest

COPY script.py /app/

COPY elasticsearch-hadoop-8.11.0.jar /opt/bitnami/spark/jars/

WORKDIR /app

CMD ["spark-submit", "--jars", "/opt/bitnami/spark/jars/elasticsearch-hadoop-8.11.0.jar", "--packages", "org.apache.spark:spark-sql-kafka-0-10_2.12:3.5.0", "script.py"]