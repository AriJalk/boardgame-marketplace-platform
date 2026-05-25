# Infrastructure

## Overview

The Infrastructure project contains system-level deployment and bootstrap configuration for the Boardgame Market distributed system.

It is responsible for:

- Kubernetes namespace and resource setup
- Database provisioning for system services
- Deployment configuration used in CI/CD pipelines
- Seed data initialization for supported contexts

This project does NOT contain application or domain business logic.

---

## Responsibilities

This module is responsible for:

- Defining Kubernetes deployment structure for the system
- Creating and configuring required infrastructure resources
- Provisioning databases used by microservices
- Providing access to seed data where applicable

It does NOT:

- Implement application logic
- Contain service-specific domain rules
- Handle runtime request processing

---

## Structure

- Kubernetes manifests (namespaces, deployments, services)
- Database setup scripts / configurations
- CI/CD pipeline deployment definitions
- Seed data definitions for initial system state

---

## Usage

This module is typically executed via CI/CD pipelines (Azure DevOps) or local scripts during system setup.

It is not intended to be run as a standalone application.

---

## Dependencies

- Kubernetes cluster (kind or cloud)
- Database engines used by services (PostgreSQL, MongoDB, etc.)

---

## Notes

This repository acts as the infrastructure orchestration layer for the system and supports all microservices defined in the Boardgame Market architecture.