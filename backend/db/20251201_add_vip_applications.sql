CREATE TABLE IF NOT EXISTS `vip_applications` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NOT NULL,
  `username` VARCHAR(150) NOT NULL,
  `steam_id` VARCHAR(50) NOT NULL,
  `server_id` INT NOT NULL,
  `hours_per_week` INT DEFAULT NULL,
  `reason` TEXT NOT NULL,
  `status` VARCHAR(20) NOT NULL DEFAULT 'pending',
  `admin_id` INT DEFAULT NULL,
  `admin_comment` TEXT DEFAULT NULL,
  `vip_group` VARCHAR(128) DEFAULT NULL,
  `duration_days` INT DEFAULT NULL,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `processed_at` DATETIME DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `idx_vip_applications_user` (`user_id`),
  INDEX `idx_vip_applications_server` (`server_id`),
  INDEX `idx_vip_applications_status` (`status`)
);

