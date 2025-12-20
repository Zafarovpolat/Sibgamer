-- Migration: Add rcon_password to Servers
ALTER TABLE `Servers` ADD COLUMN `rcon_password` VARCHAR(100) NULL DEFAULT NULL;

-- If you're using MySQL and want to be explicit about position, add AFTER `Port`:
-- ALTER TABLE `Servers` ADD COLUMN `rcon_password` VARCHAR(100) NULL DEFAULT NULL AFTER `Port`;
