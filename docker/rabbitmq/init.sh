#!/bin/sh
echo "Waiting for RabbitMQ to start..."
sleep 15

rabbitmqadmin -u "$RABBITMQ_DEFAULT_USER" -p "$RABBITMQ_DEFAULT_PASS" \
    --host=localhost \
    --port=15672 \
    declare exchange name=sale.events type=topic durable=true

rabbitmqadmin -u "$RABBITMQ_DEFAULT_USER" -p "$RABBITMQ_DEFAULT_PASS" \
    --host=localhost \
    --port=15672 \
    declare queue name=sale.created.queue durable=true

rabbitmqadmin -u "$RABBITMQ_DEFAULT_USER" -p "$RABBITMQ_DEFAULT_PASS" \
    --host=localhost \
    --port=15672 \
    declare queue name=sale.modified.queue durable=true

rabbitmqadmin -u "$RABBITMQ_DEFAULT_USER" -p "$RABBITMQ_DEFAULT_PASS" \
    --host=localhost \
    --port=15672 \
    declare binding source=sale.events destination=sale.created.queue \
    destination_type=queue routing_key="sale.created"

rabbitmqadmin -u "$RABBITMQ_DEFAULT_USER" -p "$RABBITMQ_DEFAULT_PASS" \
    --host=localhost \
    --port=15672 \
    declare binding source=sale.events destination=sale.modified.queue \
    destination_type=queue routing_key="sale.modified"