# Telematics Data Processing Microservice

A .NET microservice system for ingesting, storing, and querying vehicle telemetry data. The project demonstrates REST APIs, gRPC communication, Kafka event publishing, PostgreSQL persistence, Docker orchestration, and automated tests in a layered C# solution.

## Why This Project Matters

This project is built around a realistic backend workflow: vehicles produce telemetry events, the system stores those records, and clients can query current status or historical data. It shows experience with service boundaries, infrastructure configuration, asynchronous messaging, and maintainable .NET project structure.

## Tech Stack

- C# and .NET
- ASP.NET Core Web API
- gRPC
- Kafka
- PostgreSQL
- Entity Framework Core
- Docker Compose
- xUnit

## Solution Structure

- `TelemetryData.Api`: REST API for telemetry records and event publishing.
- `TelemetryData.GrpcService`: gRPC service for vehicle status queries.
- `TelemetryData.ConsumerService`: background service for processing telemetry events.
- `TelemetryData.Domain`: shared domain models and interfaces.
- `TelemetryData.Infrastructure`: database and service implementations.
- `TelemetryData.Tests`: API tests.

## Features

- Create and retrieve telemetry records through a REST API.
- Query real-time vehicle status through gRPC.
- Publish telemetry events to a Kafka topic.
- Store telemetry records in PostgreSQL with Entity Framework Core.
- Run API, gRPC service, consumer service, Kafka, Zookeeper, and PostgreSQL with Docker Compose.
- Keep domain, infrastructure, API, and test concerns separated.

## Getting Started

Create a `.env` file in the project root:

```env
POSTGRES_ROOT_PASSWORD=your_password
POSTGRES_DB_NAME=telemetry_data
KAFKA_BOOTSTRAP_SERVERS=broker:29092
KAFKA_GROUPID=telemetry-consumer-group
```

Start the services:

```bash
docker compose up --build
```

The REST API is exposed at:

```text
http://localhost:5046
```

## Example API Calls

Get all telemetry records:

```http
GET /Telemetry
```

Create a telemetry record:

```http
POST /Telemetry
Content-Type: application/json
```

Publish a telemetry event:

```http
POST /Telemetry/event
Content-Type: application/json
```

Get vehicle status:

```http
GET /Telemetry/status/{vehicleId}
```

## Development Notes

This repository is a backend-focused portfolio project. The most important implementation details are the service boundaries, Dockerized infrastructure, API/gRPC communication, and testable project layout.
