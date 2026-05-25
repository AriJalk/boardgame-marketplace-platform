# Listings service

## Overview

The Listings Service is a bounded-context microservice within the Boardgame Market distributed system.
This is the most critical service in the system regarding business rules and marketplace workflows.

It is responsible for:

- Managing listings
- Managing sales lifecycles

The service is designed to operate as part of a **gateway-orchestrated distributed system**, and is not intended for direct external access.

> NOTE:
> This repository is part of a snapshot-style public monorepo used for navigation and demonstration purposes.
> The primary development workflow for this service uses a separate private Azure DevOps repository with its own CI/CD pipeline.

---

## Responsibilities

This service owns the following domain concerns:

- Manage Listing Groups and all related domain entities for sellers and buyers
- Apply business rules around listing lifecycle and buyer/seller interactions
- Serve listing data in usable aggregates depending on context

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
	- Application services and use cases
- Infrastructure layer

### Design Principles

- Clean separation of concerns
- Command / use-case driven business logic
- No direct service-to-service communication
- Stateless API design

---

## Service Concepts
### ListingGroup

A complete sale of one or more listings by a single seller.

---
### Listing
A unit of sale representing either a single item or a bundle of related items.

---

### ListingItem
Individual item in a listing, references catalog items by identifier.

---

### ListingQueueEntry
A subscription entry for a specific listing by a buyer. Used by the seller in two ways:
- A time based queue: orders queue entries by time of subscription for a "First come first serve" model.
- A priority based queue: entries from the time queue can be added to a secondary priority queue for various cases where the time based queue is insufficient for a trade. Items in the priority queue can be freely reordered.

---
### ListingReservation
Represents a temporary reservation agreement between a seller and a buyer based on queue selection rules.

---
### ListingSale
A completed sale based on an existing ListingReservation agreement.

---
## External Communication


This service is accessed via:

- Market Gateway (primary entry point)
- Swagger and API testing tools such as Postman (development only)

---

## Persistence

Supported modes:

### Database-backed mode
- PostgreSQL
- Full persistence support
- Supports seeding with idempotency guarantees  

### In-memory mode
- Used for local development and testing
- No persistence guarantees

---

## Seeding

Seeding behavior:

- Controlled via `IS_SEEDING`
- Requires `SEED_ROOT`

### Notes:

- Database-backed seeding uses idempotency checks to avoid duplicate inserts
- In-memory seeding is not supported for this service

---

## Environment Variables

### General
| Variable | Required | Description |
|----------|----------|-------------|
| `IN_MEMORY` | Yes | Enables in-memory mode |
| `IS_SEEDING` | Yes | Enables data seeding |
| `SEED_ROOT` | Conditional | Required when seeding is enabled |

---

### Database

| Variable | Required | Description |
|----------|----------|-------------|
| `POSTGRES_HOST` | Conditional | DB host |
| `POSTGRES_PORT` | Conditional | DB port |
| `POSTGRES_DATABASE` | Conditional | Database name |
| `POSTGRES_USER` | Conditional | Username |
| `POSTGRES_PASSWORD` | Conditional | Password |

---

## Runtime Modes


### Full System Mode (Docker / Kubernetes)

- Requires PostgreSQL
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

### Infrastructure Dependencies 
- PostgreSQL (when using database-backed mode)

---

## Key Design Decisions

- No event-driven messaging (synchronous architecture)
- No service-to-service communication (gateway-only orchestration)

---

## Limitations

- Not intended for standalone production use
- Requires full system context for real workflows
- In-memory seeding is not supported due to the current Dapper-based seeding implementation

---

## Testing

- Unit tests: xUnit
- Manual testing: Swagger / Postman / Gateway flows

---

## Docker

### Build

```bash
docker build -t arieljalkiver/boardgame-market:listings -f ./BoardGameMarket.ListingsService/src/BoardGameMarket.ListingsService.API/Dockerfile ./BoardGameMarket.ListingsService
```
