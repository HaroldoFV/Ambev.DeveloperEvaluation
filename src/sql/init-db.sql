CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
                                                       "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
    );

START TRANSACTION;

CREATE SEQUENCE "SaleSequence" AS integer START WITH 1000 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;

CREATE TABLE "Sales" (
                         "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
                         "SaleNumber" bigint NOT NULL DEFAULT (nextval('"SaleSequence"')),
                         "SaleDate" timestamp with time zone NOT NULL,
                         "CustomerId" uuid NOT NULL,
                         "BranchId" uuid NOT NULL,
                         "TotalValue" numeric(18,2) NOT NULL,
                         "Status" character varying(20) NOT NULL,
                         "CreatedAt" timestamp with time zone NOT NULL,
                         CONSTRAINT "PK_Sales" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
                         "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
                         "Username" varchar(100) NOT NULL,
                         "Email" varchar(100) NOT NULL,
                         "Phone" varchar(100) NOT NULL,
                         "Password" varchar(100) NOT NULL,
                         "Role" character varying(20) NOT NULL,
                         "Status" character varying(20) NOT NULL,
                         "CreatedAt" timestamp with time zone NOT NULL,
                         "UpdatedAt" timestamp with time zone,
                         CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "SaleItems" (
                             "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
                             "ProductId" uuid NOT NULL,
                             "Quantity" integer NOT NULL,
                             "UnitPrice" numeric(18,2) NOT NULL,
                             "Discount" numeric NOT NULL,
                             "TotalValue" numeric(18,2) NOT NULL,
                             "SaleId" uuid NOT NULL,
                             CONSTRAINT "PK_SaleItems" PRIMARY KEY ("Id"),
                             CONSTRAINT "FK_SaleItems_Sales_SaleId" FOREIGN KEY ("SaleId") REFERENCES "Sales" ("Id")
);

CREATE INDEX "IX_SaleItems_SaleId" ON "SaleItems" ("SaleId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250428235635_InitialMigrations', '8.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE "SaleItems" ADD "CancelledAt" timestamp with time zone;

ALTER TABLE "SaleItems" ADD "IsCancelled" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250502232901_AddSaleItemCancellationFields', '8.0.10');

COMMIT;

