


# CONFIGURATION

## Overview

This system is a distributed backend application composed of multiple microservices.  
It is designed to run as a fully composed environment using **Docker Compose** or **Kubernetes**.

Individual services may support limited standalone execution for development purposes, but full functionality depends on external infrastructure services (databases, Redis, and authentication provider).

---

## Runtime Model (Important)

The system supports the following runtime modes:

### 1. Full System Mode (Recommended)

This mode represents the real system execution environment.

Supported runtimes:
- Docker Compose
- Kubernetes / kind

Requirements:
- PostgreSQL
- MongoDB
- Redis
- Microsoft Entra ID (authentication)

All services must be running for full functionality.

---

### 2. Partial Development Mode (IDE / Local)

- Individual services may be started from IDE
- Supports in-memory or mocked dependencies where available
- Only a subset of workflows is supported due to missing external dependencies or mocked integrations

---

## Authentication Requirement

Microsoft Entra ID is required for all full system executions, including:

- Docker Compose
- Kubernetes / kind deployments

Authentication is enforced at the Gateway layer and is required for most end-to-end workflows.

See **Runtime Model** for details.

---

## External Dependencies

- **BoardGameGeek API**
  - Optional external enrichment service for catalog data
  - Requires API token when using real client

---

## Configuration Philosophy

The system is designed to support multiple runtime configurations depending on environment needs.

Configuration flags allow switching between:

- In-memory persistence and database-backed persistence
- Seeded and non-seeded environments
- Mock and external integrations

These modes support:

- Rapid local iteration
- Isolated service testing
- Containerized development
- End-to-end deployment validation

> Note: Not all combinations of configuration flags are valid (see Runtime Constraints).

---

## Runtime Constraints

Seeding is supported in Users and Catalog services for both in-memory and database-backed persistence modes.  
In the Listings service, seeding requires database-backed persistence, as it relies on Dapper-based idempotency checks against persisted data.  
As a result, Listings does not support seeding when `IN_MEMORY = true`, regardless of deployment method (Docker Compose, Kubernetes, or IDE mode).

---

## Environment Variables

### Shared (Users / Catalog / Listings)

| Variable | Required | Description |
|----------|----------|-------------|
| `IN_MEMORY` | Yes | Enables in-memory persistence mode |
| `IS_SEEDING` | Yes | Enables data seeding on startup |
| `SEED_ROOT` | Conditional | Required when `IS_SEEDING = true` |

---

### Users Service

#### PostgreSQL

| Variable | Required | Description |
|----------|----------|-------------|
| `POSTGRES_HOST` | Conditional | Required when `IN_MEMORY = false` |
| `POSTGRES_PORT` | Conditional | Required when `IN_MEMORY = false` |
| `POSTGRES_DATABASE` | Conditional | Required when `IN_MEMORY = false` |
| `POSTGRES_USER` | Conditional | Required when `IN_MEMORY = false` |
| `POSTGRES_PASSWORD` | Conditional | Required when `IN_MEMORY = false` |

---

### Catalog Service

#### BoardGameGeek Integration

| Variable | Required | Description |
|----------|----------|-------------|
| `BGGCLIENT__SERVICEAPIKEY` | Optional | API token for external BGG integration |
| `LOCAL_BGG_CLIENT` | Yes | Must be `true` if no API token is provided |

#### MongoDB

| Variable | Required | Description |
|----------|----------|-------------|
| `MONGODBCONFIGURATION__MONGOHOST` | Conditional | Required when `IN_MEMORY = false` |
| `MONGODBCONFIGURATION__MONGOPORT` | Conditional | Required when `IN_MEMORY = false` |
| `MONGODBCONFIGURATION__MONGODATABASE` | Conditional | Required when `IN_MEMORY = false` |
| `MONGODBCONFIGURATION__COLLECTIONNAME` | Conditional | Required when `IN_MEMORY = false` |

---

### Listings Service

#### PostgreSQL

| Variable | Required | Description |
|----------|----------|-------------|
| `POSTGRES_HOST` | Conditional | Required when `IN_MEMORY = false` |
| `POSTGRES_PORT` | Conditional | Required when `IN_MEMORY = false` |
| `POSTGRES_DATABASE` | Conditional | Required when `IN_MEMORY = false` |
| `POSTGRES_USER` | Conditional | Required when `IN_MEMORY = false` |
| `POSTGRES_PASSWORD` | Conditional | Required when `IN_MEMORY = false` |

---

### Gateway Service

#### Service Discovery

| Variable | Required | Description |
|----------|----------|-------------|
| `LISTINGS_SERVICE_HOST` | Yes | Listings service endpoint |
| `LISTINGS_SERVICE_PORT` | Yes | Listings service port |
| `CATALOG_SERVICE_HOST` | Yes | Catalog service endpoint |
| `CATALOG_SERVICE_PORT` | Yes | Catalog service port |
| `USERS_SERVICE_HOST` | Yes | Users service endpoint |
| `USERS_SERVICE_PORT` | Yes | Users service port |

---

#### Redis

| Variable | Required | Description |
|----------|----------|-------------|
| `REDIS_HOST` | Yes | Redis host |
| `REDIS_PORT` | Yes | Redis port |

---

#### CORS

| Variable | Required | Description |
|----------|----------|-------------|
| `CORS__ALLOWEDORIGINS` | Optional | Allowed frontend origins |

---

## Frontend Configuration

Copy: environment.template.ts → environment.development.ts 
and configure API gateway URL and Entra ID details accordingly.

---

## General Service Requirements

### Catalog Service
- If no BGG API token is provided, `LOCAL_BGG_CLIENT` must be set to `true`

### Listings Service
- Seeding is supported only when using database-backed persistence
- In-memory mode does not support seeding due to architectural constraints

### Gateway Service
- Requires full system availability (all downstream services + Redis)
- Microsoft Entra ID authentication is required for most full system executions

---

## Local Development (IDE Mode)

### Users & Catalog Services
- Can run in-memory mode
- Supports mocked or simplified dependencies
- Intended for isolated development and debugging

### Listings Service
- Seeding supported only in database-backed mode

### Gateway Service
- Requires all downstream services and Redis for full execution
- Not intended for standalone execution

---

## Docker Compose (Recommended)

### General Behavior
- Full system orchestration through Market Gateway
- All services connected with required infrastructure
- Production-like environment for local development
- Includes authentication via Microsoft Entra ID

### Services
- Users, Catalog, Listings: fully supported
- Gateway: entry point to system

### Setup
- Run `build-for-compose.bat` before startup if images are missing
- Start via `docker-compose up`

---

## Kubernetes / kind

- Production-like deployment model
- Gateway acts as entry point
- Requires full environment configuration in manifests
### Local kind client
- Start via `deploy-local-kind.bat`, adjusting secrets placeholders if needed

### Remote kind client (full CI/CD environment)
- This is the main method used to validate deployment in a simulated full cloud environment, using Azure DevOps and seperate repositories for each service
- An ubuntu machine running kind + Azure Self Hosted Pipeline Agent serves as the simulated cloud host
- Each repo defines it's pipeline YAML, and Azure DevOps library is used for deployment specific environment variables and secrets in addition to the contained manifests.
- Once the environment is configured, full system deployment is achieved by triggering each repository’s pipeline in sequence, starting with Infrastructure followed by all services.

---

## Secrets Management

### BoardGameGeek API Token

Used by Catalog Service:

- Docker Compose:
  - `.env` or environment variables per service
- Kubernetes:
  - Stored as Kubernetes Secret

---

### Microsoft Entra ID

Used by Gateway for authentication:

- Docker Compose:
  - Environment variables or `.env`
- Kubernetes:
  - Kubernetes Secrets or ConfigMaps (depending on deployment setup)