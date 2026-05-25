# Catalog service

## Overview

The Catalog Service is a bounded-context microservice within the Boardgame Market distributed system.

It is responsible for:

- Managing internally stored board-game catalog items

The service is designed to operate as part of a **gateway-orchestrated distributed system**, and is not intended for direct external access.

> NOTE:
> This repository is part of a snapshot-style public monorepo used for navigation and demonstration purposes.
> The primary development workflow for this service uses a separate private Azure DevOps repository with its own CI/CD pipeline.

---

## Responsibilities

This service owns the following domain concerns:

- Maintain CatalogItem entities
- Serve item info
- Fetch, enrich, and internally store catalog items from BoardGameGeek

It does NOT:

- Communicate directly with other services
- Expose cross-service contracts
- Handle cross-domain orchestration (handled by Gateway)

---

## Architecture

### Internal Structure

- Controllers / API layer
- Core Application layer which contains:
	- Domain layer
	- Data services and use cases
- Infrastructure layer

### Design Principles

- Clean separation of concerns
- Command / use-case driven business logic
- No direct service-to-service communication
- Stateless API design
- BoardGameGeek metadata is cached internally after retrieval to minimize repeated external API calls

---

## External Communication


This service is accessed via:

- Market Gateway (primary entry point)
- Swagger and API testing tools such as Postman (development only)

### External integrations:

- BoardGameGeek API

---

## Persistence

Supported modes:

### Database-backed mode
- MongoDB
- Full persistence support
- Supports seeding with idempotency guarantees  

### In-memory mode
- Used for local development and testing
- No persistence guarantees
- Supports seeding for development scenarios

---

## Seeding

Seeding behavior:

- Controlled via `IS_SEEDING`
- Requires `SEED_ROOT`

### Notes:

- Database-backed seeding uses idempotency checks to avoid duplicate inserts
- In-memory seeding is always re-initialized per application start

---

## Environment Variables

### General
| Variable | Required | Description |
|----------|----------|-------------|
| `IN_MEMORY` | Yes | Enables in-memory mode |
| `IS_SEEDING` | Yes | Enables data seeding |
| `SEED_ROOT` | Conditional | Required when seeding is enabled |

---

### BoardGameGeek Integration

| Variable | Required | Description |
|----------|----------|-------------|
| `BGGCLIENT__SERVICEAPIKEY` | Optional | API token for external BGG integration |
| `LOCAL_BGG_CLIENT` | Yes | Must be `true` if no API token is provided |

---

### Database

| Variable | Required | Description |
|----------|----------|-------------|
| `MONGODBCONFIGURATION__MONGOHOST` | Conditional | MongoDB host |
| `MONGODBCONFIGURATION__MONGOPORT` | Conditional | MongoDB port |
| `MONGODBCONFIGURATION__MONGODATABASE` | Conditional | MongoDB database |
| `MONGODBCONFIGURATION__COLLECTIONNAME` | Conditional | MongoDB collection |

---

## Runtime Modes


### Full System Mode (Docker / Kubernetes)

- Requires MongoDB
- Runs as part of full system orchestration

---

### Local Development Mode (IDE)

- Can run in isolation
- Uses in-memory or mocked dependencies
- Limited external integration support

---

## API Documentation

- Swagger available at:
  - `/swagger` (local only)

All endpoints are intended to be consumed via the Gateway in production-like environments.

---

## Dependencies

### Internal

- None (services do not communicate directly)

### External

- BoardGameGeek API

### Infrastructure Dependencies
- MongoDB (when using database-backed mode)

---

## Key Design Decisions

- No event-driven messaging (synchronous architecture)
- No service-to-service communication (gateway-only orchestration)

---

## Limitations

- Not intended for standalone production use
- Requires full system context for real workflows

---

## Testing

- Unit tests: xUnit
- Manual testing: Swagger / Postman / Gateway flows

---

## Docker

### Build

```bash
docker build -t arieljalkiver/boardgame-market:catalog -f ./BoardGameMarket.CatalogService/src/BoardGameMarket.CatalogService.API/Dockerfile ./BoardGameMarket.CatalogService
```
