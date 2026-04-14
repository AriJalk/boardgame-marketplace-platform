CREATE TABLE IF NOT EXISTS public.users
(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    display_name VARCHAR(50) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE
);

CREATE SCHEMA IF NOT EXISTS internal;

CREATE TABLE IF NOT EXISTS internal.seed_markers (
    name TEXT PRIMARY KEY,
    seeded_at TIMESTAMP NOT NULL DEFAULT NOW()
);