CREATE TABLE public.users
(
    "id" uuid NOT NULL,
    "first_name" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "last_name" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "display_name" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "email" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "users_pkey" PRIMARY KEY ("id")
);