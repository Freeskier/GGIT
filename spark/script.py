from pyspark.sql import SparkSession
import os

kafka_bootstrap_servers = os.environ.get("KAFKA_BOOTSTRAP_SERVERS", "kafka:29092")
kafka_topic = os.environ.get("KAFKA_TOPIC", "topic")
es_host = os.environ.get('ELASTICSEARCH_HOST', 'elasticsearch')
es_port = os.environ.get('ELASTICSEARCH_PORT', '9200')


def main():
    spark = SparkSession.builder \
        .appName("KafkaToElasticsearchStream") \
        .config("spark.es.nodes", es_host) \
        .config("spark.es.port", es_port) \
        .getOrCreate()

    df = spark.readStream \
        .format("kafka") \
        .option("kafka.bootstrap.servers", kafka_bootstrap_servers) \
        .option("subscribe", kafka_topic) \
        .option("startingOffsets", "latest") \
        .option("auto.offset.reset", "latest") \
        .load()

    string_df = df.selectExpr("CAST(value AS STRING)")

    query = string_df.writeStream \
        .outputMode("append") \
        .format("org.elasticsearch.spark.sql") \
        .option("checkpointLocation", "/tmp") \
        .option("es.resource", "your-index") \
        .start()

    query.awaitTermination()

if __name__ == "__main__":
    main()