CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE TABLE `Servers` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `IpAddress` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Port` int NOT NULL,
        `MapName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `CurrentPlayers` int NOT NULL,
        `MaxPlayers` int NOT NULL,
        `IsOnline` tinyint(1) NOT NULL,
        `LastChecked` datetime(6) NOT NULL,
        CONSTRAINT `PK_Servers` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE TABLE `SiteSettings` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Key` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Value` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Category` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
        `DataType` longtext CHARACTER SET utf8mb4 NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_SiteSettings` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE TABLE `SliderImages` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `ImageUrl` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Title` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Description` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
        `Order` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_SliderImages` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE TABLE `Users` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Username` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Email` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `PasswordHash` longtext CHARACTER SET utf8mb4 NOT NULL,
        `AvatarUrl` longtext CHARACTER SET utf8mb4 NULL,
        `IsAdmin` tinyint(1) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE TABLE `News` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Title` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
        `Content` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Summary` varchar(500) CHARACTER SET utf8mb4 NULL,
        `Slug` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
        `CoverImage` longtext CHARACTER SET utf8mb4 NULL,
        `AuthorId` int NOT NULL,
        `IsPublished` tinyint(1) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        `ViewCount` int NOT NULL,
        `LikeCount` int NOT NULL,
        `CommentCount` int NOT NULL,
        CONSTRAINT `PK_News` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_News_Users_AuthorId` FOREIGN KEY (`AuthorId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE TABLE `NewsComments` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `NewsId` int NOT NULL,
        `UserId` int NOT NULL,
        `Content` varchar(2000) CHARACTER SET utf8mb4 NOT NULL,
        `ParentCommentId` int NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_NewsComments` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_NewsComments_NewsComments_ParentCommentId` FOREIGN KEY (`ParentCommentId`) REFERENCES `NewsComments` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_NewsComments_News_NewsId` FOREIGN KEY (`NewsId`) REFERENCES `News` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_NewsComments_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE TABLE `NewsLikes` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `NewsId` int NOT NULL,
        `UserId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_NewsLikes` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_NewsLikes_News_NewsId` FOREIGN KEY (`NewsId`) REFERENCES `News` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_NewsLikes_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE TABLE `NewsMedia` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `NewsId` int NOT NULL,
        `MediaUrl` longtext CHARACTER SET utf8mb4 NOT NULL,
        `MediaType` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `Order` int NOT NULL,
        CONSTRAINT `PK_NewsMedia` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_NewsMedia_News_NewsId` FOREIGN KEY (`NewsId`) REFERENCES `News` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE INDEX `IX_News_AuthorId` ON `News` (`AuthorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE UNIQUE INDEX `IX_News_Slug` ON `News` (`Slug`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE INDEX `IX_NewsComments_NewsId` ON `NewsComments` (`NewsId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE INDEX `IX_NewsComments_ParentCommentId` ON `NewsComments` (`ParentCommentId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE INDEX `IX_NewsComments_UserId` ON `NewsComments` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE UNIQUE INDEX `IX_NewsLikes_NewsId_UserId` ON `NewsLikes` (`NewsId`, `UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE INDEX `IX_NewsLikes_UserId` ON `NewsLikes` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE INDEX `IX_NewsMedia_NewsId` ON `NewsMedia` (`NewsId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE INDEX `IX_SiteSettings_Category` ON `SiteSettings` (`Category`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE UNIQUE INDEX `IX_SiteSettings_Key` ON `SiteSettings` (`Key`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE UNIQUE INDEX `IX_Users_Email` ON `Users` (`Email`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    CREATE UNIQUE INDEX `IX_Users_Username` ON `Users` (`Username`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024105710_InitialMySqlMigration') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251024105710_InitialMySqlMigration', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE TABLE `Events` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Title` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
        `Content` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Summary` varchar(500) CHARACTER SET utf8mb4 NULL,
        `Slug` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
        `CoverImage` longtext CHARACTER SET utf8mb4 NULL,
        `AuthorId` int NOT NULL,
        `IsPublished` tinyint(1) NOT NULL,
        `StartDate` datetime(6) NOT NULL,
        `EndDate` datetime(6) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        `ViewCount` int NOT NULL,
        `LikeCount` int NOT NULL,
        `CommentCount` int NOT NULL,
        CONSTRAINT `PK_Events` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Events_Users_AuthorId` FOREIGN KEY (`AuthorId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE TABLE `EventComments` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `EventId` int NOT NULL,
        `UserId` int NOT NULL,
        `Content` varchar(2000) CHARACTER SET utf8mb4 NOT NULL,
        `ParentCommentId` int NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_EventComments` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_EventComments_EventComments_ParentCommentId` FOREIGN KEY (`ParentCommentId`) REFERENCES `EventComments` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_EventComments_Events_EventId` FOREIGN KEY (`EventId`) REFERENCES `Events` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_EventComments_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE TABLE `EventLikes` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `EventId` int NOT NULL,
        `UserId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_EventLikes` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_EventLikes_Events_EventId` FOREIGN KEY (`EventId`) REFERENCES `Events` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_EventLikes_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE TABLE `EventMedia` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `EventId` int NOT NULL,
        `MediaUrl` longtext CHARACTER SET utf8mb4 NOT NULL,
        `MediaType` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `Order` int NOT NULL,
        CONSTRAINT `PK_EventMedia` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_EventMedia_Events_EventId` FOREIGN KEY (`EventId`) REFERENCES `Events` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE INDEX `IX_EventComments_EventId` ON `EventComments` (`EventId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE INDEX `IX_EventComments_ParentCommentId` ON `EventComments` (`ParentCommentId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE INDEX `IX_EventComments_UserId` ON `EventComments` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE UNIQUE INDEX `IX_EventLikes_EventId_UserId` ON `EventLikes` (`EventId`, `UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE INDEX `IX_EventLikes_UserId` ON `EventLikes` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE INDEX `IX_EventMedia_EventId` ON `EventMedia` (`EventId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE INDEX `IX_Events_AuthorId` ON `Events` (`AuthorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    CREATE UNIQUE INDEX `IX_Events_Slug` ON `Events` (`Slug`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024112832_AddEvents') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251024112832_AddEvents', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024225237_AddSteamFields') THEN

    ALTER TABLE `Users` ADD `SteamId` varchar(50) CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024225237_AddSteamFields') THEN

    ALTER TABLE `Users` ADD `SteamProfileUrl` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024225237_AddSteamFields') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251024225237_AddSteamFields', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024230913_AddViewTracking') THEN

    CREATE TABLE `EventViews` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `EventId` int NOT NULL,
        `UserId` int NULL,
        `IpAddress` varchar(45) CHARACTER SET utf8mb4 NULL,
        `ViewDate` datetime(6) NOT NULL,
        CONSTRAINT `PK_EventViews` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_EventViews_Events_EventId` FOREIGN KEY (`EventId`) REFERENCES `Events` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_EventViews_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024230913_AddViewTracking') THEN

    CREATE TABLE `NewsViews` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `NewsId` int NOT NULL,
        `UserId` int NULL,
        `IpAddress` varchar(45) CHARACTER SET utf8mb4 NULL,
        `ViewDate` datetime(6) NOT NULL,
        CONSTRAINT `PK_NewsViews` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_NewsViews_News_NewsId` FOREIGN KEY (`NewsId`) REFERENCES `News` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_NewsViews_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024230913_AddViewTracking') THEN

    CREATE INDEX `IX_EventViews_EventId_IpAddress_ViewDate` ON `EventViews` (`EventId`, `IpAddress`, `ViewDate`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024230913_AddViewTracking') THEN

    CREATE INDEX `IX_EventViews_EventId_UserId_ViewDate` ON `EventViews` (`EventId`, `UserId`, `ViewDate`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024230913_AddViewTracking') THEN

    CREATE INDEX `IX_EventViews_UserId` ON `EventViews` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024230913_AddViewTracking') THEN

    CREATE INDEX `IX_NewsViews_NewsId_IpAddress_ViewDate` ON `NewsViews` (`NewsId`, `IpAddress`, `ViewDate`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024230913_AddViewTracking') THEN

    CREATE INDEX `IX_NewsViews_NewsId_UserId_ViewDate` ON `NewsViews` (`NewsId`, `UserId`, `ViewDate`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024230913_AddViewTracking') THEN

    CREATE INDEX `IX_NewsViews_UserId` ON `NewsViews` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024230913_AddViewTracking') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251024230913_AddViewTracking', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024235205_AddEmailSystem') THEN

    CREATE TABLE `password_reset_tokens` (
        `id` int NOT NULL AUTO_INCREMENT,
        `token` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
        `user_id` int NOT NULL,
        `expires_at` datetime(6) NOT NULL,
        `is_used` tinyint(1) NOT NULL,
        `created_at` datetime(6) NOT NULL,
        `used_at` datetime(6) NULL,
        CONSTRAINT `PK_password_reset_tokens` PRIMARY KEY (`id`),
        CONSTRAINT `FK_password_reset_tokens_Users_user_id` FOREIGN KEY (`user_id`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024235205_AddEmailSystem') THEN

    CREATE TABLE `smtp_settings` (
        `id` int NOT NULL AUTO_INCREMENT,
        `host` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `port` int NOT NULL,
        `username` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `password` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
        `enable_ssl` tinyint(1) NOT NULL,
        `from_email` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `from_name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `is_configured` tinyint(1) NOT NULL,
        `updated_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_smtp_settings` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024235205_AddEmailSystem') THEN

    CREATE INDEX `IX_password_reset_tokens_token` ON `password_reset_tokens` (`token`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024235205_AddEmailSystem') THEN

    CREATE INDEX `IX_password_reset_tokens_user_id_is_used` ON `password_reset_tokens` (`user_id`, `is_used`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251024235205_AddEmailSystem') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251024235205_AddEmailSystem', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE TABLE `admin_tariffs` (
        `id` int NOT NULL AUTO_INCREMENT,
        `server_id` int NOT NULL,
        `name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `description` varchar(1000) CHARACTER SET utf8mb4 NOT NULL,
        `duration_days` int NOT NULL,
        `price` decimal(10,2) NOT NULL,
        `flags` varchar(100) CHARACTER SET utf8mb4 NULL,
        `group_name` varchar(100) CHARACTER SET utf8mb4 NULL,
        `immunity` int NOT NULL,
        `is_active` tinyint(1) NOT NULL,
        `order` int NOT NULL,
        `created_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_admin_tariffs` PRIMARY KEY (`id`),
        CONSTRAINT `FK_admin_tariffs_Servers_server_id` FOREIGN KEY (`server_id`) REFERENCES `Servers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE TABLE `donation_packages` (
        `id` int NOT NULL AUTO_INCREMENT,
        `title` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
        `description` varchar(1000) CHARACTER SET utf8mb4 NOT NULL,
        `suggested_amounts` varchar(500) CHARACTER SET utf8mb4 NULL,
        `is_active` tinyint(1) NOT NULL,
        `created_at` datetime(6) NOT NULL,
        `updated_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_donation_packages` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE TABLE `sourcebans_settings` (
        `id` int NOT NULL AUTO_INCREMENT,
        `host` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `port` int NOT NULL,
        `database` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `username` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `password` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
        `is_configured` tinyint(1) NOT NULL,
        `updated_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_sourcebans_settings` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE TABLE `yoomoney_settings` (
        `id` int NOT NULL AUTO_INCREMENT,
        `wallet_number` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `secret_key` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
        `is_configured` tinyint(1) NOT NULL,
        `updated_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_yoomoney_settings` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE TABLE `donation_transactions` (
        `id` int NOT NULL AUTO_INCREMENT,
        `transaction_id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `user_id` int NULL,
        `steam_id` varchar(50) CHARACTER SET utf8mb4 NULL,
        `amount` decimal(10,2) NOT NULL,
        `type` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `tariff_id` int NULL,
        `server_id` int NULL,
        `status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `payment_method` varchar(50) CHARACTER SET utf8mb4 NULL,
        `label` varchar(255) CHARACTER SET utf8mb4 NULL,
        `expires_at` datetime(6) NULL,
        `created_at` datetime(6) NOT NULL,
        `completed_at` datetime(6) NULL,
        CONSTRAINT `PK_donation_transactions` PRIMARY KEY (`id`),
        CONSTRAINT `FK_donation_transactions_Servers_server_id` FOREIGN KEY (`server_id`) REFERENCES `Servers` (`Id`),
        CONSTRAINT `FK_donation_transactions_Users_user_id` FOREIGN KEY (`user_id`) REFERENCES `Users` (`Id`),
        CONSTRAINT `FK_donation_transactions_admin_tariffs_tariff_id` FOREIGN KEY (`tariff_id`) REFERENCES `admin_tariffs` (`id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE TABLE `user_admin_privileges` (
        `id` int NOT NULL AUTO_INCREMENT,
        `user_id` int NOT NULL,
        `steam_id` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `server_id` int NOT NULL,
        `tariff_id` int NOT NULL,
        `transaction_id` int NOT NULL,
        `flags` varchar(100) CHARACTER SET utf8mb4 NULL,
        `group_name` varchar(100) CHARACTER SET utf8mb4 NULL,
        `immunity` int NOT NULL,
        `starts_at` datetime(6) NOT NULL,
        `expires_at` datetime(6) NOT NULL,
        `is_active` tinyint(1) NOT NULL,
        `sourcebans_admin_id` int NULL,
        `created_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_user_admin_privileges` PRIMARY KEY (`id`),
        CONSTRAINT `FK_user_admin_privileges_Servers_server_id` FOREIGN KEY (`server_id`) REFERENCES `Servers` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_user_admin_privileges_Users_user_id` FOREIGN KEY (`user_id`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_user_admin_privileges_admin_tariffs_tariff_id` FOREIGN KEY (`tariff_id`) REFERENCES `admin_tariffs` (`id`) ON DELETE CASCADE,
        CONSTRAINT `FK_user_admin_privileges_donation_transactions_transaction_id` FOREIGN KEY (`transaction_id`) REFERENCES `donation_transactions` (`id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_admin_tariffs_server_id_is_active` ON `admin_tariffs` (`server_id`, `is_active`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_donation_transactions_server_id` ON `donation_transactions` (`server_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_donation_transactions_steam_id` ON `donation_transactions` (`steam_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_donation_transactions_tariff_id` ON `donation_transactions` (`tariff_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE UNIQUE INDEX `IX_donation_transactions_transaction_id` ON `donation_transactions` (`transaction_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_donation_transactions_user_id_status` ON `donation_transactions` (`user_id`, `status`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_user_admin_privileges_expires_at` ON `user_admin_privileges` (`expires_at`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_user_admin_privileges_server_id` ON `user_admin_privileges` (`server_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_user_admin_privileges_steam_id_server_id_is_active` ON `user_admin_privileges` (`steam_id`, `server_id`, `is_active`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_user_admin_privileges_tariff_id` ON `user_admin_privileges` (`tariff_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_user_admin_privileges_transaction_id` ON `user_admin_privileges` (`transaction_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    CREATE INDEX `IX_user_admin_privileges_user_id_server_id_is_active` ON `user_admin_privileges` (`user_id`, `server_id`, `is_active`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025082943_AddDonationSystem') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251025082943_AddDonationSystem', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025085648_UpdateSourceBansForMultiServer') THEN

    ALTER TABLE `sourcebans_settings` ADD `created_at` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025085648_UpdateSourceBansForMultiServer') THEN

    ALTER TABLE `sourcebans_settings` ADD `server_id` int NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025085648_UpdateSourceBansForMultiServer') THEN

    CREATE INDEX `IX_sourcebans_settings_server_id` ON `sourcebans_settings` (`server_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025085648_UpdateSourceBansForMultiServer') THEN

    ALTER TABLE `sourcebans_settings` ADD CONSTRAINT `FK_sourcebans_settings_Servers_server_id` FOREIGN KEY (`server_id`) REFERENCES `Servers` (`Id`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025085648_UpdateSourceBansForMultiServer') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251025085648_UpdateSourceBansForMultiServer', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025091415_AddPendingPaymentTracking') THEN

    ALTER TABLE `donation_transactions` ADD `cancelled_at` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025091415_AddPendingPaymentTracking') THEN

    ALTER TABLE `donation_transactions` ADD `payment_url` varchar(1000) CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025091415_AddPendingPaymentTracking') THEN

    ALTER TABLE `donation_transactions` ADD `pending_expires_at` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025091415_AddPendingPaymentTracking') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251025091415_AddPendingPaymentTracking', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    ALTER TABLE `admin_tariffs` DROP COLUMN `duration_days`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    ALTER TABLE `admin_tariffs` DROP COLUMN `price`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    ALTER TABLE `user_admin_privileges` ADD `tariff_option_id` int NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    ALTER TABLE `donation_transactions` ADD `tariff_option_id` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    ALTER TABLE `admin_tariffs` ADD `AdminTariffGroupId` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    CREATE TABLE `admin_tariff_groups` (
        `id` int NOT NULL AUTO_INCREMENT,
        `server_id` int NOT NULL,
        `name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `description` varchar(1000) CHARACTER SET utf8mb4 NOT NULL,
        `order` int NOT NULL,
        `is_active` tinyint(1) NOT NULL,
        `created_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_admin_tariff_groups` PRIMARY KEY (`id`),
        CONSTRAINT `FK_admin_tariff_groups_Servers_server_id` FOREIGN KEY (`server_id`) REFERENCES `Servers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    CREATE TABLE `admin_tariff_options` (
        `id` int NOT NULL AUTO_INCREMENT,
        `tariff_id` int NOT NULL,
        `duration_days` int NOT NULL,
        `price` decimal(10,2) NOT NULL,
        `order` int NOT NULL,
        `is_active` tinyint(1) NOT NULL,
        `created_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_admin_tariff_options` PRIMARY KEY (`id`),
        CONSTRAINT `FK_admin_tariff_options_admin_tariffs_tariff_id` FOREIGN KEY (`tariff_id`) REFERENCES `admin_tariffs` (`id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    CREATE INDEX `IX_user_admin_privileges_tariff_option_id` ON `user_admin_privileges` (`tariff_option_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    CREATE INDEX `IX_donation_transactions_tariff_option_id` ON `donation_transactions` (`tariff_option_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    CREATE INDEX `IX_admin_tariffs_AdminTariffGroupId` ON `admin_tariffs` (`AdminTariffGroupId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    CREATE INDEX `IX_admin_tariff_groups_server_id_is_active` ON `admin_tariff_groups` (`server_id`, `is_active`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    CREATE INDEX `IX_admin_tariff_options_tariff_id_is_active` ON `admin_tariff_options` (`tariff_id`, `is_active`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    ALTER TABLE `admin_tariffs` ADD CONSTRAINT `FK_admin_tariffs_admin_tariff_groups_AdminTariffGroupId` FOREIGN KEY (`AdminTariffGroupId`) REFERENCES `admin_tariff_groups` (`id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    ALTER TABLE `donation_transactions` ADD CONSTRAINT `FK_donation_transactions_admin_tariff_options_tariff_option_id` FOREIGN KEY (`tariff_option_id`) REFERENCES `admin_tariff_options` (`id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    ALTER TABLE `user_admin_privileges` ADD CONSTRAINT `FK_user_admin_privileges_admin_tariff_options_tariff_option_id` FOREIGN KEY (`tariff_option_id`) REFERENCES `admin_tariff_options` (`id`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025104302_AddTariffOptionsSystem') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251025104302_AddTariffOptionsSystem', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025110034_AddAdminPasswordFields') THEN

    ALTER TABLE `user_admin_privileges` ADD `admin_password` varchar(100) CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025110034_AddAdminPasswordFields') THEN

    ALTER TABLE `donation_transactions` ADD `admin_password` varchar(100) CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025110034_AddAdminPasswordFields') THEN

    ALTER TABLE `admin_tariff_options` ADD `requires_password` tinyint(1) NOT NULL DEFAULT TRUE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025110034_AddAdminPasswordFields') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251025110034_AddAdminPasswordFields', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025112405_AddNotifications') THEN

    CREATE TABLE `Notifications` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `Title` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
        `Message` varchar(1000) CHARACTER SET utf8mb4 NOT NULL,
        `Type` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `IsRead` tinyint(1) NOT NULL,
        `RelatedEntityId` int NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Notifications` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Notifications_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025112405_AddNotifications') THEN

    CREATE INDEX `IX_Notifications_CreatedAt` ON `Notifications` (`CreatedAt`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025112405_AddNotifications') THEN

    CREATE INDEX `IX_Notifications_UserId_IsRead` ON `Notifications` (`UserId`, `IsRead`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025112405_AddNotifications') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251025112405_AddNotifications', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN

    ALTER TABLE `user_admin_privileges` DROP FOREIGN KEY `FK_user_admin_privileges_admin_tariff_options_tariff_option_id`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN

    ALTER TABLE `user_admin_privileges` DROP FOREIGN KEY `FK_user_admin_privileges_donation_transactions_transaction_id`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN

    ALTER TABLE `user_admin_privileges` MODIFY COLUMN `transaction_id` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN

    ALTER TABLE `user_admin_privileges` MODIFY COLUMN `tariff_option_id` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN

    ALTER TABLE `user_admin_privileges` ADD `updated_at` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN

    ALTER TABLE `user_admin_privileges` ADD CONSTRAINT `FK_user_admin_privileges_admin_tariff_options_tariff_option_id` FOREIGN KEY (`tariff_option_id`) REFERENCES `admin_tariff_options` (`id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN

    ALTER TABLE `user_admin_privileges` ADD CONSTRAINT `FK_user_admin_privileges_donation_transactions_transaction_id` FOREIGN KEY (`transaction_id`) REFERENCES `donation_transactions` (`id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025122707_AddUpdatedAtToUserAdminPrivileges') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251025122707_AddUpdatedAtToUserAdminPrivileges', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025203310_AddPrivilegeIdToDonationTransaction') THEN

    ALTER TABLE `donation_transactions` ADD `privilege_id` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025203310_AddPrivilegeIdToDonationTransaction') THEN

    CREATE INDEX `IX_donation_transactions_privilege_id` ON `donation_transactions` (`privilege_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025203310_AddPrivilegeIdToDonationTransaction') THEN

    ALTER TABLE `donation_transactions` ADD CONSTRAINT `FK_donation_transactions_user_admin_privileges_privilege_id` FOREIGN KEY (`privilege_id`) REFERENCES `user_admin_privileges` (`id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025203310_AddPrivilegeIdToDonationTransaction') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251025203310_AddPrivilegeIdToDonationTransaction', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    ALTER TABLE `donation_transactions` DROP FOREIGN KEY `FK_donation_transactions_user_admin_privileges_privilege_id`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    ALTER TABLE `donation_transactions` DROP INDEX `IX_donation_transactions_privilege_id`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE TABLE `vip_settings` (
        `id` int NOT NULL AUTO_INCREMENT,
        `server_id` int NOT NULL,
        `host` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `port` int NOT NULL,
        `database` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `username` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `password` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
        `is_configured` tinyint(1) NOT NULL,
        `created_at` datetime(6) NOT NULL,
        `updated_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_vip_settings` PRIMARY KEY (`id`),
        CONSTRAINT `FK_vip_settings_Servers_server_id` FOREIGN KEY (`server_id`) REFERENCES `Servers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE TABLE `vip_tariffs` (
        `id` int NOT NULL AUTO_INCREMENT,
        `server_id` int NOT NULL,
        `name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `description` varchar(1000) CHARACTER SET utf8mb4 NOT NULL,
        `group_name` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
        `is_active` tinyint(1) NOT NULL,
        `order` int NOT NULL,
        `created_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_vip_tariffs` PRIMARY KEY (`id`),
        CONSTRAINT `FK_vip_tariffs_Servers_server_id` FOREIGN KEY (`server_id`) REFERENCES `Servers` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE TABLE `vip_tariff_options` (
        `id` int NOT NULL AUTO_INCREMENT,
        `tariff_id` int NOT NULL,
        `duration_days` int NOT NULL,
        `price` decimal(10,2) NOT NULL,
        `order` int NOT NULL,
        `is_active` tinyint(1) NOT NULL,
        `created_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_vip_tariff_options` PRIMARY KEY (`id`),
        CONSTRAINT `FK_vip_tariff_options_vip_tariffs_tariff_id` FOREIGN KEY (`tariff_id`) REFERENCES `vip_tariffs` (`id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE TABLE `user_vip_privileges` (
        `id` int NOT NULL AUTO_INCREMENT,
        `user_id` int NOT NULL,
        `steam_id` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `server_id` int NOT NULL,
        `tariff_id` int NOT NULL,
        `tariff_option_id` int NULL,
        `group_name` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
        `starts_at` datetime(6) NOT NULL,
        `expires_at` datetime(6) NOT NULL,
        `is_active` tinyint(1) NOT NULL,
        `transaction_id` int NULL,
        `created_at` datetime(6) NOT NULL,
        `updated_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_user_vip_privileges` PRIMARY KEY (`id`),
        CONSTRAINT `FK_user_vip_privileges_Servers_server_id` FOREIGN KEY (`server_id`) REFERENCES `Servers` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_user_vip_privileges_Users_user_id` FOREIGN KEY (`user_id`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_user_vip_privileges_donation_transactions_transaction_id` FOREIGN KEY (`transaction_id`) REFERENCES `donation_transactions` (`id`),
        CONSTRAINT `FK_user_vip_privileges_vip_tariff_options_tariff_option_id` FOREIGN KEY (`tariff_option_id`) REFERENCES `vip_tariff_options` (`id`),
        CONSTRAINT `FK_user_vip_privileges_vip_tariffs_tariff_id` FOREIGN KEY (`tariff_id`) REFERENCES `vip_tariffs` (`id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_donation_transactions_privilege_id` ON `donation_transactions` (`privilege_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    ALTER TABLE `donation_transactions` ADD CONSTRAINT `FK_donation_transactions_user_admin_privileges_privilege_id` FOREIGN KEY (`privilege_id`) REFERENCES `user_admin_privileges` (`id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_user_vip_privileges_expires_at` ON `user_vip_privileges` (`expires_at`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_user_vip_privileges_server_id` ON `user_vip_privileges` (`server_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_user_vip_privileges_steam_id_server_id_is_active` ON `user_vip_privileges` (`steam_id`, `server_id`, `is_active`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_user_vip_privileges_tariff_id` ON `user_vip_privileges` (`tariff_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_user_vip_privileges_tariff_option_id` ON `user_vip_privileges` (`tariff_option_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_user_vip_privileges_transaction_id` ON `user_vip_privileges` (`transaction_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_user_vip_privileges_user_id_server_id_is_active` ON `user_vip_privileges` (`user_id`, `server_id`, `is_active`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_vip_settings_server_id` ON `vip_settings` (`server_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_vip_tariff_options_tariff_id_is_active` ON `vip_tariff_options` (`tariff_id`, `is_active`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    CREATE INDEX `IX_vip_tariffs_server_id_is_active` ON `vip_tariffs` (`server_id`, `is_active`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251031230413_AddVipSystem') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251031230413_AddVipSystem', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251105143712_AddBlockedIpAndUserLastIp') THEN

    ALTER TABLE `Users` ADD `LastIp` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251105143712_AddBlockedIpAndUserLastIp') THEN

    CREATE TABLE `BlockedIps` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `IpAddress` varchar(45) CHARACTER SET utf8mb4 NOT NULL,
        `Reason` longtext CHARACTER SET utf8mb4 NULL,
        `BlockedAt` datetime(6) NOT NULL,
        `BlockedByUserId` int NULL,
        CONSTRAINT `PK_BlockedIps` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_BlockedIps_Users_BlockedByUserId` FOREIGN KEY (`BlockedByUserId`) REFERENCES `Users` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251105143712_AddBlockedIpAndUserLastIp') THEN

    CREATE INDEX `IX_BlockedIps_BlockedByUserId` ON `BlockedIps` (`BlockedByUserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251105143712_AddBlockedIpAndUserLastIp') THEN

    CREATE UNIQUE INDEX `IX_BlockedIps_IpAddress` ON `BlockedIps` (`IpAddress`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251105143712_AddBlockedIpAndUserLastIp') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251105143712_AddBlockedIpAndUserLastIp', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251105150159_AddUserBlockingFields') THEN

    ALTER TABLE `Users` ADD `BlockReason` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251105150159_AddUserBlockingFields') THEN

    ALTER TABLE `Users` ADD `BlockedAt` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251105150159_AddUserBlockingFields') THEN

    ALTER TABLE `Users` ADD `IsBlocked` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251105150159_AddUserBlockingFields') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251105150159_AddUserBlockingFields', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123113942_AddButtonsToSliderImage') THEN

    ALTER TABLE `SliderImages` ADD `Buttons` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123113942_AddButtonsToSliderImage') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251123113942_AddButtonsToSliderImage', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123125101_AddTelegramSubscriber') THEN

    CREATE TABLE `TelegramSubscribers` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `ChatId` bigint NOT NULL,
        `UserId` int NULL,
        `Username` longtext CHARACTER SET utf8mb4 NULL,
        `FirstName` longtext CHARACTER SET utf8mb4 NULL,
        `LastName` longtext CHARACTER SET utf8mb4 NULL,
        `IsActive` tinyint(1) NOT NULL,
        `SubscribedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_TelegramSubscribers` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_TelegramSubscribers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123125101_AddTelegramSubscriber') THEN

    CREATE UNIQUE INDEX `IX_TelegramSubscribers_ChatId` ON `TelegramSubscribers` (`ChatId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123125101_AddTelegramSubscriber') THEN

    CREATE INDEX `IX_TelegramSubscribers_UserId` ON `TelegramSubscribers` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123125101_AddTelegramSubscriber') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251123125101_AddTelegramSubscriber', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123135210_AddEventNotificationFields') THEN

    ALTER TABLE `Events` ADD `IsEndNotificationSent` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123135210_AddEventNotificationFields') THEN

    ALTER TABLE `Events` ADD `IsStartNotificationSent` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123135210_AddEventNotificationFields') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251123135210_AddEventNotificationFields', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142328_AddCustomPages') THEN

    CREATE TABLE `CustomPages` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Title` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
        `Content` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Summary` varchar(500) CHARACTER SET utf8mb4 NULL,
        `Slug` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
        `CoverImage` longtext CHARACTER SET utf8mb4 NULL,
        `AuthorId` int NOT NULL,
        `IsPublished` tinyint(1) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        `ViewCount` int NOT NULL,
        CONSTRAINT `PK_CustomPages` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_CustomPages_Users_AuthorId` FOREIGN KEY (`AuthorId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142328_AddCustomPages') THEN

    CREATE TABLE `CustomPageMedia` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CustomPageId` int NOT NULL,
        `MediaUrl` longtext CHARACTER SET utf8mb4 NOT NULL,
        `MediaType` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `Order` int NOT NULL,
        CONSTRAINT `PK_CustomPageMedia` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_CustomPageMedia_CustomPages_CustomPageId` FOREIGN KEY (`CustomPageId`) REFERENCES `CustomPages` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142328_AddCustomPages') THEN

    CREATE INDEX `IX_CustomPageMedia_CustomPageId` ON `CustomPageMedia` (`CustomPageId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142328_AddCustomPages') THEN

    CREATE INDEX `IX_CustomPages_AuthorId` ON `CustomPages` (`AuthorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142328_AddCustomPages') THEN

    CREATE UNIQUE INDEX `IX_CustomPages_Slug` ON `CustomPages` (`Slug`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142328_AddCustomPages') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251123142328_AddCustomPages', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142737_AddCustomPageViews') THEN

    CREATE TABLE `CustomPageViews` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CustomPageId` int NOT NULL,
        `UserId` int NULL,
        `IpAddress` varchar(45) CHARACTER SET utf8mb4 NULL,
        `ViewDate` datetime(6) NOT NULL,
        CONSTRAINT `PK_CustomPageViews` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_CustomPageViews_CustomPages_CustomPageId` FOREIGN KEY (`CustomPageId`) REFERENCES `CustomPages` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_CustomPageViews_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142737_AddCustomPageViews') THEN

    CREATE INDEX `IX_CustomPageViews_CustomPageId_IpAddress_ViewDate` ON `CustomPageViews` (`CustomPageId`, `IpAddress`, `ViewDate`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142737_AddCustomPageViews') THEN

    CREATE INDEX `IX_CustomPageViews_CustomPageId_UserId_ViewDate` ON `CustomPageViews` (`CustomPageId`, `UserId`, `ViewDate`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142737_AddCustomPageViews') THEN

    CREATE INDEX `IX_CustomPageViews_UserId` ON `CustomPageViews` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251123142737_AddCustomPageViews') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251123142737_AddCustomPageViews', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127171334_AddVipFieldsToDonationTransactions') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND COLUMN_NAME = 'vip_tariff_id') THEN
    SET @s = 'ALTER TABLE `donation_transactions` ADD `vip_tariff_id` int NULL';
    PREPARE stmt FROM @s;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127171334_AddVipFieldsToDonationTransactions') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND COLUMN_NAME = 'vip_tariff_option_id') THEN
    SET @s = 'ALTER TABLE `donation_transactions` ADD `vip_tariff_option_id` int NULL';
    PREPARE stmt FROM @s;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127171334_AddVipFieldsToDonationTransactions') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.STATISTICS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND INDEX_NAME = 'IX_donation_transactions_vip_tariff_id') THEN
    CREATE INDEX `IX_donation_transactions_vip_tariff_id` ON `donation_transactions` (`vip_tariff_id`);
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127171334_AddVipFieldsToDonationTransactions') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.STATISTICS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND INDEX_NAME = 'IX_donation_transactions_vip_tariff_option_id') THEN
    CREATE INDEX `IX_donation_transactions_vip_tariff_option_id` ON `donation_transactions` (`vip_tariff_option_id`);
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127171334_AddVipFieldsToDonationTransactions') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND CONSTRAINT_NAME = 'FK_donation_transactions_vip_tariff_options_vip_tariff_option_id') THEN
    ALTER TABLE `donation_transactions` ADD CONSTRAINT `FK_donation_transactions_vip_tariff_options_vip_tariff_option_id` FOREIGN KEY (`vip_tariff_option_id`) REFERENCES `vip_tariff_options` (`id`);
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127171334_AddVipFieldsToDonationTransactions') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND CONSTRAINT_NAME = 'FK_donation_transactions_vip_tariffs_vip_tariff_id') THEN
    ALTER TABLE `donation_transactions` ADD CONSTRAINT `FK_donation_transactions_vip_tariffs_vip_tariff_id` FOREIGN KEY (`vip_tariff_id`) REFERENCES `vip_tariffs` (`id`);
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127171334_AddVipFieldsToDonationTransactions') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251127171334_AddVipFieldsToDonationTransactions', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127193135_AddEventCreatedNotificationSent') THEN

    ALTER TABLE `Events` ADD `IsCreatedNotificationSent` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127193135_AddEventCreatedNotificationSent') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND COLUMN_NAME = 'vip_tariff_id') THEN
    SET @s = 'ALTER TABLE `donation_transactions` ADD `vip_tariff_id` int NULL';
    PREPARE stmt FROM @s;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127193135_AddEventCreatedNotificationSent') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND COLUMN_NAME = 'vip_tariff_option_id') THEN
    SET @s = 'ALTER TABLE `donation_transactions` ADD `vip_tariff_option_id` int NULL';
    PREPARE stmt FROM @s;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127193135_AddEventCreatedNotificationSent') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.STATISTICS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND INDEX_NAME = 'IX_donation_transactions_vip_tariff_id') THEN
    CREATE INDEX `IX_donation_transactions_vip_tariff_id` ON `donation_transactions` (`vip_tariff_id`);
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127193135_AddEventCreatedNotificationSent') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.STATISTICS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND INDEX_NAME = 'IX_donation_transactions_vip_tariff_option_id') THEN
    CREATE INDEX `IX_donation_transactions_vip_tariff_option_id` ON `donation_transactions` (`vip_tariff_option_id`);
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127193135_AddEventCreatedNotificationSent') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND CONSTRAINT_NAME = 'FK_donation_transactions_vip_tariff_options_vip_tariff_option_id') THEN
    ALTER TABLE `donation_transactions` ADD CONSTRAINT `FK_donation_transactions_vip_tariff_options_vip_tariff_option_id` FOREIGN KEY (`vip_tariff_option_id`) REFERENCES `vip_tariff_options` (`id`);
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127193135_AddEventCreatedNotificationSent') THEN

    IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA = DATABASE() AND TABLE_NAME = 'donation_transactions' AND CONSTRAINT_NAME = 'FK_donation_transactions_vip_tariffs_vip_tariff_id') THEN
    ALTER TABLE `donation_transactions` ADD CONSTRAINT `FK_donation_transactions_vip_tariffs_vip_tariff_id` FOREIGN KEY (`vip_tariff_id`) REFERENCES `vip_tariffs` (`id`);
    END IF;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127193135_AddEventCreatedNotificationSent') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251127193135_AddEventCreatedNotificationSent', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251127215010_ConvertEventDatesToUtc') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251127215010_ConvertEventDatesToUtc', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

