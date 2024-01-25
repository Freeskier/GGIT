from pyspark.sql import SparkSession
import os

kafka_bootstrap_servers = os.environ.get("KAFKA_BOOTSTRAP_SERVERS", "kafka:29092")
kafka_topic = os.environ.get("KAFKA_TOPIC", "topic")
es_host = os.environ.get('ELASTICSEARCH_HOST', 'elasticsearch')
es_port = os.environ.get('ELASTICSEARCH_PORT', '9200')

spark = SparkSession.builder.appName("KafkaToElasticsearch").getOrCreate()


df = spark \
    .read \
    .format("kafka") \
    .option("kafka.bootstrap.servers", kafka_bootstrap_servers) \
    .option("startingOffsets", "earliest") \
    .option("endingOffsets", "latest") \
    .option("subscribe", kafka_topic) \
    .load()

df = df.selectExpr("CAST(value AS STRING)")


query = df.write \
    .format("org.elasticsearch.spark.sql") \
    .option("checkpointLocation", "/tmp") \
    .option("es.nodes", es_host) \
    .option("es.port", es_port) \
    .option("es.resource", "logs-index") \
    .save()

spark.stop()