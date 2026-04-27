CREATE TABLE public.listing_groups
(
    "id" UUID NOT NULL,
    "seller_user_id" UUID NOT NULL,
    "created_at" TIMESTAMP NOT NULL,
    "modified_at" TIMESTAMP NOT NULL,
    "details" VARCHAR(100),
    "pickup_options" VARCHAR(100) NOT NULL,
    "location" VARCHAR(100) NOT NULL,
    PRIMARY KEY ("id")
);

CREATE TABLE public.listings
(
    "id" UUID NOT NULL,
    "title" VARCHAR(100) NOT NULL,
    "listing_group_id" UUID NOT NULL,
    "status" INT NOT NULL,
    "created_at" TIMESTAMP NOT NULL,
    "modified_at" TIMESTAMP NOT NULL,
    "price" NUMERIC(10,2) NOT NULL,
    "details" VARCHAR(100),
    "seller_user_id" UUID NOT NULL,
    PRIMARY KEY ("id")
);

CREATE TABLE public.listing_items
(
    "id" UUID NOT NULL,
    "listing_id" UUID NOT NULL,
    "catalog_id" UUID NOT NULL,
    "details" VARCHAR(100),
    PRIMARY KEY ("id")
);

CREATE TABLE public.listing_queue_entries
(
    "id" UUID NOT NULL,
    "listing_id" UUID NOT NULL,
    "status" INT NOT NULL,
    "buyer_user_id" UUID NOT NULL,
    "created_at" TIMESTAMP NOT NULL,
    "modified_at" TIMESTAMP NOT NULL,
    "priority" DECIMAL(10,2),
    PRIMARY KEY ("id")
);

CREATE TABLE public.listing_reservations
(
    "id" UUID NOT NULL,
    "created_at" TIMESTAMP NOT NULL,
    "modified_at" TIMESTAMP NOT NULL,
    "status" INT NOT NULL,
    "listing_queue_entry_id" UUID NOT NULL,
    "listing_id" UUID NOT NULL,
    "seller_user_id" UUID NOT NULL,
    "buyer_user_id" UUID NOT NULL,
    PRIMARY KEY ("id")
);

CREATE TABLE public.listing_sales
(
    "id" UUID NOT NULL,
       "listing_id" UUID NOT NULL,
    "completed_at" TIMESTAMP NOT NULL,
    "reservation_id" UUID NOT NULL,
    "listing_queue_entry_id" UUID NOT NULL,
    "seller_user_id" UUID NOT NULL,
    "buyer_user_id" UUID NOT NULL,
    PRIMARY KEY ("id")
);

CREATE SCHEMA IF NOT EXISTS internal;

CREATE TABLE IF NOT EXISTS internal.seed_markers (
    name TEXT PRIMARY KEY,
    seeded_at TIMESTAMP NOT NULL DEFAULT NOW()
);
