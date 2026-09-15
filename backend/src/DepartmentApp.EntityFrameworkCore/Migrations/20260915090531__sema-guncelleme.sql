-- Migration 20260915090531 — sema-guncelleme
-- Provider: postgresql
ALTER TABLE "PurchaseRequests" ALTER COLUMN "Status" TYPE integer;

ALTER TABLE "PurchaseRequests" ALTER COLUMN "Status" SET NOT NULL;
